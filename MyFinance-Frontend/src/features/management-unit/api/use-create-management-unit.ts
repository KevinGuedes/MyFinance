import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { managementUnitApi } from '../../common/api'
import { ApiError } from '../../common/api-error'
import { handleError } from '../../common/handle-error'
import { handleValidationErrors } from '../../common/handle-validation-errors'

type CreateManagementUnitResponse = {
  id: string
  name: string
  income: number
  outcome: number
  balance: number
  description?: string
}

type CreateManagementUnitRequest = Pick<
  CreateManagementUnitResponse,
  'name' | 'description'
>

export function useCreateManagementUnit() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<
    CreateManagementUnitResponse,
    ApiError,
    CreateManagementUnitRequest
  >({
    mutationFn: async (createManagementUnitRequest) => {
      const { data: updatedManagementUnit } =
        await managementUnitApi.post<CreateManagementUnitResponse>(
          '',
          createManagementUnitRequest,
        )

      return updatedManagementUnit
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['management-units'] })
      toast({
        title: 'Management Unit successfully created!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to create Management Unit',
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
