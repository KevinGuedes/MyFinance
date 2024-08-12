import { MoreVertical } from 'lucide-react'
import { useRef, useState } from 'react'

import { Button } from '@/components/ui/button'
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuLabel,
  DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu'

import { Transfer } from '../../models/transfer'
import { DeleteTransferAlert } from './delete-transfer-alert'
import { UpdateTransferDialog } from './update-transfer-dialog'

type TransferItemActionsProps = {
  transfer: Transfer
}

export function TransferItemActions({ transfer }: TransferItemActionsProps) {
  const [dropdownOpen, setDropdownOpen] = useState(false)
  const [hasOpenDialog, setHasOpenDialog] = useState(false)
  const dropdownTriggerRef = useRef<null | HTMLButtonElement>(null)
  const focusRef = useRef<null | HTMLButtonElement>(null)

  function handleDialogItemSelect() {
    focusRef.current = dropdownTriggerRef.current
  }

  function handleDialogItemOpenChange(open: boolean) {
    if (!open) {
      setDropdownOpen(false)
    }
    setHasOpenDialog(open)
  }

  return (
    <DropdownMenu
      open={dropdownOpen}
      onOpenChange={setDropdownOpen}
      modal={false}
    >
      <DropdownMenuTrigger asChild>
        <Button
          variant="ghost"
          className="size-7 shrink-0 rounded-full p-0.5"
          size="icon"
          ref={dropdownTriggerRef}
        >
          <span className="sr-only">Open menu</span>

          <MoreVertical className="size-4" />
        </Button>
      </DropdownMenuTrigger>
      <DropdownMenuContent
        className="data-[state=closed]:duration-0"
        align="end"
        hidden={hasOpenDialog}
        onCloseAutoFocus={(event) => {
          if (focusRef.current) {
            focusRef.current.focus()
            focusRef.current = null
            event.preventDefault()
          }
        }}
      >
        <DropdownMenuLabel>Actions</DropdownMenuLabel>

        <UpdateTransferDialog
          transfer={transfer}
          onSelect={handleDialogItemSelect}
          onOpenChange={handleDialogItemOpenChange}
        />

        <DeleteTransferAlert
          transfer={transfer}
          onSelect={handleDialogItemSelect}
          onOpenChange={handleDialogItemOpenChange}
        />
      </DropdownMenuContent>
    </DropdownMenu>
  )
}
