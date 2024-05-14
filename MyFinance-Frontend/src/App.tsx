import { ThemeProvider } from '@/components/theme-provider'

import { AnnualBalanceChart } from './components/annual-balance-chart'
import { ThemeSwitcher } from './components/theme-switcher'
import { SignIn } from './pages/sign-in/sign-in'
import { SignUp } from './pages/sign-up/sign-up'

export function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <main className="flex w-full flex-col items-stretch justify-stretch gap-4 p-16">
        <ThemeSwitcher />
        <SignUp />
        <SignIn />
        <AnnualBalanceChart />
      </main>
    </ThemeProvider>
  )
}
