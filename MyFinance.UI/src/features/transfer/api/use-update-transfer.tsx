import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'
import { transferApi } from '@/features/common/api'
import { ApiError } from '@/features/common/api-error'
import { handleError } from '@/features/common/handle-error'
import { handleValidationErrors } from '@/features/common/handle-validation-errors'
import { getEnumValueByKey } from '@/lib/utils'

import { Transfer } from '../models/transfer'
import { TransferType } from '../models/transfer-type'

type UpdateTransferRequest = Omit<
  Transfer,
  'categoryName' | 'tag' | 'settlementDate' | 'type'
> & {
  settlementDate: Date
  managementUnitId: string
  type: string
}

export function useUpdateTransfer() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<Transfer, ApiError, UpdateTransferRequest>({
    mutationFn: async (updateTransferRequest) => {
      const { data: updatedTransfer } = await transferApi.put<Transfer>('', {
        ...updateTransferRequest,
        type: getEnumValueByKey(TransferType, updateTransferRequest.type),
        settlementDate: updateTransferRequest.settlementDate.toISOString(),
      })

      return updatedTransfer
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
        title: 'Transfer successfully updated!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to update Transfer',
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
