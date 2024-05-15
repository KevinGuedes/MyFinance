import { useState } from 'react'
import {
  Bar,
  ComposedChart,
  Legend,
  Line,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from 'recharts'

import { toMoney } from '@/utils/to-money'

import { useTheme } from './theme-provider'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from './ui/card'

// a variable called data composed of an array of objects where each one contains a month number, income, outcome and balance
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
  }
})

export function AnnualBalanceChart() {
  const { theme } = useTheme()
  const [opacity, setOpacity] = useState({
    outcome: 1,
    income: 1,
  })

  const handleMouseEnter = (o) => {
    const { dataKey } = o

    setOpacity((op) => ({ ...op, [dataKey]: 0.5 }))
  }

  const handleMouseLeave = (o) => {
    const { dataKey } = o

    setOpacity((op) => ({ ...op, [dataKey]: 1 }))
  }
  return (
    <Card className="mx-auto w-full max-w-2xl">
      <CardHeader>
        <CardTitle className="text-2xl">Annaul Balance Data</CardTitle>
        <CardDescription>
          Income, Outcome and Balance for the last 12 months
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="h-[300px]">
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
              <Tooltip
                content={({ active, payload }) => {
                  if (active && payload && payload.length) {
                    return (
                      <div className="rounded-lg border bg-background p-2 shadow-sm">
                        <div className="grid gap-2.5">
                          <div className="flex flex-col">
                            <span className="text-[0.70rem] uppercase text-muted-foreground">
                              Income
                            </span>
                            <span className="font-bold text-emerald-600">
                              {toMoney(Number(payload[1].value))}
                            </span>
                          </div>
                          <div className="flex flex-col">
                            <span className="text-[0.70rem] uppercase text-muted-foreground">
                              Outcome
                            </span>
                            <span className="font-bold text-destructive">
                              -{toMoney(Number(payload[2].value))}
                            </span>
                          </div>
                          <div className="flex flex-col">
                            <span className="text-[0.70rem] uppercase text-muted-foreground">
                              Balance
                            </span>
                            {Number(payload[0].value) > 0 ? (
                              <span className="font-bold text-emerald-600">
                                {toMoney(Number(payload[0].value))}
                              </span>
                            ) : (
                              <span className="font-bold text-destructive">
                                {toMoney(Number(payload[0].value))}
                              </span>
                            )}
                          </div>
                        </div>
                      </div>
                    )
                  }

                  return null
                }}
              />

              <Bar dataKey="balance" barSize={20} fill="#413ea0" />

              {/* <Bar
                dataKey="balance"
                barSize={20}
                fill={
                  theme === 'dark'
                    ? 'hsl(217.2 91.2% 59.8%)'
                    : 'hsl(222.2 84% 4.9%)'
                }
              /> */}

              {/* <Bar
                dataKey="balance"
                barSize={20}
                style={
                  {
                    fill: 'var(--theme-primary)',
                    opacity: 1,
                    '--theme-primary': `hsl(${
                      theme === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              /> */}

              <Line
                type="monotone"
                dataKey="income"
                strokeOpacity={opacity.income}
                strokeWidth={2}
                activeDot={{
                  r: 8,
                  style: { fill: 'var(--theme-primary)' },
                }}
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    '--theme-primary': `hsl(${
                      theme === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              />
              <Line
                type="monotone"
                strokeWidth={2}
                dataKey="outcome"
                strokeOpacity={opacity.outcome}
                activeDot={{
                  r: 6,
                  style: { fill: 'var(--theme-primary)', opacity: 0.25 },
                }}
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    opacity: 0.25,
                    '--theme-primary': `hsl(${
                      theme === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              />
              {/* <Line
                type="monotone"
                strokeWidth={2}
                dataKey="outcome"
                strokeOpacity={opacity.outcome}
                activeDot={{
                  r: 6,
                  style: { fill: 'var(--theme-primary)', opacity: 0.25 },
                }}
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    opacity: 0.25,
                    '--theme-primary': `hsl(${
                      theme === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              /> */}

              {/* <YAxis
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    '--theme-primary': `hsl(${
                      theme === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              />
              <XAxis
                dataKey="month"
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    '--theme-primary': `hsl(${
                      theme === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              /> */}
              <Legend
                className="mb-4"
                onMouseEnter={handleMouseEnter}
                onMouseLeave={handleMouseLeave}
                verticalAlign="top"
                iconSize={20}
                formatter={(value) => (
                  <span className="capitalize">{value}</span>
                )}
              />
            </ComposedChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  )
}
