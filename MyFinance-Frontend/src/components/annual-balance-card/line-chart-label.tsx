import { LabelProps } from 'recharts'

import { toMoney } from '@/utils/to-money'

export function LineChartLabel(labelData: LabelProps) {
  const { x, y, stroke, value } = labelData

  return (
    <text
      x={x}
      y={y}
      dy={-10}
      dx={0}
      fill={stroke}
      fontSize={10}
      textAnchor="middle"
      className="fill-destructive text-xs font-bold"
    >
      {toMoney(Number(value), true)}
    </text>
  )
}
