import { createFileRoute } from '@tanstack/react-router'

import { Page, PageContent, PageHeader } from '@/components/page'
import { Card, CardContent } from '@/components/ui/card'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { AnnualBalanceCard } from '@/features/management-unit/components/annual-balance-card'
import { SummaryCards } from '@/features/management-unit/components/summary-cards'

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
        <div className="lg:w-2/5">
          <Card className="h-full">
            <CardContent className="p-4">
              <Tabs defaultValue="transfers">
                <TabsList>
                  <TabsTrigger value="transfers">Transfers</TabsTrigger>
                  <TabsTrigger value="account-tags">Account Tags</TabsTrigger>
                  <TabsTrigger value="categories">Categories</TabsTrigger>
                </TabsList>
                <TabsContent value="transfers">Tab 1 content</TabsContent>
                <TabsContent value="account-tags">Tab 2 content</TabsContent>
                <TabsContent value="categories">Tab 3 content</TabsContent>
              </Tabs>
            </CardContent>
          </Card>
        </div>
      </PageContent>
    </Page>
  )
}
