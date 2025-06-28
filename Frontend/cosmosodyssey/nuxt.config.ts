import tailwindcss from '@tailwindcss/vite';
import "@nuxt/ui";


export default defineNuxtConfig({
  devtools: { enabled: true },
  modules: ['@nuxt/ui', '@pinia/nuxt', '@nuxt/fonts', '@nuxt/image', '@nuxt/icon', 'shadcn-nuxt'],

  components: {
    dirs: [
      '~/components'
    ]
  },

  imports: {
    autoImport: true,
    dirs: ["types/*.ts"],
  },

  vite: {
    plugins: [ tailwindcss() ],
  },

  runtimeConfig: {
    public: {
      apiBase: "http://localhost:3000/api/",
    },
  },

  css: ['~/assets/css/tailwind.css'],

  shadcn: {
    prefix: '',
    /**
     * Directory that the component lives in
     * @default "./components/ui"
     */
    componentDir: './components/ui'
  },

  compatibilityDate: '2025-05-03'
})