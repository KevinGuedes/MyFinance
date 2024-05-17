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
