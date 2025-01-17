import { zodResolver } from '@hookform/resolvers/zod'
import { Loader2, PlusCircle, TrendingDown, TrendingUp } from 'lucide-react'
import { useState } from 'react'
import { useForm } from 'react-hook-form'
import { z } from 'zod'

import { Button } from '@/components/ui/button'
import { DatePicker } from '@/components/ui/date-picker'
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
import { MoneyInput } from '@/components/ui/money-input'
import { RadioGroup, RadioGroupItem } from '@/components/ui/radio-group'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Textarea } from '@/components/ui/textarea'
import { TransferType } from '@/features/transfer/models/transfer-type'
import { getEnumKeys, isValidEnumKey } from '@/lib/utils'

const transferFormSchema = z.object({
  value: z
    .number({ message: 'Value is required' })
    .positive({ message: 'Value is required' }),
  relatedTo: z.string().min(1, { message: 'Related to is required' }),
  description: z.string().optional(),
  settlementDate: z
    .date({ required_error: 'Settlement Date is required' })
    .optional()
    .refine((settlementDate) => settlementDate !== undefined, {
      message: 'Settlement Date is required',
    }),
  categoryId: z.string().min(1, { message: 'Category  is required' }),
  accountTagId: z
    .string()
    .min(1, { message: 'Account Tag is required' })
    .optional(),
  type: z
    .enum(getEnumKeys(TransferType), {
      message: 'Transfer type is required',
    })
    .refine((type) => isValidEnumKey(TransferType, type), {
      message: 'Transfer type is required',
    }),
})

export type TransferFormSchema = z.infer<typeof transferFormSchema>

type TransferFormProps = {
  defaultValues?: TransferFormSchema
  onSubmit: (
    values: TransferFormSchema,
    shouldCloseDialog?: boolean,
  ) => Promise<void>
  onCancel: () => void
}

export function TransferForm({
  defaultValues,
  onSubmit,
  onCancel,
}: TransferFormProps) {
  const [isRegistering, setIsRegistering] = useState(false)
  const [isRegisteringAndAddingMore, setIsRegisteringAndAddingMore] =
    useState(false)
  const form = useForm<TransferFormSchema>({
    resolver: zodResolver(transferFormSchema),
    defaultValues,
  })

  async function handleSubmit(values: TransferFormSchema) {
    setIsRegistering(true)
    await onSubmit(values)
    setIsRegistering(false)
  }

  async function handleRegisterAndAddMore(values: TransferFormSchema) {
    setIsRegisteringAndAddingMore(true)
    await onSubmit(values, false)
    form.reset()
    setIsRegisteringAndAddingMore(false)
  }

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(handleSubmit)}
        className="flex flex-col gap-8"
      >
        <div className="flex flex-col gap-4 sm:flex-row sm:gap-8">
          <div className="flex basis-1/2 flex-col gap-4">
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
                        value={field.value}
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
                            className="group flex cursor-pointer flex-row-reverse items-center justify-center gap-3 rounded-md border-2 border-transparent bg-primary/10 p-1 text-base font-bold text-primary transition-all duration-150 hover:bg-primary/60 hover:text-accent-foreground peer-data-[state=checked]:border-primary peer-data-[state=checked]:bg-primary/60 peer-data-[state=checked]:text-accent-foreground [&:has([data-state=checked])]:border-primary"
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

          <div className="flex basis-1/2 flex-col gap-4">
            <FormField
              control={form.control}
              name="accountTagId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Account Tag</FormLabel>
                  <Select onValueChange={field.onChange} value={field.value}>
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
              name="categoryId"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Category</FormLabel>
                  <Select onValueChange={field.onChange} value={field.value}>
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
              name="description"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Description</FormLabel>
                  <FormControl>
                    <Textarea
                      {...field}
                      className="resize-none"
                      maxLength={300}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
        </div>

        <div className="flex flex-wrap-reverse content-start justify-end gap-4">
          <Button
            type="button"
            variant="outline"
            className="grow sm:grow-0"
            onClick={onCancel}
          >
            Cancel
          </Button>
          <Button
            type="button"
            className="min-w-48 grow sm:grow-0"
            variant="secondary"
            onClick={form.handleSubmit(handleRegisterAndAddMore)}
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            {isRegisteringAndAddingMore ? (
              <>
                <Loader2 className="mr-2 size-5 animate-spin" />
                Registering...
              </>
            ) : (
              <>
                <PlusCircle className="mr-2 size-5" />
                Register and Add More
              </>
            )}
          </Button>
          <Button
            type="submit"
            className="min-w-36 grow sm:grow-0"
            disabled={!form.formState.isValid || form.formState.isSubmitting}
          >
            {isRegistering ? (
              <>
                <Loader2 className="size-5 animate-spin" />
                Registering...
              </>
            ) : (
              <>
                <PlusCircle className="mr-2 size-5" />
                Register Transfer
              </>
            )}
          </Button>
        </div>
      </form>
    </Form>
  )
}
