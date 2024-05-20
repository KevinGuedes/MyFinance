import { useState } from 'react'
import {
  Cell,
  Legend,
  Pie,
  PieChart,
  ResponsiveContainer,
  Sector,
} from 'recharts'
import { PieSectorDataItem } from 'recharts/types/polar/Pie'

import { toMoney } from '@/lib/utils'

const data = [
  { name: 'Income', value: 400 },
  { name: 'Balance', value: 300 },
  { name: 'Outcome', value: 300 },
]
const COLORS = ['#2563eb', '#64748b', '#7f1d1d']

export function PeriodBalanceChart() {
  const [activeIndex, setActiveIndex] = useState<number | undefined>(undefined)

  const renderActiveShape = (pieSectorDataItem: PieSectorDataItem) => {
    const RADIAN = Math.PI / 180

    const {
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
    } = pieSectorDataItem

    const sin = Math.sin(-RADIAN * midAngle!)
    const cos = Math.cos(-RADIAN * midAngle!)
    const sx = cx! + (outerRadius! + 10) * cos
    const sy = cy! + (outerRadius! + 10) * sin
    const mx = cx! + (outerRadius! + 30) * cos
    const my = cy! + (outerRadius! + 30) * sin
    const ex = mx + (cos >= 0 ? 1 : -1) * 22
    const ey = my
    const textAnchor = cos >= 0 ? 'start' : 'end'

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
          {toMoney(value!)}
        </text>
        <text
          x={ex + (cos >= 0 ? 1 : -1) * 12}
          y={ey}
          dy={18}
          textAnchor={textAnchor}
          fill="#64748b"
        >
          {`${(percent! * 100).toFixed(2)}%`}
        </text>
      </g>
    )
  }

  return (
    <div className="h-[300px]">
      <ResponsiveContainer width="100%" height="100%">
        <PieChart width={400} height={400}>
          <Pie
            onMouseLeave={() => setActiveIndex(undefined)}
            onMouseEnter={(_, index) => setActiveIndex(index)}
            data={data}
            legendType="rect"
            cx="50%"
            cy="50%"
            activeIndex={activeIndex}
            activeShape={renderActiveShape}
            outerRadius={80}
            innerRadius={60}
            dataKey="value"
            paddingAngle={3}
          >
            {data.map((_, index) => (
              <Cell
                key={`cell-${index}`}
                stroke="none"
                fill={COLORS[index % COLORS.length]}
              />
            ))}
          </Pie>

          <Legend
            verticalAlign="bottom"
            iconSize={24}
            wrapperStyle={{
              paddingTop: 20,
            }}
            formatter={(value) => (
              <span className="font-bold capitalize">{value}</span>
            )}
            onMouseLeave={() => setActiveIndex(undefined)}
            onMouseEnter={(_, index) => setActiveIndex(index)}
          />
        </PieChart>
      </ResponsiveContainer>
    </div>
  )
}
