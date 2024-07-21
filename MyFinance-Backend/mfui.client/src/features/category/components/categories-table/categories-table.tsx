import {
  flexRender,
  getCoreRowModel,
  Row,
  useReactTable,
} from '@tanstack/react-table'
import { useVirtualizer } from '@tanstack/react-virtual'
import { Loader2, Shapes } from 'lucide-react'
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

import { useGetCategories } from '../../api/use-get-categories'
import { Category } from '../../models/category'
import { categoriesTableColumns } from './categories-table-columns'
import { CategoriesTableSkeleton } from './categories-table-skeleton'

type CategoriesTableProps = {
  managementUnitId: string
}

export function CategoriesTable({ managementUnitId }: CategoriesTableProps) {
  const parentRef = useRef<HTMLDivElement>(null)

  const {
    data,
    isFetching,
    fetchNextPage,
    isPending,
    isFetchingNextPage,
    hasNextPage,
  } = useGetCategories(managementUnitId, 50)

  const categories = useMemo(
    () => data?.pages?.flatMap((page) => page.items) ?? [],
    [data],
  )

  const table = useReactTable({
    defaultColumn: {
      minSize: 0,
      size: 0,
    },
    data: categories,
    columns: categoriesTableColumns,
    getCoreRowModel: getCoreRowModel(),
  })

  const { rows } = table.getRowModel()
  const rowVirtualizer = useVirtualizer({
    count: rows.length,
    estimateSize: useCallback(() => 45, []),
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
        if (scrollHeight - scrollTop - clientHeight < 150 && !isFetching) {
          fetchNextPage()
        }
      }
    },
    [fetchNextPage, isFetching],
  )

  useEffect(() => {
    fetchMoreOnBottomReached(parentRef.current)
  }, [fetchMoreOnBottomReached])

  if (isPending) {
    return <CategoriesTableSkeleton />
  }

  return rows.length > 0 ? (
    <ScrollArea
      type="always"
      ref={parentRef}
      className="h-[350px] grow pr-4"
      onScroll={(e) => fetchMoreOnBottomReached(e.target as HTMLDivElement)}
    >
      <ScrollBar />
      <Table>
        <TableHeader className="sticky top-0 z-10 grid rounded-lg bg-background">
          {table.getHeaderGroups().map((headerGroup) => (
            <TableRow
              key={headerGroup.id}
              className="flex w-full rounded-t-lg bg-background"
            >
              {headerGroup.headers.map((header) => {
                return (
                  <TableHead
                    key={header.id}
                    className="flex items-center p-4"
                    style={{
                      width: header.getSize() !== 0 ? header.getSize() : '··',
                    }}
                  >
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
          {rowVirtualizer.getVirtualItems().map((virtualRow) => {
            const row = rows[virtualRow.index] as Row<Category>
            return (
              <TableRow
                key={row.id}
                data-index={virtualRow.index}
                className="absolute flex w-full items-center last:rounded-b-lg"
                ref={(node) => rowVirtualizer.measureElement(node)}
                style={{
                  transform: `translateY(${virtualRow.start}px)`,
                }}
              >
                {row.getVisibleCells().map((cell) => {
                  return (
                    <TableCell
                      key={cell.id}
                      className="flex px-4 py-1.5"
                      style={{
                        width:
                          cell.column.getSize() !== 0
                            ? `${cell.column.getSize()}px`
                            : '100%',
                      }}
                    >
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
      {hasNextPage && isFetchingNextPage && (
        <div
          className="flex h-12 items-center justify-center gap-4 border-t p-4 text-muted-foreground"
          role="status"
        >
          <Loader2 className="size-6 animate-spin" />
          <p className="text-sm">
            Loading more <strong className="font-medium">Categories</strong>...
          </p>
        </div>
      )}
    </ScrollArea>
  ) : (
    <div className="flex min-h-[350px] grow flex-col items-center justify-center gap-4 px-4">
      <p className="text-center text-sm text-muted-foreground">
        You don&apos;t have <strong className="font-medium">Categories</strong>{' '}
        registered yet.
        <br />
        <strong className="font-medium">
          Click on the button below to create a Category!
        </strong>
      </p>
      <Shapes className="size-10 text-muted-foreground" />
    </div>
  )
}
