import { MoreVertical } from 'lucide-react'

import { Button } from '@/components/ui/button'
import { toMoney } from '@/lib/utils'

import { Transfer } from '../../models/transfer'
import { TransferType } from '../../models/transfer-type'

type TransferItemProps = {
  transfer: Transfer
}

export function TransferItem({
  transfer: { relatedTo, id, description, value, type, tag, categoryName },
}: TransferItemProps) {
  return (
    <li key={id} className="group flex flex-col gap-0.5 py-2">
      <div className="flex items-center justify-between">
        <p className="line-clamp-1 font-medium">{relatedTo}</p>

        <div className="flex items-center gap-2">
          {type === TransferType.Income ? (
            <p className="font-semibold text-primary">{toMoney(value)}</p>
          ) : (
            <p className="font-semibold text-destructive">
              {toMoney(-1 * value)}
            </p>
          )}
          <Button variant="ghost" className="size-7 p-0.5" size="icon">
            <MoreVertical className="size-4" />
          </Button>
        </div>
      </div>

      <div className="flex flex-col justify-between gap-1">
        <div className="flex items-center justify-between gap-2">
          <p className="line-clamp-1 text-sm text-muted-foreground">
            {description}
          </p>
          <div className="mr-8 flex shrink-0 gap-1">
            <p className="rounded-md border px-1 py-px text-sm text-muted-foreground">
              {tag}
            </p>
            <p className="rounded-md border px-1 py-px text-sm text-muted-foreground">
              {categoryName}
            </p>
          </div>
        </div>
      </div>
    </li>
  )
}
