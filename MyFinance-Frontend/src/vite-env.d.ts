/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_TEST: string
    readonly VITE_TEST_ENV_VITE: string
  }
  
  interface ImportMeta {
    readonly env: ImportMetaEnv
  }