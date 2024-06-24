import { createFileRoute } from '@tanstack/react-router'
import { Pencil } from 'lucide-react'
import { useState } from 'react'

import { Page, PageContent, PageHeader } from '@/components/page'
import { Button } from '@/components/ui/button'
import { Skeleton } from '@/components/ui/skeleton'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip'
import { CreateAccountTagDialog } from '@/features/account-tag/components/create-account-tag-dialog'
import { CreateCategoryDialog } from '@/features/category/components/create-category-dialog'
import { useGetManagementUnit } from '@/features/management-unit/api/use-get-management-unit'
import { DiscriminatedBalanceCard } from '@/features/management-unit/components/discriminated-balance-card'
import { SummaryCards } from '@/features/management-unit/components/summary-cards'
import { SummaryCardsSkeleton } from '@/features/management-unit/components/summary-cards-skeleton'
import { UpdateManagementUnitDialog } from '@/features/management-unit/components/update-management-unit-dialog'
import { useGetDiscriminatedBalance } from '@/features/transfer/api/use-get-discriminated-balance'
import { useGetTransfers } from '@/features/transfer/api/use-get-transfers'
import { DiscriminatedBalanceCardSkeleton } from '@/features/transfer/components/discriminated-balance-card-skeleton'
import { TransfersTable } from '@/features/transfer/components/transfers-table/transfers-table'
import { TransfersTableSkeleton } from '@/features/transfer/components/transfers-table/transfers-table-skeleton'

export const Route = createFileRoute(
  '/_authenticated/management-unit/$managementUnitId',
)({
  component: ManagementUnitDashboard,
  staticData: {
    name: 'Management Unit Dashboard',
  },
})

function ManagementUnitDashboard() {
  const managementUnitId = Route.useParams().managementUnitId
  const [pastMonths, setPastMonths] = useState(12)

  const transfersQuery = useGetTransfers(managementUnitId, 1, 20)
  const managementUnitQuery = useGetManagementUnit(managementUnitId)
  const discriminatedBalanceQuery = useGetDiscriminatedBalance(
    managementUnitId,
    pastMonths,
  )

  const isLoading =
    managementUnitQuery.isLoading ||
    discriminatedBalanceQuery.isLoading ||
    transfersQuery.isLoading

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
              <Tooltip>
                <TooltipTrigger asChild>
                  <UpdateManagementUnitDialog
                    managementUnit={managementUnitQuery.data}
                  />
                </TooltipTrigger>
                <TooltipContent side="bottom" align="end">
                  Edit Management Unit
                </TooltipContent>
              </Tooltip>
            )}
          </>
        )}
      </PageHeader>
      <PageContent className="flex flex-col gap-4 lg:flex-row">
        <div className="flex items-center justify-between sm:hidden">
          <h1 className="text-2xl">Kariny Bordados</h1>
          <Tooltip>
            <TooltipTrigger asChild>
              <Button
                variant="outline"
                size="icon"
                className="rounded-full border-none"
              >
                <Pencil className="size-5" />
                <span className="sr-only">Edit Management Unit</span>
              </Button>
            </TooltipTrigger>
            <TooltipContent side="bottom" align="end">
              Edit Management Unit
            </TooltipContent>
          </Tooltip>
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
            {isLoading && !discriminatedBalanceQuery.isRefetching ? (
              <DiscriminatedBalanceCardSkeleton />
            ) : (
              <>
                {discriminatedBalanceQuery.data && (
                  <DiscriminatedBalanceCard
                    pastMonths={pastMonths}
                    onSelectPastMonths={setPastMonths}
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
          <Tabs defaultValue="transfers" className="flex grow flex-col">
            <TabsList className="self-start bg-muted/40">
              <TabsTrigger value="transfers" disabled={isLoading}>
                Transfers
              </TabsTrigger>
              <TabsTrigger value="account-tags" disabled={isLoading}>
                Account Tags
              </TabsTrigger>
              <TabsTrigger value="categories" disabled={isLoading}>
                Categories
              </TabsTrigger>
            </TabsList>
            <TabsContent
              value="transfers"
              className="flex grow flex-col justify-between gap-2"
            >
              {isLoading ? <TransfersTableSkeleton /> : <TransfersTable />}
            </TabsContent>
            <TabsContent value="account-tags">
              <CreateAccountTagDialog />
            </TabsContent>
            <TabsContent value="categories">
              <CreateCategoryDialog />
            </TabsContent>
          </Tabs>
        </div>
      </PageContent>
    </Page>
  )
}
