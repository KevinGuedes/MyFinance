import {
  ChangeEvent,
  FocusEvent,
  forwardRef,
  KeyboardEvent,
  useEffect,
  useState,
} from 'react'

import { Input } from './ui/input'

// Constants for BRL
const MAXIMUM_FRACTION_DIGITS = 2
const MINIMUM_FRACTION_DIGITS = 2
const SYMBOL_LENGTH = 3

function formatCurrency(
  locale: string = 'pt-BR',
  value: number,
  currencyType = 'BRL',
  hideSymbol = false,
) {
  return new Intl.NumberFormat(locale, {
    style: 'currency',
    currency: currencyType,
    minimumFractionDigits: MINIMUM_FRACTION_DIGITS,
    maximumFractionDigits: MAXIMUM_FRACTION_DIGITS,
  })
    .format(value)
    .slice(hideSymbol ? SYMBOL_LENGTH : 0)
}

export const clearNumber = (value: number | string) => {
  if (typeof value === 'number') {
    return value
  }

  return Number(value.toString().replace(/[^0-9-]/g, ''))
}

export const normalizeValue = (value: string | number) => {
  let safeNumber = value

  if (typeof value === 'string') {
    safeNumber = clearNumber(value)

    if (safeNumber % 1 !== 0) {
      safeNumber = safeNumber.toFixed(MAXIMUM_FRACTION_DIGITS)
    }
  } else {
    safeNumber = Number.isInteger(value)
      ? Number(value) * 10 ** MAXIMUM_FRACTION_DIGITS
      : value.toFixed(MAXIMUM_FRACTION_DIGITS)
  }

  return clearNumber(safeNumber) / 10 ** MAXIMUM_FRACTION_DIGITS
}

export const maskValues = (
  locale: string,
  inputFieldValue: string | number | undefined,
  currency: string,
  shouldCutSymbol: boolean,
): [number, string] => {
  if (!inputFieldValue) return [0, '']

  const value = normalizeValue(inputFieldValue)
  const maskedValue = formatCurrency(locale, value, currency, shouldCutSymbol)

  return [value, maskedValue]
}

export interface ICurrencyMaskProps {
  value?: number | string
  max?: number
  currency?: string
  locale?: string
  hideSymbol?: boolean
  autoSelect?: boolean
  onChangeValue: (
    event: ChangeEvent<HTMLInputElement>,
    originalValue: number | string,
    maskedValue: number | string,
  ) => void
  onBlur?: (
    event: FocusEvent<HTMLInputElement, Element>,
    originalValue: number | string,
    maskedValue: number | string,
  ) => void
  onFocus?: (
    event: FocusEvent<HTMLInputElement, Element>,
    originalValue: number | string,
    maskedValue: number | string,
  ) => void
  onKeyPress?: (
    event: KeyboardEvent<HTMLInputElement>,
    originalValue: number | string,
    maskedValue: string,
  ) => void
}

export const CurrencyInput2 = forwardRef<HTMLInputElement, ICurrencyMaskProps>(
  (
    {
      value,
      hideSymbol = false,
      currency = 'BRL',
      locale = 'pt-BR',
      max,
      autoSelect,
      onChangeValue,
      onBlur,
      onFocus,
      onKeyPress,
      ...otherProps
    },
    ref,
  ) => {
    const [maskedValue, setMaskedValue] = useState<number | string>(() => {
      if (!value) return 0

      const [, calculatedMaskedValue] = maskValues(
        locale,
        value,
        currency,
        hideSymbol,
      )

      return calculatedMaskedValue
    })

    function updateValues(originalValue: string | number) {
      const [calculatedValue, calculatedMaskedValue] = maskValues(
        locale,
        originalValue,
        currency,
        hideSymbol,
      )

      if (!max || calculatedValue <= max) {
        setMaskedValue(calculatedMaskedValue)

        return [calculatedValue, calculatedMaskedValue]
      }

      return [normalizeValue(maskedValue), maskedValue]
    }

    function handleChange(event: ChangeEvent<HTMLInputElement>) {
      const [originalValue, maskedValue] = updateValues(event.target.value)
      onChangeValue(event, originalValue, maskedValue)
    }

    function handleBlur(event: FocusEvent<HTMLInputElement, Element>) {
      if (onBlur) {
        const [originalValue, maskedValue] = updateValues(event.target.value)
        onBlur(event, originalValue, maskedValue)
      }
    }

    function handleFocus(event: FocusEvent<HTMLInputElement, Element>) {
      if (autoSelect) {
        event.target.select()
      }

      const [originalValue, maskedValue] = updateValues(event.target.value)

      if (maskedValue && onFocus) {
        onFocus(event, originalValue, maskedValue)
      }
    }

    function handleKeyUp(event: KeyboardEvent<HTMLInputElement>) {
      onKeyPress && onKeyPress(event, event.key, event.key)
    }

    useEffect(() => {
      const currentValue = value || undefined
      const [, maskedValue] = maskValues(
        locale,
        currentValue,
        currency,
        hideSymbol,
      )

      setMaskedValue(maskedValue)
    }, [currency, hideSymbol, value, locale])

    return (
      <div className="flex items-center gap-2">
        <p className="text-base text-muted-foreground">R$</p>
        <Input
          placeholder="0,00"
          inputMode="numeric"
          ref={ref}
          {...otherProps}
          value={maskedValue}
          onChange={handleChange}
          onBlur={handleBlur}
          onFocus={handleFocus}
          onKeyUp={handleKeyUp}
        />
      </div>
    )
  },
)
CurrencyInput2.displayName = 'CurrencyInput2'
