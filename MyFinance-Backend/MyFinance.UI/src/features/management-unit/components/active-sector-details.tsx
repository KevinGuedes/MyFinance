import { Sector } from 'recharts'
import { PieSectorDataItem } from 'recharts/types/polar/Pie'

import { toMoney } from '@/lib/utils'

interface ActiveSectorDetailsProps extends PieSectorDataItem {
  isNegativeBalance: boolean
}

export function ActiveSectorDetails({
  cx,
  cy,
  midAngle,
  innerRadius,
  outerRadius,
  startAngle,
  endAngle,
  fill,
  name,
  percent,
  value,
  isNegativeBalance,
}: ActiveSectorDetailsProps) {
  const RADIAN = Math.PI / 180

  const sin = Math.sin(-RADIAN * midAngle!)
  const cos = Math.cos(-RADIAN * midAngle!)
  const sx = cx! + (outerRadius! + 10) * cos
  const sy = cy! + (outerRadius! + 10) * sin
  const mx = cx! + (outerRadius! + 30) * cos
  const my = cy! + (outerRadius! + 30) * sin
  const ex = mx + (cos >= 0 ? 1 : -1) * 22
  const ey = my
  const textAnchor = cos >= 0 ? 'start' : 'end'

  const isOutcomeValue = name === 'Outcome'
  const isBalanceValue = name === 'Balance'
  const shouldAddNegativeSign =
    (isNegativeBalance && isBalanceValue) || isOutcomeValue

  const formattedValue = shouldAddNegativeSign
    ? toMoney(-1 * value!)
    : toMoney(value!)

  return (
    <g>
      <text
        x={cx}
        y={cy}
        dy={8}
        textAnchor="middle"
        fill={fill}
        className="font-bold"
      >
        {name}
      </text>
      <Sector
        cx={cx}
        cy={cy}
        innerRadius={innerRadius}
        outerRadius={outerRadius}
        startAngle={startAngle}
        endAngle={endAngle}
        fill={fill}
        stroke="white"
        strokeWidth={2}
      />
      <Sector
        cx={cx}
        cy={cy}
        startAngle={startAngle}
        endAngle={endAngle}
        innerRadius={outerRadius! + 6}
        outerRadius={outerRadius! + 10}
        fill={fill}
      />
      <path
        d={`M${sx},${sy}L${mx},${my}L${ex},${ey}`}
        stroke={fill}
        fill="none"
      />
      <circle cx={ex} cy={ey} r={2} fill={fill} stroke="none" />
      <text
        x={ex + (cos >= 0 ? 1 : -1) * 12}
        y={ey}
        textAnchor={textAnchor}
        fill={fill}
        className="font-bold"
      >
        {formattedValue}
      </text>
      <text
        x={ex + (cos >= 0 ? 1 : -1) * 12}
        y={ey}
        dy={18}
        textAnchor={textAnchor}
        fill={fill}
        className="text-xs font-bold"
      >
        {`${(percent! * 100).toFixed(2)}%`}
      </text>
    </g>
  )
}
