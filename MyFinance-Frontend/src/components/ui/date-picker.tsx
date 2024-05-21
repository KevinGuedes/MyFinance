import { addDays, format } from 'date-fns'
import { Calendar as CalendarIcon } from 'lucide-react'
import { useState } from 'react'
import * as React from 'react'
import { SelectSingleEventHandler } from 'react-day-picker'

import { Button } from '@/components/ui/button'
import { Calendar } from '@/components/ui/calendar'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { cn } from '@/lib/utils'

export interface DatePickerProps {
  value?: Date
  disabled?: boolean
  showPresetDates?: boolean
  onChange: (value: Date | undefined) => void
}

export const DatePicker = React.forwardRef<
  React.ElementRef<typeof Popover>,
  DatePickerProps
>(({ value, disabled, showPresetDates = false, onChange }, ref) => {
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
    <Popover
      open={isCalendarOpen}
      onOpenChange={setIsCalendarOpen}
      modal={true}
    >
      <PopoverTrigger asChild>
        <Button
          variant="outline"
          disabled={disabled}
          className={cn(
            'w-full justify-start text-left font-normal',
            !value && 'text-muted-foreground',
          )}
        >
          <CalendarIcon className="mr-2 h-4 w-4" />
          {value ? format(value, 'PPP') : <span>Pick a value</span>}
        </Button>
      </PopoverTrigger>
      <PopoverContent className="flex w-auto flex-col space-y-2 p-2" ref={ref}>
        {showPresetDates ? (
          <>
            <Select
              onValueChange={(presetDate) =>
                handlePresetDate(addDays(new Date(), parseInt(presetDate)))
              }
            >
              <SelectTrigger>
                <SelectValue placeholder="Select a preset date" />
              </SelectTrigger>
              <SelectContent position="popper">
                <SelectItem value="-1">Yesterday</SelectItem>
                <SelectItem value="1">Tomorrow</SelectItem>
              </SelectContent>
            </Select>
            <div className="rounded-md border">
              <Calendar
                mode="single"
                selected={value}
                defaultMonth={value || new Date()}
                onSelect={handleOnSelect}
                initialFocus
              />
            </div>
          </>
        ) : (
          <Calendar
            mode="single"
            selected={value}
            defaultMonth={value || new Date()}
            onSelect={handleOnSelect}
            initialFocus
          />
        )}
      </PopoverContent>
    </Popover>
  )
})
DatePicker.displayName = 'DatePicker'
