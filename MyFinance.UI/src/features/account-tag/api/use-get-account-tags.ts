import { keepPreviousData, useInfiniteQuery } from '@tanstack/react-query'

import { accountTagApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { AccountTag } from '../models/account-tag'

export const useGetAccountTags = (managementUnitId: string) => {
  const infiniteQuery = useInfiniteQuery<Paginated<AccountTag>>({
    queryKey: ['account-tags', { managementUnitId }],
    staleTime: Infinity,
    retry: 2,
    queryFn: async ({ pageParam }) => {
      const { data: paginatedAccountTags } = await accountTagApi.get<
        Paginated<AccountTag>
      >('', {
        params: {
          pageNumber: pageParam,
          pageSize: 50,
          managementUnitId,
        },
      })

      return paginatedAccountTags
    },
    initialPageParam: 1,
    getNextPageParam: (previousFetchedPage) =>
      previousFetchedPage.hasNextPage
        ? previousFetchedPage.pageNumber + 1
        : undefined,
    placeholderData: keepPreviousData,
    refetchOnWindowFocus: false,
    refetchOnMount: false,
  })

  return infiniteQuery
}
