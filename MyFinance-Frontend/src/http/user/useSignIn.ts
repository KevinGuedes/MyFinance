import { useMutation } from '@tanstack/react-query'
import { AxiosError } from 'axios'

import { useToast } from '@/components/ui/toast/use-toast'

import { api } from '../api'
import { queryClient } from '../query-client/query-client'

type SignInRequest = {
  email: string
  plainTextPassword: string
}

export type SignInResponse = {
  shouldUpdatePassword: boolean
}

type ProblemResponse = {
  title: string
  type: string
  status: number
  detail: string
  instance: string
}

type ValidationProblemResponse = ProblemResponse & {
  errors: Map<string, string[]>
}

type TooManyFailedSignInAttemptsResponse = ProblemResponse & {
  lockoutEndOnUtc: Date
}

export const useSignIn = () => {
  const { toast } = useToast()

  const mutation = useMutation<
    SignInResponse,
    AxiosError<ProblemResponse | ValidationProblemResponse>,
    SignInRequest
  >({
    mutationFn: async (signInRequest) => {
      const { data: signInResponse } = await api.post<SignInResponse>(
        '/user/signin',
        signInRequest,
      )

      return signInResponse
    },
    onSuccess: () => {
      queryClient.clear()
    },
    onError: (error) => {
      const status = error.response?.status
      const response = error.response?.data

      switch (status) {
        case 401:
          return toast({
            variant: 'destructive',
            title: 'Invalid sign in credentials',
          })
        case 400: {
          const validationProblemResponse =
            response as ValidationProblemResponse

          console.log(validationProblemResponse.errors)
          break
        }
        case 429: {
          const tooManyFailedSignInAttemptsResponse =
            response as TooManyFailedSignInAttemptsResponse

          console.log(tooManyFailedSignInAttemptsResponse.lockoutEndOnUtc)

          const lockoutEnd = new Date(
            tooManyFailedSignInAttemptsResponse.lockoutEndOnUtc + 'Z',
          )

          return toast({
            variant: 'destructive',
            title: `User locked due to too many failed sign in attempts`,
            description: `Try again after ${lockoutEnd}`,
          })
        }
        default:
          toast({
            variant: 'destructive',
            title: 'Something went really wrong!',
            description: 'Sorry for this! Try again later',
          })
      }
    },
  })

  return mutation
}
