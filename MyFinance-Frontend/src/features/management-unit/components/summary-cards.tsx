import { SummaryCard } from './summary-card'

export function SummaryCards() {
  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
      <SummaryCard
        variant="balance"
        amount={2345}
        title="Balance"
        description="Current balance"
      />
      <SummaryCard
        variant="income"
        title="Income"
        amount={54323}
        description="Amount received"
      />
      <SummaryCard
        variant="outcome"
        amount={-30200}
        title="Outcome"
        description="Amount transfered"
      />
    </div>
  )
}
