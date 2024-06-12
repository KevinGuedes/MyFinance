import { createRootRoute, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'

export const Route = createRootRoute({
  component: Root,
})

export function Root() {
  return (
    <div className="flex min-h-dvh flex-col overflow-auto">
      <Outlet />
      <TanStackRouterDevtools initialIsOpen={false} />
    </div>
  )
}
