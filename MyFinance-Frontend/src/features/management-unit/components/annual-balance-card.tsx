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
    <Card className="mx-auto w-full max-w-2xl">
      <CardHeader>
        <CardTitle>Annual Balance Data</CardTitle>
        <CardDescription>
          <span className="flex items-center justify-between gap-4">
            Income, Outcome and Balance for the last 12 months. Hover the chart
            or the legend for more details
            <Toggle
              variant="outline"
              onClick={toggleYAxis}
              className="shrink-0"
            >
              Toggle Y Axis
            </Toggle>
          </span>
        </CardDescription>
      </CardHeader>
      <CardContent>
        <AnnualBalanceChart hideYAxis={hideYAxis} />
      </CardContent>
    </Card>
  )
}
