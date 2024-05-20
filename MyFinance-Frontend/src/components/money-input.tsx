import * as React from 'react'
import CurrencyInput from 'react-currency-input-field'

import { cn } from '@/lib/utils'

export interface MoneyInputProps {
  value?: string
  disabled?: boolean
  onChange: (value: string | undefined) => void
}

const MoneyInput = React.forwardRef<HTMLInputElement, MoneyInputProps>(
  ({ onChange, value, disabled }, ref) => {
    return (
      <CurrencyInput
        className={cn(
          'flex h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background file:border-0 file:bg-transparent file:text-sm file:font-medium placeholder:text-muted-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50',
        )}
        ref={ref}
        decimalScale={2}
        prefix="R$"
        decimalsLimit={2}
        intlConfig={{ locale: 'pt-BR', currency: 'BRL' }}
        placeholder="R$ 0,00"
        value={value}
        disabled={disabled}
        onValueChange={onChange}
      />
    )
  },
)
MoneyInput.displayName = 'MoneyInput'

export { MoneyInput }