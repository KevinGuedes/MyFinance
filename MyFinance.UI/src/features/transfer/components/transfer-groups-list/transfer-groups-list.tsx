import { useParams } from '@tanstack/react-router'
import { format } from 'date-fns'
import { ClipboardList, Loader2 } from 'lucide-react'
import { useCallback, useMemo, useRef, useState } from 'react'

import { MonthPicker } from '@/components/month-picker'
import { ScrollArea } from '@/components/ui/scroll-area'

import { useGetTransferGroups } from '../../api/use-get-transfer-groups'
import { RegisterTransferDialog } from '../register-transfer-dialog'
import { TransferGroupSection } from './transfer-group-section'
import { TransferGroupsListSkeleton } from './transfer-groups-list-skeleton'

export function TransferGroupsList() {
  const observer = useRef<IntersectionObserver>()
  const scrollArea = useRef<HTMLDivElement>(null)
  const [selectedDate, setSelectedDate] = useState(new Date())
  const { managementUnitId } = useParams({ strict: false })
  const {
    data,
    isFetching,
    fetchNextPage,
    isLoading,
    isFetchingNextPage,
    hasNextPage,
    isPending,
  } = useGetTransferGroups(
    selectedDate.getMonth() + 1,
    selectedDate.getFullYear(),
    managementUnitId!,
  )

  const observerRef = useCallback(
    (element: HTMLElement | null) => {
      if (isLoading) return
      if (observer.current) observer.current.disconnect()
      if (!element) return

      observer.current = new IntersectionObserver(
        (entries) => {
          if (entries[0].isIntersecting && hasNextPage) {
            fetchNextPage()
          }
        },
        { threshold: 0 },
      )

      observer.current.observe(element)
    },
    [hasNextPage, isLoading, fetchNextPage],
  )

  const transferGroups = useMemo(
    () => data?.pages?.flatMap((page) => page.items) ?? [],
    [data],
  )

  function handleSelectedDate(selectedDate: Date) {
    setSelectedDate(selectedDate)
  }

  return (
    <div className="flex grow flex-col justify-between gap-4">
      {isLoading ? (
        <TransferGroupsListSkeleton />
      ) : (
        <>
          {transferGroups.length > 0 ? (
            <ScrollArea
              // tricky solution to make scroll reset to top when changing selected month
              key={transferGroups[0].date.toString()}
              ref={scrollArea}
              className="h-[65vh] grow pr-2 lg:h-[350px]"
              type="always"
            >
              <div className="grow space-y-4">
                {transferGroups.map((transferGroup) => (
                  <TransferGroupSection
                    transferGroup={transferGroup}
                    key={transferGroup.date.toString()}
                  />
                ))}
              </div>
              {hasNextPage && (
                <div
                  ref={observerRef}
                  className="flex items-center justify-center gap-2 pb-2 pt-4"
                >
                  <Loader2 className="size-7 animate-spin text-muted-foreground" />
                  <p className="text-sm text-muted-foreground">
                    Loading more transfers...
                  </p>
                </div>
              )}
            </ScrollArea>
          ) : (
            <div className="flex h-[65vh] grow flex-col items-center justify-center gap-2 px-4 lg:max-h-[350px]">
              <p className="text-center text-sm text-muted-foreground">
                You don&apos;t have{' '}
                <strong className="font-medium">Transfers</strong> registered on{' '}
                <strong className="font-medium">
                  {format(selectedDate, 'MMMM, yyyy')}
                </strong>
                .
                <br />
                Click on the button below to register a new{' '}
                <strong className="font-medium">Transfer</strong>!
              </p>
              <ClipboardList className="size-10 text-muted-foreground" />
            </div>
          )}
        </>
      )}
      <div className="flex items-center justify-between gap-4">
        <MonthPicker
          value={selectedDate}
          disabled={isFetching && !isFetchingNextPage}
          isLoading={isFetching && !isFetchingNextPage && !isPending}
          onChange={handleSelectedDate}
        />
        <RegisterTransferDialog />
      </div>
    </div>
  )
}
