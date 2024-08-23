import { TanStackRouterVite } from '@tanstack/router-vite-plugin'
import react from '@vitejs/plugin-react'
import { spawnSync } from 'child_process'
import fs from 'fs'
import path from 'path'
import { env } from 'process'
import { defineConfig } from 'vite'

const baseFolder =
  env.APPDATA !== undefined && env.APPDATA !== ''
    ? `${env.APPDATA}/ASP.NET/https`
    : `${env.HOME}/.aspnet/https`

const certificateName = 'myfinance.ui'
const certFilePath = path.join(baseFolder, `${certificateName}.pem`)
const keyFilePath = path.join(baseFolder, `${certificateName}.key`)

if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath)) {
  if (
    spawnSync(
      'dotnet',
      [
        'dev-certs',
        'https',
        '--export-path',
        certFilePath,
        '--format',
        'Pem',
        '--no-password',
      ],
      { stdio: 'inherit' },
    ).status !== 0
  ) {
    throw new Error('Could not create certificate.')
  }
}

export default defineConfig({
  plugins: [react(), TanStackRouterVite()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    open: true,
    proxy: {
      '/user': {
        target: 'https://localhost:7286',
        secure: false,
        changeOrigin: true,
      },
      '/managementunit': {
        target: 'https://localhost:7286',
        secure: false,
        changeOrigin: true,
      },
      '/accounttag': {
        target: 'https://localhost:7286',
        secure: false,
        changeOrigin: true,
      },
      '/category': {
        target: 'https://localhost:7286',
        secure: false,
        changeOrigin: true,
      },
      '/transfer': {
        target: 'https://localhost:7286',
        secure: false,
        changeOrigin: true,
      },
    },
    port: 5173,
    https: {
      key: fs.readFileSync(keyFilePath),
      cert: fs.readFileSync(certFilePath),
    },
  },
})
