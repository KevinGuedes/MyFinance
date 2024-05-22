import { ThemeProvider } from '@/components/theme/theme-provider'

import { AmountCards } from './components/amount-cards/amount-cards'
import { AnnualBalanceCard } from './components/annual-balance-card/annual-balance-card'
import { CreateManagementUnitDialog } from './components/management-unit/create-management-unit-dialogs'
import { PeriodBalanceCard } from './components/period-balance-card/period-balance-card'
import { SearchManagementUnits } from './components/search-management-units/search-management-units'
import { ThemeSwitcher } from './components/theme/theme-switcher'
import { DeleteTransfer } from './components/transfer/delete-transfer'
import { RegisterTransferDialog } from './components/transfer/register-transfer-dialog'
import { Input } from './components/ui/input'
import { Toaster } from './components/ui/toast/toaster'
import { SignIn } from './pages/sign-in/sign-in'
import { SignUp } from './pages/sign-up/sign-up'

export function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <main className="flex w-full flex-col items-stretch justify-stretch gap-4 p-16">
        <Toaster />
        <ThemeSwitcher />
        <RegisterTransferDialog />
        <CreateManagementUnitDialog />
        <SearchManagementUnits />
        <DeleteTransfer />
        {/* <PeriodBalanceCard /> */}
        {/* <AmountCards />
        <AnnualBalanceCard />
        <SignUp />
        <SignIn /> */}
      </main>
    </ThemeProvider>
  )
}
