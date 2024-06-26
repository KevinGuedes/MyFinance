import { Link } from '@tanstack/react-router'
import { Home, Loader2, PanelLeft } from 'lucide-react'
import React, { ReactNode } from 'react'

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
import { Skeleton } from './ui/skeleton'

type PageProps = React.ComponentProps<'div'>

export function Page({ children, className }: PageProps) {
  return (
    <div className={cn('flex grow flex-col gap-4 pb-4 sm:px-4', className)}>
      {children}
    </div>
  )
}

type PageContentProps = React.ComponentProps<'section'>
export function PageContent({ children, className }: PageContentProps) {
  return (
    <section className={cn('flex grow flex-col gap-4 px-4 sm:px-0', className)}>
      {children}
    </section>
  )
}

type PageFooterProps = React.ComponentProps<'footer'>
export function PageFooter({ children, className }: PageFooterProps) {
  return (
    <footer className={cn('mt-auto px-4 sm:px-0', className)}>
      {children}
    </footer>
  )
}

type PageHeaderProps = {
  pageName: string | undefined
  showBackButton?: boolean
  children?: ReactNode
  isLoadingInfo?: boolean
}
export function PageHeader({
  pageName,
  children,
  isLoadingInfo = false,
}: PageHeaderProps) {
  const signOutMutation = useSignOut()
  const { user, initials } = useUserStore()

  function handleSignOut() {
    signOutMutation.mutate()
  }

  return (
    <header className="sticky top-0 z-30 flex h-14 items-center justify-between gap-4 border-b bg-background px-4 py-2 sm:static sm:z-auto sm:h-auto sm:border-b sm:bg-transparent sm:px-0">
      <div className="flex items-end justify-end gap-8 sm:grow sm:justify-between">
        {isLoadingInfo ? (
          <Skeleton className="h-9 w-1/2" />
        ) : (
          <h1 className="hidden text-3xl sm:block">{pageName}</h1>
        )}
        <div className="sm:hidden">
          {user && (
            <DropdownMenu>
              <DropdownMenuTrigger asChild>
                <Button
                  variant="default"
                  size="icon"
                  className="group overflow-hidden rounded-full"
                >
                  {initials}
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
          )}
        </div>
        <div className="hidden sm:block">{children}</div>
      </div>
      <Sheet>
        <SheetTrigger asChild>
          <Button size="icon" variant="outline" className="sm:hidden">
            <PanelLeft className="size-5 rotate-180" />
            <span className="sr-only">Toggle Menu</span>
          </Button>
        </SheetTrigger>
        <SheetContent side="right" className="sm:max-w-xs">
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
    </header>
  )
}
