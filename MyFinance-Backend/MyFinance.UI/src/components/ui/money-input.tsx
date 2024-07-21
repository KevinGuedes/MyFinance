import * as React from 'react'
import CurrencyInput from 'react-currency-input-field'
import { CurrencyInput as CurrencyMask } from 'react-currency-mask'

import { cn } from '@/lib/utils'

import { Input } from './input'

export interface MoneyInputProps {
  value?: number
  disabled?: boolean
  onChange: (value: string | number | undefined) => void
  useMaskInput?: boolean
}

const MoneyInput = React.forwardRef<HTMLInputElement, MoneyInputProps>(
  ({ onChange, value, disabled, useMaskInput = true }, ref) => {
    if (useMaskInput)
      return (
        <CurrencyMask
          value={value}
          defaultValue={0}
          onChangeValue={(_, originalValue) => onChange(originalValue)}
          InputElement={
            <Input
              placeholder="R$ 0,00"
              disabled={disabled}
              inputMode="numeric"
              ref={ref}
            />
          }
        />
      )

    return (
      <CurrencyInput
        className={cn(
          'flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50',
        )}
        ref={ref}
        prefix="R$"
        inputMode="text"
        placeholder="R$ 0,00"
        decimalScale={2}
        decimalsLimit={2}
        allowNegativeValue={false}
        intlConfig={{ locale: 'pt-BR', currency: 'BRL' }}
        value={value}
        disabled={disabled}
        onValueChange={onChange}
      />
    )
  },
)
MoneyInput.displayName = 'MoneyInput'

export { MoneyInput }
