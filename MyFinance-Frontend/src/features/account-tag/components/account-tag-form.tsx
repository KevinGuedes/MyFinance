import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2, PlusCircle } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '../../../components/ui/button'
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '../../../components/ui/form'
import { Input } from '../../../components/ui/input'
import { Textarea } from '../../../components/ui/textarea'

const accountTagFormSchema = z.object({
  tag: z.string().min(2, { message: 'Tag is required' }).max(10, {
    message: 'Tag must be less than 100 characters',
  }),
  description: z.string().max(300, {
    message: 'Description must be less than 300 characters',
  }),
})

export type AccountTagFormSchema = z.infer<typeof accountTagFormSchema>

type AccountTagFormProps = {
  defaultValues: AccountTagFormSchema
  onSubmit: (values: AccountTagFormSchema) => Promise<void>
  onCancel: () => void
}

export function AccountTagForm({
  defaultValues,
  onSubmit,
  onCancel,
}: AccountTagFormProps) {
  const form = useForm<AccountTagFormSchema>({
    resolver: zodResolver(accountTagFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: AccountTagFormSchema) {
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
          name="tag"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Tag</FormLabel>
              <FormControl>
                <Input
                  {...field}
                  autoComplete="off"
                  minLength={2}
                  maxLength={10}
                  onChange={(e) => {
                    field.onChange(e.target.value.toUpperCase())
                  }}
                />
              </FormControl>
              <FormDescription>
                The tag is an unique identifier for an account and must be
                between 2 and 10 characters.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="description"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Description (Optional)</FormLabel>
              <FormControl>
                <Textarea {...field} className="resize-none" maxLength={300} />
              </FormControl>
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
            className="min-w-40 grow"
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            {form.formState.isSubmitting ? (
              <>
                <Loader2 className="mr-2 size-4 animate-spin" />
                Creating...
              </>
            ) : (
              <>
                <PlusCircle className="mr-2 size-4" />
                Create Account Tag
              </>
            )}
          </Button>
        </div>
      </form>
    </Form>
  )
}
