import { ChevronLeft, ChevronRight, MoreHorizontal } from 'lucide-react'
import * as React from 'react'

import { Button, ButtonProps } from '@/components/ui/button'
import { cn } from '@/lib/utils'

const Pagination = ({ className, ...props }: React.ComponentProps<'nav'>) => (
  <nav
    role="navigation"
    aria-label="pagination"
    className={cn('mx-auto flex w-full justify-center', className)}
    {...props}
  />
)
Pagination.displayName = 'Pagination'

const PaginationContent = React.forwardRef<
  HTMLUListElement,
  React.ComponentProps<'ul'>
>(({ className, ...props }, ref) => (
  <ul
    ref={ref}
    className={cn('flex flex-row items-center gap-1', className)}
    {...props}
  />
))
PaginationContent.displayName = 'PaginationContent'

const PaginationItem = React.forwardRef<
  HTMLLIElement,
  React.ComponentProps<'li'>
>(({ className, ...props }, ref) => (
  <li ref={ref} className={cn('', className)} {...props} />
))
PaginationItem.displayName = 'PaginationItem'

const PaginationEllipsis = ({
  className,
  ...props
}: React.ComponentProps<'span'>) => (
  <span
    aria-hidden
    className={cn('flex h-9 w-9 items-center justify-center', className)}
    {...props}
  >
    <MoreHorizontal className="h-4 w-4" />
    <span className="sr-only">More pages</span>
  </span>
)
PaginationEllipsis.displayName = 'PaginationEllipsis'

type PaginationLinkProps = {
  isActive?: boolean
} & ButtonProps

const PaginationLink = ({
  className,
  isActive,
  size = 'icon',
  ...props
}: PaginationLinkProps) => (
  <Button
    role="link"
    aria-current={isActive ? 'page' : undefined}
    variant={isActive ? 'outline' : 'ghost'}
    size={size}
    className={cn(className, 'transition-none')}
    {...props}
  />
)
PaginationLink.displayName = 'PaginationLink'

interface PaginationPageProps
  extends Omit<React.ComponentProps<typeof PaginationLink>, 'children'> {
  page: number
}

const PaginationPage = ({ isActive, page, ...props }: PaginationPageProps) => (
  <PaginationLink
    aria-current={isActive ? 'page' : undefined}
    aria-label={`Page ${page}`}
    variant={isActive ? 'default' : 'outline'}
    preserveLabelWhenLoading={false}
    label={String(page)}
    {...props}
  />
)
PaginationPage.displayName = 'PaginationPage'

const PaginationPrevious = ({
  ...props
}: React.ComponentProps<typeof PaginationLink>) => (
  <PaginationLink
    rel="prev"
    aria-label="Previous Page"
    size="icon"
    variant="outline"
    icon={ChevronLeft}
    {...props}
  />
)
PaginationPrevious.displayName = 'PaginationPrevious'

const PaginationNext = ({
  ...props
}: React.ComponentProps<typeof PaginationLink>) => (
  <PaginationLink
    rel="next"
    aria-label="Next Page"
    iconLocation="right"
    size="icon"
    variant="outline"
    icon={ChevronRight}
    {...props}
  />
)
PaginationNext.displayName = 'PaginationNext'

export {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationNext,
  PaginationPage,
  PaginationPrevious,
}
