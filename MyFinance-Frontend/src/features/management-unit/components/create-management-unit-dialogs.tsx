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

import { useCreateManagementUnit } from '../api/use-create-management-unit'
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
        <Button variant="default" className="w-full">
          <PlusCircle className="mr-2 size-5" />
          Create Management Unit
          <span className="sr-only">Create Management Unit</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Create Management Unit</DialogTitle>
          <DialogDescription>
            Fill in the form below to create a new Management Unit.
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
