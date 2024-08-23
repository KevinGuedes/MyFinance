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
import { Button } from '@/components/ui/button'

import { useDeleteTransfer } from '../../api/use-delete-transfer'

type DeleteTransferAlertProps = {
  transferId: string
}

export function DeleteTransferAlert({ transferId }: DeleteTransferAlertProps) {
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
        },
        onSettled: () => {
          setIsDeleting(false)
        },
      },
    )
  }

  function handleCancel() {
    setIsDialogOpen(false)
  }

  function handleOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
  }

  return (
    <AlertDialog open={isDialogOpen} onOpenChange={handleOpenChange}>
      <AlertDialogTrigger asChild>
        <Button
          variant="ghost"
          className="size-7 rounded-full p-0.5 hover:bg-destructive/25"
          size="icon"
        >
          <span className="sr-only">Delete Transfer</span>
          <Trash2 className="size-4 stroke-2 text-destructive" />
        </Button>
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
