import { createFileRoute, Link, Outlet } from '@tanstack/react-router'
import { Loader2 } from 'lucide-react'
import { useEffect } from 'react'

import { SignInForm, SignInFormSchema } from '@/components/sign-in/sign-in-form'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { useGetUserInfo } from '@/http/user/use-get-user-info'
import { useSignIn } from '@/http/user/use-sign-in'
import { useUserStore } from '@/stores/user-store'

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
        <div className="flex flex-col items-center gap-2">
          <Loader2 size={112} className="animate-spin text-muted-foreground" />
          <p className="text-muted-foreground">Checking user data...</p>
        </div>
      </main>
    )
  } else if (authenticationStatus === 'signed-in') {
    return (
      <main className="flex grow flex-col">
        <header>Welcome, {user!.name}!</header>
        <div className="grow">
          <Outlet />
        </div>
      </main>
    )
  } else {
    return (
      <main className="flex grow items-center justify-center">
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
