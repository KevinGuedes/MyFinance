import { useMutation } from '@tanstack/react-query'

import { useToast } from '@/components/ui/toast/use-toast'

import { userApi } from '../api'
import { ApiError } from '../common/api-error'
import { handleApiError } from '../common/handle-api-error'
import { handleValidationErrors } from '../common/handle-validation-errors'
import { queryClient } from '../query-client/query-client'

type SignInRequest = {
  email: string
  plainTextPassword: string
}

export type SignInResponse = {
  shouldUpdatePassword: boolean
}

type TooManyFailedSignInAttemptsResponse = {
  lockoutEndOnUtc: Date
}

export const useSignIn = () => {
  const { toast } = useToast()

  const mutation = useMutation<
    SignInResponse,
    ApiError<TooManyFailedSignInAttemptsResponse>,
    SignInRequest
  >({
    mutationFn: async (signInRequest) => {
      const { data: signInResponse } = await userApi.post<SignInResponse>(
        '/signin',
        signInRequest,
      )

      return signInResponse
    },
    onSuccess: () => {
      queryClient.clear()
    },
    onError: (error) => {
      const {
        description,
        validationErrors,
        isBadRequest,
        statusCode,
        response,
        title,
      } = handleApiError(error)

      if (isBadRequest) {
        handleValidationErrors(validationErrors, (_, description) => {
          toast({
            variant: 'destructive',
            title: 'Failed to create Management Unit',
            description,
          })
        })
      } else {
        if (statusCode === 429) {
          const lockoutEndOnUtc = response?.lockoutEndOnUtc

          toast({
            variant: 'destructive',
            title: 'Too many failed sign in attempts',
            description: `Try again after ${new Date(lockoutEndOnUtc!).toLocaleString()}`,
          })
        } else {
          toast({
            variant: 'destructive',
            title,
            description,
          })
        }
      }
    },
  })

  return mutation
}
