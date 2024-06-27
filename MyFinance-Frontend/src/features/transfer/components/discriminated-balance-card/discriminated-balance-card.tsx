import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { MonthlyBalanceData } from '@/features/transfer/models/monthly-balance-data'

import { DiscriminatedBalanceChart } from './discriminated-balance-chart'
import { DiscriminatedBalanceChartSettings } from './discriminated-balance-chart-settings'

type DiscriminatedBalanceCardProps = {
  discriminatedBalanceData: MonthlyBalanceData[]
}

export function DiscriminatedBalanceCard({
  discriminatedBalanceData,
}: DiscriminatedBalanceCardProps) {
  return (
    <Card className="mx-auto flex size-full flex-col">
      <CardHeader className="flex flex-row items-center justify-between gap-0.5 p-4 pb-0">
        <CardTitle className="font-normal">
          Discrimintaded Balance Chart
        </CardTitle>
        <DiscriminatedBalanceChartSettings />
      </CardHeader>
      <CardContent className="flex grow flex-col px-1 pb-1">
        <CardDescription className="px-4">
          Hover the chart for more details about the balance, income and outcome
          of each month
        </CardDescription>
        <div className="h-64 min-h-64 grow pt-2">
          <DiscriminatedBalanceChart
            discriminatedBalanceData={discriminatedBalanceData}
          />
        </div>
      </CardContent>
    </Card>
  )
}
