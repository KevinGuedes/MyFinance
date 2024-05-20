import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { MoneyInput } from '../money-input'
import { Button } from '../ui/button'
import { DatePicker } from '../ui/date-picker'
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

const transferFormSchema = z.object({
  value: z.string().min(1, { message: 'Value is required' }),
  relatedTo: z.string().min(1, { message: 'Related to is required' }),
  description: z.string(),
  settlementDate: z.date({ required_error: 'Settlement date is required' }),
})

type TransferFormSchema = z.infer<typeof transferFormSchema>

export function TransferForm() {
  const form = useForm<TransferFormSchema>({
    resolver: zodResolver(transferFormSchema),
    defaultValues: {
      value: '',
      relatedTo: '',
      description: '',
      settlementDate: new Date(),
    },
  })

  function onSubmit(values: TransferFormSchema) {
    console.log(values)
    console.log(values.settlementDate.toISOString())
  }
  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
        <FormField
          control={form.control}
          name="value"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Value (R$)</FormLabel>
              <FormControl>
                <MoneyInput {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="relatedTo"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Related To</FormLabel>
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

        <FormField
          control={form.control}
          name="settlementDate"
          render={({ field }) => (
            <FormItem className="flex flex-col">
              <FormLabel>Date of birth</FormLabel>
              <DatePicker {...field} />
              <FormMessage />
            </FormItem>
          )}
        />

        <Button
          type="submit"
          className="w-full"
          disabled={!form.formState.isValid}
        >
          Add Transfer
        </Button>
      </form>
    </Form>
  )
}
