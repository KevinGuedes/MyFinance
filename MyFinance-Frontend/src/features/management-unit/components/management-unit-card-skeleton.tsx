import { Skeleton } from '@/components/ui/skeleton'

export function ManagementUnitCardSkeleton() {
  return (
    <div className="flex flex-col gap-2 rounded-lg border bg-background p-4">
      <Skeleton className="h-7" />
      <div className="flex flex-col gap-4">
        <Skeleton className="h-6" />
        <div className="grid h-[3.75rem] grid-cols-3 gap-2">
          <Skeleton className="h-full" />
          <Skeleton className="h-full" />
          <Skeleton className="h-full" />
        </div>
      </div>
    </div>
  )
}
