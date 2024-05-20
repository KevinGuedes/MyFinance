import { zodResolver } from '@hookform/resolvers/zod'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { MoneyInput } from '../money-input'
import { Button } from '../ui/button'
import { DatePicker } from '../ui/date-picker'
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '../ui/select'
import { Textarea } from '../ui/textarea'

const transferFormSchema = z.object({
  value: z.string().min(1, { message: 'Value is required' }),
  relatedTo: z.string().min(1, { message: 'Related to is required' }),
  description: z.string().optional(),
  settlementDate: z.date({ required_error: 'Settlement date is required' }),
  category: z.string().min(1, { message: 'Category  is required' }),
  accountTag: z.string().min(1, { message: 'Account Tag is required' }),
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
      category: '',
      accountTag: '',
    },
  })

  function onSubmit(values: TransferFormSchema) {
    console.log(values)
    console.log(values.settlementDate.toISOString())
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-4">
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
          name="accountTag"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Account Tag</FormLabel>
              <Select onValueChange={field.onChange} defaultValue={field.value}>
                <FormControl>
                  <SelectTrigger>
                    <SelectValue placeholder="Choose an Account Tag" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  <SelectItem value="guidId1">NU</SelectItem>
                  <SelectItem value="guidId2">BB</SelectItem>
                  <SelectItem value="guidId3">BRAD</SelectItem>
                </SelectContent>
              </Select>
              <FormDescription>
                Select the account you&apos;ve used to send or receive this
                transfer.
              </FormDescription>
              <FormMessage />
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="category"
          render={({ field }) => (
            <FormItem>
              <FormLabel>Category</FormLabel>
              <Select onValueChange={field.onChange} defaultValue={field.value}>
                <FormControl>
                  <SelectTrigger>
                    <SelectValue placeholder="Choose a Category" />
                  </SelectTrigger>
                </FormControl>
                <SelectContent>
                  <SelectItem value="guidId1">Health</SelectItem>
                  <SelectItem value="guidId2">Groceries</SelectItem>
                  <SelectItem value="guidId3">Emergency</SelectItem>
                  <SelectItem value="guidId4">Not Planned</SelectItem>
                </SelectContent>
              </Select>
            </FormItem>
          )}
        />

        <FormField
          control={form.control}
          name="settlementDate"
          render={({ field }) => (
            <FormItem className="flex flex-col">
              <FormLabel>Settlement Date</FormLabel>
              <DatePicker {...field} showPresetDates />
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
          disabled={!form.formState.isValid}
        >
          Add Transfer
        </Button>
      </form>
    </Form>
  )
}
