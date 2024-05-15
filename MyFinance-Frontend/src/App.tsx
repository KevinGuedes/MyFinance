import { ThemeProvider } from '@/components/theme/theme-provider'

import { AnnualBalanceCard } from './components/annual-balance-card/annual-balance-card'
import { ThemeSwitcher } from './components/theme/theme-switcher'
import { SignIn } from './pages/sign-in/sign-in'
import { SignUp } from './pages/sign-up/sign-up'

export function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <main className="flex w-full flex-col items-stretch justify-stretch gap-4 p-16">
        <ThemeSwitcher />
        <AnnualBalanceCard />
        <SignUp />
        <SignIn />
      </main>
    </ThemeProvider>
  )
}
