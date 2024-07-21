import { Skeleton } from '@/components/ui/skeleton'

export function SummaryCardsSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
      <div className="flex flex-col gap-1 rounded-lg border bg-background p-4">
        <div className="flex items-center justify-between gap-2">
          <Skeleton className="h-6 w-2/3" />
          <Skeleton className="size-8 shrink-0" />
        </div>
        <Skeleton className="h-7" />
      </div>
      <div className="flex flex-col gap-1 rounded-lg border bg-background p-4">
        <div className="flex items-center justify-between gap-2">
          <Skeleton className="h-6 w-2/3" />
          <Skeleton className="size-8 shrink-0" />
        </div>
        <Skeleton className="h-7" />
      </div>
      <div className="flex flex-col gap-1 rounded-lg border bg-background p-4">
        <div className="flex items-center justify-between gap-2">
          <Skeleton className="h-6 w-2/3" />
          <Skeleton className="size-8 shrink-0" />
        </div>
        <Skeleton className="h-7" />
      </div>
    </div>
  )
}
