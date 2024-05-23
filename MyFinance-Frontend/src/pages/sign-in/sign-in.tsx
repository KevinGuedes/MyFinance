import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { useSignIn } from '@/http/user/useSignIn'

import { SignInForm, SignInFormSchema } from './components/sign-in-form'

export function SignIn() {
  const mutation = useSignIn()

  async function onSubmit(values: SignInFormSchema) {
    await mutation.mutateAsync(values, {
      onSuccess: (response) => {
        if (response.shouldUpdatePassword) {
          // navigate to update password page
        } else {
          // navigate to dashboard
        }
      },
    })
  }

  return (
    <Card className="mx-auto max-w-sm">
      <CardHeader>
        <CardTitle className="text-2xl">Sign In</CardTitle>
        <CardDescription>
          Enter your email below to login to your account
        </CardDescription>
      </CardHeader>
      <CardContent className="space-y-6">
        <SignInForm
          onSubmit={onSubmit}
          defaultValues={{
            email: '',
            plainTextPassword: '',
          }}
        />
        <div className="text-center text-sm">
          <p className="mr-1 inline">Don&apos;t have an account?</p>
          <a href="#" className="underline">
            Sign Up
          </a>
        </div>
      </CardContent>
    </Card>
  )
}
