import { type ClassValue, clsx } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function toMoney(number: number, useCompactMode = false) {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    ...(useCompactMode && {
      currencyDisplay: 'narrowSymbol',
      notation: 'compact',
    }),
  }).format(number)
}

export function getEnumKeys<T>(
  enumType: {
    [key: string]: T
  },
  includeDefaultOption: boolean = true,
) {
  if (includeDefaultOption)
    return Object.keys(enumType).concat('') as [string, ...string[]]

  return Object.keys(enumType) as [string, ...string[]]
}

export function isValidEnumKey<T>(
  enumType: { [key: string]: T },
  value?: string,
): boolean {
  if (value === undefined) return false
  return Object.keys(enumType).includes(value)
}

export function getEnumValueByKey<T>(
  enumType: { [key: string]: T },
  key: string,
): T {
  const enumKey = key as keyof typeof enumType
  return enumType[enumKey]
}

export function getKeyByEnumValue<T>(
  enumType: { [key: string]: T },
  value: T,
): string {
  return Object.keys(enumType).find((key) => enumType[key] === value) || ''
}
