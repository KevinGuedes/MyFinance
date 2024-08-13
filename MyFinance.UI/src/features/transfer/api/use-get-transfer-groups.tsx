import { keepPreviousData, useInfiniteQuery } from '@tanstack/react-query'

import { transferApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { TransferGroup } from '../models/transfer-group'

export const useGetTransferGroups = (
  month: number,
  year: number,
  managementUnitId: string,
  ) => {
  const query = useInfiniteQuery<Paginated<TransferGroup>>({
    queryKey: ['transfers', { managementUnitId, month, year }],
    staleTime: 10 * 1000 * 60,
    placeholderData: keepPreviousData,
    initialPageParam: 1,
    retry: 2,
    refetchOnWindowFocus: false,
    refetchOnMount: false,
    retryDelay: 1000,
    getNextPageParam: (previousFetchedPage) =>
      previousFetchedPage.hasNextPage
        ? previousFetchedPage.pageNumber + 1
        : undefined,
    queryFn: async ({ pageParam }) => {
      const { data: paginatedTransferGroups } = await transferApi.get<
        Paginated<TransferGroup>
      >('', {
        params: {
          month,
          year,
          pageSize: 3,
          managementUnitId,
          pageNumber: pageParam,
        },
      })

      const formattedTransferGroups = paginatedTransferGroups.items.map(
        (group) => {
          const formattedTransfers = group.transfers.map((transfer) => ({
            ...transfer,
            settlementDate: new Date(transfer.settlementDate),
          }))

          return {
            ...group,
            date: formattedTransfers[0].settlementDate,
            transfers: formattedTransfers,
          }
        },
      )

      return { ...paginatedTransferGroups, items: formattedTransferGroups }
    },
  })

  return query
}
