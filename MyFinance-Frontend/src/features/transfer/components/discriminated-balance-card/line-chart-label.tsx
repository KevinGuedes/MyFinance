import { LabelProps } from 'recharts'
import { tv, VariantProps } from 'tailwind-variants'

import { cn, toMoney } from '@/lib/utils'

const lineChartLabelVariants = tv({
  base: 'text-sm font-bold',
  variants: {
    variant: {
      balance: 'fill-muted-foreground',
      income: 'fill-primary',
      outcome: 'fill-destructive',
    },
  },
  defaultVariants: {
    variant: 'balance',
  },
})

interface LineChartLabelProps
  extends VariantProps<typeof lineChartLabelVariants>,
    LabelProps {}

export function LineChartLabel(props: LineChartLabelProps) {
  const { x, y, stroke, value, variant } = props

  return (
    <text
      x={x}
      y={y}
      dy={-10}
      dx={0}
      fill={stroke}
      fontSize={10}
      textAnchor="middle"
      className={cn(lineChartLabelVariants({ variant }))}
    >
      {toMoney(Number(value), true)}
    </text>
  )
}
