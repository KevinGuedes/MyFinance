import { QueryClient } from '@tanstack/react-query'
import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'

export const Route = createRootRouteWithContext<{
  queryClient: QueryClient
}>()({
  component: Root,
})

export function Root() {
  return (
    <div className="flex min-h-dvh flex-col overflow-auto">
      <Outlet />
      {/* <TanStackRouterDevtools initialIsOpen={false} /> */}
    </div>
  )
}
