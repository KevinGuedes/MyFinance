import { createFileRoute, Link } from '@tanstack/react-router'

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
})

function Home() {
  return (
    <div>
      <p>Home Page</p>
      <Link to="/management-unit-dashboard">Management Unit Dashboard</Link>
    </div>
  )
}
