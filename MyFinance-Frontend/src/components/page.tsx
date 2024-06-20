import { Link } from '@tanstack/react-router'
import { Home, Loader2, PanelLeft, User2 } from 'lucide-react'
import React from 'react'

import { useSignOut } from '@/features/user/api/use-sign-out'
import { useUserStore } from '@/features/user/stores/user-store'
import { cn } from '@/lib/utils'

import { Button } from './ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from './ui/dropdown-menu'
import { Sheet, SheetContent, SheetTrigger } from './ui/sheet'

type PageProps = React.ComponentProps<'div'>

export function Page({ children, className }: PageProps) {
  return (
    <div className={cn('flex grow flex-col gap-4', className)}>{children}</div>
  )
}

type PageContentProps = React.ComponentProps<'section'>
export function PageContent({ children, className }: PageContentProps) {
  return (
    <section className={cn('flex grow flex-col gap-4', className)}>
      {children}
    </section>
  )
}

type PageFooterProps = React.ComponentProps<'footer'>
export function PageFooter({ children, className }: PageFooterProps) {
  return <footer className={cn('mt-auto', className)}>{children}</footer>
}

type PageHeaderProps = {
  pageName: string
}
export function PageHeader({ pageName }: PageHeaderProps) {
  const signOutMutation = useSignOut()
  const { user } = useUserStore()

  function handleSignOut() {
    signOutMutation.mutate()
  }

  return (
    <header className="sticky top-0 z-30 flex h-14 items-center justify-between gap-4 bg-background sm:static sm:z-auto sm:h-auto sm:border-0 sm:bg-transparent">
      <Sheet>
        <SheetTrigger asChild>
          <Button size="icon" variant="outline" className="sm:hidden">
            <PanelLeft className="h-5 w-5" />
            <span className="sr-only">Toggle Menu</span>
          </Button>
        </SheetTrigger>
        <SheetContent side="left" className="sm:max-w-xs">
          <div className="space-y-4">
            <h3 className="text-lg">Pages</h3>

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
                Management Units
              </Link>
            </nav>
          </div>
        </SheetContent>
      </Sheet>
      <div className="flex grow items-end justify-end gap-8 sm:justify-between sm:border-b-2 sm:pb-2">
        <h1 className="hidden text-3xl sm:block">{pageName}</h1>
        {user && (
          <div className="flex items-center gap-2">
            <p className="text-foreground">{user.name}</p>
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button
                  variant="outline"
                  size="icon"
                  className="group overflow-hidden rounded-full border-none"
                >
                  <User2 className="size-5 transition-transform group-hover:scale-110" />
                </Button>
              </DropdownMenuTrigger>
              <DropdownMenuContent align="end">
                <DropdownMenuLabel>My Account</DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem>Settings</DropdownMenuItem>
                <DropdownMenuItem>Support</DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem
                  onClick={handleSignOut}
                  className="flex justify-between"
                >
                  Sign Out
                  {signOutMutation.isPending && (
                    <Loader2 className="ml-2 size-4 animate-spin text-muted-foreground" />
                  )}
                </DropdownMenuItem>
              </DropdownMenuContent>
            </DropdownMenu>
          </div>
        )}
      </div>
    </header>
  )
}
