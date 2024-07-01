import { useMutation, useQueryClient } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { accountTagApi } from '../../common/api'
import { ApiError } from '../../common/api-error'
import { handleError } from '../../common/handle-error'
import { handleValidationErrors } from '../../common/handle-validation-errors'
import { AccountTag } from '../models/account-tag'

type useCreateAccountTagRequest = Pick<AccountTag, 'tag' | 'description'> & {
  managementUnitId: string
}

export function useCreateAccountTag() {
  const queryClient = useQueryClient()
  const { toast } = useToast()

  const mutation = useMutation<
    AccountTag,
    ApiError,
    useCreateAccountTagRequest
  >({
    mutationFn: async (createAccountTagRequest) => {
      const { data: createdAccountTag } = await accountTagApi.post<AccountTag>(
        '',
        createAccountTagRequest,
      )

      return createdAccountTag
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['account-tags'] })
      toast({
        title: 'Account Tag successfully created!',
      })
    },
    onError: (error) => {
      const { description, validationErrors, isBadRequest } = handleError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to create Account Tag',
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
