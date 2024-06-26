import { Settings } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { Checkbox } from '@/components/ui/checkbox'
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

import { useDiscriminatedBalanceChartSettings } from '../../store/discriminated-balance-chart-settings-store'

export function DiscriminatedBalanceChartSettings() {
  const {
    pastMonths,
    showYAxis,
    includeCurrentMonth,
    showLegend,
    showDataPointsWhenHovering,
    toggleShowLegend,
    toggleShowYAxis,
    toggleIncludeCurrentMonth,
    toggleShowDataPointsWhenHovering,
    setPastMonths,
  } = useDiscriminatedBalanceChartSettings()

  return (
    <Dialog>
      <DialogTrigger asChild>
        <Button variant="ghost" size="icon" className="rounded-full">
          <Settings className="size-5" />
          <span className="sr-only">Settings</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-md">
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
        <div className="mt-2 divide-y-[1px]">
          <fieldset className="flex items-center justify-between gap-3 pb-3 pr-2">
            <Label>Show Y-Axis</Label>
            <Checkbox
              name="showYAxis"
              checked={showYAxis}
              onCheckedChange={toggleShowYAxis}
            />
          </fieldset>

          <fieldset className="flex items-center justify-between gap-3 py-3 pr-2">
            <Label>
              Include Current Month
              <p className="mt-1 text-balance text-sm font-normal text-muted-foreground">
                Current Balance, Income and Outcome of the current month will be
                shown in the chart
              </p>
            </Label>
            <Checkbox
              name="includeCurrentMonth"
              checked={includeCurrentMonth}
              onCheckedChange={toggleIncludeCurrentMonth}
            />
          </fieldset>
          <fieldset className="flex items-center justify-between gap-3 py-3 pr-2">
            <Label>
              Show Legend
              <p className="mt-1 text-balance text-sm font-normal text-muted-foreground">
                When one of the legend items is hovered, the regarding data set
                will be highlighted while the other will be faded.
              </p>
            </Label>
            <Checkbox
              name="showLegend"
              checked={showLegend}
              onCheckedChange={toggleShowLegend}
            />
          </fieldset>
          <fieldset className="flex items-center justify-between gap-4 py-3 pr-2">
            <Label>
              Show Data Points when hovering Legend
              <p className="mt-1 text-balance text-sm font-normal text-muted-foreground">
                When hovering the legend, the Y value (R$) of each data point of
                the selected data set will be shown.{' '}
                <strong>
                  Available only if &apos;Show Legend&apos; is checked
                </strong>
              </p>
            </Label>
            <Checkbox
              disabled={!showLegend}
              name="showDataPointsWhenHovering"
              checked={showDataPointsWhenHovering}
              onCheckedChange={toggleShowDataPointsWhenHovering}
            />
          </fieldset>
          <fieldset className="flex items-center justify-between gap-4 pt-3">
            <Label className="w-4/5">Amount of past months to show</Label>
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
      </DialogContent>
    </Dialog>
  )
}
