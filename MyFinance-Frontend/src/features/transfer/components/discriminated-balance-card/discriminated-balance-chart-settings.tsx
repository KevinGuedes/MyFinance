import { Settings } from 'lucide-react'

import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Switch } from '@/components/ui/switch'

import { useDiscriminatedBalanceChartSettings } from '../../../management-unit/store/discriminated-balance-chart-settings-store'

export function DiscriminatedBalanceChartSettings() {
  const {
    pastMonths,
    showYAxis,
    includeCurrentMonth,
    showLegend,
    showBalance,
    showIncome,
    showOutcome,
    showDataPointsWhenHovering,
    toggleShowLegend,
    toggleShowBalance,
    toggleShowIncome,
    toggleShowOutcome,
    toggleShowYAxis,
    toggleIncludeCurrentMonth,
    toggleShowDataPointsWhenHovering,
    setPastMonths,
  } = useDiscriminatedBalanceChartSettings()

  function handleToggleShowLegend() {
    if (showDataPointsWhenHovering) toggleShowDataPointsWhenHovering()
    toggleShowLegend()
  }

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="ghost" size="icon" className="rounded-full">
          <Settings className="size-5" />
          <span className="sr-only">Settings</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Discriminated Balance Chart Settings</DialogTitle>
          <DialogDescription>
            Select the settings according to your preferences.{' '}
            <strong>
              These settings will be applied for all Discriminated Balance
              Charts.
            </strong>
          </DialogDescription>
        </DialogHeader>
        <div className="flex flex-col gap-6 sm:flex-row sm:gap-8">
          <div className="flex flex-col gap-6 sm:w-1/2">
            <fieldset className="flex items-center justify-between gap-4">
              <Label>Show Balance</Label>
              <Switch
                name="showBalance"
                checked={showBalance}
                onCheckedChange={toggleShowBalance}
              />
            </fieldset>
            <fieldset className="flex items-center justify-between gap-4">
              <Label>Show Income</Label>

              <Switch
                name="showIncome"
                checked={showIncome}
                onCheckedChange={toggleShowIncome}
              />
            </fieldset>
            <fieldset className="flex items-center justify-between gap-4">
              <Label>Show Outcome</Label>
              <Switch
                name="showOutcome"
                checked={showOutcome}
                onCheckedChange={toggleShowOutcome}
              />
            </fieldset>

            <fieldset className="flex items-center justify-between gap-4">
              <Label>
                Include Current Month
                <p className="mt-1 text-sm font-normal text-muted-foreground">
                  Current Balance, Income and Outcome of the current month will
                  be shown in the chart
                </p>
              </Label>
              <Switch
                name="includeCurrentMonth"
                checked={includeCurrentMonth}
                onCheckedChange={toggleIncludeCurrentMonth}
              />
            </fieldset>
            <fieldset className="flex items-center justify-between gap-4">
              <Label>Amount of past months to show</Label>
              <Select
                name="pastMonths"
                defaultValue={pastMonths.toString()}
                onValueChange={(pastMonths) =>
                  setPastMonths(parseInt(pastMonths))
                }
              >
                <SelectTrigger className="w-14">
                  <SelectValue placeholder="Past months">
                    {pastMonths}
                  </SelectValue>
                </SelectTrigger>
                <SelectContent>
                  {[3, 6, 9, 12].map((months) => (
                    <SelectItem key={months} value={months.toString()}>
                      {months}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </fieldset>
          </div>

          <div className="flex grow flex-col justify-between gap-6 sm:w-1/2">
            <fieldset className="flex items-center justify-between gap-4">
              <Label>Show Y-Axis</Label>
              <Switch
                name="showYAxis"
                checked={showYAxis}
                onCheckedChange={toggleShowYAxis}
              />
            </fieldset>
            <fieldset className="flex items-center justify-between gap-4">
              <Label>
                Show Legend
                <p className="mt-1 text-sm font-normal text-muted-foreground">
                  When one of the legend items is hovered, the regarding data
                  set will be highlighted while the others will be faded
                </p>
              </Label>
              <Switch
                name="showLegend"
                checked={showLegend}
                onCheckedChange={handleToggleShowLegend}
              />
            </fieldset>
            <fieldset className="flex items-center justify-between gap-4">
              <Label>
                Show Data Points when hovering Legend
                <p className="mt-1 text-sm font-normal text-muted-foreground">
                  When hovering the legend, the Y value (R$) of each data point
                  of the selected data set will be shown.{' '}
                  <strong>
                    Available only if &apos;Show Legend&apos; is checked
                  </strong>
                </p>
              </Label>
              <Switch
                disabled={!showLegend}
                name="showDataPointsWhenHovering"
                checked={showDataPointsWhenHovering}
                onCheckedChange={toggleShowDataPointsWhenHovering}
              />
            </fieldset>
          </div>
        </div>
      </DialogContent>
    </Dialog>
  )
}
