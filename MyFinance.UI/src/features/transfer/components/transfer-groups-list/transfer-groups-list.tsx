import { useParams } from '@tanstack/react-router'
import { format } from 'date-fns'
import { ClipboardList } from 'lucide-react'
import { useCallback, useEffect, useMemo, useRef } from 'react'

import { MonthPicker } from '@/components/month-picker'
import { ScrollArea } from '@/components/ui/scroll-area'

import { useGetTransferGroups } from '../../api/use-get-transfer-groups'
import { RegisterTransferDialog } from '../register-transfer-dialog'
import { TransferGroupSection } from './transfer-group-section'
import { TransferGroupsListSkeleton } from './transfer-groups-list-skeleton'

export function TransferGroupsList() {
  const parentRef = useRef<HTMLDivElement>(null)
  const today = new Date()
  const { managementUnitId } = useParams({ strict: false })
  const { data, isFetching, fetchNextPage, isLoading } = useGetTransferGroups(
    today.getMonth() + 1,
    today.getFullYear(),
    managementUnitId!,
    30,
  )

  const transferGroups = useMemo(
    () => data?.pages?.flatMap((page) => page.items) ?? [],
    [data],
  )

  const hasTransferGroups = transferGroups.length > 0

  const fetchMoreOnBottomReached = useCallback(
    (containerRefElement?: HTMLDivElement | null) => {
      if (containerRefElement) {
        const { scrollHeight, scrollTop, clientHeight } = containerRefElement
        if (scrollHeight - scrollTop - clientHeight < 150 && !isFetching) {
          fetchNextPage()
        }
      }
    },
    [fetchNextPage, isFetching],
  )

  console.log('transferGroups', transferGroups)

  useEffect(() => {
    fetchMoreOnBottomReached(parentRef.current)
  }, [fetchMoreOnBottomReached])

  if (isLoading) {
    return <TransferGroupsListSkeleton />
  }

  return (
    <div className="flex grow flex-col justify-between gap-4">
      {hasTransferGroups ? (
        <ScrollArea
          className="h-[65vh] grow border-b pb-2 lg:h-[350px]"
          type="always"
          ref={parentRef}
          onScroll={(e) => fetchMoreOnBottomReached(e.target as HTMLDivElement)}
        >
          <div className="grow space-y-4 pr-2">
            {transferGroups.map((transferGroup) => (
              <TransferGroupSection
                key={transferGroup.date.toString()}
                transferGroup={transferGroup}
              />
            ))}
          </div>
        </ScrollArea>
      ) : (
        <div className="flex h-[65vh] grow flex-col items-center justify-center gap-2 px-4 lg:max-h-[350px]">
          <p className="text-center text-sm text-muted-foreground">
            You don&apos;t have{' '}
            <strong className="font-medium">Transfers</strong> registered on{' '}
            <strong className="font-medium">
              {format(today, 'MMMM, yyyy')}
            </strong>
            .
            <br />
            Click on the button below to register a new{' '}
            <strong className="font-medium">Transfer</strong>!
          </p>
          <ClipboardList className="size-10 text-muted-foreground" />
        </div>
      )}
      <div className="flex items-center justify-between gap-2">
        <p className="text-sm text-muted-foreground">
          Showing transfers from{' '}
          <strong className="font-medium">{format(today, 'MMMM, yyyy')}</strong>
        </p>
        <div className="flex items-center gap-2">
          <MonthPicker />
          <RegisterTransferDialog />
        </div>
      </div>
    </div>
  )
}
