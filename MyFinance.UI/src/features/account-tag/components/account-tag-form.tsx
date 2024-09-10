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
import { Textarea } from '@/components/ui/textarea'

const accountTagFormSchema = z.object({
  tag: z.string().min(2, { message: 'Tag is required' }).max(5, {
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
  mode: 'create' | 'update'
}

export function AccountTagForm({
  defaultValues,
  onSubmit,
  onCancel,
  mode,
}: AccountTagFormProps) {
  const form = useForm<AccountTagFormSchema>({
    resolver: zodResolver(accountTagFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: AccountTagFormSchema) {
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
          name="tag"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Tag</FormLabel>
              <FormControl>
                <Input
                  {...field}
                  autoComplete="off"
                  minLength={2}
                  maxLength={5}
                  onChange={(e) => {
                    field.onChange(e.target.value.toUpperCase())
                  }}
                />
              </FormControl>
              <FormDescription>
                The tag is an unique identifier for an account and must be
                between 2 and 5 characters.
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
            className="grow sm:grow-0"
            variant="outline"
            onClick={onCancel}
            disabled={isSubmitting}
            label="Cancel"
          />

          {mode === 'create' ? (
            <Button
              type="submit"
              className="min-w-[11.5rem] grow sm:grow-0"
              label="Create Account Tag"
              loadingLabel="Creating..."
              icon={PlusCircle}
              disabled={!isValid || isSubmitting}
              isLoading={isSubmitting}
            />
          ) : (
            <Button
              type="submit"
              className="min-w-[11.5rem] grow sm:grow-0"
              label="Update Account Tag"
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
