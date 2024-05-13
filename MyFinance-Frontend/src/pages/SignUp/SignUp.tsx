import { FormEvent } from 'react'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/Card'
import { api } from '@/lib/api'
import { SignUpResponse } from '@/stores/userStore'

import { SignUpForm } from './components/SignUpForm'

export function SignUp() {
  async function addUser(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    console.log(event.currentTarget.password.value)

    const response = await api.post<SignUpResponse>('/user/signin', {
      name: event.currentTarget.userName.value,
      email: event.currentTarget.email.value,
      plainTextPassword: event.currentTarget.password.value,
      plainTextPasswordConfirmation:
        event.currentTarget['password-confirmation'].value,
    })

    console.log(response)
  }

  return (
    <Card className="mx-auto max-w-sm">
      <CardHeader>
        <CardTitle className="text-xl">Sign Up</CardTitle>
        <CardDescription>
          Enter your information to create an account
        </CardDescription>
      </CardHeader>
      <CardContent>
        <SignUpForm />
      </CardContent>
    </Card>
  )
}
