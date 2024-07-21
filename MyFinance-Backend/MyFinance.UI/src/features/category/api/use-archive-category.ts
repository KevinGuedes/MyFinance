import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'
import { categoryApi } from '@/features/common/api'
import { ApiError } from '@/features/common/api-error'
import { handleError } from '@/features/common/handle-error'
import { handleValidationErrors } from '@/features/common/handle-validation-errors'

import { Category } from '../models/category'

type ArchiveCategoryRequest = Pick<Category, 'id' | 'name'> & {
  reasonToArchive: string
}

export function useArchiveCategory() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<void, ApiError, ArchiveCategoryRequest>({
    mutationFn: async ({ id }) => {
      await categoryApi.delete<void>(`/${id}`)
    },
    onSuccess: (_, { id, name }) => {
      queryClient.setQueryData<Category[] | undefined>(
        ['categories'],
        (categories) => categories?.filter((category) => category.id !== id),
      )

      toast({
        title: `Category ${name} successfully archived!`,
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
