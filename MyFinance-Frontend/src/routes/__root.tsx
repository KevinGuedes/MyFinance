import { createRootRoute, Outlet } from '@tanstack/react-router'
import { TanStackRouterDevtools } from '@tanstack/router-devtools'

export const Route = createRootRoute({
  component: Root,
})

export function Root() {
  return (
    <div className="flex h-dvh flex-col">
      <hr />
      <main className="flex grow items-center justify-center p-8">
        <Outlet />
      </main>
      <TanStackRouterDevtools initialIsOpen={false} />
    </div>
  )
}
