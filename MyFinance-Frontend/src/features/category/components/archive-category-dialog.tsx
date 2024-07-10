import { Archive } from 'lucide-react'
import { useState } from 'react'

import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from '@/components/ui/alert-dialog'
import { DropdownMenuItem } from '@/components/ui/dropdown-menu'

import { Category } from '../models/category'

type ArchiveCategoryDialogProps = {
  category: Category
  onSelect?: (event?: Event) => void
  onOpenChange?: (isOpen: boolean) => void
}

export function ArchiveCategoryDialog({
  category,
  onSelect,
  onOpenChange,
}: ArchiveCategoryDialogProps) {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function handleArchive() {
    console.log('in')
  }

  function handleCancel() {
    setIsDialogOpen(false)
    onOpenChange?.(false)
  }

  function handleOnOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
    onOpenChange?.(isDialogOpen)
  }

  return (
    <AlertDialog open={isDialogOpen} onOpenChange={handleOnOpenChange}>
      <AlertDialogTrigger asChild>
        <DropdownMenuItem
          onSelect={(event) => {
            event.preventDefault()
            onSelect && onSelect()
          }}
        >
          <Archive className="mr-2 size-5 text-destructive" />
          Archive Category
        </DropdownMenuItem>
      </AlertDialogTrigger>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>
            Do you want to archive the Category {category.name}?
          </AlertDialogTitle>
          <AlertDialogDescription>
            This action cannot be undone. This will permanently delete your
            account and remove your data from our servers.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel onClick={handleCancel}>Cancel</AlertDialogCancel>
          <AlertDialogAction onClick={handleArchive}>Archive</AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
