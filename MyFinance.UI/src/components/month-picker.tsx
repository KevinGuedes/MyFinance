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
import { cn } from '@/lib/utils'

import { Button } from './ui/button'

interface MonthPickerProps {
  value?: Date
  onChange?: (newMonth: Date) => void
  disabled?: boolean
  isLoading?: boolean
  closeOnPick?: boolean
}

export function MonthPicker({
  value = new Date(),
  onChange,
  disabled = false,
  isLoading = false,
  closeOnPick = true,
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
    closeOnPick && setIsMonthPickerOpen(false)
    onChange?.(month)
  }

  return (
    <Popover open={isMonthPickerOpen} onOpenChange={setIsMonthPickerOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          label={format(value, 'MMM, yyyy')}
          disabled={disabled}
          isLoading={isLoading}
          className="flex min-w-32 items-center justify-between"
          icon={CalendarIcon}
        />
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
              rel="prev"
              name="previous-year"
              aria-label="Previous Year"
              size="icon-sm"
              variant="outline"
              icon={ChevronLeft}
              type="button"
              className="absolute left-1 size-7 text-muted-foreground"
              onClick={goTreviousYear}
            />
            <Button
              rel="next"
              name="next-year"
              aria-label="Next Year"
              size="icon-sm"
              variant="outline"
              icon={ChevronRight}
              type="button"
              className="absolute right-1 size-7 text-muted-foreground"
              onClick={goToNextYear}
            />
          </div>
        </div>
        <div
          className="grid w-full grid-cols-3 gap-px"
          role="grid"
          aria-labelledby="month-picker"
        >
          {months.map((month) => {
            const isSelectedMonth = isEqual(month, startOfMonth(value))
            const isCurrentMonth = isEqual(month, startOfMonth(new Date()))
            const dateTime = format(month, 'yyyy-MM')
            const label = format(month, 'MMM')

            return (
              <Button
                role="gridcell"
                type="button"
                name="month"
                size="sm"
                key={dateTime}
                tabIndex={isSelectedMonth ? 0 : -1}
                onClick={() => handleMonthSelection(month)}
                variant={isSelectedMonth ? 'default' : 'ghost'}
                className={cn(
                  isSelectedMonth && 'hover:bg-primary',
                  isCurrentMonth &&
                    !isSelectedMonth &&
                    'bg-accent text-accent-foreground',
                  'font-normal',
                )}
              >
                <time dateTime={dateTime}>{label}</time>
              </Button>
            )
          })}
        </div>
      </PopoverContent>
    </Popover>
  )
}
