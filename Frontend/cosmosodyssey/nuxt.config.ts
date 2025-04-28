// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-04-17',
  devtools: { enabled: true },
  modules: ['@nuxt/ui', '@pinia/nuxt', '@nuxt/fonts', '@nuxt/image', '@nuxt/icon'],
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
    resolve: {
      alias: {
        '#head': 'nuxt/dist/head/runtime/composables/v3.js',
      },
    },
    build: {
      rollupOptions: {
        output: {
          // manualChunks(id) {
          //   if (
          //     id.includes('nuxt/dist/head/runtime/composables/v3.js') ||
          //     id.includes('nuxt/dist/app/composables/head.js')
          //   ) {
          //     return 'head-composables';
          //   }
          // },
          preserveModules: false,
        },
      },
    },
  },
  runtimeConfig: {
    public: {
      apiBase: "http://localhost:3000/api/",
    },
  },

})