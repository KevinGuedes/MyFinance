import { keepPreviousData, useQuery } from '@tanstack/react-query'

import { transferApi } from '../../common/api'
import { Paginated } from '../../common/paginated'
import { TransferGroup } from '../models/transfer-group'

export const useGetTransferGroups = (
  managementUnitId: string,
  pageNumber: number,
  pageSize: number,
) => {
  const query = useQuery({
    queryKey: ['transfers', { pageNumber, pageSize, managementUnitId }],
    staleTime: Infinity,
    placeholderData: keepPreviousData,
    queryFn: async () => {
      const { data: paginatedTransferGroups } = await transferApi.get<
        Paginated<TransferGroup>
      >('', {
        params: {
          pageNumber,
          pageSize,
          managementUnitId,
        },
      })

      console.log(paginatedTransferGroups)

      const formattedTransferGroups = paginatedTransferGroups.items.map(
        (group) => {
          const formattedTransfers = group.transfers.map((transfer) => ({
            ...transfer,
            settlementDate: new Date(transfer.settlementDate),
          }))

          return {
            ...group,
            date: new Date(
              formattedTransfers[0].settlementDate.getFullYear(),
              formattedTransfers[0].settlementDate.getMonth(),
              formattedTransfers[0].settlementDate.getDate(),
            ),
            transfers: formattedTransfers,
          }
        },
      )

      return { ...paginatedTransferGroups, items: formattedTransferGroups }
    },
  })

  return query
}
