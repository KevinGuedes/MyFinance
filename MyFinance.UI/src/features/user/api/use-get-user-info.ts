import { useQuery } from '@tanstack/react-query'

import { userApi } from '../../common/api'

type UserInfoResponse = {
  shouldUpdatePassword: boolean
  name: string
}

export const useGetUserInfo = () => {
  const query = useQuery({
    queryKey: ['user-info'],
    enabled: false,
    retry: false,
    queryFn: async () => {
      const { data: userResponse } = await userApi.get<UserInfoResponse>('info')
      return userResponse
    },
  })

  return query
}
