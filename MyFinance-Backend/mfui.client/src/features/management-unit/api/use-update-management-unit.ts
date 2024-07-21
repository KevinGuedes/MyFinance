import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { managementUnitApi } from '../../common/api'
import { ApiError } from '../../common/api-error'
import { handleError } from '../../common/handle-error'
import { handleValidationErrors } from '../../common/handle-validation-errors'
import { ManagementUnit } from '../models/management-unit'

type UpdateManagementUnitRequest = Pick<
  ManagementUnit,
  'id' | 'name' | 'description'
>

export function useUpdateManagementUnit() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<
    ManagementUnit,
    ApiError,
    UpdateManagementUnitRequest
  >({
    mutationFn: async (updateManagementUnitRequest) => {
      const { data: updatedManagementUnit } =
        await managementUnitApi.put<ManagementUnit>(
          '',
          updateManagementUnitRequest,
        )

      return updatedManagementUnit
    },
    onSuccess: (updatedManagementUnit) => {
      queryClient.invalidateQueries({ queryKey: ['management-units'] })
      queryClient.setQueryData(
        ['management-unit', { managementUnitId: updatedManagementUnit.id }],
        updatedManagementUnit,
      )

      toast({
        title: 'Management Unit successfully updated!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to update Management Unit',
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
