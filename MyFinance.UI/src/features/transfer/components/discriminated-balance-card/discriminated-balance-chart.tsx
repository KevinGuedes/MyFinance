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

import { MonthlyBalanceData } from '@/features/transfer/models/monthly-balance-data'

import { useTheme } from '../../../../components/ui/theme-provider'
import { useDiscriminatedBalanceChartSettings } from '../../../management-unit/store/discriminated-balance-chart-settings-store'
import { BalanceDataTooltip } from './balance-data-tooltip'
import { LineChartLabel } from './line-chart-label'

type DiscriminatedBalanceChartProps = {
  discriminatedBalanceData: MonthlyBalanceData[]
}

export function DiscriminatedBalanceChart({
  discriminatedBalanceData,
}: DiscriminatedBalanceChartProps) {
  const {
    showYAxis,
    showLegend,
    showDataPointsWhenHovering,
    showBalance,
    showIncome,
    showOutcome,
  } = useDiscriminatedBalanceChartSettings()
  const { theme } = useTheme()
  const [dateKeyHovered, setDateKeyHovered] = useState<string | null>(null)
  const [opacity, setOpacity] = useState({
    outcome: 1,
    income: 1,
    balance: 1,
  })

  function handleMouserEnterOnLegend(dataSetBeingHovered: string) {
    setDateKeyHovered(dataSetBeingHovered)
    setOpacity(() => ({
      outcome: 0.2,
      income: 0.2,
      balance: 0.2,
      [dataSetBeingHovered]: 1,
    }))
  }

  function handleMouseLeaveOnLegend() {
    setDateKeyHovered(null)
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
        data={discriminatedBalanceData}
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

        <YAxis
          stroke={axesColor}
          hide={!showYAxis}
          type="number"
          label={{
            value: 'R$',
            style: {
              textAnchor: 'middle',
              fill: axesColor,
              fontSize: 14,
              fontWeight: 600,
            },
            position: 'left',
            offset: -5,
          }}
          tick={{ fontSize: 14, fontWeight: 600 }}
        />
        <XAxis
          dataKey="reference"
          stroke={axesColor}
          angle={-30}
          textAnchor="end"
          tick={{ fontSize: 14, fontWeight: 600 }}
        />

        {showBalance && (
          <Bar
            animationDuration={2000}
            dataKey="balance"
            opacity={opacity.balance}
            fill={balanceBarColor}
            activeBar={{ stroke: 'white', strokeWidth: 2 }}
            label={
              dateKeyHovered === 'balance' && showDataPointsWhenHovering ? (
                <LineChartLabel variant="balance" />
              ) : undefined
            }
          />
        )}

        {showIncome && (
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
            label={
              dateKeyHovered === 'income' && showDataPointsWhenHovering ? (
                <LineChartLabel variant="income" />
              ) : undefined
            }
          />
        )}

        {showOutcome && (
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
            label={
              dateKeyHovered === 'outcome' && showDataPointsWhenHovering ? (
                <LineChartLabel variant="outcome" />
              ) : undefined
            }
          />
        )}

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
