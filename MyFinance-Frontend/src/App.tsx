import { ThemeProvider } from '@/components/theme/theme-provider'

import { AmountCards } from './components/amount-cards/amount-cards'
import { AnnualBalanceCard } from './components/annual-balance-card/annual-balance-card'
import { CreateManagementUnitDialog } from './components/management-unit/create-management-unit-dialogs'
import { PeriodBalanceCard } from './components/period-balance-card/period-balance-card'
import { ThemeSwitcher } from './components/theme/theme-switcher'
import { RegisterTransferDialog } from './components/transfer/register-transfer-dialog'
import { Input } from './components/ui/input'
import { SignIn } from './pages/sign-in/sign-in'
import { SignUp } from './pages/sign-up/sign-up'

export function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <main className="flex w-full flex-col items-stretch justify-stretch gap-4 p-16">
        <ThemeSwitcher />
        <RegisterTransferDialog />
        <CreateManagementUnitDialog />
        {/* <PeriodBalanceCard /> */}
        {/* <AmountCards />
        <AnnualBalanceCard />
        <SignUp />
        <SignIn /> */}
      </main>
    </ThemeProvider>
  )
}
