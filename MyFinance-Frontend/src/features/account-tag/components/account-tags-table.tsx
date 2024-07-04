import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  Row,
  useReactTable,
} from '@tanstack/react-table'
import { useVirtualizer } from '@tanstack/react-virtual'
import { Loader2 } from 'lucide-react'
import { useCallback, useEffect, useMemo, useRef } from 'react'

import { ScrollArea, ScrollBar } from '@/components/ui/scroll-area'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'

import { useGetAccountTags } from '../api/use-get-account-tags'
import { AccountTag } from '../models/account-tag'

export const columns: ColumnDef<AccountTag>[] = [
  {
    accessorKey: 'tag',
    header: 'Tag',
  },
  {
    accessorKey: 'description',
    header: 'Description',
  },
]

type AccountTagsTableProps = {
  managementUnitId: string
}

export function AccountTagsTable({ managementUnitId }: AccountTagsTableProps) {
  const parentRef = useRef<HTMLDivElement>(null)

  const { data, isFetchingNextPage, isFetching, fetchNextPage, isPending } =
    useGetAccountTags(managementUnitId, 20)

  const flatData = useMemo(
    () => data?.pages?.flatMap((page) => page.items) ?? [],
    [data],
  )

  const table = useReactTable({
    data: flatData,
    columns,
    getCoreRowModel: getCoreRowModel(),
  })

  const { rows } = table.getRowModel()

  const rowVirtualizer = useVirtualizer({
    count: rows.length,
    estimateSize: useCallback(() => 41, []),
    getScrollElement: () => parentRef.current,
    overscan: 3,
    measureElement:
      typeof window !== 'undefined' &&
      navigator.userAgent.indexOf('Firefox') === -1
        ? (element) => element?.getBoundingClientRect().height
        : undefined,
  })

  const fetchMoreOnBottomReached = useCallback(
    (containerRefElement?: HTMLDivElement | null) => {
      if (containerRefElement) {
        const { scrollHeight, scrollTop, clientHeight } = containerRefElement
        if (scrollHeight - scrollTop - clientHeight < 10 && !isFetching) {
          fetchNextPage()
        }
      }
    },
    [fetchNextPage, isFetching],
  )

  useEffect(() => {
    fetchMoreOnBottomReached(parentRef.current)
  }, [fetchMoreOnBottomReached])

  return (
    <ScrollArea
      type="always"
      ref={parentRef}
      className="relative h-[350px] w-full grow overflow-auto pr-3"
      onScroll={(e) => fetchMoreOnBottomReached(e.target as HTMLDivElement)}
    >
      <ScrollBar className="z-20" />
      <div
        className="w-full"
        style={{ height: `${rowVirtualizer.getTotalSize()}px` }}
      >
        <Table className="grid w-full">
          <TableHeader className="sticky top-0 z-10 grid bg-background">
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id} className="flex">
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id} className="flex w-fit py-4">
                      {header.isPlaceholder
                        ? null
                        : flexRender(
                            header.column.columnDef.header,
                            header.getContext(),
                          )}
                    </TableHead>
                  )
                })}
              </TableRow>
            ))}
          </TableHeader>
          <TableBody
            className="relative grid"
            style={{
              height: `${rowVirtualizer.getTotalSize()}px`,
            }}
          >
            {rows?.length > 0 ? (
              <>
                {rowVirtualizer.getVirtualItems().map((virtualRow) => {
                  const row = rows[virtualRow.index] as Row<AccountTag>
                  return (
                    <TableRow
                      key={row.id}
                      data-index={virtualRow.index}
                      className="absolute flex w-full text-sm"
                      ref={(node) => rowVirtualizer.measureElement(node)}
                      style={{
                        transform: `translateY(${virtualRow.start}px)`,
                      }}
                    >
                      {row.getVisibleCells().map((cell) => {
                        return (
                          <TableCell key={cell.id} className="w-fit py-2.5">
                            {flexRender(
                              cell.column.columnDef.cell,
                              cell.getContext(),
                            )}
                          </TableCell>
                        )
                      })}
                    </TableRow>
                  )
                })}
              </>
            ) : (
              <TableRow>
                <TableCell
                  colSpan={columns.length}
                  className="absolute flex w-full grow justify-center border text-center text-sm text-muted-foreground"
                >
                  No results.
                </TableCell>
              </TableRow>
            )}
          </TableBody>
        </Table>
        <div>
          {/* <div>
              {isFetchingNextPage && (
                <div
                  className="flex h-20 flex-col items-center gap-0.5 border-t pt-4 text-muted-foreground"
                  role="status"
                >
                  <Loader2 className="size-8 animate-spin" />
                  <p className="text-sm">Loading more items...</p>
                </div>
              )}
            </div>

            {isPending && (
              <p className="pt-4 text-center text-sm text-muted-foreground">
                Loading...
              </p>
            )} */}

          {/* {!hasNextPage && !isFetching && (
            <div
              className="flex h-20 flex-col items-center gap-0.5 border-t pt-4 text-muted-foreground"
              role="status"
            >
              <List className="size-8" />
              <p className="text-sm">No more items to fetch</p>
            </div>
          )} */}
        </div>
      </div>
    </ScrollArea>
  )
}
