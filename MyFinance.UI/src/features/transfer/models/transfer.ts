import { TransferType } from './transfer-type'

export interface Transfer {
  id: string
  relatedTo: string
  value: number
  settlementDate: Date
  type: TransferType
  description: string
  categoryName: string
  tag: string
  accountTagId: string
  categoryId: string
}
