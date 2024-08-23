import { useParams } from '@tanstack/react-router'
import { Pencil } from 'lucide-react'
import { useState } from 'react'

import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'

import { useUpdateTransfer } from '../../api/use-update-transfer'
import { Transfer } from '../../models/transfer'
import { TransferForm, TransferFormSchema } from '../transfer-form'

type UpdateTransferDialogProps = {
  transfer: Transfer
}

export function UpdateTransferDialog({ transfer }: UpdateTransferDialogProps) {
  const updateCategoryMutation = useUpdateTransfer()
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const { managementUnitId } = useParams({ strict: false })

  async function onSubmit(values: TransferFormSchema) {
    await updateCategoryMutation.mutateAsync(
      {
        ...values,
        id: transfer.id,
        managementUnitId: managementUnitId!,
        type: values.type!,
      },
      {
        onSuccess: () => {
          setIsDialogOpen(false)
        },
      },
    )
  }

  function onCancel() {
    setIsDialogOpen(false)
  }

  function handleOnOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={handleOnOpenChange}>
      <DialogTrigger asChild>
        <Button
          variant="ghost"
          className="size-7 shrink-0 rounded-full p-0.5"
          size="icon"
        >
          <span className="sr-only">Edit Transfer</span>
          <Pencil className="size-4 stroke-2" />
        </Button>
      </DialogTrigger>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Update Transfer</DialogTitle>
          <DialogDescription>
            Change the information in the form below to update the Transfer.
          </DialogDescription>
        </DialogHeader>
        <TransferForm
          mode="update"
          onSubmit={onSubmit}
          onCancel={onCancel}
          managementUnitId={managementUnitId!}
          defaultValues={{
            value: transfer.value,
            relatedTo: transfer.relatedTo,
            description: transfer.description,
            settlementDate: transfer.settlementDate,
            categoryId: transfer.categoryId,
            accountTagId: transfer.accountTagId,
            type: transfer.type,
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
