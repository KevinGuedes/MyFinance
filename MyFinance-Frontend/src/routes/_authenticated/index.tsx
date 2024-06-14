import { createFileRoute } from '@tanstack/react-router'
import { Search } from 'lucide-react'

import { Input } from '@/components/ui/input'
import { ManagementUnitCard } from '@/features/management-unit/components/management-unit-card'
import { ManagementUnit } from '@/features/management-unit/models/management-unit'

export const Route = createFileRoute('/_authenticated/')({
  component: Home,
  staticData: {
    name: 'Home',
  },
})

const x: ManagementUnit[] = [
  {
    id: '1',
    name: 'Management Unit 1',
    income: 1000000,
    outcome: 500,
    balance: 500,
  },
  {
    id: '2',
    name: 'Management Unit 2',
    income: 1000,
    outcome: 500,
    balance: 500,
  },
  {
    id: '3',
    name: 'Management Unit 3',
    income: 1000,
    outcome: 500,
    balance: 500,
  },
  {
    id: '4',
    name: 'Management Unit 3',
    income: 1000,
    outcome: 500,
    balance: 500,
  },
  {
    id: '5',
    name: 'Management Unit 3',
    income: 1000,
    outcome: 500,
    balance: 500,
  },
  {
    id: '6',
    name: 'Management Unit 3',
    income: 1244400,
    outcome: 500,
    balance: 500,
  },
  {
    id: '7',
    name: 'Management Unit 3',
    income: 1000,
    outcome: 500,
    balance: 500,
  },
]

function Home() {
  return (
    <section className="flex flex-col gap-4">
      <header className="flex flex-col gap-2 sm:flex-row sm:justify-between">
        <h2 className="shrink-0 text-xl">Management Units</h2>
        <div className="relative">
          <Search className="absolute left-2.5 top-3 size-4 text-muted-foreground" />
          <Input
            type="search"
            placeholder="Search Management Unit..."
            className="w-full rounded-lg bg-background pl-8 placeholder-shown:text-ellipsis sm:w-[280px]"
          />
        </div>
      </header>
      <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
        {x.map((managementUnit) => (
          <ManagementUnitCard
            key={managementUnit.id}
            managementUnit={managementUnit}
          />
        ))}
      </div>
    </section>
  )
}
