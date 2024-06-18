import { createFileRoute } from '@tanstack/react-router'
import { ChevronLeft, ChevronRight, Loader2, Search } from 'lucide-react'
import { useEffect, useState } from 'react'
import { z } from 'zod'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { useGetManagementUnits } from '@/features/management-unit/api/use-get-management-units'
import { CreateManagementUnitDialog } from '@/features/management-unit/components/create-management-unit-dialogs'
import { ManagementUnitCard } from '@/features/management-unit/components/management-unit-card'
import { ManagementUnitCardSkeleton } from '@/features/management-unit/components/management-unit-card-skeleton'
import useDebouncedValue from '@/hooks/useDebouncedValue'

const searchManagementUnitsSchema = z.object({
  pageNumber: z.number().optional(),
  search: z.string().optional().catch(undefined),
})

type SearchManagementUnitsSchema = z.infer<typeof searchManagementUnitsSchema>

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
  validateSearch: searchManagementUnitsSchema,
  staticData: {
    name: 'Home',
  },
})

const PAGE_SIZE = 1

function Home() {
  const navigate = Route.useNavigate()
  const [areaInUse, setAreaInUse] = useState<
    'next' | 'previous' | 'search' | undefined
  >()
  const { pageNumber, search } = Route.useSearch()
  const [searchTerm, setSearchTerm] = useState(search)
  const debouncedSearchTerm = useDebouncedValue(searchTerm, 500)
  const {
    data,
    isSuccess,
    isFetching,
    isPlaceholderData,
    isPending,
    isRefetching,
  } = useGetManagementUnits(pageNumber || 1, PAGE_SIZE, debouncedSearchTerm)

  useEffect(() => {
    navigate({
      search: (prev: SearchManagementUnitsSchema) => {
        return {
          pageNumber: prev.pageNumber,
          search:
            debouncedSearchTerm === '' || debouncedSearchTerm === undefined
              ? undefined
              : debouncedSearchTerm,
        }
      },
    })
  }, [debouncedSearchTerm, navigate])

  function handleSearchTermChange(e: React.ChangeEvent<HTMLInputElement>) {
    navigate({
      search: () => {
        return {
          pageNumber: 1,
          search: e.target.value === '' ? undefined : e.target.value,
        }
      },
    })
    setAreaInUse('search')
    setSearchTerm(e.target.value)
  }

  function handleGoToNextPage() {
    setAreaInUse('next')
    navigate({
      search: (prev: SearchManagementUnitsSchema) => {
        if (prev.pageNumber === undefined) return { ...prev, pageNumber: 2 }
        else return { ...prev, pageNumber: prev.pageNumber + 1 }
      },
    })
  }

  function handleBackToPreviousPage() {
    setAreaInUse('previous')
    navigate({
      search: (prev: SearchManagementUnitsSchema) => {
        return {
          ...prev,
          pageNumber: prev.pageNumber! - 1,
        }
      },
    })
  }

  const skeletonItems = Array.from({ length: PAGE_SIZE }).map(() => ({
    id: crypto.randomUUID(),
  }))

  const isLoadingNextPage = isFetching && areaInUse === 'next'
  const isLoadingPreviousPage = isFetching && areaInUse === 'previous'
  const shouldShowSearchSpinner = isRefetching && areaInUse === 'search'

  return (
    <section className="flex grow flex-col gap-4">
      <header className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        <h2 className="shrink-0 text-xl">Management Units</h2>
        <div className="md:col-star-2 flex gap-2 xl:col-start-3">
          <div className="relative grow">
            {shouldShowSearchSpinner ? (
              <Loader2 className="absolute left-2.5 top-3 size-4 animate-spin text-muted-foreground" />
            ) : (
              <Search className="absolute left-2.5 top-3 size-4 text-muted-foreground" />
            )}
            <Input
              type="search"
              value={searchTerm || ''}
              onChange={handleSearchTermChange}
              placeholder="Search Management Unit..."
              className="pl-8 placeholder-shown:text-ellipsis"
            />
          </div>
          <CreateManagementUnitDialog />
        </div>
      </header>
      <>
        {isPending && !isPlaceholderData && (
          <div
            className="grid gap-4 md:grid-cols-2 xl:grid-cols-3 2xl:grid-cols-4"
            role="status"
          >
            {skeletonItems.map((skeleton) => (
              <ManagementUnitCardSkeleton key={skeleton.id} />
            ))}
          </div>
        )}
        {isSuccess && (
          <>
            <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3 2xl:grid-cols-4">
              {data.items.map((managementUnit) => (
                <ManagementUnitCard
                  key={managementUnit.id}
                  managementUnit={managementUnit}
                />
              ))}
            </div>
          </>
        )}
        {isSuccess && data.items.length === 0 && (
          <div className="flex grow items-center justify-center">
            <p className="text-center text-muted-foreground">
              No management units found!
            </p>
          </div>
        )}
      </>
      <footer className="item-center mt-auto flex justify-center gap-2 justify-self-end">
        <Button
          onClick={handleBackToPreviousPage}
          disabled={!data?.hasPreviousPage || isLoadingPreviousPage}
          variant="outline"
          className="w-28"
        >
          {isLoadingPreviousPage ? (
            <Loader2 className="mr-2 size-4 animate-spin" />
          ) : (
            <ChevronLeft className="mr-2 size-4" />
          )}
          Previous
        </Button>
        <Button
          onClick={handleGoToNextPage}
          disabled={!data?.hasNextPage || isLoadingNextPage}
          variant="outline"
          className="w-28"
        >
          Next
          {isLoadingNextPage ? (
            <Loader2 className="ml-2 size-4 animate-spin" />
          ) : (
            <ChevronRight className="ml-2 size-4" />
          )}
        </Button>
      </footer>
    </section>
  )
}
