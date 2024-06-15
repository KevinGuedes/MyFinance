import { Link } from '@tanstack/react-router'
import { Home } from 'lucide-react'

import { ThemeSwitcher } from './ui/theme-switcher'
import { Tooltip, TooltipContent, TooltipTrigger } from './ui/tooltip'

export function NavBar() {
  return (
    <aside className="= fixed inset-y-0 left-0 z-10 hidden w-14 flex-col sm:flex">
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
                className: 'bg-muted/40 text-primary border',
              }}
            >
              <Home className="size-5" />
              <span className="sr-only">Home</span>
            </Link>
          </TooltipTrigger>
          <TooltipContent side="right">Home</TooltipContent>
        </Tooltip>
      </nav>
      <div className="flex grow items-end justify-center pb-2">
        <ThemeSwitcher />
      </div>
    </aside>
  )
}
