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
  function handleManagementUnitSelection(managementUnitId: string) {
    console.log('Management Unit selected:', managementUnitId)
  }

  const description =
    managementUnit.description ||
    `Financial management for ${managementUnit.name}`
  const income = managementUnit.income
  const outcome = managementUnit.outcome * -1
  const balance =
    managementUnit.balance >= 0
      ? managementUnit.balance
      : -1 * managementUnit.balance

  return (
    <Card
      className="border-2 shadow-lg ring-offset-background transition-colors hover:cursor-pointer hover:border-ring focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2"
      tabIndex={0}
      onClick={() => handleManagementUnitSelection(managementUnit.id)}
      onKeyDown={(key) => {
        if (key.key === 'Enter')
          handleManagementUnitSelection(managementUnit.id)
      }}
    >
      <CardHeader className="p-4 pb-0">
        <CardTitle className="line-clamp-1 text-xl">
          {managementUnit.name}
        </CardTitle>
      </CardHeader>
      <CardContent className="space-y-4 p-4 pt-2">
        <p className="line-clamp-1 text-muted-foreground">{description}</p>
        <div className="grid grid-cols-3 gap-2">
          <Tooltip>
            <TooltipTrigger asChild>
              <div className="flex flex-col gap-2">
                <div className="flex gap-2">
                  <PiggyBank className="inline-block size-6 shrink-0 rounded-md bg-muted-foreground/30 p-1 text-muted-foreground" />
                  <p className="font-bold text-muted-foreground">Balance</p>
                </div>
                <p className="text-lg font-bold text-muted-foreground">
                  {toMoney(balance, true)}
                </p>
              </div>
            </TooltipTrigger>
            <TooltipContent className="font-bold text-muted-foreground">
              {toMoney(balance)}
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
                  {toMoney(income, true)}
                </p>
              </div>
            </TooltipTrigger>
            <TooltipContent className="font-bold text-primary">
              {toMoney(income)}
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
                  {toMoney(outcome, true)}
                </p>
              </div>
            </TooltipTrigger>
            <TooltipContent className="font-bold text-destructive">
              {toMoney(outcome)}
            </TooltipContent>
          </Tooltip>
        </div>
      </CardContent>
    </Card>
  )
}
