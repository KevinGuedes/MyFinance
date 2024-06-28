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

import { AccountTagForm, AccountTagFormSchema } from './account-tag-form'

export function CreateAccountTagDialog() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: AccountTagFormSchema) {
    console.log(values)
  }

  function onCancel() {
    setIsDialogOpen(false)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
      <DialogTrigger asChild>
        <Button variant="outline" size="sm">
          <PlusCircle className="mr-2 size-5" />
          Create Account Tag
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Create Account Tag</DialogTitle>
          <DialogDescription>
            Fill in the form below to create a new Account Tag.
          </DialogDescription>
        </DialogHeader>
        <AccountTagForm
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
