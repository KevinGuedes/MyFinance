import { zodResolver } from '@hookform/resolvers/zod'
import { Settings2 } from 'lucide-react'
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
import { Label } from '@/components/ui/label'
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { Switch } from '@/components/ui/switch'
import { useDiscriminatedBalanceChartSettings } from '@/features/management-unit/store/discriminated-balance-chart-settings-store'

const discriminatedBalanceChartSettingsFormSchema = z.object({
  showBalance: z.boolean(),
  showIncome: z.boolean(),
  showOutcome: z.boolean(),
  includeCurrentMonth: z.boolean(),
  pastMonths: z.coerce.number(),
  showYAxis: z.boolean(),
  showLegend: z.boolean(),
  showDataPointsWhenHovering: z.boolean(),
})

export type DiscriminatedBalanceChartSettingsFormSchema = z.infer<
  typeof discriminatedBalanceChartSettingsFormSchema
>

type DiscriminatedBalanceChartSettingsFormProps = {
  onSubmit: (values: DiscriminatedBalanceChartSettingsFormSchema) => void
  onCancel: () => void
}

export function DiscriminatedBalanceChartSettingsForm({
  onSubmit,
  onCancel,
}: DiscriminatedBalanceChartSettingsFormProps) {
  const {
    pastMonths,
    showYAxis,
    includeCurrentMonth,
    showLegend,
    showBalance,
    showIncome,
    showOutcome,
    showDataPointsWhenHovering,
  } = useDiscriminatedBalanceChartSettings()

  const form = useForm<DiscriminatedBalanceChartSettingsFormSchema>({
    resolver: zodResolver(discriminatedBalanceChartSettingsFormSchema),
    defaultValues: {
      showBalance,
      showIncome,
      showOutcome,
      includeCurrentMonth,
      pastMonths,
      showYAxis,
      showLegend,
      showDataPointsWhenHovering,
    },
  })

  return (
    <Form {...form}>
      <form
        onSubmit={form.handleSubmit(onSubmit)}
        className="flex grow flex-col gap-8"
      >
        <div className="flex flex-col gap-6 sm:flex-row sm:gap-8">
          <div className="flex flex-col gap-8 sm:w-1/2">
            <FormField
              control={form.control}
              name="showBalance"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between gap-4 space-y-0">
                  <FormLabel>Show Balance</FormLabel>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="showIncome"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between gap-4 space-y-0">
                  <FormLabel>Show Income</FormLabel>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="showOutcome"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between gap-4 space-y-0">
                  <FormLabel>Show Outcome</FormLabel>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="includeCurrentMonth"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between gap-4 space-y-0">
                  <FormLabel> Include Current Month</FormLabel>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="pastMonths"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between gap-4">
                  <Label>Amount of past months to show</Label>
                  <Select
                    value={field.value.toString()}
                    onValueChange={field.onChange}
                  >
                    <FormControl>
                      <SelectTrigger className="w-14">
                        <SelectValue placeholder="Past months" />
                      </SelectTrigger>
                    </FormControl>

                    <SelectContent>
                      <SelectItem value="3">3</SelectItem>
                      <SelectItem value="6">6</SelectItem>
                      <SelectItem value="9">9</SelectItem>
                      <SelectItem value="12">12</SelectItem>
                    </SelectContent>
                  </Select>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
          <div className="flex grow flex-col justify-between gap-6 sm:w-1/2">
            <FormField
              control={form.control}
              name="showYAxis"
              render={({ field }) => (
                <FormItem className="flex items-center justify-between gap-4 space-y-0">
                  <FormLabel>Show Y Axis</FormLabel>
                  <FormControl>
                    <Switch
                      checked={field.value}
                      onCheckedChange={field.onChange}
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              )}
            />

            <FormField
              control={form.control}
              name="showLegend"
              render={({ field }) => (
                <FormItem className="space-y-0">
                  <div className="flex items-center justify-between gap-4">
                    <FormLabel>Show Legend</FormLabel>
                    <FormControl>
                      <Switch
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                  </div>
                  <FormDescription className="mt-0 w-10/12">
                    When one of the legend items is hovered, the regarding data
                    set will be highlighted while the others will be faded
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="showDataPointsWhenHovering"
              render={({ field }) => (
                <FormItem className="space-y-0">
                  <div className="flex items-center justify-between gap-4">
                    <FormLabel>Show Data Points when hovering Legend</FormLabel>
                    <FormControl className="shrink-0">
                      <Switch
                        checked={field.value}
                        onCheckedChange={field.onChange}
                      />
                    </FormControl>
                  </div>

                  <FormDescription className="mt-0 w-10/12">
                    When hovering the legend, the Y value (R$) of each data
                    point of the selected data set will be shown.{' '}
                    <strong>
                      Available only if &apos;Show Legend&apos; is checked
                    </strong>
                  </FormDescription>
                  <FormMessage />
                </FormItem>
              )}
            />
          </div>
        </div>
        <div className="flex grow flex-wrap-reverse content-start items-end gap-4 self-end">
          <Button
            type="button"
            variant="outline"
            className="grow"
            onClick={onCancel}
          >
            Cancel
          </Button>
          <Button
            type="submit"
            className="grow"
            disabled={!form.formState.isValid || !form.formState.isDirty}
          >
            <Settings2 className="mr-2 size-4" />
            Save and Apply
          </Button>
        </div>
      </form>
    </Form>
  )
}
