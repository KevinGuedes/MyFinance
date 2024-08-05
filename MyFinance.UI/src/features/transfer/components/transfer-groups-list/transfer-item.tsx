import { toMoney } from '@/lib/utils'

import { Transfer } from '../../models/transfer'
import { TransferType } from '../../models/transfer-type'
import { TransferItemActions } from './transfer-item-actions'

type TransferItemProps = {
  transfer: Transfer
}

export function TransferItem({ transfer }: TransferItemProps) {
  return (
    <li key={transfer.id} className="group flex flex-col gap-1 py-2">
      <div className="flex items-center justify-between">
        <p className="line-clamp-1 font-medium">{transfer.relatedTo}</p>

        <div className="flex items-center gap-2">
          {transfer.type === TransferType.Income ? (
            <p className="font-semibold text-primary">
              {toMoney(transfer.value)}
            </p>
          ) : (
            <p className="font-semibold text-destructive">
              {toMoney(-1 * transfer.value)}
            </p>
          )}
          <TransferItemActions transfer={transfer} />
        </div>
      </div>

      <div className="flex flex-col justify-between gap-1">
        <div className="flex items-center justify-between gap-2">
          <p className="line-clamp-1 text-sm text-muted-foreground">
            {transfer.description}
          </p>
          <div className="mr-8 flex shrink-0 gap-1">
            <p className="rounded-md border px-1 py-px text-sm text-muted-foreground">
              {transfer.tag}
            </p>
            <p className="rounded-md border px-1 py-px text-sm text-muted-foreground">
              {transfer.categoryName}
            </p>
          </div>
        </div>
      </div>
    </li>
  )
}
