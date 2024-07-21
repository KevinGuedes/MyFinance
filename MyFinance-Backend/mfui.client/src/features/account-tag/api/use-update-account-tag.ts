import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { accountTagApi } from '../../common/api'
import { ApiError } from '../../common/api-error'
import { handleError } from '../../common/handle-error'
import { handleValidationErrors } from '../../common/handle-validation-errors'
import { AccountTag } from '../models/account-tag'

type UpdateAccountTagRequest = AccountTag

export function useUpdateAccountTag() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<AccountTag, ApiError, UpdateAccountTagRequest>({
    mutationFn: async (updateAccountTagRequest) => {
      const { data: updatedAccountTag } = await accountTagApi.put<AccountTag>(
        '',
        updateAccountTagRequest,
      )

      return updatedAccountTag
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['account-tags'] })

      toast({
        title: 'Account Tag successfully updated!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to update Account Tag',
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
