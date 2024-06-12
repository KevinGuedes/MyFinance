import { createFileRoute, Link } from '@tanstack/react-router'
import { z } from 'zod'

import { SignInForm, SignInFormSchema } from '@/components/sign-in/sign-in-form'
import { Button } from '@/components/ui/button'
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { useSignIn } from '@/http/user/use-sign-in'

const signInSchema = z.object({
  redirectTo: z.string().optional(),
})

export const Route = createFileRoute('/_default/sign-in')({
  component: Index,
  validateSearch: (search) => signInSchema.parse(search),
})

export function Index() {
  const signInMutation = useSignIn()
  const { redirectTo } = Route.useSearch()

  async function onSubmit(values: SignInFormSchema) {
    await signInMutation.mutateAsync({
      signInRequest: values,
      redirectTo,
    })
  }

  return (
    <Card className="mx-auto w-full max-w-sm">
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
          <Button asChild variant="link" className="h-auto p-0">
            <Link to="/sign-up">Sign Up</Link>
          </Button>
        </div>
      </CardContent>
    </Card>
  )
}
