import { SummaryCard } from './summary-card'

type SummaryCardsProps = {
  balance: number
  income: number
  outcome: number
}

export function SummaryCards({ balance, income, outcome }: SummaryCardsProps) {
  return (
    <div className="grid grid-cols-1 gap-4 md:grid-cols-3">
      <SummaryCard
        variant="balance"
        amount={balance}
        title="Balance"
        description="Current balance"
      />
      <SummaryCard
        variant="income"
        title="Income"
        amount={income}
        description="Amount received"
      />
      <SummaryCard
        variant="outcome"
        amount={outcome}
        title="Outcome"
        description="Amount transfered"
      />
    </div>
  )
}
