import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { transferApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { Transfer } from '../models/transfer'

export const useGetTransfers = (
  managementUnitId: string,
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
) => {
  const query = useQuery({
    queryKey: [
      'transfers',
      { pageNumber, pageSize, searchTerm, managementUnitId },
    ],
    staleTime: searchTerm ? 3 * 60 * 1000 : Infinity,
    placeholderData: keepPreviousData,
    queryFn: async () => {
      const { data: paginatedTransfers } = await transferApi.get<
        Paginated<Transfer>
      >('', {
        params: {
          pageNumber,
          pageSize,
          searchTerm,
          managementUnitId,
        },
      })

      console.log(paginatedTransfers)

      return paginatedTransfers
    },
  })

  return query
}
