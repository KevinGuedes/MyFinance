import { zodResolver } from '@hookform/resolvers/zod'
import { Pencil, PlusCircle } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '@/components/ui/button'
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { Input } from '@/components/ui/input'

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
  mode: 'create' | 'update'
}

export function CategoryForm({
  defaultValues,
  onSubmit,
  onCancel,
  mode,
}: CategoryFormProps) {
  const form = useForm<CategoryFormSchema>({
    resolver: zodResolver(categoryFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: CategoryFormSchema) {
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
            className="grow sm:grow-0"
            onClick={onCancel}
            disabled={isSubmitting}
            label="Cancel"
          />

          {mode === 'create' ? (
            <Button
              type="submit"
              className="min-w-[10.5rem] grow sm:grow-0"
              label="Create Category"
              loadingLabel="Creating..."
              icon={PlusCircle}
              disabled={!isValid || isSubmitting}
              isLoading={isSubmitting}
            />
          ) : (
            <Button
              type="submit"
              className="min-w-[10.5rem] grow sm:grow-0"
              label="Update Category"
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
