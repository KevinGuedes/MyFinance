import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'

import { TransferForm } from './transfer-form'

export function RegisterTransferDialog() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">Register Transfer</Button>
      </DialogTrigger>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Register Transfer</DialogTitle>
          <DialogDescription>
            Fill in the form below to register a new transfer.
          </DialogDescription>
        </DialogHeader>
        <TransferForm />
      </DialogContent>
    </Dialog>
  )
}
