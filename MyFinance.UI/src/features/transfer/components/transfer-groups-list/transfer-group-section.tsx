import { format } from 'date-fns'
import { PiggyBank, TrendingDown, TrendingUp } from 'lucide-react'

import { toMoney } from '@/lib/utils'

import { TransferGroup } from '../../models/transfer-group'
import { TransferItem } from './transfer-item'

type TransferGroupProps = {
  transferGroup: TransferGroup
}

export function TransferGroupSection({ transferGroup }: TransferGroupProps) {
  return (
    <article className="space-y-2 pr-2">
      <div className="sticky top-0 bg-background">
        <h3 className="mb-2 line-clamp-1 rounded-md bg-muted/40 px-4 py-2 font-medium">
          {format(transferGroup.date, 'EEEE dd, MMMM yyyy')}
        </h3>
      </div>
      <header className="space-y-2 px-1">
        <h4 className="text-base font-medium text-muted-foreground">
          Daily Summary
        </h4>
        <div className="flex justify-between gap-2 sm:flex-row sm:items-center">
          <div className="space-y-1">
            <div className="flex gap-2">
              <PiggyBank className="inline-block size-7 shrink-0 rounded-md bg-muted-foreground/30 p-1 text-muted-foreground" />
              <p className="hidden font-bold text-muted-foreground sm:block">
                Balance
              </p>
            </div>
            <p className="text-start font-bold text-muted-foreground">
              {toMoney(transferGroup.balance, true)}
            </p>
          </div>
          <div className="space-y-1">
            <div className="flex gap-2">
              <TrendingUp className="inline-block size-7 shrink-0 rounded-md bg-primary/10 p-1 text-primary" />
              <p className="hidden font-bold text-primary sm:block">Income</p>
            </div>
            <p className="text-start font-bold text-primary">
              {toMoney(transferGroup.income, true)}
            </p>
          </div>
          <div className="space-y-1">
            <div className="flex gap-2">
              <TrendingDown className="inline-block size-7 shrink-0 rounded-md bg-destructive/25 p-1 text-destructive" />
              <p className="hidden font-bold text-destructive sm:block">
                Outcome
              </p>
            </div>
            <p className="font-bold text-destructive sm:text-end">
              {toMoney(transferGroup.outcome * -1, true)}
            </p>
          </div>
        </div>
      </header>
      <section>
        <ul className="list-none divide-y-[1px] border-t">
          {transferGroup.transfers.map((transfer) => (
            <TransferItem key={transfer.id} transfer={transfer} />
          ))}
        </ul>
      </section>
    </article>
  )
}
