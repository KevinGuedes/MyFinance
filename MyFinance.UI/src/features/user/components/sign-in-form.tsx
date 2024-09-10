import { zodResolver } from '@hookform/resolvers/zod'
import { Link } from '@tanstack/react-router'
import { LogIn } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '@/components/ui/button'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { PasswordInput } from '@/components/ui/password-input'

import { useSignIn } from '../api/use-sign-in'

const signInFormSchema = z.object({
  email: z
    .string()
    .min(1, { message: 'Email is required' })
    .email({ message: 'Invalid email' }),
  plainTextPassword: z.string().min(1, { message: 'Password is required' }),
})

export type SignInFormSchema = z.infer<typeof signInFormSchema>

export function SignInForm() {
  const signInMutation = useSignIn()
  const form = useForm<SignInFormSchema>({
    resolver: zodResolver(signInFormSchema),
    defaultValues: {
      email: '',
      plainTextPassword: '',
    },
  })

  async function handleSubmit(values: SignInFormSchema) {
    await signInMutation.mutateAsync(values)
  }

  const { isValid, isSubmitting } = form.formState

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(handleSubmit)}
        className="flex grow flex-col space-y-4"
      >
        <FormField
          control={form.control}
          name="email"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Email</FormLabel>
              <FormControl>
                <Input type="email" {...field} autoComplete="email" />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="plainTextPassword"
          render={({ field }) => (
            <FormItem>
              <div className="flex items-center">
                <FormLabel htmlFor="password">Password</FormLabel>
                <Button
                  variant="link"
                  asChild
                  className="ml-auto inline-block h-auto p-0 text-sm"
                >
                  <Link to="/">Forgot your password?</Link>
                </Button>
              </div>
              <FormControl>
                <PasswordInput {...field} id="password" />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex grow flex-col items-end gap-4">
          <Button
            label="Sign In"
            icon={LogIn}
            loadingLabel="Signing In..."
            type="submit"
            className="w-full"
            isLoading={isSubmitting}
            disabled={isSubmitting || !isValid}
          />
        </div>
      </form>
    </Form>
  )
}
