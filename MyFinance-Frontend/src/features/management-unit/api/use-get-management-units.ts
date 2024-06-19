import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { ManagementUnit } from '@/features/management-unit/models/management-unit'

import { managementUnitApi } from '../../common/api'
import { Paginated } from '../../common/paginated'

export const useGetManagementUnits = (
  pageNumber: number,
  pageSize: number,
  searchTerm?: string,
) => {
  const query = useQuery({
    queryKey: ['management-units', { pageNumber, pageSize, searchTerm }],
    staleTime: searchTerm ? 3 * 60 * 1000 : Infinity,
    placeholderData: keepPreviousData,
    queryFn: async () => {
      const { data: getManagementUnitsResponse } = await managementUnitApi.get<
        Paginated<ManagementUnit>
      >('', {
        params: {
          pageNumber,
          pageSize,
          searchTerm,
        },
      })

      return getManagementUnitsResponse
    },
  })

  return query
}
