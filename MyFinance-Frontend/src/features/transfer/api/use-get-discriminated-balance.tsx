import { useQuery } from '@tanstack/react-query'

import { transferApi } from '../../common/api'
import { MonthlyBalanceData } from '../models/monthly-balance-data'

type DiscriminatedBalanceDataResponse = {
  discriminatedBalanceData: MonthlyBalanceData[]
}

export const useGetDiscriminatedBalance = (
  managementUnitId: string,
  pastMonths: number,
) => {
  const query = useQuery({
    queryKey: ['discriminated-balance', { managementUnitId, pastMonths }],
    staleTime: Infinity,
    queryFn: async () => {
      const { data: discriminatedBalanceDataResponse } =
        await transferApi.get<DiscriminatedBalanceDataResponse>(
          'DiscriminatedBalance',
          {
            params: {
              managementUnitId,
              pastMonths,
            },
          },
        )

      return discriminatedBalanceDataResponse
    },
  })

  return query
}
