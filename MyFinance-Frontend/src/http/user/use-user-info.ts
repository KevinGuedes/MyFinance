import { useMutation, useQueryClient } from '@tanstack/react-query'
import { useRouter } from '@tanstack/react-router'

import { useUserStore } from '@/stores/user-store'

import { userApi } from '../api'
import { ApiError } from '../common/api-error'

type Payload = {
  originPath: string
}

type UserResponse = {
  shouldUpdatePassword: boolean
  name: string
}

export const useUserInfo = () => {
  const setUserInfo = useUserStore((state) => state.setUserInfo)
  const queryClient = useQueryClient()
  const router = useRouter()

  const mutation = useMutation<UserResponse, ApiError, Payload>({
    mutationFn: async () => {
      const { data: userResponse } = await userApi.get<UserResponse>('info')
      return userResponse
    },
    onSuccess: (userResponse) => {
      setUserInfo(userResponse)
    },
    onError: (_, variables) => {
      router.navigate({
        to: '/sign-in',
        search: { redirectTo: variables.originPath },
      })
    },
    onSettled: () => {
      queryClient.clear()
    },
  })

  return mutation
}
