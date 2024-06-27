import {
  NameType,
  Payload,
  ValueType,
} from 'recharts/types/component/DefaultTooltipContent'

import { useDiscriminatedBalanceChartSettings } from '@/features/management-unit/store/discriminated-balance-chart-settings-store'
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
  const { showBalance, showIncome, showOutcome } =
    useDiscriminatedBalanceChartSettings()

  if (active && payload && payload.length) {
    console.log(payload)
    const formattedDate = new Date(
      payload[0].payload.year,
      payload[0].payload.month,
    ).toLocaleDateString('en-US', { year: 'numeric', month: 'long' })

    const balance = Number(
      payload.filter((p) => p.dataKey === 'balance')[0]?.value || 0,
    )

    const income = Number(
      payload.filter((p) => p.dataKey === 'income')[0]?.value || 0,
    )

    const outcome = Number(
      payload.filter((p) => p.dataKey === 'outcome')[0]?.value || 0,
    )

    return (
      <div className="space-y-1.5 rounded-lg border bg-background p-2 shadow-md">
        <p className="text-base font-bold capitalize">{formattedDate}</p>
        <div className="flex gap-3">
          {showBalance && (
            <div className="flex flex-col">
              <p className="text-sm font-bold capitalize text-muted-foreground">
                Balance
              </p>
              <p className="font-bold text-muted-foreground">
                {toMoney(balance)}
              </p>
            </div>
          )}
          {showIncome && (
            <div className="flex flex-col">
              <p className="text-sm font-bold capitalize text-primary">
                Income
              </p>
              <p className="font-bold text-primary">{toMoney(income)}</p>
            </div>
          )}
          {showOutcome && (
            <div className="flex flex-col">
              <p className="text-sm font-bold capitalize text-destructive">
                Outcome
              </p>
              <p className="font-bold text-destructive">
                {toMoney(outcome * -1)}
              </p>
            </div>
          )}
        </div>
      </div>
    )
  }

  return null
}
