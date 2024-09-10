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

import { useCreateAccountTag } from '../api/use-create-account-tag'
import { AccountTagForm, AccountTagFormSchema } from './account-tag-form'

type CreateAccountTagDialogProps = {
  managementUnitId: string
}

export function CreateAccountTagDialog({
  managementUnitId,
}: CreateAccountTagDialogProps) {
  const createAccountTagMutation = useCreateAccountTag()
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: AccountTagFormSchema) {
    await createAccountTagMutation.mutateAsync(
      { ...values, managementUnitId },
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
      <DialogTrigger asChild>
        <Button
          variant="outline"
          icon={PlusCircle}
          label="Create Account Tag"
        />
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Create Account Tag</DialogTitle>
          <DialogDescription>
            Fill in the form below to create a new Account Tag.
          </DialogDescription>
        </DialogHeader>
        <AccountTagForm
          mode="create"
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            tag: '',
            description: '',
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
