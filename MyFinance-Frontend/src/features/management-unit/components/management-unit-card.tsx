import { PiggyBank, TrendingDown, TrendingUp } from 'lucide-react'

import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import {
  Tooltip,
  TooltipContent,
  TooltipTrigger,
} from '@/components/ui/tooltip'
import { toMoney } from '@/lib/utils'

import { ManagementUnit } from '../models/management-unit'

type ManagementUnitCardProps = {
  managementUnit: ManagementUnit
}

export function ManagementUnitCard({
  managementUnit,
}: ManagementUnitCardProps) {
  return (
    <Card className="border-2 shadow-2xl transition-colors hover:cursor-pointer hover:border-ring">
      <CardHeader className="p-4 pb-0">
        <CardTitle className="text-xl">{managementUnit.name}</CardTitle>
      </CardHeader>
      <CardContent className="space-y-4 p-4 pt-2">
        <p className="line-clamp-2 text-muted-foreground">
          Lorem ipsum dolor sit, amet consectetur adipisicing elit. In facilis
          maxime cupiditate beatae quod neque! Mollitia, adipisci ab ut unde
          laboriosam corrupti deserunt similique beatae assumenda sunt
          voluptatum obcaecati et?
        </p>
        <div className="grid grid-cols-3 gap-2">
          <Tooltip>
            <TooltipTrigger asChild>
              <div className="flex flex-col gap-2">
                <div className="flex gap-2">
                  <PiggyBank className="inline-block size-6 shrink-0 rounded-md bg-muted-foreground/30 p-1 text-muted-foreground" />
                  <p className="font-bold text-muted-foreground">Balance</p>
                </div>
                <p className="text-lg font-bold text-muted-foreground">
                  {toMoney(managementUnit.balance, true)}
                </p>
              </div>
            </TooltipTrigger>
            <TooltipContent>
              <p className="text-base font-bold text-muted-foreground">
                {toMoney(managementUnit.balance)}
              </p>
            </TooltipContent>
          </Tooltip>
          <Tooltip>
            <TooltipTrigger asChild>
              <div className="flex flex-col gap-2">
                <div className="flex gap-2">
                  <TrendingUp className="inline-block size-6 shrink-0 rounded-md bg-primary/10 p-1 text-primary" />
                  <p className="font-bold text-primary">Income</p>
                </div>
                <p className="text-lg font-bold text-primary">
                  {toMoney(managementUnit.income, true)}
                </p>
              </div>
            </TooltipTrigger>
            <TooltipContent>
              <p className="text-base font-bold text-primary">
                {toMoney(managementUnit.income)}
              </p>
            </TooltipContent>
          </Tooltip>
          <Tooltip>
            <TooltipTrigger asChild>
              <div className="flex flex-col gap-2">
                <div className="flex gap-2">
                  <TrendingDown className="inline-block size-6 shrink-0 rounded-md bg-destructive/25 p-1 text-destructive" />
                  <p className="font-bold text-destructive">Outcome</p>
                </div>

                <p className="text-lg font-bold text-destructive">
                  {toMoney(managementUnit.outcome, true)}
                </p>
              </div>
            </TooltipTrigger>
            <TooltipContent>
              <p className="text-base font-bold text-destructive">
                {toMoney(managementUnit.outcome)}
              </p>
            </TooltipContent>
          </Tooltip>
        </div>
      </CardContent>
    </Card>
  )
}
