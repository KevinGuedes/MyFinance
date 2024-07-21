import { toMoney } from '@/lib/utils'

import { Transfer } from '../../models/transfer'
import { TransferType } from '../../models/transfer-type'

type TransferItemProps = {
  transfer: Transfer
}

export function TransferItem({
  transfer: { relatedTo, id, description, value, type },
}: TransferItemProps) {
  return (
    <li key={id} className="flex items-start justify-between gap-2 py-1.5">
      <div className="flex flex-col">
        <p className="line-clamp-1 font-medium">{relatedTo}</p>
        <p className="line-clamp-1 text-sm text-muted-foreground">
          WOW - {description}
        </p>
      </div>

      <div className="flex flex-col items-end">
        {type === TransferType.Income ? (
          <p className="text-sm font-semibold text-primary">{toMoney(value)}</p>
        ) : (
          <p className="text-sm font-semibold text-destructive">
            {toMoney(-1 * value)}
          </p>
        )}
        {/* <p className="rounded-lg border p-0.5 text-end text-sm text-muted-foreground">
          BB
        </p> */}
      </div>
    </li>
  )
}
