<template>
  <v-card
    class="h-100 note-card"
    elevation="2"
    :class="{ 'note-card-hover': isHovered }"
    @mouseenter="isHovered = true"
    @mouseleave="isHovered = false"
  >
    <v-card-title class="d-flex justify-space-between align-center pa-4">
      <div class="text-truncate">{{ note.title }}</div>
      <NoteActions @edit="emit('edit')" @delete="emit('delete')" />
    </v-card-title>

    <v-card-text class="pa-4">
      <p class="text-body-1 text-grey-lighten-1">{{ note.content }}</p>
      <div class="mt-4 d-flex justify-space-between text-caption text-grey">
        <span>Created: {{ formatDate(note.createdAt) }}</span>
        <span>Updated: {{ formatDate(note.updatedAt) }}</span>
      </div>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { ref } from "vue";
import type { Note } from "../interfaces/types";
import NoteActions from "./NoteActions.vue";
import { formatDate } from "../utils/dateUtils.ts";

defineProps<{
  note: Note;
}>();

const emit = defineEmits<{
  (e: "edit"): void;
  (e: "delete"): void;
}>();

const isHovered = ref(false);
</script>

<style scoped>
.note-card {
  transition: all 0.3s ease;
  background: rgba(30, 41, 59, 0.8);
  backdrop-filter: blur(10px);
  border: 1px solid rgba(255, 255, 255, 0.1);
}

.note-card-hover {
  transform: translateY(-4px);
  box-shadow: 0 4px 25px 0 rgba(0, 0, 0, 0.2);
}
</style>
