import {
  add,
  eachMonthOfInterval,
  endOfYear,
  format,
  isEqual,
  parse,
  startOfMonth,
} from 'date-fns'
import { CalendarIcon, ChevronLeft, ChevronRight } from 'lucide-react'
import * as React from 'react'

import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'

import { Button } from './ui/button'

interface MonthPickerProps {
  value?: Date
  onChange?: (newMonth: Date) => void
  disabled?: boolean
}

export function MonthPicker({
  value = new Date(),
  onChange,
  disabled = false,
}: MonthPickerProps) {
  const [isMonthPickerOpen, setIsMonthPickerOpen] = React.useState(false)
  const [currentYear, setCurrentYear] = React.useState(() =>
    format(value, 'yyyy'),
  )

  const firstDayCurrentYear = parse(currentYear, 'yyyy', new Date())

  const months = eachMonthOfInterval({
    start: firstDayCurrentYear,
    end: endOfYear(firstDayCurrentYear),
  })

  function goTreviousYear() {
    const firstDayNextYear = add(firstDayCurrentYear, { years: -1 })
    setCurrentYear(format(firstDayNextYear, 'yyyy'))
  }

  function goToNextYear() {
    const firstDayNextYear = add(firstDayCurrentYear, { years: 1 })
    setCurrentYear(format(firstDayNextYear, 'yyyy'))
  }

  function handleMonthSelection(month: Date) {
    setIsMonthPickerOpen(false)
    onChange?.(month)
  }

  return (
    <Popover open={isMonthPickerOpen} onOpenChange={setIsMonthPickerOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          disabled={disabled}
          className="flex min-w-32 items-center justify-between"
        >
          <CalendarIcon className="mr-1 size-4" />
          {format(value, 'MMM, yyyy')}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="flex w-auto flex-col space-y-4 p-5">
        <div className="relative flex items-center justify-center pt-1">
          <p
            className="text-sm font-medium"
            aria-live="polite"
            role="presentation"
            id="month-picker"
          >
            {format(firstDayCurrentYear, 'yyyy')}
          </p>
          <div className="flex items-center">
            <Button
              name="previous-year"
              aria-label="Go to previous year"
              size="icon"
              variant="outline"
              type="button"
              className="absolute left-1 size-7"
              onClick={goTreviousYear}
            >
              <ChevronLeft className="size-4" />
            </Button>
            <Button
              name="next-year"
              aria-label="Go to next year"
              size="icon"
              variant="outline"
              type="button"
              className="absolute right-1 size-7"
              onClick={goToNextYear}
            >
              <ChevronRight className="size-4" />
            </Button>
          </div>
        </div>
        <div
          className="grid w-full grid-cols-3 gap-px"
          role="grid"
          aria-labelledby="month-picker"
        >
          {months.map((month) => (
            <Button
              key={month.toString()}
              name="day"
              role="gridcell"
              tabIndex={-1}
              variant={
                isEqual(month, startOfMonth(value)) ? 'default' : 'ghost'
              }
              type="button"
              size="sm"
              className="font-normal"
              onClick={() => handleMonthSelection(month)}
            >
              <time dateTime={format(month, 'yyyy-MM')}>
                {format(month, 'MMM')}
              </time>
            </Button>
          ))}
        </div>
      </PopoverContent>
    </Popover>
  )
}
