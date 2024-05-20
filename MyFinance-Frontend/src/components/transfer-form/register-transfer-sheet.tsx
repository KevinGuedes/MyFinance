import { Button } from '../ui/button'
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from '../ui/sheet'
import { TransferForm } from './transfer-form'

export function RegisterTransferSheet() {
  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="outline">Register Transfer</Button>
      </SheetTrigger>
      <SheetContent>
        <SheetHeader>
          <SheetTitle>Register Transfer</SheetTitle>
          <SheetDescription>
            Fill in the form below to register a new transfer.
          </SheetDescription>
        </SheetHeader>
        <TransferForm />
      </SheetContent>
    </Sheet>
  )
}
