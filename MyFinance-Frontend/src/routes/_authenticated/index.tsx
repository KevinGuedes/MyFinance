import { createFileRoute } from '@tanstack/react-router'
import { ChevronLeft, ChevronRight, Loader2, Search } from 'lucide-react'
import { useState } from 'react'
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
})

type SearchManagementUnitsSchema = z.infer<typeof searchManagementUnitsSchema>

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
  validateSearch: searchManagementUnitsSchema,
  staticData: {
    name: 'Home',
  },
})

function Home() {
  const [searchTerm, setSearchTerm] = useState('')
  const { debouncedValue: debouncedSearchTerm, isChanging } = useDebouncedValue(
    searchTerm,
    500,
  )
  const navigate = Route.useNavigate()
  const { pageNumber } = Route.useSearch()
  const { data, isSuccess, isFetching } = useGetManagementUnits(
    pageNumber || 1,
    9,
    debouncedSearchTerm,
  )

  const skeletonItems = Array.from({ length: 9 }).map((_, index) => ({
    id: index,
  }))

  function handleSearchTermChange(e: React.ChangeEvent<HTMLInputElement>) {
    setSearchTerm(e.target.value)
    navigate({
      search: () => {
        return { pageNumber: 1 }
      },
    })
  }

  function handleGoToNextPage() {
    navigate({
      search: (prev: SearchManagementUnitsSchema) => {
        if (prev.pageNumber === undefined) return { pageNumber: 2 }
        return { pageNumber: prev.pageNumber + 1 }
      },
    })
  }

  function handleBackToPreviousPage() {
    navigate({
      search: (prev: SearchManagementUnitsSchema) => ({
        pageNumber: prev.pageNumber! - 1,
      }),
    })
  }

  const isLoading = isChanging || isFetching

  return (
    <section className="flex grow flex-col gap-4">
      <header className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        <h2 className="shrink-0 text-xl">Management Units</h2>
        <div className="md:col-star-2 flex gap-2 xl:col-start-3">
          <div className="relative grow">
            {isLoading ? (
              <Loader2 className="absolute left-2.5 top-3 size-4 animate-spin text-muted-foreground" />
            ) : (
              <Search className="absolute left-2.5 top-3 size-4 text-muted-foreground" />
            )}
            <Input
              type="search"
              value={searchTerm}
              onChange={handleSearchTermChange}
              placeholder="Search Management Unit..."
              className="w-full rounded-lg bg-background pl-8 placeholder-shown:text-ellipsis"
            />
          </div>
          <CreateManagementUnitDialog />
        </div>
      </header>
      <>
        {isLoading && (
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
          disabled={!data?.hasPreviousPage}
          variant="outline"
          className="w-28"
        >
          <ChevronLeft className="mr-2 size-4" />
          Previous
        </Button>
        <Button
          onClick={handleGoToNextPage}
          disabled={!data?.hasNextPage}
          variant="outline"
          className="w-28"
        >
          Next
          <ChevronRight className="ml-2 size-4" />
        </Button>
      </footer>
    </section>
  )
}
