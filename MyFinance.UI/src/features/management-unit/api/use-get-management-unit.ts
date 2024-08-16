import { useQuery } from '@tanstack/react-query'

import { ManagementUnit } from '@/features/management-unit/models/management-unit'

import { managementUnitApi } from '../../common/api'

export const useGetManagementUnit = (managementUnitId: string) => {
  const query = useQuery({
    queryKey: ['management-unit', { managementUnitId }],
    staleTime: Infinity,
    retry: 2,
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    queryFn: async () => {
      const { data: managemenUnit } =
        await managementUnitApi.get<ManagementUnit>(managementUnitId)

      return managemenUnit
    },
  })

  return query
}
