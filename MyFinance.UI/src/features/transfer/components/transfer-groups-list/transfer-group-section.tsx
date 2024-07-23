import { format } from 'date-fns'
import { PiggyBank, TrendingDown, TrendingUp } from 'lucide-react'

import { toMoney } from '@/lib/utils'

import { TransferGroup } from '../../models/transfer-group'
import { TransferItem } from './transfer-item'

type TransferGroupProps = {
  transferGroup: TransferGroup
}

export function TransferGroupSection({ transferGroup }: TransferGroupProps) {
  const formattedGroupDate = format(transferGroup.date, 'EEEE dd, MMMM yyyy')

  return (
    <article className="space-y-1 pr-2">
      <div className="sticky top-0 bg-background">
        <h3 className="mb-2 rounded-lg bg-muted/40 px-4 py-2 font-medium">
          {formattedGroupDate}
        </h3>
      </div>
      <header className="space-y-1.5 pb-2">
        <h4 className="px-1 text-base font-medium text-muted-foreground">
          Daily Balance
        </h4>
        <div className="flex items-center justify-between px-1">
          <div className="space-y-1">
            <div className="flex gap-2">
              <PiggyBank className="inline-block size-6 shrink-0 rounded-md bg-muted-foreground/30 p-1 text-muted-foreground" />
              <p className="text-sm font-bold text-muted-foreground">Income</p>
            </div>
            <p className="text-start text-sm font-bold text-muted-foreground">
              {toMoney(transferGroup.balance, true)}
            </p>
          </div>
          <div className="space-y-1">
            <div className="flex gap-2">
              <TrendingUp className="inline-block size-6 shrink-0 rounded-md bg-primary/10 p-1 text-primary" />
              <p className="text-sm font-bold text-primary">Income</p>
            </div>
            <p className="text-start text-sm font-bold text-primary">
              {toMoney(transferGroup.income, true)}
            </p>
          </div>
          <div className="space-y-1">
            <div className="flex gap-2">
              <TrendingDown className="inline-block size-6 shrink-0 rounded-md bg-destructive/25 p-1 text-destructive" />
              <p className="text-sm font-bold text-destructive">Outcome</p>
            </div>
            <p className="text-end text-sm font-bold text-destructive">
              {toMoney(transferGroup.outcome * -1, true)}
            </p>
          </div>
        </div>
      </header>
      <section className="px-1">
        <ul className="list-none gap-6 divide-y-[1px] first:border-t last:border-b">
          {transferGroup.transfers.map((transfer) => (
            <TransferItem key={transfer.id} transfer={transfer} />
          ))}
        </ul>
      </section>
    </article>
  )
}
