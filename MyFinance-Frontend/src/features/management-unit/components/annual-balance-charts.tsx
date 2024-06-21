import { useState } from 'react'
import {
  Bar,
  ComposedChart,
  Legend,
  Line,
  ResponsiveContainer,
  Tooltip as ChartTooltip,
  XAxis,
  YAxis,
} from 'recharts'

import { useTheme } from '../../../components/ui/theme-provider'
import { BalanceDataTooltip } from './balance-data-tooltip'
// import { LineChartLabel } from './line-chart-label'

const months = [
  'Jan',
  'Feb',
  'Mar',
  'Apr',
  'May',
  'Jun',
  'Jul',
  'Aug',
  'Sep',
  'Oct',
  'Nov',
  'Dec',
]

const data = months.map((month, index) => {
  const income = Math.floor(Math.random() * 10000)
  const outcome = Math.floor(Math.random() * 10000)
  const balance = income - outcome

  return {
    monthNumber: index + 1,
    month,
    income,
    outcome,
    balance,
    date: `${month}/24`,
  }
})

type AnnualBalanceChartProps = {
  hideYAxis: boolean
  showLegend?: boolean
}

export function AnnualBalanceChart({
  hideYAxis = true,
  showLegend = false,
}: AnnualBalanceChartProps) {
  const { theme } = useTheme()
  const [opacity, setOpacity] = useState({
    outcome: 1,
    income: 1,
    balance: 1,
  })

  function handleMouserEnterOnLegend(dataSetBeingHovered: string) {
    setOpacity(() => ({
      outcome: 0.2,
      income: 0.2,
      balance: 0.2,
      [dataSetBeingHovered]: 1,
    }))
  }

  function handleMouseLeaveOnLegend() {
    setOpacity(() => ({
      outcome: 1,
      income: 1,
      balance: 1,
    }))
  }

  const axesColor = '#64748b'
  const balanceBarColor = '#64748b'
  const incomeLineColor = '	#2563eb'
  const outcomeLineColor = theme === 'dark' ? '	#7f1d1d' : '#ef4444'

  return (
    <ResponsiveContainer width="100%" height="100%">
      <ComposedChart
        data={data}
        margin={{
          top: 20,
          right: 20,
          left: 20,
          bottom: 20,
        }}
      >
        <ChartTooltip
          content={(chartTooltipContent) => {
            return (
              <BalanceDataTooltip
                active={chartTooltipContent.active}
                payload={chartTooltipContent.payload}
                label={chartTooltipContent.label}
              />
            )
          }}
        />

        <YAxis stroke={axesColor} hide={hideYAxis} type="number" />
        <XAxis
          dataKey="date"
          stroke={axesColor}
          angle={-25}
          textAnchor="end"
          tick={{ fontSize: 14, fontWeight: 600 }}
        />

        <Bar
          animationDuration={2000}
          dataKey="balance"
          opacity={opacity.balance}
          fill={balanceBarColor}
          activeBar={{ stroke: 'white', strokeWidth: 2 }}
        />

        <Line
          animationDuration={2000}
          type="monotone"
          dataKey="income"
          legendType="plainline"
          strokeWidth={3}
          strokeOpacity={opacity.income}
          fill={incomeLineColor}
          stroke={incomeLineColor}
          dot={{
            style: {
              opacity: opacity.income,
            },
          }}
          activeDot={{
            r: 8,
            style: {
              fill: incomeLineColor,
            },
          }}
        />

        <Line
          animationDuration={2000}
          type="monotone"
          dataKey="outcome"
          legendType="plainline"
          strokeWidth={2}
          strokeOpacity={opacity.outcome}
          fill={outcomeLineColor}
          stroke={outcomeLineColor}
          dot={{
            style: {
              opacity: opacity.outcome,
            },
          }}
          activeDot={{
            r: 6,
            style: { fill: outcomeLineColor },
          }}
          // label={opacity.income === 0.2 ? <LineChartLabel /> : undefined}
        />

        {showLegend && (
          <Legend
            verticalAlign="bottom"
            iconSize={24}
            onMouseLeave={handleMouseLeaveOnLegend}
            onMouseEnter={(payload) =>
              handleMouserEnterOnLegend(String(payload.dataKey))
            }
            wrapperStyle={{
              paddingTop: 20,
            }}
            formatter={(value) => (
              <span className="font-bold capitalize">{value}</span>
            )}
          />
        )}
      </ComposedChart>
    </ResponsiveContainer>
  )
}
