import {
  NameType,
  Payload,
  ValueType,
} from 'recharts/types/component/DefaultTooltipContent'

import { toMoney } from '@/lib/utils'

type BalanceDataTooltipProps = {
  active: boolean | undefined
  label: string
  payload: Payload<ValueType, NameType>[] | undefined
}

export function BalanceDataTooltip({
  active,
  payload,
}: BalanceDataTooltipProps) {
  if (active && payload && payload.length) {
    const formattedDate = new Date(
      payload[0].payload.year,
      payload[0].payload.month,
    ).toLocaleDateString('en-US', { year: 'numeric', month: 'long' })

    return (
      <div className="space-y-1.5 rounded-lg border bg-background p-2 shadow-md">
        <p className="text-base font-bold capitalize">{formattedDate}</p>
        <div className="grid grid-cols-3 gap-3">
          <div className="flex flex-col">
            <p className="text-sm font-bold capitalize text-muted-foreground">
              Balance
            </p>
            <p className="font-bold text-muted-foreground">
              {toMoney(Number(payload[0].value))}
            </p>
          </div>
          <div className="flex flex-col">
            <p className="text-sm font-bold capitalize text-primary">Income</p>
            <p className="font-bold text-primary">
              {toMoney(Number(payload[1].value))}
            </p>
          </div>
          <div className="flex flex-col">
            <p className="text-sm font-bold capitalize text-destructive">
              Outcome
            </p>
            <p className="font-bold text-destructive">
              -{toMoney(Number(payload[2].value))}
            </p>
          </div>
        </div>
      </div>
    )
  }

  return null
}
