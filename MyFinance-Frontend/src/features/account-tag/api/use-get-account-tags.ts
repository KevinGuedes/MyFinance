import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { accountTagApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { AccountTag } from '../models/account-tag'

export const useGetAccountTags = (pageNumber: number, pageSize: number) => {
  const query = useQuery({
    queryKey: ['account-tags', { pageNumber, pageSize }],
    staleTime: Infinity,
    placeholderData: keepPreviousData,
    queryFn: async () => {
      const { data: paginatedAccountTags } = await accountTagApi.get<
        Paginated<AccountTag>
      >('', {
        params: {
          pageNumber,
          pageSize,
        },
      })

      return paginatedAccountTags
    },
  })

  return query
}
