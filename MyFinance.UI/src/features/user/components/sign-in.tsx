import { Link } from '@tanstack/react-router'

import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'

import { SignInForm } from './sign-in-form'

export function SignIn() {
  return (
    <Card className="flex h-dvh w-full flex-col sm:mx-auto sm:h-auto sm:max-w-sm">
      <CardHeader>
        <CardTitle className="text-2xl">Sign In</CardTitle>
        <CardDescription>
          Enter your credentials below to sign in to your account
        </CardDescription>
      </CardHeader>
      <CardContent className="flex grow flex-col space-y-6">
        <SignInForm />
        <div className="text-center text-sm">
          <p className="mr-1 inline">Don&apos;t have an account?</p>
          <Button asChild variant="link" className="h-auto p-0">
            <Link to="/sign-up">Sign Up</Link>
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}
