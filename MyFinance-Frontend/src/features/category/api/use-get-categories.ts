import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { categoryApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { Category } from '../models/category'

export const useGetCategories = (pageNumber: number, pageSize: number) => {
  const query = useQuery({
    queryKey: ['categories', { pageNumber, pageSize }],
    staleTime: Infinity,
    placeholderData: keepPreviousData,
    queryFn: async () => {
      const { data: paginatedCategories } = await categoryApi.get<
        Paginated<Category>
      >('', {
        params: {
          pageNumber,
          pageSize,
        },
      })

      return paginatedCategories
    },
  })

  return query
}
