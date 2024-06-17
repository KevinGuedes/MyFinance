import { createFileRoute, Outlet } from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useCallback, useEffect } from 'react'

import { Header } from '@/components/header'
import { NavBar } from '@/components/nav-bar'
import { SearchManagementUnits } from '@/features/management-unit/components/search-management-units'
import { useGetUserInfo } from '@/features/user/api/use-get-user-info'
import { SignIn } from '@/features/user/components/sign-in'
import { useUserStore } from '@/features/user/stores/user-store'

export const Route = createFileRoute('/_authenticated')({
  component: AuthenticatedLayout,
})

function AuthenticatedLayout() {
  const { authenticationStatus, setUserInfo, user } = useUserStore()
  const { refetch } = useGetUserInfo()

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

  // else if (isError) {
  //   return (
  //     <main className="flex grow items-center justify-center">
  //       <div className="flex flex-col items-center gap-6" role="status">
  //         <div className="flex flex-col items-center">
  //           <p className="text-muted-foreground">
  //             Uh oh! My Finance seems to be offline now, please try again later.
  //           </p>
  //           <p className="text-muted-foreground">
  //             Sorry for the inconvenience.
  //           </p>
  //         </div>
  //         <Button onClick={handleRetry} variant="default">
  //           Retry
  //         </Button>
  //       </div>
  //     </main>
  //   )
  // }
  if (authenticationStatus === 'signed-in' && user) {
    return (
      <div className="flex grow flex-col">
        <SearchManagementUnits />
        <NavBar />
        <div className="flex grow flex-col bg-background sm:gap-4 sm:pl-14 sm:pt-4">
          <div className="flex grow flex-col bg-muted/40 sm:rounded-tl-2xl sm:border-l-2 sm:border-t-2 sm:p-6">
            <Header />
            <main className="flex grow flex-col p-4 sm:px-0 sm:pb-0">
              <Outlet />
            </main>
          </div>
        </div>
      </div>
    )
  } else if (authenticationStatus === 'signed-out') {
    return (
      <main className="flex grow items-center justify-center bg-muted/40">
        <SignIn />
      </main>
    )
  } else {
    return (
      <main className="flex grow items-center justify-center">
        <div className="flex flex-col items-center gap-2" role="status">
          <Loader2 size={112} className="animate-spin text-muted-foreground" />
          <p className="text-muted-foreground">Checking user data...</p>
        </div>
      </main>
    )
  }
}
