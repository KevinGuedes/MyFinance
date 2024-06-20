import { TooltipProvider } from '@radix-ui/react-tooltip'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'
import { createRouter, RouterProvider } from '@tanstack/react-router'

import { ThemeProvider } from '@/components/ui/theme-provider'

import { Toaster } from './components/ui/toast/toaster'
import { routeTree } from './routeTree.gen'

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 1000 * 60 * 5,
    },
  },
})

export const router = createRouter({
  routeTree,
  context: {
    queryClient,
  },
})

// type ValidRoutes = ParseRoute<typeof routeTree>['fullPath']

export function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <ThemeProvider>
        <TooltipProvider delayDuration={300}>
          <Toaster />
          <RouterProvider router={router} />
        </TooltipProvider>
      </ThemeProvider>
      {/* <ReactQueryDevtools initialIsOpen={false} /> */}
    </QueryClientProvider>
  )
}
