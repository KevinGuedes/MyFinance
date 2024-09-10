import { Slot } from '@radix-ui/react-slot'
import { Loader2 } from 'lucide-react'
import * as React from 'react'
import { tv, type VariantProps } from 'tailwind-variants'

import { cn } from '@/lib/utils'

const buttonVariants = tv({
  base: 'inline-flex items-center justify-center whitespace-nowrap rounded-md text-sm font-medium ring-offset-background transition-colors focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50 disabled:select-none',
  variants: {
    variant: {
      default: 'bg-primary text-primary-foreground hover:bg-primary/90',
      destructive:
        'bg-destructive text-destructive-foreground hover:bg-destructive/90',
      outline:
        'border border-input bg-background hover:bg-accent hover:text-accent-foreground',
      secondary: 'bg-secondary text-secondary-foreground hover:bg-secondary/80',
      ghost: 'hover:bg-accent hover:text-accent-foreground',
      link: 'text-primary relative after:absolute after:bg-primary after:-bottom-0.5 after:left-0 after:h-[1px] after:w-full after:origin-bottom-right after:scale-x-0 hover:after:origin-bottom-left hover:after:scale-x-100 after:transition-transform after:ease-in-out after:duration-300',
    },
    size: {
      default: 'h-10 px-4 py-2',
      sm: 'h-9 rounded-md px-3',
      lg: 'h-11 rounded-md px-8',
      icon: 'size-10',
      'icon-sm': 'size-7',
      'icon-md': 'size-8',
    },
  },
  defaultVariants: {
    variant: 'default',
    size: 'default',
  },
})

const iconSizeMap: Record<
  keyof typeof buttonVariants.variants.size,
  `size-${number}`
> = {
  sm: 'size-4',
  default: 'size-5',
  lg: 'size-6',
  'icon-sm': 'size-4',
  icon: 'size-5',
  'icon-md': 'size-4',
}

export interface ButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement>,
    VariantProps<typeof buttonVariants> {
  label?: string
  icon?: React.ComponentType<{
    className?: string
  }>
  iconLocation?: 'left' | 'right'
  isLoading?: boolean
  loadingLabel?: string
  loadingIcon?: React.ComponentType<{
    className?: string
  }>
  preserveLabelWhenLoading?: boolean
  screenReaderLabel?: string
  asChild?: boolean
  children?: React.ReactNode
}

const Button = React.forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant, size, asChild = false, ...props }, ref) => {
    if (asChild) {
      return (
        <Slot
          className={cn(buttonVariants({ variant, size, className }))}
          ref={ref}
          {...props}
        />
      )
    }

    // This allows us to use the Button component as a wrapper for other components and create more fancy behaviors and styles. But most of the time (> 99%), we'll use the Button with the label and icon props.
    if (React.Children.count(props.children) > 0) {
      return (
        <button
          className={cn(buttonVariants({ variant, size, className }))}
          ref={ref}
          {...props}
        />
      )
    }

    const {
      label,
      icon,
      iconLocation = 'left',
      isLoading = false,
      loadingLabel,
      loadingIcon = Loader2,
      disabled,
      preserveLabelWhenLoading = true,
      screenReaderLabel,
      ...otherProps
    } = props

    const isSizeIcon =
      size === 'icon' || size === 'icon-sm' || size === 'icon-md'
    const iconSize = iconSizeMap[size || 'default']
    const shouldAnimateLoadingIcon = loadingIcon === Loader2 && isLoading
    const actualLoadingLabel =
      loadingLabel || (preserveLabelWhenLoading ? label : null)

    const Icon = isLoading ? loadingIcon : icon

    return (
      <button
        className={cn(buttonVariants({ variant, size, className }))}
        disabled={disabled || isLoading}
        ref={ref}
        {...otherProps}
      >
        {Icon && iconLocation === 'left' && (
          <Icon
            className={cn(
              !isSizeIcon && 'mr-2',
              shouldAnimateLoadingIcon && 'animate-spin',
              iconSize,
            )}
          />
        )}
        {isLoading ? actualLoadingLabel : label}
        {Icon && iconLocation === 'right' && (
          <Icon
            className={cn(
              !isSizeIcon && 'ml-2',
              shouldAnimateLoadingIcon && 'animate-spin',
              iconSize,
            )}
          />
        )}
        <span className="sr-only">{screenReaderLabel}</span>
      </button>
    )
  },
)
Button.displayName = 'Button'

export { Button, buttonVariants }
