import { useParams } from '@tanstack/react-router'
import { PlusCircle } from 'lucide-react'
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
import { TransferType } from '@/features/transfer/models/transfer-type'
import { getEnumValueByKey } from '@/lib/utils'

import { useRegisterTransfer } from '../api/use-register-transfer'
import { TransferForm, TransferFormSchema } from './transfer-form'

export function RegisterTransferDialog() {
  const registerTransferMutation = useRegisterTransfer()
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const { managementUnitId } = useParams({ strict: false })

  async function onSubmit(
    values: TransferFormSchema,
    shouldCloseDialog: boolean = true,
  ) {
    await registerTransferMutation.mutateAsync({
      ...formatValues(values),
      managementUnitId: managementUnitId!,
    })

    setIsDialogOpen(!shouldCloseDialog)
  }

  function onCancel() {
    setIsDialogOpen(false)
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
    <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" className="w-full">
          <PlusCircle className="mr-2 size-5" />
          Register Transfer
        </Button>
      </DialogTrigger>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Register Transfer</DialogTitle>
          <DialogDescription>
            Fill in the form below to register a new transfer.
          </DialogDescription>
        </DialogHeader>
        <TransferForm
          mode="register"
          onSubmit={onSubmit}
          onCancel={onCancel}
          managementUnitId={managementUnitId!}
          defaultValues={{
            value: 0,
            relatedTo: '',
            description: '',
            settlementDate: new Date(),
            categoryId: '',
            accountTagId: '',
            type: '',
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
