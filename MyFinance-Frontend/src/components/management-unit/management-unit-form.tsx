import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2 } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { useCreateManagementUnit } from '@/http/management-units/use-create-management-unit'

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
  name: z.string().min(1, { message: 'Name is required' }),
  description: z.string().optional(),
})

type ManagementUnitFormSchema = z.infer<typeof managementUnitFormSchema>

export function ManagementUnitForm() {
  const { mutation, isCreating } = useCreateManagementUnit()

  const form = useForm<ManagementUnitFormSchema>({
    resolver: zodResolver(managementUnitFormSchema),
    defaultValues: {
      name: '',
      description: '',
    },
  })

  async function onSubmit(values: ManagementUnitFormSchema) {
    mutation.mutate(values)
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex grow flex-col gap-4"
      >
        <FormField
          control={form.control}
          name="name"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Name</FormLabel>
              <FormControl>
                <Input {...field} />
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
                <Textarea {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <div className="flex grow items-end">
          <Button
            type="submit"
            className="w-full"
            disabled={
              !form.formState.isValid ||
              form.formState.isSubmitting ||
              isCreating
            }
          >
            {isCreating ? (
              <>
                <Loader2 className="mr-2 size-4 animate-spin" />
              </>
            ) : (
              <>Create Management Unit</>
            )}
          </Button>
        </div>
      </form>
    </Form>
  )
}
