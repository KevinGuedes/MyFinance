import * as React from 'react'

import {
  CommandDialog,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
  CommandLoading,
} from '@/components/ui/command'

const managementUnits = [
  'Gastos Da Casa MUITO GRANDE ESSE NOME MINHA NOSSA ONDE VAMOS PARAR?',
  'Casa Kariny',
  'Casa Kevin',
  'Casarão',
  'Imprevistos',
  'Mercantil',
  'Pessoal',
  'Rendimentos',
  'Transportes',
  'Viagens',
]

export function SearchManagementUnits() {
  const [open, setOpen] = React.useState(false)
  const [loading, setLoading] = React.useState(false)
  const [items, setItems] = React.useState<string[]>([])
  const [searchTerm, setSearchTerm] = React.useState<string>('')

  React.useEffect(() => {
    async function getItems(searchTerm: string) {
      if (searchTerm === '') return
      const data = await new Promise<string[]>((resolve) =>
        setTimeout(
          () =>
            resolve(
              managementUnits.filter((item) =>
                item.toLowerCase().includes(searchTerm.toLowerCase()),
              ),
            ),
          500,
        ),
      )
      setItems(data)
      setLoading(false)
    }

    setLoading(searchTerm !== '')
    setItems([])
    const timeout = setTimeout(async () => getItems(searchTerm), 500)

    return () => clearTimeout(timeout)
  }, [searchTerm])

  React.useEffect(() => {
    const down = (e: KeyboardEvent) => {
      if (e.key === 'k' && (e.metaKey || e.ctrlKey)) {
        e.preventDefault()
        setOpen((open) => !open)
      }
    }

    document.addEventListener('keydown', down)
    return () => document.removeEventListener('keydown', down)
  }, [])

  function handleChange(value: string) {
    setSearchTerm(value)
  }

  function navigateToManagementUnit() {
    console.log('Navigate to management unit')
  }

  function handleOpen(open: boolean) {
    setItems([])
    setOpen(open)
  }

  return (
    <CommandDialog open={open} onOpenChange={handleOpen}>
      <CommandInput
        isLoading={loading}
        placeholder="Search for a management unit"
        onValueChange={handleChange}
      />
      <CommandList>
        {loading ? (
          <CommandLoading>Fetching Management Units...</CommandLoading>
        ) : (
          <CommandEmpty>No results found.</CommandEmpty>
        )}
        {items.length > 0 && (
          <CommandGroup>
            {items.map((mu) => (
              <CommandItem
                key={mu}
                onSelect={navigateToManagementUnit}
                className="cursor-pointer"
              >
                {mu}
              </CommandItem>
            ))}
          </CommandGroup>
        )}
      </CommandList>
    </CommandDialog>
  )
}
