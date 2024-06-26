import { create } from 'zustand'
import { persist } from 'zustand/middleware'

interface DiscriminatedBalanceChartSettings {
  includeCurrentMonth: boolean
  pastMonths: number
  showYAxis: boolean
  showLegend: boolean
  showDataPointsWhenHovering: boolean
  toggleIncludeCurrentMonth: () => void
  toggleShowYAxis: () => void
  toggleShowLegend: () => void
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
        showDataPointsWhenHovering: false,
        toggleIncludeCurrentMonth: () =>
          set({ includeCurrentMonth: !get().includeCurrentMonth }),
        toggleShowYAxis: () => set({ showYAxis: !get().showYAxis }),
        toggleShowLegend: () => set({ showLegend: !get().showLegend }),
        toggleShowDataPointsWhenHovering: () =>
          set({
            showDataPointsWhenHovering: !get().showDataPointsWhenHovering,
          }),
        setPastMonths: (months) => set({ pastMonths: months }),
      }),
      { name: '@my-finance:v1.0.0:discriminated-balance-chart-settings' },
    ),
  )
