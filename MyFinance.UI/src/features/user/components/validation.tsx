import { CheckCircle, XCircle } from 'lucide-react'

type ValidationProps = {
  isValid: boolean
  label: string
}

export function Validation({ isValid, label }: ValidationProps) {
  return (
    <>
      {isValid ? (
        <>
          <CheckCircle className="size-4 text-primary" />
          <p className="text-sm text-muted-foreground">{label}</p>
        </>
      ) : (
        <>
          <XCircle className="size-4 text-muted-foreground" />
          <p className="text-sm text-muted-foreground">{label}</p>
        </>
      )}
    </>
  )
}
