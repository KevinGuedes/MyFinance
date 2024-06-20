import { QueryClient } from '@tanstack/react-query'
import { createRootRouteWithContext, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'

import { ScrollArea, ScrollBar } from '@/components/ui/scroll-area'

export const Route = createRootRouteWithContext<{
  queryClient: QueryClient
}>()({
  component: Root,
})

export function Root() {
  return (
    <ScrollArea className="h-dvh">
      <div className="flex min-h-screen flex-col pr-px">
        <Outlet />
      </div>
      <ScrollBar className="z-50" />
      <TanStackRouterDevtools initialIsOpen={false} />
    </ScrollArea>
  )
}
