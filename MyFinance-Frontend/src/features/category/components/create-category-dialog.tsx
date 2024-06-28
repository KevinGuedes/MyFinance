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

import { CategoryForm, CategoryFormSchema } from './category-form'

export function CreateCategoryDialog() {
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: CategoryFormSchema) {
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
          Create Category
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Create Category</DialogTitle>
          <DialogDescription>
            Fill in the form below to create a new Category.
          </DialogDescription>
        </DialogHeader>
        <CategoryForm
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            name: '',
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
