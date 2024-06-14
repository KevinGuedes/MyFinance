import { Link } from '@tanstack/react-router'
import { Home, PanelLeft } from 'lucide-react'

import { useUserStore } from '@/features/user/stores/user-store'

import { Button } from './ui/button'
import { Sheet, SheetContent, SheetTrigger } from './ui/sheet'

export function Header() {
  const { user } = useUserStore()

  return (
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
      <p>{user?.name}</p>
    </header>
  )
}
