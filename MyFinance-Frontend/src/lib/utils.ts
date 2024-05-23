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
