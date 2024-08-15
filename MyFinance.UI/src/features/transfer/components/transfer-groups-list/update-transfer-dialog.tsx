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
import { getEnumValueByKey, getKeyByEnumValue } from '@/lib/utils'

import { useUpdateTransfer } from '../../api/use-update-transfer'
import { Transfer } from '../../models/transfer'
import { TransferType } from '../../models/transfer-type'
import { TransferForm, TransferFormSchema } from '../transfer-form'

type UpdateTransferDialogProps = {
  transfer: Transfer
  onSelect: () => void
  onOpenChange: (isOpen: boolean) => void
}

export function UpdateTransferDialog({
  transfer,
  onSelect,
  onOpenChange,
}: UpdateTransferDialogProps) {
  const updateCategoryMutation = useUpdateTransfer()
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const { managementUnitId } = useParams({ strict: false })

  async function onSubmit(values: TransferFormSchema) {
    await updateCategoryMutation.mutateAsync(
      {
        ...formatValues(values),
        managementUnitId: managementUnitId!,
        id: transfer.id,
        settlementDate: transfer.settlementDate,
      },
      {
        onSuccess: () => {
          setIsDialogOpen(false)
          onOpenChange(false)
        },
      },
    )
  }

  function onCancel() {
    setIsDialogOpen(false)
    onOpenChange(false)
  }

  function handleOnOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
    onOpenChange(isDialogOpen)
  }

  function formatValues(values: TransferFormSchema) {
    const type = getEnumValueByKey(TransferType, values.type)
    const settlementDate = values.settlementDate?.toISOString()

    return {
      ...values,
      type,
      settlementDate,
    }
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={handleOnOpenChange}>
      <DialogTrigger asChild>
        <DropdownMenuItem
          onSelect={(event) => {
            event.preventDefault()
            onSelect()
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
