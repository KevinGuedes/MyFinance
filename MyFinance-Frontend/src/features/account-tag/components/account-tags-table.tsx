import {
  ColumnDef,
  flexRender,
  getCoreRowModel,
  Row,
  useReactTable,
} from '@tanstack/react-table'
import { useVirtualizer } from '@tanstack/react-virtual'
import { Loader2, MoreHorizontal, Tag } from 'lucide-react'
import { useCallback, useEffect, useMemo, useRef } from 'react'

import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'
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
    size: 100,
    cell: ({ row }) => {
      const tag = row.getValue<string>('tag')
      return <p className="text-left font-medium">{tag}</p>
    },
  },
  {
    accessorKey: 'description',
    header: 'Description',
    cell: ({ row }) => {
      const description = row.getValue<string>('description')
      return <p className="line-clamp-1">{description}</p>
    },
  },
  {
    id: 'actions',
    size: 80,
    cell: ({ row }) => {
      const payment = row.original

      return (
        <DropdownMenu>
          <DropdownMenuTrigger asChild>
            <Button variant="ghost" className="ml-auto size-8 p-0">
              <span className="sr-only">Open menu</span>
              <MoreHorizontal className="size-4" />
            </Button>
          </DropdownMenuTrigger>
          <DropdownMenuContent align="end">
            <DropdownMenuLabel>Actions</DropdownMenuLabel>
            <DropdownMenuItem
              onClick={() => navigator.clipboard.writeText(payment.id)}
            >
              Copy payment ID
            </DropdownMenuItem>
            <DropdownMenuSeparator />
            <DropdownMenuItem>View customer</DropdownMenuItem>
            <DropdownMenuItem>View payment details</DropdownMenuItem>
          </DropdownMenuContent>
        </DropdownMenu>
      )
    },
  },
]

type AccountTagsTableProps = {
  managementUnitId: string
}

export function AccountTagsTable({ managementUnitId }: AccountTagsTableProps) {
  const parentRef = useRef<HTMLDivElement>(null)

  const { data, isFetchingNextPage, isFetching, fetchNextPage, isPending } =
    useGetAccountTags(managementUnitId, 50)

  const flatData = useMemo(
    () => data?.pages?.flatMap((page) => page.items) ?? [],
    [data],
  )

  const table = useReactTable({
    defaultColumn: {
      minSize: 0,
      size: 0,
    },
    data: flatData,
    columns,
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
        console.log({ scrollHeight, scrollTop, clientHeight })
        if (scrollHeight - scrollTop - clientHeight < 50 && !isFetching) {
          fetchNextPage()
        }
      }
    },
    [fetchNextPage, isFetching],
  )

  useEffect(() => {
    fetchMoreOnBottomReached(parentRef.current)
  }, [fetchMoreOnBottomReached])

  return rows.length > 0 ? (
    <ScrollArea
      type="always"
      ref={parentRef}
      className="relative h-[350px] grow pr-4"
      onScroll={(e) => fetchMoreOnBottomReached(e.target as HTMLDivElement)}
    >
      <ScrollBar />
      <Table>
        <TableHeader className="sticky top-0 z-10 grid bg-background">
          {table.getHeaderGroups().map((headerGroup) => (
            <TableRow key={headerGroup.id} className="flex w-full">
              {headerGroup.headers.map((header) => {
                return (
                  <TableHead
                    key={header.id}
                    className="flex p-4"
                    style={{
                      width: header.getSize() !== 0 ? header.getSize() : '100%',
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
            height: rowVirtualizer.getTotalSize(),
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
                    className="absolute flex w-full items-center"
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
    </ScrollArea>
  ) : (
    <div className="flex min-h-[350px] grow flex-col items-center justify-center gap-2 px-4">
      <p className="text-center text-sm text-muted-foreground">
        You don&apos;t have{' '}
        <strong className="font-medium">Account Tags</strong> registered yet.
        <br />
        <strong className="font-medium">
          Click on the button below to create an Account Tag!
        </strong>
      </p>
      <Tag className="size-10 text-muted-foreground" />
    </div>
  )
}
