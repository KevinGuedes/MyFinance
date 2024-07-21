export interface Transfer {
  id: string
  relatedTo: string
  value: number
  settlementDate: Date
  type: number
  accountTag: string
  category: string
  description: string
}
