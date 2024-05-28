import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2 } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '../ui/button'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '../ui/form'
import { Input } from '../ui/input'
import { Textarea } from '../ui/textarea'

const managementUnitFormSchema = z.object({
  name: z.string().min(1, { message: 'Name is required' }).max(100, {
    message: 'Name must be less than 100 characters',
  }),
  description: z
    .string()
    .max(300, {
      message: 'Description must be less than 300 characters',
    })
    .optional(),
})

export type ManagementUnitFormSchema = z.infer<typeof managementUnitFormSchema>

type ManagementUnitFormProps = {
  defaultValues: ManagementUnitFormSchema
  onSubmit: (values: ManagementUnitFormSchema) => Promise<void>
  onCancel: () => void
}

export function ManagementUnitForm({
  defaultValues,
  onSubmit,
  onCancel,
}: ManagementUnitFormProps) {
  const form = useForm<ManagementUnitFormSchema>({
    resolver: zodResolver(managementUnitFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: ManagementUnitFormSchema) {
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
                  minLength={1}
                  maxLength={100}
                />
              </FormControl>
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
            className="min-w-52 grow"
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            {form.formState.isSubmitting ? (
              <>
                <Loader2 className="mr-2 size-4 animate-spin" />
                Creating...
              </>
            ) : (
              'Create Management Unit'
            )}
          </Button>
        </div>
      </form>
    </Form>
  )
}
