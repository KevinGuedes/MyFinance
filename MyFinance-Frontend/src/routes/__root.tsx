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
    <ScrollArea className="flex max-h-dvh min-h-dvh flex-col overflow-y-auto">
      <Outlet />
      <TanStackRouterDevtools initialIsOpen={false} />
      <ScrollBar />
    </ScrollArea>
  )
}
