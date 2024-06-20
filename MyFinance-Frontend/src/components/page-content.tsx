import { cn } from '@/lib/utils'

type PageContentProps = React.ComponentProps<'section'>

export function PageContent({ children, className }: PageContentProps) {
  return (
    <section className={cn('flex grow flex-col gap-4', className)}>
      {children}
    </section>
  )
}
