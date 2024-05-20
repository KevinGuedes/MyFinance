import { AmountCard } from './amount-card'

export function AmountCards() {
  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
      <AmountCard
        variant="balance"
        amount={2345}
        title="Balance"
        description="Current balance"
      />
      <AmountCard
        variant="income"
        title="Income"
        amount={54323}
        description="Amount received"
      />
      <AmountCard
        variant="outcome"
        amount={-30200}
        title="Outcome"
        description="Amount transfered"
      />
    </div>
  )
}
