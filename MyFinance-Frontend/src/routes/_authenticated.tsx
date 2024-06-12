import {
  createFileRoute,
  Navigate,
  Outlet,
  useLocation,
} from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useEffect } from 'react'

import { useUserInfo } from '@/http/user/use-user-info'
import { useUserStore } from '@/stores/user-store'

export const Route = createFileRoute('/_authenticated')({
  component: Authenticated,
})

function Authenticated() {
  const { authenticationStatus, user } = useUserStore()
  const userInfoMutation = useUserInfo()
  const originPath = useLocation({
    select: (location) => location.pathname,
  })

  useEffect(() => {
    if (authenticationStatus === 'indeterminate') {
      userInfoMutation.mutate({ originPath })
    }
  }, [])

  if (authenticationStatus === 'indeterminate') {
    return (
      <main className="flex grow items-center justify-center">
        <div className="flex flex-col items-center gap-2">
          <Loader2 size={112} className="animate-spin text-muted-foreground" />
          <p className="text-muted-foreground">Checking user data...</p>
        </div>
      </main>
    )
  } else if (authenticationStatus === 'signed-in' && user) {
    return (
      <main className="flex grow flex-col">
        <header>Welcome, {user.name}!</header>
        <div className="grow">
          <Outlet />
        </div>
      </main>
    )
  } else {
    return <Navigate to="/sign-in" />
  }
}
