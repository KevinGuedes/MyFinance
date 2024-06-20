import { createFileRoute } from '@tanstack/react-router'

import { Page, PageContent, PageHeader } from '@/components/page'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { CreateAccountTagDialog } from '@/features/account-tag/components/create-account-tag-dialog'
import { CreateCategoryDialog } from '@/features/category/components/create-category-dialog'
import { AnnualBalanceCard } from '@/features/management-unit/components/annual-balance-card'
import { SummaryCards } from '@/features/management-unit/components/summary-cards'
import { RegisterTransferDialog } from '@/features/transfer/components/register-transfer-dialog'

export const Route = createFileRoute(
  '/_authenticated/management-unit/$managementUnitId',
)({
  component: ManagementUnitDashboard,
  staticData: {
    name: 'Management Unit Dashboard',
  },
})

function ManagementUnitDashboard() {
  return (
    <Page>
      <PageHeader pageName="Kariny Bordados" />
      <PageContent className="flex flex-col gap-4 pb-2 lg:flex-row">
        <div className="flex flex-col justify-between gap-4 lg:w-3/5">
          <SummaryCards />
          <div className="h-full">
            <AnnualBalanceCard />
          </div>
        </div>
        <div className="flex grow flex-col rounded-lg border bg-background p-4 lg:w-2/5">
          <Tabs defaultValue="transfers" className="flex grow flex-col border">
            <TabsList className="self-start">
              <TabsTrigger value="transfers">Transfers</TabsTrigger>
              <TabsTrigger value="account-tags">Account Tags</TabsTrigger>
              <TabsTrigger value="categories">Categories</TabsTrigger>
            </TabsList>
            <TabsContent value="transfers">
              <RegisterTransferDialog />
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
