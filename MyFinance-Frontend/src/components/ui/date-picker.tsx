import { format } from 'date-fns'
import { Calendar as CalendarIcon } from 'lucide-react'
import { useState } from 'react'
import * as React from 'react'
import { SelectSingleEventHandler } from 'react-day-picker'
import { twMerge } from 'tailwind-merge'

import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'

export interface DatePickerProps {
  value?: Date
  disabled?: boolean
  onChange: (value: Date | undefined) => void
}

export const DatePicker = React.forwardRef<
  React.ElementRef<typeof Popover>,
  DatePickerProps
>(({ value, disabled, onChange }, ref) => {
  const [isCalendarOpen, setIsCalendarOpen] = useState(false)

  const handleOnSelect: SelectSingleEventHandler = (date) => {
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
          {value ? format(value, 'PPP') : <span>Pick a value</span>}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-auto p-0" ref={ref}>
        <Calendar
          mode="single"
          selected={value}
          defaultMonth={value || new Date()}
          onSelect={handleOnSelect}
          initialFocus
        />
      </PopoverContent>
    </Popover>
  )
})
DatePicker.displayName = 'DatePicker'
