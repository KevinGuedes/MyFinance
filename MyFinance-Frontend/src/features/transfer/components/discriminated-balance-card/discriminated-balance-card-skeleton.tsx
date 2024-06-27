import { Skeleton } from '@/components/ui/skeleton'

export function DiscriminatedBalanceCardSkeleton() {
  return (
    <div className="flex size-full flex-col gap-2 rounded-lg border bg-background p-4">
      <div className="flex items-center justify-between gap-2">
        <Skeleton className="mt-2 h-7 w-[18.25rem]" />
        <Skeleton className="size-10 rounded-full" />
      </div>
      <Skeleton className="h-5 w-2/3" />

      <Skeleton className="h-64 min-h-64 grow" />
    </div>
  )
}
