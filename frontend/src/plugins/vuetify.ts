import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'
import '@mdi/font/css/materialdesignicons.css'

export default createVuetify({
  components,
  directives,
  theme: {
    defaultTheme: 'dark',
    themes: {
      dark: {
        dark: true,
        colors: {
          primary: '#10B981',
          'green-accent-4': '#4ADE80',
          background: '#0f172a',
          surface: '#1e293b',
          'grey-darken-4': '#1f2937',
        }
      }
    }
  },
  defaults: {
    VBtn: {
      rounded: 'lg',
      height: 44,
    },
    VTextField: {
      variant: 'outlined',
      density: 'comfortable',
    }
  }
})