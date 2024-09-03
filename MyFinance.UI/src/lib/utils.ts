import { type ClassValue, clsx } from 'clsx'
import { twMerge } from 'tailwind-merge'

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function toMoney(number: number, useCompactMode = false) {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
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
  const key = Object.keys(enumType).find((key) => enumType[key] === value)
  if (key === undefined) throw new Error('Invalid enum value')
  return key
}

export function isValidEnumValue<T>(enumType: { [key: string]: T }, value: T) {
  return Object.values(enumType).includes(value)
}

export function buildPagination(
  currentPage: number,
  totalPages: number,
  siblingPages: number,
) {
  const pages = new Set<number>()
  pages.add(1)
  pages.add(totalPages)
  pages.add(currentPage)

  if (totalPages > 1) {
    pages.add(2)
  }

  if (totalPages > 2) {
    pages.add(totalPages - 1)
  }

  for (let i = 1; i <= siblingPages; i++) {
    if (currentPage - i > 1) {
      pages.add(currentPage - i)
    }
    if (currentPage + i < totalPages) {
      pages.add(currentPage + i)
    }
  }

  const sortedPages = Array.from(pages).sort((a, b) => a - b)

  const result = []

  for (let i = 0; i < sortedPages.length; i++) {
    result.push(sortedPages[i])
    if (i < sortedPages.length - 1 && sortedPages[i + 1] > sortedPages[i] + 1) {
      result.push(0)
    }
  }

  return result
}

export function buildPaginationInfo(
  page: number,
  pageSize: number,
  totalCount: number,
) {
  const start = (page - 1) * pageSize + 1
  const end = Math.min(page * pageSize, totalCount)

  if (start > totalCount) {
    return { start: totalCount + 1, end: totalCount, total: totalCount }
  }

  return { start, end, total: totalCount }
}

export function getInitials(name: string) {
  const words = name.trim().toUpperCase().split(/\s+/)

  if (words.length <= 2) {
    return words.map((word) => word[0]).join('')
  } else {
    return words[0][0] + words[words.length - 1][0]
  }
}
