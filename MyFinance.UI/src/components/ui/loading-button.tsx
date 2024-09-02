import { Loader2 } from 'lucide-react'
import React from 'react'

import { cn } from '@/lib/utils'

import { Button, buttonVariants } from './button'

const sizeMap: Record<
  keyof typeof buttonVariants.variants.size,
  `size-${number}`
> = {
  sm: 'size-4',
  default: 'size-5',
  lg: 'size-6',
  icon: 'size-6',
}

const getIconSize = (size?: keyof typeof buttonVariants.variants.size) => {
  return sizeMap[size || 'default']
}

// https://www.totaltypescript.com/pass-component-as-prop-react
// shows how pass a comp which has a className? prop
// Also has a implementation of how to pass a comp and infer its props
// Whith that, the wrapper comp can recevive another comp and determine the value of the props which will be passed to it

interface LoadingButtonProps
  extends Omit<React.ComponentProps<typeof Button>, 'asChild'> {
  label?: string
  icon?: React.ComponentType<{
    className?: string
  }>
  isLoading: boolean
  loadingLabel?: string
  loadingIcon?: React.ComponentType<{
    className?: string
  }>
}

const LoadingButton = React.forwardRef<HTMLButtonElement, LoadingButtonProps>(
  (
    {
      label,
      icon: Icon,
      isLoading,
      loadingLabel,
      loadingIcon: LoadingIcon,
      disabled,
      size,
      ...props
    },
    ref,
  ) => {
    const iconSize = getIconSize(size)
    const showLabels = size !== 'icon'
    const showLoadingLabel = showLabels && (loadingLabel || label)
    const showLabel = showLabels && label

    return (
      <Button disabled={disabled || isLoading} size={size} {...props} ref={ref}>
        {isLoading ? (
          <>
            {LoadingIcon ? (
              <LoadingIcon className={cn(iconSize, 'mr-2')} />
            ) : (
              <Loader2 className={cn(iconSize, 'mr-2 animate-spin')} />
            )}

            {showLoadingLabel && (loadingLabel || label)}
          </>
        ) : (
          <>
            {Icon && <Icon className={cn(iconSize, 'mr-2')} />}
            {showLabel && label}
          </>
        )}
      </Button>
    )
  },
)
LoadingButton.displayName = 'LoadingButton'

export { LoadingButton }
