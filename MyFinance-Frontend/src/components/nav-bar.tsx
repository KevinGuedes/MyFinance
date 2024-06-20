import { Link } from '@tanstack/react-router'
import { Home, Loader2 } from 'lucide-react'

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
import { ThemeSwitcher } from './ui/theme-switcher'
import { Tooltip, TooltipContent, TooltipTrigger } from './ui/tooltip'

export function NavBar() {
  const signOutMutation = useSignOut()
  const { user, initials } = useUserStore()

  function handleSignOut() {
    signOutMutation.mutate()
  }

  return (
    <aside className="fixed inset-y-0 left-0 z-10 hidden w-14 flex-col sm:flex">
      <div className="mt-6 flex items-end justify-center pb-2">
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
            <DropdownMenuContent align="start">
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
      <nav className="flex flex-col items-center gap-4 px-2 sm:py-5">
        <Tooltip>
          <TooltipTrigger asChild>
            <Link
              to="/"
              className="flex size-9 items-center justify-center rounded-lg transition-colors md:size-8"
              inactiveProps={{
                className: 'text-muted-foreground hover:text-primary ',
              }}
              activeProps={{
                className: 'bg-muted/40 text-primary',
              }}
            >
              <Home className="size-5" />
              <span className="sr-only">Management Units</span>
            </Link>
          </TooltipTrigger>
          <TooltipContent side="right">Management Units</TooltipContent>
        </Tooltip>
      </nav>
      <div className="flex grow items-end justify-center pb-2">
        <ThemeSwitcher />
      </div>
    </aside>
  )
}
