import { useQuery } from '@tanstack/react-query'

import { userApi } from '../api'

type UserResponse = {
  shouldUpdatePassword: boolean
  name: string
}

export const useGetUserInfo = () => {
  const query = useQuery({
    queryKey: ['user-info'],
    enabled: false,
    retry: false,
    queryFn: async () => {
      const { data: userResponse } = await userApi.get<UserResponse>('info')
      return userResponse
    },
  })

  return query
}
