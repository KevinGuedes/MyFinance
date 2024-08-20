import type { Dispatch, SetStateAction } from 'react'
import { useCallback, useEffect, useState } from 'react'
import { useEventCallback, useEventListener } from 'usehooks-ts'

const LOCAL_STORAGE_MAIN_KEY = '@my-finance:v1.0.0:'

declare global {
  interface WindowEventMap {
    'local-storage': CustomEvent
  }
}

type UseLocalStorageOptions = {
  initializeWithValue?: boolean
}

export function useLocalStorage<T>(
  key: string,
  initialValue: T | (() => T),
  options: UseLocalStorageOptions = {
    initializeWithValue: true,
  },
): [T, Dispatch<SetStateAction<T>>, () => void] {
  const actualKey = LOCAL_STORAGE_MAIN_KEY + key

  const { initializeWithValue } = options

  const serializer = useCallback<(value: T) => string>((value) => {
    return JSON.stringify(value)
  }, [])

  const deserializer = useCallback<(value: string) => T>(
    (value) => {
      if (value === 'undefined') {
        return undefined as unknown as T
      }

      const defaultValue =
        initialValue instanceof Function ? initialValue() : initialValue

      let parsed: unknown
      try {
        parsed = JSON.parse(value)
      } catch (error) {
        console.error('Error parsing JSON:', error)
        return defaultValue
      }

      return parsed as T
    },
    [initialValue],
  )

  const readValue = useCallback((): T => {
    const initialValueToUse =
      initialValue instanceof Function ? initialValue() : initialValue

    try {
      const raw = window.localStorage.getItem(actualKey)
      return raw ? deserializer(raw) : initialValueToUse
    } catch (error) {
      console.warn(`Error reading localStorage key "${actualKey}":`, error)
      return initialValueToUse
    }
  }, [initialValue, actualKey, deserializer])

  const [storedValue, setStoredValue] = useState(() => {
    if (initializeWithValue) {
      return readValue()
    }

    return initialValue instanceof Function ? initialValue() : initialValue
  })

  const setValue: Dispatch<SetStateAction<T>> = useEventCallback((value) => {
    try {
      const newValue = value instanceof Function ? value(readValue()) : value
      window.localStorage.setItem(actualKey, serializer(newValue))
      setStoredValue(newValue)
      window.dispatchEvent(
        new StorageEvent('local-storage', { key: actualKey }),
      )
    } catch (error) {
      console.warn(`Error setting localStorage key "${actualKey}":`, error)
    }
  })

  const removeValue = useEventCallback(() => {
    const defaultValue =
      initialValue instanceof Function ? initialValue() : initialValue

    window.localStorage.removeItem(actualKey)
    setStoredValue(defaultValue)
    window.dispatchEvent(new StorageEvent('local-storage', { key: actualKey }))
  })

  useEffect(() => {
    setStoredValue(readValue())
  }, [actualKey, readValue])

  const handleStorageChange = useCallback(
    (event: StorageEvent | CustomEvent) => {
      if (
        (event as StorageEvent).key &&
        (event as StorageEvent).key !== actualKey
      ) {
        return
      }
      setStoredValue(readValue())
    },
    [actualKey, readValue],
  )

  useEventListener('storage', handleStorageChange)
  useEventListener('local-storage', handleStorageChange)

  return [storedValue, setValue, removeValue]
}
