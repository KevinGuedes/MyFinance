import { Validation } from './validation'

type PasswordConfirmationValidation = {
  password: string
  passwordConfirmation: string
}

export function PasswordConfirmationValidation({
  password,
  passwordConfirmation,
}: PasswordConfirmationValidation) {
  const isValid =
    passwordConfirmation === password &&
    passwordConfirmation.length > 0 &&
    password.length > 0

  return (
    <div className="flex items-center gap-2">
      <Validation isValid={isValid} label="Passwords must match" />
    </div>
  )
}
