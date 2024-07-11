export function handleValidationErrors(
  validationErrors: Record<string, string[]> | undefined,
  toastCallback: (title: string, description: string) => void,
) {
  if (validationErrors === undefined) return

  Object.keys(validationErrors).forEach((key) => {
    const errorMessages = validationErrors[key]
    const title = 'Uh oh! Something went wrong!'
    const description = errorMessages[0]

    toastCallback(title, description)
  })
}
