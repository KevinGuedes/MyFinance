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
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip'

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
      <Tooltip>
        <TooltipTrigger asChild>
          <DialogTrigger asChild>
            <Button variant="default" size="icon">
              <PlusCircle className="size-5" />
              <span className="sr-only">Create Management Unit</span>
            </Button>
          </DialogTrigger>
        </TooltipTrigger>
        <TooltipContent>Create Management Unit</TooltipContent>
      </Tooltip>
      <DialogContent
        className="sm:max-w-md"
        onOpenAutoFocus={(e) => e.preventDefault()}
      >
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
