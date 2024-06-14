import { useQuery } from '@tanstack/react-query'

import { ManagementUnit } from '@/features/management-unit/models/management-unit'

import { managementUnitApi } from '../../common/api'
import { Paginated } from '../../common/paginated'

export const useGetManagementUnits = (pageNumber: number, pageSize: number) => {
  const query = useQuery({
    queryKey: ['management-units', { pageNumber, pageSize }],
    staleTime: Infinity,
    queryFn: async () => {
      const { data: getManagementUnitsResponse } = await managementUnitApi.get<
        Paginated<ManagementUnit>
      >('', {
        params: {
          pageNumber,
          pageSize,
        },
      })

      return getManagementUnitsResponse
    },
  })

  return query
}
