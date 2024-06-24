import { Skeleton } from '@/components/ui/skeleton'

export function DiscriminatedBalanceCardSkeleton() {
  return (
    <div className="flex size-full flex-col gap-4 bg-background p-4">
      <div className="flex items-center justify-between gap-2">
        <Skeleton className="h-7 w-52" />
        <Skeleton className="h-9 w-28" />
      </div>
      <Skeleton className="size-full" />
    </div>
  )
}
