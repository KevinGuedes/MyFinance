import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { categoryApi } from '../../common/api'
import { ApiError } from '../../common/api-error'
import { handleError } from '../../common/handle-error'
import { handleValidationErrors } from '../../common/handle-validation-errors'
import { Category } from '../models/category'

type UpdateCategoryRequest = Category

export function useUpdateCategory() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<Category, ApiError, UpdateCategoryRequest>({
    mutationFn: async (updateCategoryRequest) => {
      const { data: updatedCategory } = await categoryApi.put<Category>(
        '',
        updateCategoryRequest,
      )

      return updatedCategory
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['categories'] })

      toast({
        title: 'Category successfully updated!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to update Category',
            description,
          })
        })
      } else {
        toast({
          variant: 'destructive',
          title: 'Uh oh! Something went wrong!',
          description,
        })
      }
    },
  })

  return mutation
}
