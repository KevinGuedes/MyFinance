/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_MY_FINANCE_API_URL: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
