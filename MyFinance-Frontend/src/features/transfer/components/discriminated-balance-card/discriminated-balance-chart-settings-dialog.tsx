import { Settings } from 'lucide-react'
import { useState } from 'react'

import { Button } from '@/components/ui/button'
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from '@/components/ui/dialog'
import { useDiscriminatedBalanceChartSettings } from '@/features/management-unit/store/discriminated-balance-chart-settings-store'

import {
  DiscriminatedBalanceChartSettingsForm,
  DiscriminatedBalanceChartSettingsFormSchema,
} from './discriminated-balance-chart-settings-form'

export function DiscriminatedBalanceChartSettingsDialog() {
  const { updateSettings } = useDiscriminatedBalanceChartSettings()
  const [isDialogOpen, setIsDialogOpen] = useState(false)

  async function onSubmit(values: DiscriminatedBalanceChartSettingsFormSchema) {
    updateSettings(values)
    setIsDialogOpen(false)
  }

  function onCancel() {
    setIsDialogOpen(false)
  }

  return (
    <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
      <DialogTrigger asChild>
        <Button variant="ghost" size="icon" className="rounded-full">
          <Settings className="size-5" />
          <span className="sr-only">Settings</span>
        </Button>
      </DialogTrigger>
      <DialogContent className="max-w-3xl">
        <DialogHeader>
          <DialogTitle>Discriminated Balance Chart Settings</DialogTitle>
          <DialogDescription>
            Select the settings according to your preferences.{' '}
            <strong>
              These settings will be applied for all Discriminated Balance
              Charts.
            </strong>
          </DialogDescription>
        </DialogHeader>
        <DiscriminatedBalanceChartSettingsForm
          onSubmit={onSubmit}
          onCancel={onCancel}
        />
      </DialogContent>
    </Dialog>
  )
}
