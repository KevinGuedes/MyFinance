import { Pencil } from 'lucide-react'
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

import { useUpdateManagementUnit } from '../api/use-update-management-unit'
import { ManagementUnit } from '../models/management-unit'
import {
  ManagementUnitForm,
  ManagementUnitFormSchema,
} from './management-unit-form'

type UpdateManagementUnitDialogProps = {
  managementUnit: ManagementUnit
}

export function UpdateManagementUnitDialog({
  managementUnit,
}: UpdateManagementUnitDialogProps) {
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const mutation = useUpdateManagementUnit()

  async function onSubmit(values: ManagementUnitFormSchema) {
    await mutation.mutateAsync(
      {
        id: managementUnit.id,
        name: values.name,
        description: values.description,
      },
      {
        onSuccess: () => {
          setIsDialogOpen(false)
        },
      },
    )
  }

  function onCancel() {
    setIsDialogOpen(false)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
      <Tooltip>
        <DialogTrigger asChild>
          <TooltipTrigger asChild>
            <Button
              variant="outline"
              size="icon"
              className="size-9 rounded-full border-none"
              icon={Pencil}
              screenReaderLabel="Update Management Unit"
            />
          </TooltipTrigger>
        </DialogTrigger>
        <TooltipContent side="bottom" align="end">
          Edit Management Unit
        </TooltipContent>
      </Tooltip>

      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Update Management Unit</DialogTitle>
          <DialogDescription>
            Update the data in the form below to update the Management Unit.
          </DialogDescription>
        </DialogHeader>
        <ManagementUnitForm
          mode="update"
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            name: managementUnit.name,
            description: managementUnit.description,
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
