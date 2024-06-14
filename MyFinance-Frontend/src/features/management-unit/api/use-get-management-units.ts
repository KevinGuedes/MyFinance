import { useQuery } from '@tanstack/react-query'

import { ManagementUnit } from '@/features/management-unit/models/management-unit'

import { managementUnitApi } from '../../common/api'
import { Paginated } from '../../common/paginated'

type GetManagementUnitsResponse = Paginated<ManagementUnit>

export const useGetManagementUnits = (pageNumber: number, pageSize: number) => {
  const query = useQuery({
    queryKey: ['management-units', { pageNumber, pageSize }],
    queryFn: async () => {
      const { data: getManagementUnitsResponse } =
        await managementUnitApi.get<GetManagementUnitsResponse>('', {
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
