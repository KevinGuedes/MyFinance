import { useState } from 'react'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '../../../components/ui/card'
import { Toggle } from '../../../components/ui/toggle'
import { AnnualBalanceChart } from './annual-balance-charts'

export function AnnualBalanceCard() {
  const [hideYAxis, setHideYAxis] = useState(true)

  function toggleYAxis() {
    setHideYAxis((hideYAxis) => !hideYAxis)
  }

  return (
    <Card className="mx-auto size-full">
      <CardHeader className="p-4 pb-0">
        <div className="flex items-center justify-between">
          <CardTitle>Annual Balance Data</CardTitle>
          <Toggle variant="outline" onClick={toggleYAxis} className="shrink-0">
            Toggle Y Axis
          </Toggle>
        </div>
        <CardDescription>
          Income, Outcome and Balance for the last 12 months. Hover the chart or
          the legend for more details
        </CardDescription>
      </CardHeader>
      <CardContent className="px-4 pb-0 pt-2">
        <AnnualBalanceChart hideYAxis={hideYAxis} />
      </CardContent>
    </Card>
  )
}
