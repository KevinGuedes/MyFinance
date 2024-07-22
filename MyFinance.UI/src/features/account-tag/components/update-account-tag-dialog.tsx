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

import { useUpdateAccountTag } from '../api/use-update-account-tag'
import { AccountTag } from '../models/account-tag'
import { AccountTagForm, AccountTagFormSchema } from './account-tag-form'

type UpdateAccountTagDialogProps = {
  accountTag: AccountTag
}

export function UpdateAccountTagDialog({
  accountTag,
}: UpdateAccountTagDialogProps) {
  const updateAccountTagMutation = useUpdateAccountTag()
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: AccountTagFormSchema) {
    await updateAccountTagMutation.mutateAsync(
      { ...values, id: accountTag.id },
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
        <Button variant="outline" size="sm">
          <Pencil className="mr-2 size-5" />
          Edit Account Tag
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Update Account Tag</DialogTitle>
          <DialogDescription>
            Fill in the form below to update the Account Tag.
          </DialogDescription>
        </DialogHeader>
        <AccountTagForm
          mode="update"
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            tag: accountTag.tag,
            description: accountTag.description,
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
