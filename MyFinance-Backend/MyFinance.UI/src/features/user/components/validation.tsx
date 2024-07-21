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
          <span className="text-sm text-muted-foreground">{label}</span>
        </>
      ) : (
        <>
          <XCircle className="size-4 text-muted-foreground" />
          <span className="text-sm text-muted-foreground">{label}</span>
        </>
      )}
    </>
  )
}
