import { Pencil } from 'lucide-react'
import { useState } from 'react'

import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { DropdownMenuItem } from '@/components/ui/dropdown-menu'

import { useUpdateCategory } from '../api/use-update-category'
import { Category } from '../models/category'
import { CategoryForm, CategoryFormSchema } from './category-form'

type UpdateCategoryDialogProps = {
  category: Category
  onSelect?: (event?: Event) => void
  onOpenChange?: (isOpen: boolean) => void
}

export function UpdateCategoryDialog({
  category,
  onSelect,
  onOpenChange,
}: UpdateCategoryDialogProps) {
  const updateCategoryMutation = useUpdateCategory()
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: CategoryFormSchema) {
    await updateCategoryMutation.mutateAsync(
      { ...values, id: category.id },
      {
        onSuccess: () => {
          setIsDialogOpen(false)
          onOpenChange?.(false)
        },
      },
    )
  }

  function onCancel() {
    setIsDialogOpen(false)
    onOpenChange?.(false)
  }

  function handleOnOpenChange(isDialogOpen: boolean) {
    setIsDialogOpen(isDialogOpen)
    onOpenChange?.(isDialogOpen)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={handleOnOpenChange}>
      <DialogTrigger asChild>
        <DropdownMenuItem
          onSelect={(event) => {
            event.preventDefault()
            onSelect && onSelect()
          }}
        >
          <Pencil className="mr-2 size-5" />
          Edit Category
        </DropdownMenuItem>
      </DialogTrigger>
      <DialogContent className="sm:max-w-lg">
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
