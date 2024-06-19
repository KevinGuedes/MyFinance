import { Loader2 } from 'lucide-react'
import { useMemo } from 'react'

import { Paginated } from '@/features/common/paginated'
import { buildPagination } from '@/lib/utils'

import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from './ui/pagination'

type PaginationBuilderProps = {
  data: Paginated<unknown> | undefined
  onPageClick: (pageNumber: number) => void
  onNextClick: () => void
  isLoadingNext?: boolean
  isNextButtonDisabled?: boolean
  onPreviousClick: () => void
  isLoadingPrevious?: boolean
  isPreviousButtonDisabled?: boolean
}

export function PaginationBuilder({
  data,
  isLoadingNext,
  isLoadingPrevious,
  onNextClick,
  onPageClick,
  onPreviousClick,
  isNextButtonDisabled,
  isPreviousButtonDisabled,
}: PaginationBuilderProps) {
  const paginationSetup = useMemo(() => {
    if (data === undefined) return []
    return buildPagination(data.pageNumber, data.totalPages, 1)
  }, [data])

  return (
    <div className="flex w-full flex-col-reverse flex-wrap content-center items-center justify-center gap-1 md:flex-row lg:flex-nowrap">
      {data ? (
        <p className="shrink-0 text-muted-foreground">
          Showing{' '}
          <strong>
            {((data.pageNumber || 1) - 1) * data.pageSize + 1} -{' '}
            {data.pageSize * data.pageNumber}
          </strong>{' '}
          of <strong>{data.totalPages * data.pageSize}</strong> Management Units
        </p>
      ) : (
        <p className="flex items-center gap-2 text-muted-foreground">
          <Loader2 className="size-4 animate-spin" />
          Loading data...
        </p>
      )}
      <Pagination className="justify-center lg:justify-end">
        <PaginationContent>
          <PaginationItem>
            <PaginationPrevious
              isLoading={isLoadingPrevious}
              onClick={onPreviousClick}
              disabled={isPreviousButtonDisabled}
            />
          </PaginationItem>
          <div className="mx-1 hidden justify-center gap-1 sm:flex">
            {data ? (
              paginationSetup.map((page, index) => {
                if (page === 0) {
                  return (
                    <PaginationItem key={index}>
                      <PaginationEllipsis />
                    </PaginationItem>
                  )
                }

                if (page === 1) {
                  return (
                    <PaginationItem key={index}>
                      <PaginationLink
                        onClick={() => onPageClick(page)}
                        isActive={
                          data.pageNumber === undefined || data.pageNumber === 1
                        }
                      >
                        {page}
                      </PaginationLink>
                    </PaginationItem>
                  )
                }

                return (
                  <PaginationItem key={index}>
                    <PaginationLink
                      onClick={() => onPageClick(page)}
                      isActive={data.pageNumber === page}
                    >
                      {page}
                    </PaginationLink>
                  </PaginationItem>
                )
              })
            ) : (
              <Loader2 className="size-4 min-w-32 animate-spin text-muted-foreground" />
            )}
          </div>
          <PaginationItem>
            <PaginationNext
              isLoading={isLoadingNext}
              onClick={onNextClick}
              disabled={isNextButtonDisabled}
            />
          </PaginationItem>
        </PaginationContent>
      </Pagination>
    </div>
  )
}
