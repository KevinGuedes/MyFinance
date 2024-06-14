import { createFileRoute, Outlet } from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useEffect } from 'react'

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
  const { refetch, isLoading } = useGetUserInfo()

  useEffect(() => {
    async function getUserInfo() {
      if (authenticationStatus === 'indeterminated') {
        const { data: userInfo } = await refetch()
        if (userInfo) {
          setUserInfo(userInfo)
        }
      }
    }

    getUserInfo()
  }, [authenticationStatus, refetch, setUserInfo])

  if (authenticationStatus === 'indeterminated' || isLoading) {
    return (
      <main className="flex grow items-center justify-center">
        <div className="flex flex-col items-center gap-2" role="status">
          <Loader2 size={112} className="animate-spin text-muted-foreground" />
          <p className="text-muted-foreground">Checking user data...</p>
        </div>
      </main>
    )
  } else if (authenticationStatus === 'signed-in' && user) {
    return (
      <div className="flex grow flex-col">
        <SearchManagementUnits />
        <NavBar />
        <div className="flex grow flex-col bg-background sm:gap-4 sm:pl-14 sm:pt-4">
          <div className="grow bg-muted/40 sm:rounded-tl-2xl sm:border-l-2 sm:border-t-2 sm:p-6">
            <Header />
            <main className="p-4 sm:px-0">
              <Outlet />
            </main>
          </div>
        </div>
      </div>
    )
  } else {
    return (
      <main className="flex grow items-center justify-center bg-muted/40">
        <SignIn />
      </main>
    )
  }
}
