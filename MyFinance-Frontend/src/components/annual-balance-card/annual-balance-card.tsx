import { useState } from 'react'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '../ui/card'
import { Toggle } from '../ui/toggle'
import { AnnualBalanceChart } from './annual-balance-charts'

export function AnnualBalanceCard() {
  const [hideYAxis, setHideYAxis] = useState(true)

  function toggleYAxis() {
    setHideYAxis((hideYAxis) => !hideYAxis)
  }

  return (
    <Card className="mx-auto w-full max-w-2xl">
      <CardHeader>
        <CardTitle className="text-2xl">Annual Balance Data</CardTitle>
        <CardDescription>
          <span className="flex items-center justify-between">
            Income, Outcome and Balance for the last 12 months
            <Toggle variant="outline" onClick={toggleYAxis}>
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
