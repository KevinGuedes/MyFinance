import { motion } from 'framer-motion'
import { MoreVertical } from 'lucide-react'
import { useState } from 'react'

import { Button } from '@/components/ui/button'
import { Transfer } from '@/features/transfer/models/transfer'
import { TransferType } from '@/features/transfer/models/transfer-type'
import { toMoney } from '@/lib/utils'

import { DeleteTransferAlert } from './delete-transfer-alert'
import { UpdateTransferDialog } from './update-transfer-dialog'

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
              <UpdateTransferDialog transfer={transfer} />
            </motion.div>
          ) : (
            <Button
              variant="ghost"
              className="rounded-full"
              size="icon-sm"
              onClick={handleShowActionsClick}
              icon={MoreVertical}
              screenReaderLabel="Show Transfer Menu"
            />
          )}
        </div>
      </div>
      <div className="flex items-center justify-between gap-4">
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
              <DeleteTransferAlert transferId={transfer.id} />
            </motion.div>
          ) : (
            <div className="size-7"></div>
          )}
        </div>
      </div>
    </li>
  )
}
