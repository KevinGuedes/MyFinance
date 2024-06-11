import { Validation } from './validation'

type PasswordValidationProps = {
  password?: string
}

export function PasswordValidation({ password }: PasswordValidationProps) {
  const validations = [
    {
      label: '16 characters',
      regex: /.{16,}/,
    },
    {
      label: '2 special characters',
      regex: /(?=(.*[^a-zA-Z0-9]){2,})/,
    },
    {
      label: '2 numbers',
      regex: /(\D*\d){2,}/,
    },
    {
      label: '2 uppercase letters',
      regex: /(?=(.*[A-Z]){2,})/,
    },
    {
      label: '2 lowercase letters',
      regex: /(?=(.*[a-z]){2,})/,
    },
  ]

  return (
    <ul className="space-y-1">
      {validations.map(({ label, regex }) => (
        <li key={label} className="flex items-center gap-2">
          <Validation isValid={regex.test(password ?? '')} label={label} />
        </li>
      ))}
    </ul>
  )
}
