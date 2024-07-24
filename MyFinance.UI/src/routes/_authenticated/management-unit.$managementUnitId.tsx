import { createFileRoute } from '@tanstack/react-router'
import { ClipboardList, Shapes, Tag } from 'lucide-react'

import { Page, PageContent, PageHeader } from '@/components/page'
import { Skeleton } from '@/components/ui/skeleton'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { AccountTagsTable } from '@/features/account-tag/components/account-tags-table/account-tags-table'
import { CreateAccountTagDialog } from '@/features/account-tag/components/create-account-tag-dialog'
import { CategoriesTable } from '@/features/category/components/categories-table/categories-table'
import { CreateCategoryDialog } from '@/features/category/components/create-category-dialog'
import { useGetManagementUnit } from '@/features/management-unit/api/use-get-management-unit'
import { SummaryCards } from '@/features/management-unit/components/summary-cards'
import { SummaryCardsSkeleton } from '@/features/management-unit/components/summary-cards-skeleton'
import { UpdateManagementUnitDialog } from '@/features/management-unit/components/update-management-unit-dialog'
import { useDiscriminatedBalanceChartSettings } from '@/features/management-unit/store/discriminated-balance-chart-settings-store'
import { useGetDiscriminatedBalance } from '@/features/transfer/api/use-get-discriminated-balance'
import { useGetTransferGroups } from '@/features/transfer/api/use-get-transfer-groups'
import { DiscriminatedBalanceCard } from '@/features/transfer/components/discriminated-balance-card/discriminated-balance-card'
import { DiscriminatedBalanceCardSkeleton } from '@/features/transfer/components/discriminated-balance-card/discriminated-balance-card-skeleton'
import { TransferGroupsList } from '@/features/transfer/components/transfer-groups-list/transfer-groups-list'
import { TransferGroupsListSkeleton } from '@/features/transfer/components/transfer-groups-list/transfer-groups-list-skeleton'

export const Route = createFileRoute(
  '/_authenticated/management-unit/$managementUnitId',
)({
  component: ManagementUnitDashboard,
  staticData: {
    name: 'Management Unit Dashboard',
  },
})

function ManagementUnitDashboard() {
  const { pastMonths, includeCurrentMonth } =
    useDiscriminatedBalanceChartSettings()
  const managementUnitId = Route.useParams().managementUnitId
  const transferGroupsQuery = useGetTransferGroups(managementUnitId, 1, 50)
  const managementUnitQuery = useGetManagementUnit(managementUnitId)
  const discriminatedBalanceQuery = useGetDiscriminatedBalance(
    managementUnitId,
    pastMonths,
    includeCurrentMonth,
  )

  const isLoading =
    managementUnitQuery.isLoading &&
    discriminatedBalanceQuery.isLoading &&
    transferGroupsQuery.isLoading

  return (
    <Page>
      <PageHeader
        pageName={managementUnitQuery.data?.name}
        isLoadingInfo={isLoading}
      >
        {isLoading ? (
          <Skeleton className="size-9 rounded-full" />
        ) : (
          <>
            {managementUnitQuery.data && (
              <UpdateManagementUnitDialog
                managementUnit={managementUnitQuery.data}
              />
            )}
          </>
        )}
      </PageHeader>
      <PageContent className="flex flex-col gap-4 lg:flex-row">
        <div className="flex items-center justify-between sm:hidden">
          {managementUnitQuery.data && (
            <>
              <h1 className="text-2xl">{managementUnitQuery.data.name}</h1>
              <UpdateManagementUnitDialog
                managementUnit={managementUnitQuery.data}
              />
            </>
          )}
        </div>
        <div className="flex flex-col justify-between gap-4 lg:w-3/5">
          {isLoading ? (
            <SummaryCardsSkeleton />
          ) : (
            <>
              {managementUnitQuery.data && (
                <SummaryCards
                  balance={managementUnitQuery.data.balance}
                  income={managementUnitQuery.data.income}
                  outcome={managementUnitQuery.data.outcome}
                />
              )}
            </>
          )}
          <div className="h-full">
            {discriminatedBalanceQuery.isPending ? (
              <DiscriminatedBalanceCardSkeleton />
            ) : (
              <>
                {discriminatedBalanceQuery.data && (
                  <DiscriminatedBalanceCard
                    discriminatedBalanceData={
                      discriminatedBalanceQuery.data.discriminatedBalanceData
                    }
                  />
                )}
              </>
            )}
          </div>
        </div>
        <div className="flex grow flex-col rounded-lg border bg-background p-4 lg:w-2/5">
          <Tabs defaultValue="transfers" className="flex grow flex-col gap-2">
            <TabsList className="self-start bg-muted/40">
              <TabsTrigger value="transfers" disabled={isLoading}>
                <ClipboardList className="mr-1 size-4" />
                <span className="hidden sm:block">Transfers</span>
              </TabsTrigger>
              <TabsTrigger value="account-tags" disabled={isLoading}>
                <Tag className="mr-1 size-4" />
                <span className="hidden sm:block">Account Tags</span>
              </TabsTrigger>
              <TabsTrigger value="categories" disabled={isLoading}>
                <Shapes className="mr-1 size-4" />
                <span className="hidden sm:block">Categories</span>
              </TabsTrigger>
            </TabsList>
            <TabsContent
              value="transfers"
              className="flex grow flex-col justify-between gap-2 rounded-lg data-[state=inactive]:hidden"
            >
              {transferGroupsQuery.isLoading ? (
                <TransferGroupsListSkeleton />
              ) : (
                <>
                  {transferGroupsQuery.data && (
                    <TransferGroupsList
                      tranferGroups={transferGroupsQuery.data.items}
                    />
                  )}
                </>
              )}
            </TabsContent>
            <TabsContent
              value="account-tags"
              className="flex grow flex-col justify-between gap-4 rounded-lg data-[state=inactive]:hidden"
            >
              <AccountTagsTable managementUnitId={managementUnitId} />
              <CreateAccountTagDialog managementUnitId={managementUnitId} />
            </TabsContent>
            <TabsContent
              value="categories"
              className="flex grow flex-col justify-between gap-4 rounded-lg data-[state=inactive]:hidden"
            >
              <CategoriesTable managementUnitId={managementUnitId} />
              <CreateCategoryDialog managementUnitId={managementUnitId} />
            </TabsContent>
          </Tabs>
        </div>
      </PageContent>
    </Page>
  )
}
