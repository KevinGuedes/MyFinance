import { keepPreviousData, useInfiniteQuery } from '@tanstack/react-query'
import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  Row,
  useReactTable,
} from '@tanstack/react-table'
import { useVirtualizer } from '@tanstack/react-virtual'
import { List, Loader2 } from 'lucide-react'
import { useCallback, useEffect, useMemo, useRef } from 'react'

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'

import { ScrollArea, ScrollBar } from './components/ui/scroll-area'
import { AccountTag } from './features/account-tag/models/account-tag'
import { Paginated } from './features/common/paginated'

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

async function generateAccountTags(
  page: number,
): Promise<Paginated<AccountTag>> {
  console.log('loading page: ', page)
  await new Promise((resolve) => setTimeout(resolve, 300))
  let i = 1
  const ats = Array.from({ length: 10 }, () => {
    const id = crypto.randomUUID()

    const data = {
      id: `${id}-${page}-${i}`,
      tag: `Tag ${id}-${page}-${i}`,
      description: `Description ${id}-${page}-${i}`,
    }

    i++
    return data
  })

  return {
    items: ats,
    pageNumber: page,
    pageSize: 12,
    totalCount: 300,
    totalPages: 4,
    hasNextPage: page < 4,
    hasPreviousPage: page > 1,
  }
}

export function Test() {
  const parentRef = useRef<HTMLDivElement>(null)

  const {
    data,
    isFetchingNextPage,
    isFetching,
    fetchNextPage,
    isPending,
    hasNextPage,
    isLoading,
  } = useInfiniteQuery<Paginated<AccountTag>>({
    queryKey: ['account-tags'],
    queryFn: async ({ pageParam = 1 }) => {
      const fetchedData = await generateAccountTags(Number(pageParam))
      return fetchedData
    },
    initialPageParam: 0,
    getNextPageParam: (lastPage) =>
      lastPage.hasNextPage ? lastPage.pageNumber + 1 : undefined,
    refetchOnWindowFocus: false,
    placeholderData: keepPreviousData,
  })

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
    estimateSize: useCallback(() => 53, []),
    getScrollElement: () => parentRef.current,
    overscan: 5,
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
        if (scrollHeight - scrollTop - clientHeight < 200 && !isFetching) {
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
      className="relative h-[400px] w-full grow overflow-auto pr-3"
      onScroll={(e) => fetchMoreOnBottomReached(e.target as HTMLDivElement)}
    >
      <ScrollBar className="z-20" />
      <div
        className="w-full"
        style={{ height: `${rowVirtualizer.getTotalSize()}px` }}
      >
        <Table className="grid w-full">
          <TableHeader className="sticky top-0 z-10 grid border-t bg-background">
            {table.getHeaderGroups().map((headerGroup) => (
              <TableRow key={headerGroup.id} className="flex">
                {headerGroup.headers.map((header) => {
                  return (
                    <TableHead key={header.id} className="flex w-full p-4">
                      {flexRender(
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
                      <TableCell key={cell.id} className="w-full">
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
          </TableBody>
        </Table>
        <div>
          <div>
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
          )}

          {!hasNextPage && !isFetching && (
            <div
              className="flex h-20 flex-col items-center gap-0.5 border-t pt-4 text-muted-foreground"
              role="status"
            >
              <List className="size-8" />
              <p className="text-sm">No more items to fetch</p>
            </div>
          )}
        </div>
      </div>
    </ScrollArea>
  )
}
