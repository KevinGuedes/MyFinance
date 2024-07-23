import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'
import { transferApi } from '@/features/common/api'
import { ApiError } from '@/features/common/api-error'
import { handleError } from '@/features/common/handle-error'
import { handleValidationErrors } from '@/features/common/handle-validation-errors'

import { Transfer } from '../models/transfer'

type RegisterTransferRequest = Pick<
  Transfer,
  'value' | 'relatedTo' | 'description' | 'settlementDate' | 'type'
> & {
  managementUnitId: string
  accountTagId: string
  categoryId: string
}

export function useRegisterTransfer() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<Transfer, ApiError, RegisterTransferRequest>({
    mutationFn: async (registerTransferRequest) => {
      const { data: createdTransfer } = await transferApi.post<Transfer>(
        '',
        registerTransferRequest,
      )

      return createdTransfer
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
        title: 'Transfer successfully created!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to register Transfer',
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
