import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'

import { ManagementUnitForm } from './management-unit-form'

export function CreateManagementUnitDialog() {
  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="outline">Create Management Unit</Button>
      </DialogTrigger>
      <DialogContent
        className="sm:max-w-md"
        onOpenAutoFocus={(e) => e.preventDefault()}
      >
        <DialogHeader>
          <DialogTitle>Create Management Unit</DialogTitle>
          <DialogDescription>
            Fill in the form below to create a new management unit.
          </DialogDescription>
        </DialogHeader>
        <ManagementUnitForm />
      </DialogContent>
    </Dialog>
  )
}
