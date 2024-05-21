import { zodResolver } from '@hookform/resolvers/zod'
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

const managementUnitFormSchema = z
  .object({
    name: z.string().min(1, { message: 'Name is required' }),
    description: z.string().optional(),
    dateRange: z.object(
      {
        from: z.date(),
        to: z.date(),
      },
      {
        required_error: 'Please select a date range',
      },
    ),
  })
  .refine((data) => data.dateRange.from < data.dateRange.to, {
    path: ['dateRange'],
    message: 'From date must be before to date',
  })

type ManagementUnitFormSchema = z.infer<typeof managementUnitFormSchema>

export function ManagementUnitForm() {
  const form = useForm<ManagementUnitFormSchema>({
    resolver: zodResolver(managementUnitFormSchema),
    defaultValues: {
      name: '',
      description: '',
    },
  })

  function onSubmit(values: ManagementUnitFormSchema) {
    console.log(values)
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
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
              <FormLabel>Description</FormLabel>
              <FormControl>
                <Textarea {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          className="w-full"
          disabled={!form.formState.isValid || form.formState.isSubmitting}
        >
          Create Management Unit
        </Button>
      </form>
    </Form>
  )
}
