import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface DiscriminatedBalanceChartSettings {
  includeCurrentMonth: boolean
  pastMonths: number
  showYAxis: boolean
  showLegend: boolean
  showBalance: boolean
  showIncome: boolean
  showOutcome: boolean
  showDataPointsWhenHovering: boolean
  toggleIncludeCurrentMonth: () => void
  toggleShowYAxis: () => void
  toggleShowLegend: () => void
  toggleShowBalance: () => void
  toggleShowIncome: () => void
  toggleShowOutcome: () => void
  toggleShowDataPointsWhenHovering: () => void
  setPastMonths: (months: number) => void
}

export const useDiscriminatedBalanceChartSettings =
  create<DiscriminatedBalanceChartSettings>()(
    persist(
      (set, get) => ({
        includeCurrentMonth: true,
        pastMonths: 12,
        showYAxis: false,
        showLegend: false,
        showBalance: true,
        showIncome: true,
        showOutcome: true,
        showDataPointsWhenHovering: false,
        toggleIncludeCurrentMonth: () =>
          set({ includeCurrentMonth: !get().includeCurrentMonth }),
        toggleShowYAxis: () => set({ showYAxis: !get().showYAxis }),
        toggleShowLegend: () => set({ showLegend: !get().showLegend }),
        toggleShowBalance: () => set({ showBalance: !get().showBalance }),
        toggleShowIncome: () => set({ showIncome: !get().showIncome }),
        toggleShowOutcome: () => set({ showOutcome: !get().showOutcome }),
        toggleShowDataPointsWhenHovering: () =>
          set({
            showDataPointsWhenHovering: !get().showDataPointsWhenHovering,
          }),
        setPastMonths: (months) => set({ pastMonths: months }),
      }),
      { name: '@my-finance:v1.0.0:discriminated-balance-chart-settings' },
    ),
  )
