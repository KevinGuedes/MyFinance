import { keepPreviousData, useInfiniteQuery } from '@tanstack/react-query'

import { categoryApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { Category } from '../models/category'

export const useGetCategories = (
  managementUnitId: string,
  pageSize: number,
) => {
  const infiniteQuery = useInfiniteQuery<Paginated<Category>>({
    queryKey: ['categories', { pageSize, managementUnitId }],
    staleTime: Infinity,
    retry: 2,
    queryFn: async ({ pageParam }) => {
      const { data: paginatedCategories } = await categoryApi.get<
        Paginated<Category>
      >('', {
        params: {
          pageNumber: pageParam,
          pageSize,
          managementUnitId,
        },
      })

      return paginatedCategories
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
