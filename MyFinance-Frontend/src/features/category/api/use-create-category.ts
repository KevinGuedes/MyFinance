import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { categoryApi } from '../../common/api'
import { ApiError } from '../../common/api-error'
import { handleError } from '../../common/handle-error'
import { handleValidationErrors } from '../../common/handle-validation-errors'
import { Category } from '../models/category'

type CreateCategoryRequest = Pick<Category, 'name'> & {
  managementUnitId: string
}

export function useCreateCategory() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<Category, ApiError, CreateCategoryRequest>({
    mutationFn: async (createCategoryRequest) => {
      const { data: createdCategory } = await categoryApi.post<Category>(
        '',
        createCategoryRequest,
      )

      return createdCategory
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['categories'] })
      toast({
        title: 'Category successfully created!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to create Category',
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
