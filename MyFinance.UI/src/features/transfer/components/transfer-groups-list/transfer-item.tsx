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
    <li key={id} className="group flex items-start justify-between gap-2 py-2">
      <div className="flex flex-col">
        <p className="line-clamp-1 font-medium">{relatedTo}</p>
        <p className="line-clamp-1 text-sm text-muted-foreground">
          {description}
        </p>
      </div>

      <div className="flex flex-col items-end">
        {type === TransferType.Income ? (
          <p className="font-semibold text-primary">{toMoney(value)}</p>
        ) : (
          <p className="font-semibold text-destructive">
            {toMoney(-1 * value)}
          </p>
        )}
      </div>
    </li>
  )
}
