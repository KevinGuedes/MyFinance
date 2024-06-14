import { useState } from 'react'
import { Cell, Legend, Pie, PieChart, ResponsiveContainer } from 'recharts'

import { ActiveSectorDetails } from './active-sector-details'

const data = [
  { type: 'Balance', value: -2233 },
  { type: 'Income', value: 440 },
  { type: 'Outcome', value: 2673 },
]
const COLORS = ['#64748b', '#2563eb', '#7f1d1d']

export function PeriodBalanceChart() {
  const [activeIndex, setActiveIndex] = useState<number | undefined>(undefined)

  function resetActiveIndex() {
    setActiveIndex(undefined)
  }

  function showSectorDetailsFor(index: number) {
    setActiveIndex(index)
  }

  const isNegativeBalance =
    data.filter((item) => item.type === 'Balance')[0].value < 0

  const formattedData = data.map((item) => ({
    ...item,
    absoluteValue: Math.abs(item.value),
  }))

  return (
    <div className="h-[300px]">
      <ResponsiveContainer width="100%" height="100%">
        <PieChart width={400} height={400}>
          <Pie
            data={formattedData}
            dataKey="absoluteValue"
            nameKey="type"
            legendType="rect"
            cx="50%"
            cy="50%"
            outerRadius={80}
            innerRadius={60}
            paddingAngle={5}
            activeIndex={activeIndex}
            activeShape={
              <ActiveSectorDetails isNegativeBalance={isNegativeBalance} />
            }
            onMouseLeave={resetActiveIndex}
            onMouseEnter={(_, index) => showSectorDetailsFor(index)}
          >
            {data.map((_, index) => (
              <Cell
                key={`sector-${index}`}
                stroke="none"
                fill={COLORS[index % COLORS.length]}
              />
            ))}
          </Pie>

          <Legend
            verticalAlign="bottom"
            iconSize={24}
            onMouseLeave={resetActiveIndex}
            onMouseEnter={(_, index) => showSectorDetailsFor(index)}
            wrapperStyle={{
              paddingTop: 20,
            }}
            formatter={(value) => (
              <span className="font-bold capitalize">{value}</span>
            )}
          />
        </PieChart>
      </ResponsiveContainer>
    </div>
  )
}
