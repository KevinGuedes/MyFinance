import { createFileRoute, Link, Outlet } from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useEffect } from 'react'

import { Header } from '@/components/header'
import { NavBar } from '@/components/nav-bar'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { useGetUserInfo } from '@/features/user/api/use-get-user-info'
import { useSignIn } from '@/features/user/api/use-sign-in'
import {
  SignInForm,
  SignInFormSchema,
} from '@/features/user/components/sign-in-form'
import { useUserStore } from '@/features/user/stores/user-store'

export const Route = createFileRoute('/_authenticated')({
  component: AuthenticatedLayout,
})

function AuthenticatedLayout() {
  const { authenticationStatus, setUserInfo, user } = useUserStore()
  const { refetch, isLoading } = useGetUserInfo()
  const signInMutation = useSignIn()

  async function onSubmit(values: SignInFormSchema) {
    await signInMutation.mutateAsync(values)
  }

  useEffect(() => {
    async function getUserInfo() {
      if (authenticationStatus === 'indeterminated') {
        setTimeout(async () => {
          const { data: userInfo } = await refetch()
          if (userInfo) {
            setUserInfo(userInfo)
          }
        }, 1000)
      }
    }

    getUserInfo()
  }, [authenticationStatus, refetch, setUserInfo])

  if (authenticationStatus === 'indeterminated' || isLoading) {
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
      <div className="flex grow flex-col">
        <NavBar />

        <div className="flex grow flex-col bg-background sm:gap-4 sm:pl-14 sm:pt-4">
          <div className="grow rounded-tl-2xl bg-muted/40">
            <Header />
            <main className="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-8 lg:grid-cols-3 xl:grid-cols-3">
              <Outlet />
            </main>
          </div>
        </div>
      </div>
    )
  } else {
    return (
      <main className="flex grow items-center justify-center bg-muted/40">
        <Card className="mx-auto w-full max-w-sm">
          <CardHeader>
            <CardTitle className="text-2xl">Sign In</CardTitle>
            <CardDescription>
              Enter your email below to login to your account
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-6">
            <SignInForm
              onSubmit={onSubmit}
              defaultValues={{
                email: '',
                plainTextPassword: '',
              }}
            />
            <div className="text-center text-sm">
              <p className="mr-1 inline">Don&apos;t have an account?</p>
              <Button asChild variant="link" className="h-auto p-0">
                <Link to="/sign-up">Sign Up</Link>
              </Button>
            </div>
          </CardContent>
        </Card>
      </main>
    )
  }
}
