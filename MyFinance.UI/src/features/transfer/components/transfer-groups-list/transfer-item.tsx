import { motion } from 'framer-motion'
import { Edit2, MoreVertical, Trash2 } from 'lucide-react'
import { useState } from 'react'

import { Button } from '@/components/ui/button'
import { Transfer } from '@/features/transfer/models/transfer'
import { TransferType } from '@/features/transfer/models/transfer-type'
import { toMoney } from '@/lib/utils'

type TransferItemProps = {
  transfer: Transfer
}

export function TransferItem({ transfer }: TransferItemProps) {
  const [showActions, setShowActions] = useState(false)
  const [timeoutId, setTimeoutId] = useState<ReturnType<
    typeof setTimeout
  > | null>(null)

  function handleMouseEnter() {
    if (timeoutId == null) return

    clearTimeout(timeoutId)
    setTimeoutId(null)
    setShowActions(true)
  }

  function handleMouseLeave() {
    if (!timeoutId && showActions) {
      const timeoutId = setTimeout(() => {
        setShowActions(false)
        setTimeoutId(null)
      }, 3000)

      setTimeoutId(timeoutId)
    }
  }

  function handleShowActionsClick() {
    setShowActions(true)
  }

  return (
    <li
      key={transfer.id}
      onMouseLeave={handleMouseLeave}
      onMouseEnter={handleMouseEnter}
      className="flex w-full flex-col gap-1 px-1 py-2 transition-colors last-of-type:rounded-b-md hover:bg-muted/20"
    >
      <div className="flex items-center justify-between">
        <p className="line-clamp-1 font-medium">{transfer.relatedTo}</p>
        <div className="flex items-center gap-2">
          {transfer.type === TransferType.Income ? (
            <p className="shrink-0 font-bold text-primary">
              {toMoney(transfer.value)}
            </p>
          ) : (
            <p className="shrink-0 font-bold text-destructive">
              {toMoney(-1 * transfer.value)}
            </p>
          )}
          {showActions ? (
            <motion.div
              animate={{ opacity: 1 }}
              initial={{ opacity: 0.2 }}
              transition={{ ease: 'easeOut', duration: 0.3 }}
            >
              <Button
                variant="ghost"
                className="size-7 shrink-0 rounded-full p-0.5"
                size="icon"
              >
                <span className="sr-only">Open menu</span>
                <Edit2 className="size-4 stroke-2" />
              </Button>
            </motion.div>
          ) : (
            <Button
              variant="ghost"
              className="size-7 shrink-0 rounded-full p-0.5"
              size="icon"
              onClick={handleShowActionsClick}
            >
              <span className="sr-only">Open menu</span>
              <MoreVertical className="size-4" />
            </Button>
          )}
        </div>
      </div>
      <div className="flex items-center justify-between">
        <p className="line-clamp-1 shrink text-sm text-muted-foreground">
          {transfer.description}
        </p>
        <div className="flex shrink-0 items-center gap-2">
          <div className="flex gap-2">
            <p className="rounded-sm border bg-background px-[5px] text-sm font-medium text-muted-foreground">
              {transfer.tag}
            </p>
            <p className="rounded-sm border bg-background px-[5px] text-sm font-medium text-muted-foreground">
              {transfer.categoryName}
            </p>
          </div>
          {showActions ? (
            <motion.div
              animate={{ opacity: 1 }}
              initial={{ opacity: 0.2 }}
              transition={{ ease: 'easeInOut', duration: 0.3 }}
            >
              <Button
                variant="ghost"
                className="size-7 rounded-full p-0.5 hover:bg-destructive/25"
                size="icon"
              >
                <span className="sr-only">Open menu</span>
                <Trash2 className="size-4 stroke-2 text-destructive" />
              </Button>
            </motion.div>
          ) : (
            <div className="size-7"></div>
          )}
        </div>
      </div>
    </li>
  )
}
