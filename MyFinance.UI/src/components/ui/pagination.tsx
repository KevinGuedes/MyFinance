import {
  ChevronLeft,
  ChevronRight,
  Loader2,
  MoreHorizontal,
} from 'lucide-react'
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

type PaginationLinkProps = {
  isActive?: boolean
  isLoading?: boolean
  isPageOnlyLink?: boolean
} & ButtonProps

const PaginationLink = ({
  className,
  isActive,
  size = 'icon',
  isPageOnlyLink = true,
  isLoading = false,
  ...props
}: PaginationLinkProps) => (
  <Button
    aria-current={isActive ? 'page' : undefined}
    variant={isActive ? 'outline' : 'ghost'}
    size={size}
    className={cn(className, 'transition-none')}
    {...props}
  >
    {isPageOnlyLink && isLoading ? (
      <Loader2 className="size-4 animate-spin" />
    ) : (
      props.children
    )}
  </Button>
)
PaginationLink.displayName = 'PaginationLink'

const PaginationPrevious = ({
  className,
  isLoading = false,
  disabled,
  ...props
}: React.ComponentProps<typeof PaginationLink>) => (
  <PaginationLink
    aria-label="Go to previous page"
    size="default"
    disabled={disabled}
    className={cn('gap-2', className)}
    isPageOnlyLink={false}
    {...props}
  >
    {isLoading ? (
      <Loader2 className="size-4 animate-spin" />
    ) : (
      <ChevronLeft className="size-4" />
    )}
    <span>Previous</span>
  </PaginationLink>
)
PaginationPrevious.displayName = 'PaginationPrevious'

const PaginationNext = ({
  className,
  isLoading = false,
  disabled,
  ...props
}: React.ComponentProps<typeof PaginationLink>) => (
  <PaginationLink
    aria-label="Go to next page"
    size="default"
    disabled={disabled}
    className={cn('gap-2', className)}
    isPageOnlyLink={false}
    {...props}
  >
    <span>Next</span>
    {isLoading ? (
      <Loader2 className="size-4 animate-spin" />
    ) : (
      <ChevronRight className="size-4" />
    )}
  </PaginationLink>
)
PaginationNext.displayName = 'PaginationNext'

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

export {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
}
