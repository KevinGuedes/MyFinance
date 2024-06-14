import { createFileRoute } from '@tanstack/react-router'
import { Search } from 'lucide-react'
import { useState } from 'react'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { useGetManagementUnits } from '@/features/management-unit/api/use-get-management-units'
import { CreateManagementUnitDialog } from '@/features/management-unit/components/create-management-unit-dialogs'
import { ManagementUnitCard } from '@/features/management-unit/components/management-unit-card'

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
  staticData: {
    name: 'Home',
  },
})

function Home() {
  const [page, setPage] = useState(1)
  const { data, isSuccess } = useGetManagementUnits(page, 9)

  return (
    <section className="flex flex-col gap-4">
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
      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3 2xl:grid-cols-4">
        {isSuccess &&
          data.items.map((managementUnit) => (
            <ManagementUnitCard
              key={managementUnit.id}
              managementUnit={managementUnit}
            />
          ))}
      </div>
      <div className="item-center flex justify-center gap-2">
        <Button
          onClick={() => setPage((prev) => prev - 1)}
          disabled={!data?.hasPreviousPage}
          className="w-20"
          variant="outline"
        >
          Previous
        </Button>
        <Button
          onClick={() => {
            setPage((prev) => {
              return prev + 1
            })
          }}
          disabled={!data?.hasNextPage}
          className="w-20"
          variant="outline"
        >
          Next
        </Button>
      </div>
    </section>
  )
}
