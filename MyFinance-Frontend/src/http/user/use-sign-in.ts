import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useRouter } from '@tanstack/react-router'

import { useToast } from '@/components/ui/toast/use-toast'
import { useUserStore } from '@/stores/user-store'

import { userApi } from '../api'
import { ApiError } from '../common/api-error'
import { handleError } from '../common/handle-error'
import { handleValidationErrors } from '../common/handle-validation-errors'

type SignInPayload = {
  redirectTo?: string
  signInRequest: {
    email: string
    plainTextPassword: string
  }
}

type UserResponse = {
  shouldUpdatePassword: boolean
  name: string
}

type TooManyFailedSignInAttemptsResponse = {
  lockoutEndOnUtc: Date
}

export const useSignIn = () => {
  const setUserInfo = useUserStore((state) => state.setUserInfo)
  const queryClient = useQueryClient()
  const router = useRouter()
  const { toast } = useToast()

  const mutation = useMutation<
    UserResponse,
    ApiError<TooManyFailedSignInAttemptsResponse>,
    SignInPayload
  >({
    mutationFn: async ({ signInRequest }) => {
      const { data: userResponse } = await userApi.post<UserResponse>(
        '/signin',
        signInRequest,
      )

      return userResponse
    },
    onSuccess: (userResponse, { redirectTo }) => {
      setUserInfo(userResponse)
      queryClient.clear()
      router.navigate({ to: redirectTo ?? '/' })
    },
    onError: (error) => {
      const {
        description,
        validationErrors,
        isBadRequest,
        statusCode,
        response,
        title,
      } = handleError(error)

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
