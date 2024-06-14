import { Link, useMatches } from '@tanstack/react-router'
import { Home, Loader2, PanelLeft, User2 } from 'lucide-react'

import { useSignOut } from '@/features/user/api/use-sign-out'
import { useUserStore } from '@/features/user/stores/user-store'

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

export function Header() {
  const signOutMutation = useSignOut()
  const { user } = useUserStore()
  const pageName = useMatches({
    select: (match) => {
      return match[match.length - 1].staticData.name
    },
  })

  function handleSignOut() {
    signOutMutation.mutate()
  }

  return (
    <header className="sticky top-0 z-30 flex h-14 items-center justify-between gap-4 border-b bg-background px-4 sm:static sm:h-auto sm:border-0 sm:bg-transparent sm:p-0">
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
        <div className="flex grow items-end justify-between gap-8 sm:border-b-2 sm:pb-2">
          <h1 className="hidden text-3xl sm:block">{pageName}</h1>
          {user && (
            <div className="flex items-center gap-2">
              <p className="text-foreground">{user.name}</p>
              <DropdownMenu>
                <DropdownMenuTrigger asChild>
                  <Button
                    variant="outline"
                    size="icon"
                    className="group overflow-hidden rounded-full"
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
      </Sheet>
    </header>
  )
}
