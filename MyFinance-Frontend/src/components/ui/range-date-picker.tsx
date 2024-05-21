import { format } from 'date-fns'
import { Calendar as CalendarIcon } from 'lucide-react'
import * as React from 'react'
import { DateRange, SelectRangeEventHandler } from 'react-day-picker'

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
  const handleOnSelect: SelectRangeEventHandler = (dateRange) => {
    onChange(dateRange)
  }

  return (
    <Popover modal={true}>
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
            <span>Pick a date</span>
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
          onSelect={handleOnSelect}
          numberOfMonths={2}
        />
      </PopoverContent>
    </Popover>
  )
})
RangeDatePicker.displayName = 'RangeDatePicker'
