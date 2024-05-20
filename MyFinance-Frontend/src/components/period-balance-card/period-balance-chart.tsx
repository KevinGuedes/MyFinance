import { useState } from 'react'
import { Cell, Legend, Pie, PieChart, ResponsiveContainer } from 'recharts'

import { ActiveSectorDetails } from './active-sector-details'

const data = [
  { type: 'Balance', value: 322 },
  { type: 'Income', value: 440 },
  { type: 'Outcome', value: 673 },
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

  return (
    <div className="h-[300px]">
      <ResponsiveContainer width="100%" height="100%">
        <PieChart width={400} height={400}>
          <Pie
            data={data}
            dataKey="value"
            nameKey="type"
            legendType="rect"
            cx="50%"
            cy="50%"
            outerRadius={80}
            innerRadius={60}
            paddingAngle={5}
            activeIndex={activeIndex}
            activeShape={<ActiveSectorDetails />}
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
