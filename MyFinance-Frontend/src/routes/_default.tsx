import { createFileRoute, Outlet } from '@tanstack/react-router'

export const Route = createFileRoute('/_default')({
  component: Layout,
})

function Layout() {
  return (
    <main className="flex h-full grow items-center justify-center p-6">
      <Outlet />
    </main>
  )
}
