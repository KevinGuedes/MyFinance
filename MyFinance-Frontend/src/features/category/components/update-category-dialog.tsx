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

import { useUpdateCategory } from '../api/use-update-category'
import { Category } from '../models/category'
import { CategoryForm, CategoryFormSchema } from './category-form'

type UpdateCategoryDialogProps = {
  category: Category
}

export function UpdateCategoryDialog({ category }: UpdateCategoryDialogProps) {
  const updateCategoryMutation = useUpdateCategory()
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: CategoryFormSchema) {
    await updateCategoryMutation.mutateAsync(
      { ...values, id: category.id },
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
          Edit Category
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
        <DialogHeader>
          <DialogTitle>Update Category</DialogTitle>
          <DialogDescription>
            Fill in the form below to update the Category.
          </DialogDescription>
        </DialogHeader>
        <CategoryForm
          mode="update"
          onSubmit={onSubmit}
          onCancel={onCancel}
          defaultValues={{
            name: category.name,
          }}
        />
      </DialogContent>
    </Dialog>
  )
}
