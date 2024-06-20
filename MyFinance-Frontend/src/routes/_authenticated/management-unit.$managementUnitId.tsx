import { createFileRoute, useRouter } from '@tanstack/react-router'

import { Page, PageContent, PageFooter, PageHeader } from '@/components/page'
import { Button } from '@/components/ui/button'
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
  const { history } = useRouter()

  function handleGoBack() {
    history.go(-1)
  }

  return (
    <Page>
      <PageHeader pageName="Kariny Bordados" />
      <PageContent>
        <header>
          <SummaryCards />
        </header>
      </PageContent>
      <PageFooter>
        <Button variant="outline" onClick={handleGoBack}>
          Go Back
        </Button>
      </PageFooter>
    </Page>
  )
}
