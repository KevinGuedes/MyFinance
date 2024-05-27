import { useQuery } from '@tanstack/react-query'

import { ManagementUnit } from '@/models/entities/management-unit'

import { managementUnitApi } from '../api'
import { Paginated } from '../common/paginated'

type GetManagementUnitsResponse = Paginated<ManagementUnit>

export const useGetManagementUnits = (pageNumber: number, pageSize: number) => {
  const query = useQuery({
    queryKey: ['management-units', { pageNumber, pageSize }],
    queryFn: async () => {
      const response = await managementUnitApi.get<GetManagementUnitsResponse>(
        '',
        {
          params: {
            pageNumber,
            pageSize,
          },
        },
      )

      return response.data.items
    },
  })

  return query
}
