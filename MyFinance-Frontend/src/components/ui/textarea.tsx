import { Mic } from 'lucide-react'
import * as React from 'react'

import { cn } from '@/lib/utils'

import { Button } from './button'
import { useToast } from './toast/use-toast'
import { Tooltip, TooltipContent, TooltipTrigger } from './tooltip'

const SpeechRecognitionAPI =
  window.SpeechRecognition || window.webkitSpeechRecognition

const speechRecognition = new SpeechRecognitionAPI()

export interface TextareaProps
  extends Omit<React.TextareaHTMLAttributes<HTMLTextAreaElement>, 'onChange'> {
  useSpeechRecognition?: boolean
  onChange: (value?: string) => void
}

const Textarea = React.forwardRef<HTMLTextAreaElement, TextareaProps>(
  (
    {
      className,
      disabled,
      value,
      onChange,
      useSpeechRecognition = true,
      ...props
    },
    ref,
  ) => {
    const { toast } = useToast()
    const [isRecording, setIsRecording] = React.useState(false)
    const [content, setContent] = React.useState(value || '')

    function handleStartRecording() {
      setIsRecording(true)

      speechRecognition.lang = 'pt-BR'
      speechRecognition.continuous = false
      speechRecognition.maxAlternatives = 1
      speechRecognition.interimResults = false

      speechRecognition.onresult = (event) => {
        const transcription = Array.from(event.results).reduce(
          (text, result) => {
            return text.concat(result[0].transcript)
          },
          '',
        )

        setContent((prevContent) => {
          const isContentEmpty = prevContent === ''

          if (isContentEmpty) {
            const newContent =
              transcription.charAt(0).toUpperCase() + transcription.slice(1)
            onChange?.(newContent)
            return newContent
          }

          const hasEmptySpaceInTheEnd = prevContent.toString().endsWith(' ')

          if (hasEmptySpaceInTheEnd) {
            const newContent = prevContent + transcription
            onChange?.(newContent)
            return newContent
          } else {
            const newContent = prevContent + ' ' + transcription
            onChange?.(newContent)
            return newContent
          }
        })
      }

      speechRecognition.onend = () => {
        setIsRecording(false)
      }

      speechRecognition.onerror = (event) => {
        console.error(event.error)

        toast({
          title: 'Unable to record',
          description: 'Sorry for the inconvenience.',
          variant: 'destructive',
        })
      }

      speechRecognition.start()
    }

    function handleStopRecording() {
      setIsRecording(false)

      if (speechRecognition !== null) {
        speechRecognition.stop()
      }
    }

    function handleChange(event: React.ChangeEvent<HTMLTextAreaElement>) {
      setContent(event.target.value)
      onChange?.(event.target.value)
    }

    const isSpeechRecognitionApiAvailable =
      'SpeechRecognition' in window || 'webkitSpeechRecognition' in window

    return (
      <div className="relative">
        <textarea
          disabled={disabled || isRecording}
          className={cn(
            'flex min-h-[80px] w-full rounded-md border border-input bg-background px-3 py-2 text-sm ring-offset-background placeholder:text-muted-foreground  focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50',
            className,
          )}
          ref={ref}
          value={content}
          {...props}
          onChange={handleChange}
        />
        {isSpeechRecognitionApiAvailable && useSpeechRecognition && (
          <div className="absolute bottom-1 right-1 ">
            {isRecording ? (
              <Tooltip delayDuration={1000}>
                <TooltipTrigger asChild>
                  <Button
                    className="size-7 hover:bg-transparent"
                    variant="ghost"
                    size="icon"
                    type="button"
                    onClick={handleStopRecording}
                  >
                    <div className="size-3 animate-pulse rounded-full bg-red-500" />
                  </Button>
                </TooltipTrigger>
                <TooltipContent side="left">Stop recording</TooltipContent>
              </Tooltip>
            ) : (
              <Tooltip delayDuration={1000}>
                <TooltipTrigger asChild>
                  <Button
                    type="button"
                    variant="ghost"
                    size="icon"
                    className="size-7 text-muted-foreground transition-colors hover:bg-transparent hover:text-foreground"
                    onClick={handleStartRecording}
                  >
                    <Mic className="size-5" />
                  </Button>
                </TooltipTrigger>
                <TooltipContent side="left">Start recording</TooltipContent>
              </Tooltip>
            )}
          </div>
        )}
      </div>
    )
  },
)
Textarea.displayName = 'Textarea'

export { Textarea }
