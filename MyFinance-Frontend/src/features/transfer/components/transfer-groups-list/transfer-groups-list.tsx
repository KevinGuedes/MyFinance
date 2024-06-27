import { ScrollArea } from '@/components/ui/scroll-area'

import { TransferGroup } from '../../models/transfer-group'
import { TransferGroupSection } from './transfer-group-section'

type TransferGroupList = {
  tranferGroups: TransferGroup[]
}

export function TransferGroupsList({ tranferGroups }: TransferGroupList) {
  return (
    <div className="flex grow flex-col justify-between gap-2">
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
    </div>
  )
}
