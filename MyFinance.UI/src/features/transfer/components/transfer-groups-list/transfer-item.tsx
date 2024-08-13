import { toMoney } from '@/lib/utils'

import { Transfer } from '../../models/transfer'
import { TransferType } from '../../models/transfer-type'
import { TransferItemActions } from './transfer-item-actions'

type TransferItemProps = {
  transfer: Transfer
}

export function TransferItem({ transfer }: TransferItemProps) {
  return (
    <li
      key={transfer.id}
      className="flex justify-between gap-2 py-2 last-of-type:pb-0"
    >
      <div className="flex w-full flex-col gap-1">
        <div className="flex items-center justify-between gap-2">
          <p className="line-clamp-1 font-medium">{transfer.relatedTo}</p>

          {transfer.type === TransferType.Income ? (
            <p className="shrink-0 font-semibold text-primary">
              {toMoney(transfer.value)}
            </p>
          ) : (
            <p className="shrink-0 font-semibold text-destructive">
              {toMoney(-1 * transfer.value)}
            </p>
          )}
        </div>

        <div className="flex items-center justify-between gap-2">
          <p className="line-clamp-1 text-sm text-muted-foreground">
            {transfer.description}
          </p>
          <div className="flex shrink-0 gap-2">
            <p className="rounded-sm border px-1 text-sm text-muted-foreground">
              {transfer.tag}
            </p>
            <p className="rounded-sm border px-1 text-sm text-muted-foreground">
              {transfer.categoryName}
            </p>
          </div>
        </div>
      </div>

      <TransferItemActions transfer={transfer} />
    </li>
  )
}
