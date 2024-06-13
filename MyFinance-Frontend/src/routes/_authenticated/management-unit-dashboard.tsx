import { createFileRoute, Link } from '@tanstack/react-router'

export const Route = createFileRoute(
  '/_authenticated/management-unit-dashboard',
)({
  component: ManagementUnitDashboard,
})

function ManagementUnitDashboard() {
  return (
    <div>
      <p>Management Unit Dashboard</p>
      <Link to="/">Home</Link>
    </div>
  )
}