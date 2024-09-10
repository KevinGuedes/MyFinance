import { ColumnDef } from '@tanstack/react-table'
import { MoreVertical } from 'lucide-react'

import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'

import { AccountTag } from '../../models/account-tag'

export const accountTagsTableColumns: ColumnDef<AccountTag>[] = [
  {
    accessorKey: 'tag',
    header: 'Tag',
    size: 100,
    cell: ({ row }) => {
      const tag = row.getValue<string>('tag')
      return <p className="text-left font-medium">{tag}</p>
    },
  },
  {
    accessorKey: 'description',
    header: 'Description',
    cell: ({ row }) => {
      const description = row.getValue<string>('description')
      return <p className="line-clamp-1">{description}</p>
    },
  },
  {
    id: 'actions',
    size: 80,
    cell: ({ row }) => {
      const payment = row.original

      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button
              variant="ghost"
              className="ml-auto rounded-full"
              size="icon-md"
              icon={MoreVertical}
              screenReaderLabel="Oepn Account Tag Menu"
            />
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem
              onClick={() => navigator.clipboard.writeText(payment.id)}
            >
              Copy payment ID
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem>View customer</DropdownMenuItem>
            <DropdownMenuItem>View payment details</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    },
  },
]
