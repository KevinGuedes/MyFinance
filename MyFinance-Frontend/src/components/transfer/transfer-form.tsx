import { zodResolver } from '@hookform/resolvers/zod'
import { TrendingDown, TrendingUp } from 'lucide-react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { BRLToFloat } from '@/lib/utils'

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
import { MoneyInput } from '../ui/money-input'
import { RadioGroup, RadioGroupItem } from '../ui/radio-group'
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
  type: z.enum(['Income', 'Outcome'], {
    required_error: 'Transfer type is required',
  }),
})

type TransferFormSchema = z.infer<typeof transferFormSchema>

export function TransferForm() {
  const form = useForm<TransferFormSchema>({
    resolver: zodResolver(transferFormSchema),
    defaultValues: {
      value: '',
      relatedTo: '',
      description: '',
      settlementDate: undefined,
      category: '',
      accountTag: '',
    },
  })

  function onSubmit(values: TransferFormSchema) {
    console.log(values)
    console.log(values.settlementDate.toISOString())
    console.log(BRLToFloat(values.value))
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex grow flex-col sm:grid sm:grid-cols-2 sm:gap-8"
      >
        <div className="flex flex-col gap-4">
          <FormField
            control={form.control}
            name="value"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Value</FormLabel>
                <FormControl>
                  <MoneyInput {...field} />
                </FormControl>
                <FormMessage />
              </FormItem>
            )}
          />

          <div className="grow">
            <FormField
              control={form.control}
              name="type"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Type</FormLabel>
                  <FormControl>
                    <RadioGroup
                      onValueChange={field.onChange}
                      defaultValue={field.value}
                      className="grid grid-cols-2 gap-4"
                    >
                      <FormItem className="space-y-0">
                        <FormControl>
                          <RadioGroupItem
                            value="Income"
                            id="income"
                            className="peer sr-only"
                          />
                        </FormControl>
                        <FormLabel
                          htmlFor="income"
                          className="group flex cursor-pointer flex-row-reverse items-center justify-center gap-3 rounded-md border-2 border-transparent bg-primary/20 p-1 text-base font-bold text-primary transition-all duration-150 hover:bg-primary/60 hover:text-accent-foreground peer-data-[state=checked]:border-primary peer-data-[state=checked]:bg-primary/60 peer-data-[state=checked]:text-accent-foreground [&:has([data-state=checked])]:border-primary"
                        >
                          <TrendingUp className="mb-1 size-6" />
                          Income
                        </FormLabel>
                      </FormItem>
                      <FormItem className="space-y-0">
                        <FormControl>
                          <RadioGroupItem
                            value="Outcome"
                            id="outcome"
                            className="peer sr-only"
                          />
                        </FormControl>
                        <FormLabel
                          htmlFor="outcome"
                          className="group flex cursor-pointer flex-row-reverse items-center justify-center gap-3 rounded-md border-2 border-transparent bg-destructive/25 p-1 text-base font-bold text-destructive transition-all duration-150 hover:bg-destructive/60 hover:text-accent-foreground peer-data-[state=checked]:border-2 peer-data-[state=checked]:border-destructive peer-data-[state=checked]:bg-destructive/60 peer-data-[state=checked]:text-accent-foreground [&:has([data-state=checked])]:border-destructive"
                        >
                          <TrendingDown className="group mb-1 size-6" />
                          Outcome
                        </FormLabel>
                      </FormItem>
                    </RadioGroup>
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>

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
            name="settlementDate"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Settlement Date</FormLabel>
                <DatePicker {...field} />
                <FormMessage />
              </FormItem>
            )}
          />
        </div>

        <div className="mt-4 space-y-4 sm:mt-0">
          <FormField
            control={form.control}
            name="description"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Description</FormLabel>
                <FormControl>
                  <Textarea {...field} className="resize-none" />
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
                <Select
                  onValueChange={field.onChange}
                  defaultValue={field.value}
                >
                  <FormControl>
                    <SelectTrigger>
                      <SelectValue placeholder="Choose an Account Tag" />
                    </SelectTrigger>
                  </FormControl>
                  <SelectContent>
                    <SelectItem value="guidId1">NU</SelectItem>
                    <SelectItem value="guidId2">BB</SelectItem>
                    <SelectItem value="guidId3">BRAD</SelectItem>
                    <SelectItem value="guidId4">Not Planned</SelectItem>
                    <SelectItem value="guidId5">Not Planned</SelectItem>
                    <SelectItem value="guidId6">Not Planned</SelectItem>
                    <SelectItem value="guidId7">Not Planned</SelectItem>
                    <SelectItem value="guidId8">Not Planned</SelectItem>
                    <SelectItem value="guidId81">Not Planned</SelectItem>
                    <SelectItem value="guidId82">Not Planned</SelectItem>
                    <SelectItem value="guidId83">Not Planned</SelectItem>
                    <SelectItem value="guidId84">Not Planned</SelectItem>
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
                <Select
                  onValueChange={field.onChange}
                  defaultValue={field.value}
                >
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
                    <SelectItem value="guidId5">Not Planned</SelectItem>
                    <SelectItem value="guidId6">Not Planned</SelectItem>
                    <SelectItem value="guidId7">Not Planned</SelectItem>
                    <SelectItem value="guidId8">Not Planned</SelectItem>
                    <SelectItem value="guidId81">Not Planned</SelectItem>
                    <SelectItem value="guidId82">Not Planned</SelectItem>
                    <SelectItem value="guidId83">Not Planned</SelectItem>
                    <SelectItem value="guidId84">Not Planned</SelectItem>
                  </SelectContent>
                </Select>
              </FormItem>
            )}
          />
        </div>

        <div className="mt-4 flex grow items-end sm:col-span-2 sm:mt-0 sm:w-1/2 sm:justify-self-center">
          <Button
            type="submit"
            className="w-full"
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            Add Transfer
          </Button>
        </div>
      </form>
    </Form>
  )
}
