import { createFileRoute, useRouter } from '@tanstack/react-router'

import { Header } from '@/components/header'
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
    <div>
      <Header pageName="Kariny Bordados" />
      <section>
        <header>
          <SummaryCards />
        </header>
        <Button variant="outline" onClick={handleGoBack}>
          Go Back
        </Button>
      </section>
    </div>
  )
}
