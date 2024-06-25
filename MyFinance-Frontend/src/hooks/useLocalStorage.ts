import {
  Dispatch,
  SetStateAction,
  useCallback,
  useEffect,
  useState,
} from 'react'
import superjson from 'superjson'
import { useEventCallback, useEventListener } from 'usehooks-ts'

const LOCAL_STORAGE_MAIN_KEY = '@my-finance:v1.0.0:settings'

type UseLocalStorageOptions = {
  initializeWithValue?: boolean
}

export function useLocalStorage<T>(
  key: string,
  initialValue: T | (() => T),
  options: UseLocalStorageOptions = {
    initializeWithValue: true,
  },
) {
  const serializer = useCallback<(value: T) => string>((value) => {
    return superjson.stringify(value)
  }, [])

  const deserializer = useCallback<(value: string) => T>((value) => {
    return superjson.parse<T>(value)
  }, [])

  const readValue = useCallback((): T => {
    const initialValueToUse =
      initialValue instanceof Function ? initialValue() : initialValue

    try {
      const raw = window.localStorage.getItem(key)
      return raw ? deserializer(raw) : initialValueToUse
    } catch (error) {
      console.warn(`Error reading localStorage key "${key}": `, error)
      return initialValueToUse
    }
  }, [initialValue, key, deserializer])

  const [storedValue, setStoredValue] = useState(() => {
    if (options.initializeWithValue) {
      return readValue()
    }

    return initialValue instanceof Function ? initialValue() : initialValue
  })

  const setValue: Dispatch<SetStateAction<T>> = useEventCallback((value) => {
    try {
      const newValue = value instanceof Function ? value(readValue()) : value
      window.localStorage.setItem(key, serializer(newValue))
      setStoredValue(newValue)
      window.dispatchEvent(new StorageEvent('local-storage', { key }))
    } catch (error) {
      console.warn(`Error setting localStorage key "${key}": `, error)
    }
  })

  const removeValue = useEventCallback(() => {
    const defaultValue =
      initialValue instanceof Function ? initialValue() : initialValue

    window.localStorage.removeItem(key)
    setStoredValue(defaultValue)
    window.dispatchEvent(new StorageEvent('local-storage', { key }))
  })

  useEffect(() => {
    setStoredValue(readValue())
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [key])

  const handleStorageChange = useCallback(
    (event: StorageEvent | CustomEvent) => {
      if ((event as StorageEvent).key && (event as StorageEvent).key !== key) {
        return
      }
      setStoredValue(readValue())
    },
    [key, readValue],
  )

  useEventListener('storage', handleStorageChange)

  useEventListener('local-storage', handleStorageChange)

  return [storedValue, setValue, removeValue]
}
