<template>
  <v-row>
    <v-col
      v-for="note in notes"
      :key="note.id"
      cols="12"
      sm="6"
      md="4"
      class="note-card-col"
    >
      <NoteCard
        :note="note"
        @edit="emit('edit', note)"
        @delete="emit('delete', note)"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import type { Note } from "../interfaces/types";
import NoteCard from "./NoteCard.vue";

defineProps<{
  notes: Note[];
}>();

const emit = defineEmits<{
  (e: "edit", note: Note): void;
  (e: "delete", note: Note): void;
}>();
</script>
