import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'

import { ManagementUnit } from '../models/management-unit'
type ManagementUnitCardProps = {
  managementUnit: ManagementUnit
}

export function ManagementUnitCard({
  managementUnit,
}: ManagementUnitCardProps) {
  return (
    <Card className="shadow-xl">
      <CardHeader className="p-4">
        <CardTitle className="text-xl">{managementUnit.name}</CardTitle>
      </CardHeader>
      <CardContent className="">
        <p>Income: {managementUnit.income}</p>
        <p>Outcome: {managementUnit.outcome}</p>
        <p>Balance: {managementUnit.balance}</p>
      </CardContent>
    </Card>
  )
}
