import { useParams } from '@tanstack/react-router'
import { Loader2, Trash2 } from 'lucide-react'
import { MouseEvent, useState } from 'react'

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

import { useDeleteTransfer } from '../../api/use-delete-transfer'

type DeleteTransferAlertProps = {
  transferId: string
  onSelect: (event?: Event) => void
  onOpenChange: (isOpen: boolean) => void
}

export function DeleteTransferAlert({
  onSelect,
  onOpenChange,
  transferId,
}: DeleteTransferAlertProps) {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const deleteTransferMutation = useDeleteTransfer()
  const { managementUnitId } = useParams({ strict: false })
  const [isDeleting, setIsDeleting] = useState(false)

  async function handleDelete(event: MouseEvent<HTMLButtonElement>) {
    event.preventDefault()

    setIsDeleting(true)
    await deleteTransferMutation.mutateAsync(
      {
        managementUnitId: managementUnitId!,
        id: transferId,
      },
      {
        onSuccess: () => {
          setIsDialogOpen(false)
          onOpenChange(false)
        },
        onSettled: () => {
          setIsDeleting(false)
        },
      },
    )
  }

  function handleCancel() {
    setIsDialogOpen(false)
    onOpenChange(false)
  }

  function handleOnOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
    onOpenChange(isDialogOpen)
  }

  return (
    <AlertDialog open={isDialogOpen} onOpenChange={handleOnOpenChange}>
      <AlertDialogTrigger asChild>
        <DropdownMenuItem
          onSelect={(event) => {
            event.preventDefault()
            onSelect()
          }}
        >
          <Trash2 className="mr-2 size-5 text-destructive" />
          Delete Transfer
        </DropdownMenuItem>
      </AlertDialogTrigger>
      <AlertDialogContent>
        <AlertDialogHeader>
          <AlertDialogTitle>
            Do you want to delete this{' '}
            <strong className="font-medium">Transfer</strong>?
          </AlertDialogTitle>
          <AlertDialogDescription>
            This action cannot be undone. This will permanently delete your{' '}
            <strong className="font-medium">Transfer</strong> and the{' '}
            <strong className="font-medium">Management Unit</strong> balance
            data will also be updated.
          </AlertDialogDescription>
        </AlertDialogHeader>
        <AlertDialogFooter>
          <AlertDialogCancel onClick={handleCancel}>Cancel</AlertDialogCancel>
          <AlertDialogAction
            onClick={(e) => handleDelete(e)}
            variant="destructive"
            className="min-w-[11.25rem]"
          >
            {isDeleting ? (
              <>
                <Loader2 className="mr-2 size-5 animate-spin" />
                Deleting Transfer...
              </>
            ) : (
              <>
                <Trash2 className="mr-2 size-5" />
                Delete Transfer
              </>
            )}
          </AlertDialogAction>
        </AlertDialogFooter>
      </AlertDialogContent>
    </AlertDialog>
  )
}
