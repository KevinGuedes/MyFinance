import { Skeleton } from '@/components/ui/skeleton'

export function TransferGroupsListSkeleton() {
  return (
    <div className="flex flex-col gap-3 rounded-lg">
      <Skeleton className="h-10" />
      <div className="space-y-1">
        <Skeleton className="h-6 w-24" />
        <div className="flex justify-between">
          <Skeleton className="h-12 w-24" />
          <Skeleton className="h-12 w-24" />
          <Skeleton className="h-12 w-24" />
        </div>
      </div>
      <div className="mt-2 space-y-2">
        <Skeleton className="h-12 py-1" />
        <Skeleton className="h-12 py-1" />
        <Skeleton className="h-12 py-1" />
      </div>
    </div>
  )
}
