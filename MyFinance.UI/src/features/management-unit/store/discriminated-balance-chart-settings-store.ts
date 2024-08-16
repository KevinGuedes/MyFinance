import { create } from 'zustand'
import { persist } from 'zustand/middleware'

type DiscriminatedBalanceChartSettings = {
  includeCurrentMonth: boolean
  pastMonths: number
  showYAxis: boolean
  showLegend: boolean
  showBalance: boolean
  showIncome: boolean
  showOutcome: boolean
  showDataPointsWhenHovering: boolean
}

interface DiscriminatedBalanceChartSettingsState {
  includeCurrentMonth: boolean
  pastMonths: number
  showYAxis: boolean
  showLegend: boolean
  showBalance: boolean
  showIncome: boolean
  showOutcome: boolean
  showDataPointsWhenHovering: boolean
  updateSettings: (settings: Partial<DiscriminatedBalanceChartSettings>) => void
}

export const useDiscriminatedBalanceChartSettings =
  create<DiscriminatedBalanceChartSettingsState>()(
    persist(
      (set) => ({
        includeCurrentMonth: true,
        pastMonths: 3,
        showYAxis: false,
        showLegend: false,
        showBalance: true,
        showIncome: true,
        showOutcome: true,
        showDataPointsWhenHovering: false,
        updateSettings: (settings) => {
          set((state) => ({
            ...state,
            ...settings,
          }))
        },
      }),
      {
        name: '@my-finance:v1.0.0:discriminated-balance-chart-settings',
        version: 0,
      },
    ),
  )
