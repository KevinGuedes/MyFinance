import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'

import { SignInForm } from './components/sign-in-form'

export function SignIn() {
  return (
    <Card className="mx-auto max-w-sm">
      <CardHeader>
        <CardTitle className="text-xl">Sign In</CardTitle>
      </CardHeader>
      <CardContent>
        <SignInForm />
      </CardContent>
    </Card>
  )
}
