import { zodResolver } from '@hookform/resolvers/zod'
import { Pencil, PlusCircle } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '@/components/ui/button'
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'
import { LoadingButton } from '@/components/ui/loading-button'
import { Textarea } from '@/components/ui/textarea'

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
  mode: 'create' | 'update'
}

export function ManagementUnitForm({
  defaultValues,
  onSubmit,
  onCancel,
  mode,
}: ManagementUnitFormProps) {
  const form = useForm<ManagementUnitFormSchema>({
    resolver: zodResolver(managementUnitFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: ManagementUnitFormSchema) {
    await onSubmit(values)
  }

  const { isValid, isDirty, isSubmitting } = form.formState

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
                <Textarea
                  {...field}
                  className="h-[138px] resize-none"
                  maxLength={300}
                />
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
            disabled={isSubmitting}
            className="grow sm:grow-0"
          >
            Cancel
          </Button>

          {mode === 'create' ? (
            <LoadingButton
              type="submit"
              className="min-w-56 grow sm:grow-0"
              label="Create Management Unit"
              loadingLabel="Creating..."
              icon={PlusCircle}
              disabled={!isValid || isSubmitting}
              isLoading={isSubmitting}
            />
          ) : (
            <LoadingButton
              type="submit"
              className="min-w-56 grow sm:grow-0"
              label="Update Management Unit"
              loadingLabel="Updating..."
              icon={Pencil}
              disabled={!isValid || !isDirty || isSubmitting}
              isLoading={isSubmitting}
            />
          )}
        </div>
      </form>
    </Form>
  )
}
