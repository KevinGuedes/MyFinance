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

import { TransferForm, TransferFormSchema } from './transfer-form'

export function RegisterTransferDialog() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(
    values: TransferFormSchema,
    shouldCloseDialog: boolean = true,
  ) {
    console.log(formatValues(values))
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
        <Button variant="outline" size="sm">
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
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            value: 0,
            relatedTo: '',
            description: '',
            settlementDate: undefined,
            categoryId: '',
            accountTagId: '',
            type: '',
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
