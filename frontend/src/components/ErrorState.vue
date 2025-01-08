<template>
  <v-row>
    <v-col cols="12">
      <v-alert
        type="error"
        variant="tonal"
        :text="message"
        closable
        class="mb-4"
        @click:close="emit('close')"
      >
        <template v-if="$slots.default" #default>
          <slot />
        </template>
      </v-alert>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
defineProps<{
  message: string;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();
</script>
