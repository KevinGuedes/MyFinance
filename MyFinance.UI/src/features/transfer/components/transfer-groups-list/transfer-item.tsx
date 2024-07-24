import { Eye, MoreVertical, Pencil, Trash2 } from 'lucide-react'

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
    <li key={id} className="group flex justify-between gap-2 py-2">
      <div className="flex flex-col justify-between gap-1">
        <div className="flex gap-2">
          <p className="line-clamp-1 font-medium">{relatedTo}</p>

          <div className="flex gap-2">
            <p className="rounded-md border px-1 py-0.5 text-sm text-muted-foreground">
              {tag}
            </p>
            <p className="rounded-md border px-1 py-0.5 text-sm text-muted-foreground">
              {categoryName}
            </p>
          </div>
        </div>

        <p className="line-clamp-1 text-sm text-muted-foreground">
          {description}
        </p>
      </div>
      <div className="flex flex-col items-end justify-between gap-1">
        <div className='flex gap-2'>
          {type === TransferType.Income ? (
            <p className="font-semibold text-primary">{toMoney(value)}</p>
          ) : (
            <p className="font-semibold text-destructive">
              {toMoney(-1 * value)}
            </p>
          )}
          <Button
            variant="ghost"
            className="size-7 rounded-full p-0.5 text-muted-foreground"
            size="icon"
          >
            <MoreVertical className="size-4 text-muted-foreground" />
          </Button>
        </div>
        <div className="visible flex gap-2 sm:invisible sm:group-hover:visible">
          <Button
            variant="outline"
            className="size-7 rounded-full p-0.5 text-muted-foreground"
            size="icon"
          >
            <Eye className="size-4"/>
          </Button>
          <Button
            variant="outline"
            className="size-7 rounded-full p-0.5 text-muted-foreground"
            size="icon"
          >
            <Pencil className="size-4" />
          </Button>
          <Button
            variant="outline"
            className="size-7 rounded-full p-0.5 text-muted-foreground"
            size="icon"
          >
            <Trash2 className="size-4" />
          </Button>
        </div>
      </div>
    </li>
  )
}
