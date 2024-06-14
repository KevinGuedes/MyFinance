import { createFileRoute } from '@tanstack/react-router'
import { ChevronLeft, ChevronRight, Search } from 'lucide-react'
import { useState } from 'react'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { useGetManagementUnits } from '@/features/management-unit/api/use-get-management-units'
import { CreateManagementUnitDialog } from '@/features/management-unit/components/create-management-unit-dialogs'
import { ManagementUnitCard } from '@/features/management-unit/components/management-unit-card'
import { ManagementUnitCardSkeleton } from '@/features/management-unit/components/management-unit-card-skeleton'

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
  staticData: {
    name: 'Home',
  },
})

function Home() {
  const [page, setPage] = useState(1)
  const { data, isSuccess, isFetching } = useGetManagementUnits(page, 9)

  const skeletonItems = Array.from({ length: 9 }).map((_, index) => ({
    id: index,
  }))

  return (
    <section className="flex grow flex-col gap-4">
      <header className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        <h2 className="shrink-0 text-xl">Management Units</h2>
        <div className="md:col-star-2 flex gap-2 xl:col-start-3">
          <div className="relative grow">
            <Search className="absolute left-2.5 top-3 size-4 text-muted-foreground" />
            <Input
              type="search"
              placeholder="Search Management Unit..."
              className="w-full rounded-lg bg-background pl-8 placeholder-shown:text-ellipsis"
            />
          </div>
          <CreateManagementUnitDialog />
        </div>
      </header>
      <>
        {isFetching && (
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
      </>
      <footer className="item-center mt-auto flex justify-center gap-2 justify-self-end">
        <Button
          onClick={() => setPage((prev) => prev - 1)}
          disabled={!data?.hasPreviousPage}
          variant="outline"
          className="w-28"
        >
          <ChevronLeft className="mr-2 size-4" />
          Previous
        </Button>
        <Button
          onClick={() => {
            setPage((prev) => {
              return prev + 1
            })
          }}
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
