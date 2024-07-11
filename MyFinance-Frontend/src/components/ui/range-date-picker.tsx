import { format } from 'date-fns'
import { Calendar as CalendarIcon } from 'lucide-react'
import * as React from 'react'
import { DateRange } from 'react-day-picker'

import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'
import { cn } from '@/lib/utils'

export interface RangeDatePickerProps {
  value?: DateRange
  disabled?: boolean
  onChange: (value: DateRange | undefined) => void
}

export const RangeDatePicker = React.forwardRef<
  React.ElementRef<typeof Popover>,
  RangeDatePickerProps
>(({ value, disabled, onChange }, ref) => {
  const [isCalendarOpen, setIsCalendarOpen] = React.useState(false)

  function closeCalendar() {
    setIsCalendarOpen(false)
  }

  function resetCalendar() {
    onChange(undefined)
  }

  return (
    <Popover
      modal={true}
      open={isCalendarOpen}
      onOpenChange={setIsCalendarOpen}
    >
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          disabled={disabled}
          className={cn(
            'w-full justify-start text-left font-normal',
            value?.from && 'text-muted-foreground',
          )}
        >
          <CalendarIcon className="mr-2 h-4 w-4" />
          {value?.from ? (
            value?.to ? (
              <>
                {format(value?.from, 'LLL dd, y')} -{' '}
                {format(value?.to, 'LLL dd, y')}
              </>
            ) : (
              format(value?.from, 'LLL dd, y')
            )
          ) : (
            <span className="text-muted-foreground">Select a date range</span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-auto p-0" align="center" ref={ref}>
        <Calendar
          initialFocus
          mode="range"
          defaultMonth={value?.from}
          selected={{
            from: value?.from,
            to: value?.to,
          }}
          onSelect={onChange}
          numberOfMonths={2}
        />

        <div className="flex justify-end gap-3 px-3 pb-3">
          <Button onClick={resetCalendar} variant="outline">
            Clear
          </Button>
          <Button onClick={closeCalendar}>Apply</Button>
        </div>
      </PopoverContent>
    </Popover>
  )
})
RangeDatePicker.displayName = 'RangeDatePicker'
