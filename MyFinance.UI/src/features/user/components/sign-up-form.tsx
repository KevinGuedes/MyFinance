import { zodResolver } from '@hookform/resolvers/zod'
import { UserPlus } from 'lucide-react'
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

import { PasswordConfirmationValidation } from './password-confirmation-validation'
import { PasswordValidation } from './password-validation'

const signUpFormSchema = z
  .object({
    name: z
      .string()
      .min(1, { message: 'Name is required' })
      .min(3, { message: 'Name must be at least 3 chracters long' }),
    email: z
      .string()
      .min(1, { message: 'Email is required' })
      .email({ message: 'Invalid email' }),
    plainTextPassword: z
      .string()
      .min(1, { message: 'Password is required' })
      .min(16, { message: 'Password is too short' })
      .regex(/(?=(.*[^a-zA-Z0-9]){2,})/, {
        message: 'Password must have at least 2 special characters',
      })
      .regex(/(\D*\d){2,}/, {
        message: 'Password must have at least 2 numbers',
      })
      .regex(/(?=(.*[A-Z]){2,})/, {
        message: 'Password must have at least 2 uppercase letters',
      })
      .regex(/(?=(.*[a-z]){2,})/, {
        message: 'Password must have at least 2 lowercase letters',
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

  const password = form.watch('plainTextPassword')
  const passwordConfirmation = form.watch('plainTextPasswordConfirmation')
  const { isSubmitting, isValid } = form.formState

  // https://stackoverflow.com/questions/15738259/disabling-chrome-autofill
  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="space-y-4"
        autoComplete="chrome-off"
      >
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input {...field} autoComplete="one-time-code" />
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
                <Input type="email" {...field} autoComplete="one-time-code" />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <div>
          <FormField
            control={form.control}
            name="plainTextPassword"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Password</FormLabel>
                <FormControl>
                  <PasswordInput {...field} autoComplete="one-time-code" />
                </FormControl>
                <PasswordValidation password={password} />
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <FormField
          control={form.control}
          name="plainTextPasswordConfirmation"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Password Confirmation</FormLabel>
              <FormControl>
                <PasswordInput
                  onPaste={(event) => event.preventDefault()}
                  {...field}
                  autoComplete="one-time-code"
                />
              </FormControl>
              <PasswordConfirmationValidation
                password={password}
                passwordConfirmation={passwordConfirmation}
              />
              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          className="w-full"
          label="Sign Up"
          loadingLabel="Signing Up..."
          icon={UserPlus}
          isLoading={isSubmitting}
          disabled={!isValid || isSubmitting}
        />
      </form>
    </Form>
  )
}
