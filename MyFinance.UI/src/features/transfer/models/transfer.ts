export interface Transfer {
  id: string
  relatedTo: string
  value: number
  settlementDate: Date
  type: number
  description: string
  categoryName: string
  tag: string
  accountTagId: string
  categoryId: string
}
