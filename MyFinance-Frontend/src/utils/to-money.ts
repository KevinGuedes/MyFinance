export function toMoney(number: number, shortFormatting = false) {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    ...(shortFormatting && {
      currencyDisplay: 'narrowSymbol',
      notation: 'compact',
    }),
  }).format(number)
}
