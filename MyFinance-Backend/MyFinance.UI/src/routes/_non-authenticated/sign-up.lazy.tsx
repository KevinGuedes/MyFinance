import { createLazyFileRoute, Link } from '@tanstack/react-router'

import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { SignUpForm } from '@/features/user/components/sign-up-form'

export const Route = createLazyFileRoute('/_non-authenticated/sign-up')({
  component: SignUp,
})

function SignUp() {
  return (
    <Card className="mx-auto w-full max-w-sm">
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
          <Button asChild variant="link" className="h-auto p-0">
            <Link to="/">Sign In</Link>
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}
