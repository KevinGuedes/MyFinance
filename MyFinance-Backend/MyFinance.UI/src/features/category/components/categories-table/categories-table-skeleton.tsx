import { Skeleton } from '@/components/ui/skeleton'

export function CategoriesTableSkeleton() {
  return (
    <div className="flex grow flex-col" role="status">
      <Skeleton className="mb-2 h-[40px] w-full" />
      {Array.from({ length: 6 }).map((_, index) => (
        <div
          key={index}
          className="flex h-[45px] w-full items-center justify-between gap-2 border-t py-2"
        >
          <Skeleton
            className="h-6"
            style={{
              width: `${Math.floor(Math.random() * 50) + 20}%`,
            }}
          />
          <Skeleton className="size-8" />
        </div>
      ))}
    </div>
  )
}
