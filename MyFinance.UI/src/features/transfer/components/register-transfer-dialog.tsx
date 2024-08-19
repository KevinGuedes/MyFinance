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
    await registerTransferMutation.mutateAsync(
      {
        ...values,
        managementUnitId: managementUnitId!,
        type: values.type!,
      },
      {
        onSuccess: () => {
          setIsDialogOpen(!shouldCloseDialog)
        },
      },
    )
  }

  function onCancel() {
    setIsDialogOpen(false)
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
            type: undefined,
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
