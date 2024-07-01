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
        <ScrollArea className="h-[365px] grow" type="always">
          <div className="grow space-y-4 pr-3">
            {tranferGroups.map((transferGroup) => (
              <TransferGroupSection
                key={transferGroup.date.toString()}
                transferGroup={transferGroup}
              />
            ))}
          </div>
        </ScrollArea>
      ) : (
        <div className="flex grow items-center justify-center rounded-lg border">
          <p className="text-center text-sm text-muted-foreground">
            You don&apos;t have any transfers registered on the current month
          </p>
        </div>
      )}
      <RegisterTransferDialog />
    </div>
  )
}
