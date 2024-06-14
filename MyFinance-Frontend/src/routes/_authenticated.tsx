import { createFileRoute, Link, Outlet } from '@tanstack/react-router'
import { Home, Loader2, Package2, PanelLeft } from 'lucide-react'
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
import { Sheet, SheetContent, SheetTrigger } from '@/components/ui/sheet'
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip'
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
        <aside className="fixed inset-y-0 left-0 z-10 hidden w-14 flex-col border-r bg-background sm:flex">
          <nav className="flex flex-col items-center gap-4 px-2 sm:py-5">
            <Link
              href="#"
              className="group flex h-9 w-9 shrink-0 items-center justify-center gap-2 rounded-full bg-primary text-lg font-semibold text-primary-foreground md:h-8 md:w-8 md:text-base"
            >
              <Package2 className="h-4 w-4 transition-all group-hover:scale-110" />
              <span className="sr-only">Acme Inc</span>
            </Link>
            <Tooltip>
              <TooltipTrigger asChild>
                <Link
                  to="/"
                  className="flex size-9 items-center justify-center rounded-lg transition-colors hover:text-foreground md:size-8"
                  inactiveProps={{
                    className: 'text-muted-foreground',
                  }}
                  activeProps={{
                    className: 'bg-muted/40 text-accent-foreground',
                  }}
                >
                  <Home className="size-5" />
                  <span className="sr-only">Home</span>
                </Link>
              </TooltipTrigger>
              <TooltipContent side="right">Home</TooltipContent>
            </Tooltip>
          </nav>
        </aside>

        <div className="flex flex-col sm:gap-4 sm:py-4 sm:pl-14">
          <header className="sticky top-0 z-30 flex h-14 items-center gap-4 border-b bg-background px-4 sm:static sm:h-auto sm:border-0 sm:bg-transparent sm:px-6">
            <Sheet>
              <SheetTrigger asChild>
                <Button size="icon" variant="outline" className="sm:hidden">
                  <PanelLeft className="h-5 w-5" />
                  <span className="sr-only">Toggle Menu</span>
                </Button>
              </SheetTrigger>
              <SheetContent side="left" className="sm:max-w-xs">
                <nav className="grid gap-6 text-lg font-medium">
                  <Link
                    to="/"
                    className="flex items-center gap-4 px-2.5"
                    inactiveProps={{
                      className: 'text-muted-foreground hover:text-foreground',
                    }}
                    activeProps={{
                      className: 'text-foreground',
                    }}
                  >
                    <Home className="size-5" />
                    Home
                  </Link>
                </nav>
              </SheetContent>
            </Sheet>
          </header>
          <main className="grid flex-1 items-start gap-4 p-4 sm:px-6 sm:py-0 md:gap-8 lg:grid-cols-3 xl:grid-cols-3">
            <Outlet />
          </main>
        </div>
      </div>
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
