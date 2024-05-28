import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2 } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '../ui/button'
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '../ui/form'
import { Input } from '../ui/input'

const categoryFormSchema = z.object({
  name: z.string().min(3, { message: 'Name is required' }).max(50, {
    message: 'Name must be less than 100 characters',
  }),
})

export type CategoryFormSchema = z.infer<typeof categoryFormSchema>

type CategoryFormProps = {
  defaultValues: CategoryFormSchema
  onSubmit: (values: CategoryFormSchema) => Promise<void>
  onCancel: () => void
}

export function CategoryForm({
  defaultValues,
  onSubmit,
  onCancel,
}: CategoryFormProps) {
  const form = useForm<CategoryFormSchema>({
    resolver: zodResolver(categoryFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: CategoryFormSchema) {
    await onSubmit(values)
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(handleSubmit)}
        className="flex grow flex-col gap-4"
      >
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input
                  {...field}
                  autoComplete="off"
                  minLength={3}
                  maxLength={50}
                />
              </FormControl>
              <FormDescription>
                Unique category name with a minimum of 3 characters and maximum
                of 50 characters.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex grow flex-wrap-reverse content-start items-end gap-4 self-end">
          <Button
            type="button"
            variant="outline"
            onClick={onCancel}
            className="grow sm:grow-0"
          >
            Cancel
          </Button>
          <Button
            type="submit"
            className="min-w-36 grow"
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            {form.formState.isSubmitting ? (
              <>
                <Loader2 className="mr-2 size-4 animate-spin" />
                Creating...
              </>
            ) : (
              'Create Category'
            )}
          </Button>
        </div>
      </form>
    </Form>
  )
}
