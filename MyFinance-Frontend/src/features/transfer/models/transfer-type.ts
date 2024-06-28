export const TransferType = {
  Income: 0,
  Outcome: 1,
} as const

export type TransferType = (typeof TransferType)[keyof typeof TransferType]
