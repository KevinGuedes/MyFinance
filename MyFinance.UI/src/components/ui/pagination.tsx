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
  isLoading?: boolean
} & ButtonProps

const PaginationLink = ({
  className,
  isActive,
  size = 'icon',
  ...props
}: PaginationLinkProps) => (
  <Button
    aria-current={isActive ? 'page' : undefined}
    variant={isActive ? 'outline' : 'ghost'}
    size={size}
    className={cn(className, 'transition-none')}
    {...props}
  />
)
PaginationLink.displayName = 'PaginationLink'

interface PaginationPageProps
  extends Omit<
    React.ComponentProps<typeof PaginationLink>,
    'size' | 'children'
  > {
  page: number
}

const PaginationPage = ({
  className,
  isActive,
  page,
  isLoading,
  disabled,
  ...props
}: PaginationPageProps) => (
  <Button
    size="icon"
    aria-current={isActive ? 'page' : undefined}
    variant={isActive ? 'outline' : 'ghost'}
    disabled={disabled || isLoading}
    className={cn(className, 'transition-none')}
    {...props}
  >
    {isLoading ? <Loader2 className="size-5 animate-spin" /> : page}
  </Button>
)
PaginationPage.displayName = 'PaginationPage'

const PaginationPrevious = ({
  isLoading,
  disabled,
  ...props
}: React.ComponentProps<typeof PaginationLink>) => (
  <PaginationLink
    rel="prev"
    aria-label="Previous Page"
    size="default"
    variant="outline"
    disabled={disabled || isLoading}
    {...props}
  >
    {isLoading ? (
      <Loader2 className="mr-2 size-5 animate-spin" />
    ) : (
      <ChevronLeft className="mr-2 size-5" />
    )}
    Previous
  </PaginationLink>
)
PaginationPrevious.displayName = 'PaginationPrevious'

const PaginationNext = ({
  isLoading,
  disabled,
  ...props
}: React.ComponentProps<typeof PaginationLink>) => (
  <PaginationLink
    rel="next"
    aria-label="Next Page"
    size="default"
    variant="outline"
    disabled={disabled || isLoading}
    {...props}
  >
    Next
    {isLoading ? (
      <Loader2 className="ml-2 size-5 animate-spin" />
    ) : (
      <ChevronRight className="ml-2 size-5" />
    )}
  </PaginationLink>
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
