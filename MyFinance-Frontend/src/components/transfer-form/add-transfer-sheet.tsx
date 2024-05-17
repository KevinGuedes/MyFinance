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

export function AddTransferSheet() {
  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="outline">Add Transfer</Button>
      </SheetTrigger>
      <SheetContent>
        <SheetHeader>
          <SheetTitle>Add Transfer</SheetTitle>
          <SheetDescription>
            Fill in the form below to add a new transfer.
          </SheetDescription>
        </SheetHeader>
        <TransferForm />
      </SheetContent>
    </Sheet>
  )
}
