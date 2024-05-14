import { ThemeProvider } from '@/components/theme-provider'

import { ThemeSwitcher } from './components/theme-switcher'
import { SignIn } from './pages/sign-in/sign-in'
import { SignUp } from './pages/sign-up/sign-up'

export function App() {
  return (
    <ThemeProvider defaultTheme="system" storageKey="vite-ui-theme">
      <main className="grid justify-stretch gap-4">
        <ThemeSwitcher />
        <SignUp />
        <SignIn />
      </main>
    </ThemeProvider>
  )
}
