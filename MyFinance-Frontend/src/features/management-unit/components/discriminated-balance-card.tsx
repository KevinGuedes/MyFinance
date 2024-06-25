import { useState } from 'react'

import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Separator } from '@/components/ui/separator'
import { MonthlyBalanceData } from '@/features/transfer/models/monthly-balance-data'

import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from '../../../components/ui/card'
import { Toggle } from '../../../components/ui/toggle'
import { DiscriminatedBalanceChart } from './discriminated-balance-charts'

type DiscriminatedBalanceCardProps = {
  discriminatedBalanceData: MonthlyBalanceData[]
  pastMonths: number
  onSelectPastMonths: (months: number) => void
}

export function DiscriminatedBalanceCard({
  discriminatedBalanceData,
  pastMonths,
  onSelectPastMonths,
}: DiscriminatedBalanceCardProps) {
  const [hideYAxis, setHideYAxis] = useState(true)

  function toggleYAxis() {
    setHideYAxis((hideYAxis) => !hideYAxis)
  }

  function handlePastMonthsSelection(pastMonthsAsString: string) {
    onSelectPastMonths(parseInt(pastMonthsAsString))
  }

  return (
    <Card className="mx-auto flex size-full flex-col">
      <CardHeader className="flex flex-row items-center justify-between gap-0.5 p-4 pb-0">
        <CardTitle className="font-normal">Annual Balance Data</CardTitle>
        <div className="flex items-center gap-4 text-muted-foreground">
          <div className="flex items-center gap-1.5 text-sm">
            Showing last
            <Select
              defaultValue={pastMonths.toString()}
              onValueChange={handlePastMonthsSelection}
            >
              <SelectTrigger className="h-9 w-14">
                <SelectValue placeholder="Choose a Category" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="12">12</SelectItem>
                <SelectItem value="9">9</SelectItem>
                <SelectItem value="6">6</SelectItem>
                <SelectItem value="3">3</SelectItem>
              </SelectContent>
            </Select>
            months
          </div>
          <Separator orientation="vertical" className="h-9 w-px" />
          <Toggle
            variant="outline"
            onClick={toggleYAxis}
            size="sm"
            className="shrink-0"
          >
            Toggle Y Axis
          </Toggle>
        </div>
      </CardHeader>
      <CardContent className="flex grow flex-col px-1 pb-1 pt-2">
        <div className="h-64 min-h-64 grow">
          <DiscriminatedBalanceChart
            hideYAxis={hideYAxis}
            discriminatedBalanceData={discriminatedBalanceData}
          />
        </div>
      </CardContent>
    </Card>
  )
}
