import { format } from 'date-fns'
import { Calendar as CalendarIcon } from 'lucide-react'
import { useState } from 'react'
import * as React from 'react'
import { DateRange, SelectSingleEventHandler } from 'react-day-picker'
import { twMerge } from 'tailwind-merge'

import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'

export interface DatePickerProps {
  value?: DateRange
  disabled?: boolean
  onChange: (value: Date | undefined) => void
}

export const RangeDatePicker = React.forwardRef<
  React.ElementRef<typeof Popover>,
  DatePickerProps
>(({ value, disabled, onChange }, ref) => {
  const [isCalendarOpen, setIsCalendarOpen] = useState(false)

  const handleOnSelect: SelectSingleEventHandler = (date) => {
    onChange(date)
    setIsCalendarOpen(false)
  }

  function handlePresetDate(date: Date) {
    onChange(date)
    setIsCalendarOpen(false)
  }

  return (
    <Popover open={isCalendarOpen} onOpenChange={setIsCalendarOpen}>
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          disabled={disabled}
          className={twMerge(
            'w-full justify-start text-left font-normal',
            !value && 'text-muted-foreground',
          )}
        >
          <CalendarIcon className="mr-2 h-4 w-4" />
          {value?.from ? (
            value.to ? (
              <>
                {format(value.from, 'LLL dd, y')} -{' '}
                {format(value.to, 'LLL dd, y')}
              </>
            ) : (
              format(value.from, 'LLL dd, y')
            )
          ) : (
            <span>Pick a date</span>
          )}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="flex w-auto flex-col space-y-2 p-2" ref={ref}>
        <Calendar
          mode="single"
          selected={value}
          defaultMonth={value?.from || new Date()}
          onSelect={handleOnSelect}
          initialFocus
        />
      </PopoverContent>
    </Popover>
  )
})
RangeDatePicker.displayName = 'RangeDatePicker'
