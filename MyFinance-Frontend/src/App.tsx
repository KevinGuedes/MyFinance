import { ThemeProvider } from '@/components/theme/theme-provider'

import { ThemeSwitcher } from './components/theme/theme-switcher'
import { AddTransferSheet } from './components/transfer-form/add-transfer-sheet'

export function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <main className="flex w-full flex-col items-stretch justify-stretch gap-4 p-16">
        <ThemeSwitcher />
        <AddTransferSheet />
        {/* <AnnualBalanceCard />
        <SignUp />
        <SignIn /> */}
      </main>
    </ThemeProvider>
  )
}
