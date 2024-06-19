import { PiggyBank, TrendingDown, TrendingUp } from 'lucide-react'
import CountUp from 'react-countup'
import { tv, type VariantProps } from 'tailwind-variants'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { cn, toMoney } from '@/lib/utils'

const summaryCardVariants = tv({
  base: 'font-bold',
  variants: {
    variant: {
      balance: 'text-muted-foreground',
      income: 'text-primary',
      outcome: 'text-destructive',
    },
  },
  defaultVariants: {
    variant: 'balance',
  },
})

interface SummaryCardProps extends VariantProps<typeof summaryCardVariants> {
  title: string
  description: string
  amount: number
}

export function SummaryCard({
  title,
  description,
  amount,
  variant,
}: SummaryCardProps) {
  return (
    <Card>
      <CardHeader className="p-4">
        <div className="flex justify-between gap-8">
          <div className="space-y-2">
            <CardTitle
              className={cn('line-clamp-1', summaryCardVariants({ variant }))}
            >
              {title}
            </CardTitle>
            <CardDescription className="line-clamp-1">
              {description}
            </CardDescription>
          </div>
          {variant === 'income' && (
            <TrendingUp className="inline-block size-10 rounded-md bg-primary/10 p-2 text-primary" />
          )}
          {variant === 'outcome' && (
            <TrendingDown className="inline-block size-10 rounded-md bg-destructive/25 p-2 text-destructive" />
          )}
          {variant === 'balance' && (
            <PiggyBank className="inline-block size-10 rounded-md bg-muted-foreground/30 p-2 text-muted-foreground" />
          )}
        </div>
      </CardHeader>
      <CardContent className="px-4 pb-4">
        <h2
          className={cn(
            'line-clamp-1 break-all text-2xl font-bold',
            summaryCardVariants({ variant }),
          )}
        >
          <CountUp
            preserveValue
            start={0}
            end={amount}
            decimals={2}
            decimalPlaces={2}
            formattingFn={toMoney}
          />
        </h2>
      </CardContent>
    </Card>
  )
}
