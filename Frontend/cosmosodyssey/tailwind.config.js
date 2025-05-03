/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    './components/**/*.{vue,js}',
    './layouts/**/*.vue',
    './pages/**/*.vue',
    './plugins/**/*.js',
    './nuxt.config.{js,ts}',
    '.app.config.{js,ts}',
    './app.vue',
    'node_modules/@nuxt/ui/dist/**/*.js',
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};
