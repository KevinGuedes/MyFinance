import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { transferApi } from '../../common/api'
import { MonthlyBalanceData } from '../models/monthly-balance-data'

type DiscriminatedBalanceDataResponse = {
  discriminatedBalanceData: MonthlyBalanceData[]
}

export const useGetDiscriminatedBalance = (
  managementUnitId: string,
  pastMonths: number,
  includeCurrentMonth: boolean,
) => {
  const query = useQuery({
    queryKey: [
      'discriminated-balance',
      { managementUnitId, pastMonths, includeCurrentMonth },
    ],
    staleTime: Infinity,
    retry: 3,
    placeholderData: keepPreviousData,
    queryFn: async () => {
      const { data: discriminatedBalanceDataResponse } =
        await transferApi.get<DiscriminatedBalanceDataResponse>(
          'DiscriminatedBalance',
          {
            params: {
              managementUnitId,
              pastMonths,
              includeCurrentMonth,
            },
          },
        )

      return discriminatedBalanceDataResponse
    },
    refetchOnWindowFocus: false,
    refetchOnMount: false
  })

  return query
}
