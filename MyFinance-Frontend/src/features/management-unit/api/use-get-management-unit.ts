import { useQuery } from '@tanstack/react-query'

import { ManagementUnit } from '@/features/management-unit/models/management-unit'

import { managementUnitApi } from '../../common/api'

export const useGetManagementUnit = (managementUnitId: string) => {
  const query = useQuery({
    queryKey: ['management-unit', { managementUnitId }],
    staleTime: Infinity,
    queryFn: async () => {
      await new Promise((resolve) => setTimeout(resolve, 3000))
      const { data: managemenUnit } =
        await managementUnitApi.get<ManagementUnit>(managementUnitId)

      console.log(managemenUnit)
      return managemenUnit
    },
  })

  return query
}
