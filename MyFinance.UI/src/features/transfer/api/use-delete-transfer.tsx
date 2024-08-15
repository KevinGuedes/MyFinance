import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'
import { transferApi } from '@/features/common/api'
import { ApiError } from '@/features/common/api-error'
import { handleError } from '@/features/common/handle-error'
import { handleValidationErrors } from '@/features/common/handle-validation-errors'

type DeleteTransferRequest = {
  managementUnitId: string
  id: string
}

export function useDeleteTransfer() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<void, ApiError, DeleteTransferRequest>({
    mutationFn: async (deleteTransferRequest) => {
      await new Promise((resolve) => setTimeout(resolve, 1000))
      await transferApi.delete(deleteTransferRequest.id)
    },
    onSuccess: (_, { managementUnitId }) => {
      queryClient.invalidateQueries({
        queryKey: ['transfers', { managementUnitId }],
      })

      queryClient.invalidateQueries({
        queryKey: ['discriminated-balance', { managementUnitId }],
      })

      queryClient.invalidateQueries({
        queryKey: ['management-unit', { managementUnitId }],
      })

      toast({
        title: 'Transfer successfully deleted!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to delete Transfer',
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
