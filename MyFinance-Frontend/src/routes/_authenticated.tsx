import { createFileRoute, Outlet } from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useCallback, useEffect } from 'react'

import { NavBar } from '@/components/nav-bar'
import { Button } from '@/components/ui/button'
import { SearchManagementUnits } from '@/features/management-unit/components/search-management-units'
import { useGetUserInfo } from '@/features/user/api/use-get-user-info'
import { SignIn } from '@/features/user/components/sign-in'
import { useUserStore } from '@/features/user/stores/user-store'

export const Route = createFileRoute('/_authenticated')({
  component: AuthenticatedLayout,
})

function AuthenticatedLayout() {
  const { authenticationStatus, setUserInfo, user } = useUserStore()
  const { refetch, isError } = useGetUserInfo()

  const getUserInfo = useCallback(async () => {
    if (authenticationStatus === 'indeterminated') {
      const { data: userInfo } = await refetch()
      if (userInfo) {
        setUserInfo(userInfo)
      }
    }
  }, [authenticationStatus, refetch, setUserInfo])

  useEffect(() => {
    getUserInfo()
  }, [getUserInfo])

  function handleRetry() {
    getUserInfo()
  }

  if (authenticationStatus === 'signed-in' && user) {
    return (
      <div className="flex grow flex-col">
        <SearchManagementUnits />
        <NavBar />
        <div className="flex grow flex-col bg-background sm:gap-4 sm:pl-14 sm:pt-4">
          <main className="flex grow flex-col bg-muted/40 sm:rounded-tl-2xl sm:border-l sm:border-t">
            <Outlet />
          </main>
        </div>
      </div>
    )
  } else if (authenticationStatus === 'signed-out') {
    return (
      <main className="flex grow items-center justify-center bg-muted/40">
        <SignIn />
      </main>
    )
  } else if (isError) {
    // Make this a generic error comp. Or maybe a status store, if services are offline the whole app will show a nice error comp. 500 and 401 are api error right? So we know when the api is working. Currently this error html onle handles error for the user info api. If we have a ping endpoint we can use that to check if the api is online, and if not, we can show a nice error comp globally using the store and axios interceptors. Ping and err connection refused can be sued
    // Global layout to handle app status. Inside it we have all the pages, if the ping is working, all good. If ping is not, and connection refused show error comp. This way we can handle also a signup when the api is offline
    // see if router has something to handle this with error comp and before load
    return (
      <main className="flex grow items-center justify-center">
        <div className="flex flex-col items-center gap-4" role="status">
          <div className="flex flex-col items-center px-4 text-center">
            <p className="text-muted-foreground">
              Uh oh! My Finance seems to be offline now, please try again later.
            </p>
            <p className="text-muted-foreground">
              Sorry for the inconvenience.
              {import.meta.env.TEST_ENV_VITE}
            </p>
          </div>
          <Button variant="default" onClick={handleRetry}>
            Retry
          </Button>
        </div>
      </main>
    )
  } else {
    return (
      <main className="flex grow items-center justify-center">
        <div className="flex flex-col items-center gap-2" role="status">
          <Loader2 size={112} className="animate-spin text-muted-foreground" />
          <p className="text-muted-foreground">Loading application...</p>
        </div>
      </main>
    )
  }
}
