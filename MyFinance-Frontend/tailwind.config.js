/** @type {import('tailwindcss').Config} */
import defaultTheme from 'tailwindcss/defaultTheme'

export const plugins = []
export const content = ['./index.html', './src/**/*.{js,ts,jsx,tsx}']
export const theme = {
  extend: {
    fontFamily: {
      serif: ['"Playfair Display"', ...defaultTheme.fontFamily.serif],
      sans: ['"Open Sans"', ...defaultTheme.fontFamily.sans],
    },
  },
}
