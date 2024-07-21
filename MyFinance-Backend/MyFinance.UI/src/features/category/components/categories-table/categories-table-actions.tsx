import { MoreHorizontal } from 'lucide-react'
import { useRef, useState } from 'react'

import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'

import { Category } from '../../models/category'
import { ArchiveCategoryDialog } from '../archive-category-dialog'
import { UpdateCategoryDialog } from '../update-category-dialog'

type CategoriesTableActionProps = {
  category: Category
}

export function CategoriesTableAction({
  category,
}: CategoriesTableActionProps) {
  const [dropdownOpen, setDropdownOpen] = useState(false)
  const [hasOpenDialog, setHasOpenDialog] = useState(false)
  const dropdownTriggerRef = useRef<null | HTMLButtonElement>(null)
  const focusRef = useRef<null | HTMLButtonElement>(null)

  function handleDialogItemSelect() {
    focusRef.current = dropdownTriggerRef.current
  }

  function handleDialogItemOpenChange(open: boolean) {
    if (!open) {
      setDropdownOpen(false)
    }
    setHasOpenDialog(open)
  }

  return (
    <DropdownMenu
      open={dropdownOpen}
      onOpenChange={setDropdownOpen}
      modal={false}
    >
      <DropdownMenuTrigger asChild>
        <Button
          variant="ghost"
          className="ml-auto size-8 p-0"
          ref={dropdownTriggerRef}
        >
          <span className="sr-only">Open menu</span>
          <MoreHorizontal className="size-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent
        className="data-[state=closed]:duration-0"
        align="end"
        hidden={hasOpenDialog}
        onCloseAutoFocus={(event) => {
          if (focusRef.current) {
            focusRef.current.focus()
            focusRef.current = null
            event.preventDefault()
          }
        }}
      >
        <DropdownMenuLabel>Actions</DropdownMenuLabel>

        <UpdateCategoryDialog
          category={category}
          onSelect={handleDialogItemSelect}
          onOpenChange={handleDialogItemOpenChange}
        />
        <ArchiveCategoryDialog
          category={category}
          onSelect={handleDialogItemSelect}
          onOpenChange={handleDialogItemOpenChange}
        />
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
