import { ColumnDef } from '@tanstack/react-table'

import { Category } from '../../models/category'
import { CategoriesTableAction } from './categories-table-actions'

export const categoriesTableColumns: ColumnDef<Category>[] = [
  {
    accessorKey: 'name',
    header: 'Name',
    cell: ({ row }) => {
      const name = row.getValue<string>('name')
      return <p className="text-left font-medium">{name}</p>
    },
  },

  {
    id: 'actions',
    size: 80,
    cell: ({ row }) => {
      const category = row.original
      return <CategoriesTableAction category={category} />
    },
  },
]
