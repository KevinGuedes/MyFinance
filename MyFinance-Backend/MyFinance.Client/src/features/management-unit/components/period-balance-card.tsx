import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

import { PeriodBalanceChart } from './period-balance-chart'

export function PeriodBalanceCard() {
  return (
    <Card className="mx-auto w-full max-w-2xl">
      <CardHeader>
        <CardTitle>Period Balance Data</CardTitle>
        <CardDescription>
          Income, Outcome and Balance of the selected period. Hover the chart or
          the legend for more details
        </CardDescription>
      </CardHeader>
      <CardContent>
        <PeriodBalanceChart />
      </CardContent>
    </Card>
  )
}
