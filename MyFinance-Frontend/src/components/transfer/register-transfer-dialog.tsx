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
import { getEnumValueByKey } from '@/lib/utils'
import { TransferType } from '@/models/enums/transfer-type'

import { TransferForm, TransferFormSchema } from './transfer-form'

export function RegisterTransferDialog() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: TransferFormSchema) {
    console.log(formatValues(values))
  }

  async function onRegisterAndAddMore(values: TransferFormSchema) {
    console.log(formatValues(values))
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
        <Button variant="outline">Register Transfer</Button>
      </DialogTrigger>
      <DialogContent
        className="max-w-3xl"
        onOpenAutoFocus={(e) => e.preventDefault()}
      >
        <DialogHeader>
          <DialogTitle>Register Transfer</DialogTitle>
          <DialogDescription>
            Fill in the form below to register a new transfer.
          </DialogDescription>
        </DialogHeader>
        <TransferForm
          onSubmit={onSubmit}
          onRegisterAndAddMore={onRegisterAndAddMore}
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
