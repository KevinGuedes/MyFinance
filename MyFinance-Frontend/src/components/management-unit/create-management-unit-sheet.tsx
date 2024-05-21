import { Button } from '../ui/button'
import {
  Sheet,
  SheetContent,
  SheetDescription,
  SheetHeader,
  SheetTitle,
  SheetTrigger,
} from '../ui/sheet'
import { ManagementUnitForm } from './management-unit-form'

export function CreateManagementUnitSheet() {
  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button variant="outline">Create Management Unit</Button>
      </SheetTrigger>
      <SheetContent>
        <SheetHeader>
          <SheetTitle>Create Management Unit</SheetTitle>
          <SheetDescription>
            Fill in the form below to create a new management unit.
          </SheetDescription>
        </SheetHeader>
        <ManagementUnitForm />
      </SheetContent>
    </Sheet>
  )
}
