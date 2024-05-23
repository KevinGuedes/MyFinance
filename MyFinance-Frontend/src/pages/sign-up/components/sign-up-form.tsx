import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2 } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '@/components/ui/button'
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'

const signUpFormSchema = z
  .object({
    name: z
      .string()
      .min(1, { message: 'Name is required' })
      .min(3, { message: 'Name must tbe at least 3 chracters long' }),
    email: z
      .string()
      .min(1, { message: 'Email is required' })
      .email({ message: 'Invalid email' }),
    plainTextPassword: z
      .string()
      .min(1, { message: 'Password is required' })
      .min(16, { message: 'Password is too short' })
      .regex(/[!@#$%^&*()_+\-=[\]{};':"\\|,.<>/?]/g, {
        message: 'Password must include at least 2 special characters',
      })
      .regex(/\d/g, { message: 'Password must include at least 2 numbers' })
      .regex(/[A-Z]/g, {
        message: 'Password must include at least 2 uppercase letters',
      })
      .regex(/[a-z]/g, {
        message: 'Password must include at least 2 lowercase letters',
      }),
    plainTextPasswordConfirmation: z
      .string()
      .min(1, { message: 'Password confirmation is required' }),
  })
  .refine(
    (data) => data.plainTextPassword === data.plainTextPasswordConfirmation,
    {
      message: "Passwords don't match",
      path: ['plainTextPasswordConfirmation'],
    },
  )

type SignUpFormSchema = z.infer<typeof signUpFormSchema>

export function SignUpForm() {
  const form = useForm<SignUpFormSchema>({
    resolver: zodResolver(signUpFormSchema),
    defaultValues: {
      name: '',
      email: '',
      plainTextPassword: '',
      plainTextPasswordConfirmation: '',
    },
  })

  function onSubmit(values: SignUpFormSchema) {
    console.log(values)
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

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
              <FormLabel>Password</FormLabel>
              <FormControl>
                <Input type="password" {...field} />
              </FormControl>

              <FormDescription className="text-justify">
                16 characters minimum and must include at least 2 capital and
                lower letters, 2 numbers and 2 special characters
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="plainTextPasswordConfirmation"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Password Confirmation</FormLabel>
              <FormControl>
                <Input type="password" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          className="w-full"
          disabled={!form.formState.isValid || form.formState.isSubmitting}
        >
          {form.formState.isSubmitting && (
            <Loader2 className="mr-2 size-4 animate-spin" />
          )}
          Sign Up
        </Button>
      </form>
    </Form>
  )
}
