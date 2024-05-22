import * as React from 'react'

import {
  CommandDialog,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
  CommandList,
} from '@/components/ui/command'

const managementUnits = [
  'Gastos Da Casa',
  'Imprevistos',
  'Mercantil',
  'Pessoal',
  'Rendimentos',
  'Transportes',
  'Viagens',
]

export function SearchManagementUnits() {
  const [open, setOpen] = React.useState(false)

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

  function navigateToManagementUnit() {
    console.log('Navigate to management unit')
  }

  return (
    <>
      <p className="text-sm text-muted-foreground">
        Press
        <kbd className="pointer-events-none inline-flex h-5 select-none items-center gap-1 rounded border bg-muted px-1.5 font-mono text-[10px] font-medium text-muted-foreground opacity-100">
          <span className="text-xs">CTRL</span>K
        </kbd>
      </p>
      <CommandDialog open={open} onOpenChange={setOpen}>
        <CommandInput placeholder="Search for a management unit" />
        <CommandList>
          <CommandEmpty>No results found.</CommandEmpty>
          <CommandGroup>
            {managementUnits.map((mu) => (
              <CommandItem
                key={mu}
                className="cursor-pointer"
                onSelect={navigateToManagementUnit}
              >
                {mu}
              </CommandItem>
            ))}
          </CommandGroup>
        </CommandList>
      </CommandDialog>
    </>
  )
}
