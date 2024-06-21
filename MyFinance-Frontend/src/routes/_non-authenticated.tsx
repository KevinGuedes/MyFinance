import { createFileRoute, Outlet } from '@tanstack/react-router'

export const Route = createFileRoute('/_non-authenticated')({
  component: DefaultLayout,
})

function DefaultLayout() {
  return (
    <main className="flex grow items-center justify-center bg-muted/40 p-6">
      <Outlet />
    </main>
  )
}
