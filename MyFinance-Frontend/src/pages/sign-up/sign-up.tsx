import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

import { SignUpForm } from './components/sign-up-form'

export function SignUp() {
  return (
    <Card className="mx-auto max-w-sm">
      <CardHeader>
        <CardTitle className="text-2xl">Sign Up</CardTitle>
        <CardDescription>
          Enter your information to create an account
        </CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <SignUpForm />
        <div className="text-center text-sm">
          <p className="mr-1 inline">Already have an account?</p>
          <a href="#" className="underline">
            Sign In
          </a>
        </div>
      </CardContent>
    </Card>
  )
}
