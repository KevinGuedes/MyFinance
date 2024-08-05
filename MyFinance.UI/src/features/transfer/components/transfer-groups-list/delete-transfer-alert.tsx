import { Trash2 } from 'lucide-react'
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

import { Transfer } from '../../models/transfer'

type DeleteTransferAlertProps = {
  transfer: Transfer
  onSelect?: (event?: Event) => void
  onOpenChange?: (isOpen: boolean) => void
}

export function DeleteTransferAlert({
  onSelect,
  onOpenChange,
}: DeleteTransferAlertProps) {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function handleDelete() {
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
          <Trash2 className="mr-2 size-5 text-destructive" />
          Delete Transfer
        </DropdownMenuItem>
      </AlertDialogTrigger>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>
            Do you want to delete this <strong>Transfer</strong>?
          </AlertDialogTitle>
          <AlertDialogDescription>
            This action cannot be undone. This will permanently delete your
            transfer and the <strong>Management Unit</strong> balance data will
            also be updated.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel onClick={handleCancel}>Cancel</AlertDialogCancel>
          <AlertDialogAction onClick={handleDelete}>Delete</AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
