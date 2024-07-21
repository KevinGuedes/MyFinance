import { Transfer } from './transfer'

export interface TransferGroup {
  date: Date
  transfers: Transfer[]
  income: number
  outcome: number
  balance: number
}
