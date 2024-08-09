import {
  add,
  eachMonthOfInterval,
  endOfYear,
  format,
  isEqual,
  isFuture,
  parse,
  startOfMonth,
  startOfToday,
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
}

export function MonthPicker({
  value = new Date(),
  onChange,
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

  function previousYear() {
    const firstDayNextYear = add(firstDayCurrentYear, { years: -1 })
    setCurrentYear(format(firstDayNextYear, 'yyyy'))
  }

  function nextYear() {
    const firstDayNextYear = add(firstDayCurrentYear, { years: 1 })
    setCurrentYear(format(firstDayNextYear, 'yyyy'))
  }

  return (
    <Popover open={isMonthPickerOpen} onOpenChange={setIsMonthPickerOpen}>
      <PopoverTrigger asChild>
        <Button variant="outline" size="icon">
          <CalendarIcon className="size-4" />
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
              onClick={previousYear}
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
              onClick={nextYear}
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
                isEqual(month, startOfMonth(startOfToday()))
                  ? 'default'
                  : 'ghost'
              }
              type="button"
              size="sm"
              className="font-normal"
              onClick={() => onChange?.(month)}
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
