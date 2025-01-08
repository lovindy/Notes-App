<template>
  <v-row class="mb-8">
    <v-col cols="12">
      <div
        class="d-flex flex-column flex-md-row justify-space-between align-center mb-4"
      >
        <h1 class="text-h4 font-weight-bold text-white mb-4 mb-md-0">
          My Notes
        </h1>
        <v-btn
          color="green-accent-4"
          prepend-icon="mdi-plus"
          @click="emit('addNote')"
          size="large"
          class="px-6"
        >
          Add Note
        </v-btn>
      </div>
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
const emit = defineEmits<{
  (e: "addNote"): void;
}>();
</script>