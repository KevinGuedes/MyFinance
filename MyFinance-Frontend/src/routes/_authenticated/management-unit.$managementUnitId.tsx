import { createFileRoute } from '@tanstack/react-router'
import { Pencil } from 'lucide-react'

import { Page, PageContent, PageHeader } from '@/components/page'
import { Button } from '@/components/ui/button'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip'
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
      <PageHeader pageName="Kariny Bordados">
        <Tooltip>
          <TooltipTrigger asChild>
            <Button
              variant="outline"
              size="icon"
              className="rounded-full border-none"
            >
              <Pencil className="size-5" />
            </Button>
          </TooltipTrigger>
          <TooltipContent side="bottom" align="end">
            Edit Management Unit
          </TooltipContent>
        </Tooltip>
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
          <SummaryCards />
          <div className="h-full">
            <AnnualBalanceCard />
          </div>
        </div>
        <div className="flex grow flex-col rounded-lg border bg-background p-4 lg:w-2/5">
          <Tabs defaultValue="transfers" className="flex grow flex-col border">
            <TabsList className="self-start bg-muted/40">
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
