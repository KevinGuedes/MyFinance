/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_TEST: string
    readonly TEST_ENV_VITE: string
  }
  
  interface ImportMeta {
    readonly env: ImportMetaEnv
  }