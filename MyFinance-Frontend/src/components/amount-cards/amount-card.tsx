import CountUp from 'react-countup'
import { tv, type VariantProps } from 'tailwind-variants'

import { cn, toMoney } from '@/lib/utils'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '../ui/card'

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
        <CardTitle
          className={cn('line-clamp-1', amountCardVariants({ variant }))}
        >
          {title}
        </CardTitle>
        <CardDescription className="line-clamp-1">
          {description}
        </CardDescription>
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
