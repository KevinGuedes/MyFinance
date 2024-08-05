import { format } from 'date-fns'
import { ClipboardList } from 'lucide-react'

import { ScrollArea } from '@/components/ui/scroll-area'

import { TransferGroup } from '../../models/transfer-group'
import { RegisterTransferDialog } from '../register-transfer-dialog'
import { TransferGroupSection } from './transfer-group-section'

type TransferGroupList = {
  tranferGroups: TransferGroup[]
}

export function TransferGroupsList({ tranferGroups }: TransferGroupList) {
  const hasTransferGroups = tranferGroups.length > 0

  return (
    <div className="flex grow flex-col justify-between gap-4">
      {hasTransferGroups ? (
        <ScrollArea className="h-[65vh] grow lg:h-[350px]" type="always">
          <div className="grow space-y-4 pr-2">
            {tranferGroups.map((transferGroup) => (
              <TransferGroupSection
                key={transferGroup.date.toString()}
                transferGroup={transferGroup}
              />
            ))}
          </div>
        </ScrollArea>
      ) : (
        <div className="flex h-[65vh] grow flex-col items-center justify-center gap-2 px-4 lg:max-h-[350px]">
          <p className="text-center text-sm text-muted-foreground">
            You don&apos;t have{' '}
            <strong className="font-medium">Transfers</strong> registered on{' '}
            <strong className="font-medium">
              {format(new Date(), 'MMMM, yyyy')}
            </strong>
            .
            <br />
            Click on the button below to register a new{' '}
            <strong className="font-medium">Transfer</strong>!
          </p>
          <ClipboardList className="size-10 text-muted-foreground" />
        </div>
      )}
      <RegisterTransferDialog />
    </div>
  )
}
