import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2, LogIn } from 'lucide-react'
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

const signInFormSchema = z.object({
  name: z.string().min(1, { message: 'Name is required' }),
  email: z
    .string()
    .min(1, { message: 'Email is required' })
    .email({ message: 'Invalid email' }),
  plainTextPassword: z.string().min(1, { message: 'Password is required' }),
})

type SignInFormSchema = z.infer<typeof signInFormSchema>

export function SignInForm() {
  const form = useForm<SignInFormSchema>({
    resolver: zodResolver(signInFormSchema),
    defaultValues: {
      name: '',
      email: '',
      plainTextPassword: '',
    },
  })

  function onSubmit(values: SignInFormSchema) {
    console.log(values)
  }

  return (
    <div className="grid gap-4">
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
          <FormField
            control={form.control}
            name="email"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Email</FormLabel>
                <FormControl>
                  <Input type="email" {...field} />
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
                  <a
                    href="#"
                    className="ml-auto inline-block text-sm underline"
                  >
                    Forgot your password?
                  </a>
                </div>
                <FormControl>
                  <Input type="password" {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <div className="grid gap-y-4">
            <Button
              type="submit"
              className="mt-2 w-full"
              disabled={form.formState.isSubmitting}
            >
              {form.formState.isSubmitting && (
                <Loader2 className="mr-2 size-4 animate-spin" />
              )}
              <LogIn className="mr-2 size-4" />
              Sign In
            </Button>
            {/* 
            <Button className="float-right m-0 p-0" variant="link">
              Forgot Password?
            </Button> */}
          </div>
        </form>
      </Form>
      <div className="mt-4 text-center text-sm">
        <p className="mr-1 inline">Don&apos;t have an account?</p>
        <a href="#" className="underline">
          Sign up
        </a>
      </div>
    </div>
  )
}
