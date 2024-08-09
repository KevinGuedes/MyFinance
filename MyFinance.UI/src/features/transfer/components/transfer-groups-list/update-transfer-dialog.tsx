import { useParams } from '@tanstack/react-router'
import { Pencil } from 'lucide-react'
import { useState } from 'react'

import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { DropdownMenuItem } from '@/components/ui/dropdown-menu'
import { getKeyByEnumValue } from '@/lib/utils'

import { Transfer } from '../../models/transfer'
import { TransferType } from '../../models/transfer-type'
import { TransferForm, TransferFormSchema } from '../transfer-form'

type UpdateTransferDialogProps = {
  transfer: Transfer
  onSelect?: (event?: Event) => void
  onOpenChange?: (isOpen: boolean) => void
}

export function UpdateTransferDialog({
  transfer,
  onSelect,
  onOpenChange,
}: UpdateTransferDialogProps) {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const { managementUnitId } = useParams({ strict: false })

  async function onSubmit(values: TransferFormSchema) {
    console.log(values)
    // await updateCategoryMutation.mutateAsync(
    //   { ...values, id: category.id },
    //   {
    //     onSuccess: () => {
    //       setIsDialogOpen(false)
    //       onOpenChange?.(false)
    //     },
    //   },
    // )
  }

  function onCancel() {
    setIsDialogOpen(false)
    onOpenChange?.(false)
  }

  function handleOnOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
    onOpenChange?.(isDialogOpen)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={handleOnOpenChange}>
      <DialogTrigger asChild>
        <DropdownMenuItem
          onSelect={(event) => {
            event.preventDefault()
            onSelect && onSelect()
          }}
        >
          <Pencil className="mr-2 size-5" />
          Edit Transfer
        </DropdownMenuItem>
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
            type: getKeyByEnumValue(TransferType, transfer.type),
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
