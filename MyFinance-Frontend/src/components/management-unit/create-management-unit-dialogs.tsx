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
import { useCreateManagementUnit } from '@/http/management-units/use-create-management-unit'

import {
  ManagementUnitForm,
  ManagementUnitFormSchema,
} from './management-unit-form'

export function CreateManagementUnitDialog() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const mutation = useCreateManagementUnit()

  async function onSubmit(values: ManagementUnitFormSchema) {
    await mutation.mutateAsync(values, {
      onSuccess: () => {
        setIsDialogOpen(false)
      },
    })
  }

  function onCancel() {
    setIsDialogOpen(false)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
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
        <ManagementUnitForm
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            name: '',
            description: '',
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
