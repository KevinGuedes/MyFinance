import {
  Bar,
  ComposedChart,
  Legend,
  Line,
  ResponsiveContainer,
  Tooltip,
} from 'recharts'

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

  if (index === 0) {
    return {
      monthNumber: index + 1,
      month,
      income: 10000,
      outcome: 3000,
      balance: 7000,
    }
  }

  return {
    monthNumber: index + 1,
    month,
    income,
    outcome,
    balance,
  }
})

export function AnnualBalanceChart() {
  const { theme: mode } = useTheme()
  console.log(mode)

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
                top: 5,
                right: 10,
                left: 10,
                bottom: 0,
              }}
            >
              <Tooltip
                content={({ active, payload }) => {
                  if (active && payload && payload.length) {
                    return (
                      <div className="rounded-lg border bg-background p-2 shadow-sm">
                        <div className="grid grid-cols-3 gap-2">
                          <div className="flex flex-col">
                            <span className="text-[0.70rem] uppercase text-muted-foreground">
                              Outcome
                            </span>
                            <span className="font-bold text-muted-foreground">
                              {payload[0].value}
                            </span>
                          </div>
                          <div className="flex flex-col">
                            <span className="text-[0.70rem] uppercase text-muted-foreground">
                              Income
                            </span>
                            <span className="font-bold">
                              {payload[1].value}
                            </span>
                          </div>
                          <div className="flex flex-col">
                            <span className="text-[0.70rem] uppercase text-muted-foreground">
                              Balance
                            </span>
                            <span className="font-bold">
                              {payload[2].value}
                            </span>
                          </div>
                        </div>
                      </div>
                    )
                  }

                  return null
                }}
              />
              <Bar dataKey="balance" barSize={20} fill="#413ea0" />

              <Line
                type="monotone"
                dataKey="income"
                strokeWidth={2}
                activeDot={{
                  r: 8,
                  style: { fill: 'var(--theme-primary)' },
                }}
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    '--theme-primary': `hsl(${
                      mode === 'dark'
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
                activeDot={{
                  r: 6,
                  style: { fill: 'var(--theme-primary)', opacity: 0.25 },
                }}
                style={
                  {
                    stroke: 'var(--theme-primary)',
                    opacity: 0.25,
                    '--theme-primary': `hsl(${
                      mode === 'dark'
                        ? '217.2 91.2% 59.8%'
                        : '221.2 83.2% 53.3%'
                    })`,
                  } as React.CSSProperties
                }
              />
              <Legend />
            </ComposedChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  )
}
