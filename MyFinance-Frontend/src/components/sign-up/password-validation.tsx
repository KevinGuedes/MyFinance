import { CheckCircle, XCircle } from 'lucide-react'

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
      label: '2 digits',
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
          {password && regex.test(password) ? (
            <>
              <CheckCircle className="size-4 text-primary" />
              <span className="text-sm text-muted-foreground">{label}</span>
            </>
          ) : (
            <>
              <XCircle className="size-4 text-muted-foreground" />
              <span className="text-sm text-muted-foreground">{label}</span>
            </>
          )}
        </li>
      ))}
    </ul>
  )
}
