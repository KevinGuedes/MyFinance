import { Skeleton } from '@/components/ui/skeleton'

export function ManagementUnitCardSkeleton() {
  return (
    <div className="flex h-[10.5rem] flex-col gap-2 rounded-lg border bg-background p-4">
      <Skeleton className="h-7" />
      <div className="flex flex-col gap-4">
        <Skeleton className="h-6" />
        <div className="grid grid-cols-3 gap-2">
          <Skeleton className="h-[3.75rem]" />
          <Skeleton className="h-[3.75rem]" />
          <Skeleton className="h-[3.75rem]" />
        </div>
      </div>
    </div>
  )
}
