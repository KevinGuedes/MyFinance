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

const amountCardVariants = tv({
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

interface AmountCardProps extends VariantProps<typeof amountCardVariants> {
  title: string
  description: string
  amount: number
}

export function AmountCard({
  title,
  description,
  amount,
  variant,
}: AmountCardProps) {
  return (
    <Card>
      <CardHeader className="pb-2">
        <div className="flex justify-between gap-8">
          <div className="space-y-2">
            <CardTitle
              className={cn('line-clamp-1', amountCardVariants({ variant }))}
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
      <CardContent>
        <h2
          className={cn(
            'line-clamp-1 break-all text-2xl font-bold',
            amountCardVariants({ variant }),
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
