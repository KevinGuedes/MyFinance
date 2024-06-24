import { Skeleton } from '@/components/ui/skeleton'

export function TransfersTableSkeleton() {
  return (
    <div className="flex h-full grow flex-col justify-between gap-2">
      <div className="mt-[0.125rem] space-y-1">
        <Skeleton className="h-10" />
        {Array.from({ length: 10 }).map((_, index) => (
          <Skeleton key={index} className="h-7" />
        ))}
      </div>
      <div className="flex items-center justify-between gap-2">
        <Skeleton className="h-9 w-40" />
        <div className="flex items-center gap-2">
          <Skeleton className="h-9 w-20" />
          <Skeleton className="h-9 w-14" />
        </div>
      </div>
    </div>
  )
}
