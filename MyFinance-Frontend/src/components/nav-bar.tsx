import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@radix-ui/react-tooltip'
import { Link } from '@tanstack/react-router'
import { Home, Package2 } from 'lucide-react'

export function NavBar() {
  return (
    <aside className="fixed inset-y-0 left-0 z-10 hidden w-14 flex-col bg-background sm:flex">
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
  )
}
