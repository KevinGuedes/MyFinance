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

import { useCreateCategory } from '../api/use-create-category'
import { CategoryForm, CategoryFormSchema } from './category-form'

type CreateCategoryDialogProps = {
  managementUnitId: string
}

export function CreateCategoryDialog({
  managementUnitId,
}: CreateCategoryDialogProps) {
  const createCategoryMutation = useCreateCategory()
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: CategoryFormSchema) {
    await createCategoryMutation.mutateAsync(
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
