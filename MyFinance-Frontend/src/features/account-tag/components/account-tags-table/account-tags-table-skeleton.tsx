import { Skeleton } from '@/components/ui/skeleton'

export function AccountTagsTableSkeleton() {
  return (
    <div className="flex grow flex-col" role="status">
      <Skeleton className="mb-2 h-[40px] w-full" />
      {Array.from({ length: 6 }).map((_, index) => (
        <div
          key={index}
          className="flex h-[45px] w-full items-center justify-between gap-4 border-t py-2"
        >
          <div className="flex h-6 w-full items-center gap-2">
            <div className="w-[100px]">
              <Skeleton
                className="h-6"
                style={{
                  width: `${Math.floor(Math.random() * 70) + 30}px`,
                }}
              />
            </div>
            <Skeleton
              className="h-6"
              style={{
                width: `${Math.floor(Math.random() * 50) + 20}%`,
              }}
            />
          </div>
          <Skeleton className="size-8 shrink-0" />
        </div>
      ))}
    </div>
  )
}
