import { Skeleton } from '@/components/ui/skeleton'

export function TransferGroupsListSkeleton() {
  return (
    <div className="flex flex-col gap-2 rounded-lg pr-4">
      <Skeleton className="h-10" />
      <div className="space-y-2">
        <Skeleton className="h-6 w-28" />
        <div className="flex justify-between">
          <Skeleton className="h-14 w-[104px]" />
          <Skeleton className="h-14 w-[104px]" />
          <Skeleton className="h-14 w-[104px]" />
        </div>
      </div>
      <div className="space-y-2">
        <Skeleton className="h-[62px]" />
        <Skeleton className="h-[62px]" />
        <Skeleton className="h-[62px]" />
      </div>
    </div>
  )
}
