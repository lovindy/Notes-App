<template>
  <div class="notes-page">
    <v-container>
      <NotesHeader @add-note="openNoteDialog" />
      <NotesFilters
        v-model:search="store.searchQuery"
        v-model:sort-field="store.sortField"
        v-model:sort-direction="store.sortDirection"
        :loading="loading"
      />

      <LoadingState v-if="loading" />
      <ErrorState v-else-if="error" :message="error" @close="error = null" />
      <NotesGrid
        v-else
        :notes="store.filteredAndSortedNotes"
        @edit="openNoteDialog"
        @delete="confirmDelete"
      />

      <NoteDialog
        v-model="noteDialog"
        :note="editingNote"
        :loading="saving"
        @save="saveNote"
        @close="closeNoteDialog"
      />

      <DeleteConfirmationDialog
        v-model="deleteDialog"
        :loading="deleting"
        @confirm="handleDeleteNote"
      />
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";
import { useNotesStore } from "../stores/notes.store";
import type { Note } from "../interfaces/types";
import NotesHeader from "./NotesHeader.vue";
import NotesFilters from "./NotesFilters.vue";
import NotesGrid from "./NotesGrid.vue";
import NoteDialog from "./NoteDialog.vue";
import DeleteConfirmationDialog from "./DeleteConfirmationDialog.vue";
import LoadingState from "./LoadingState.vue";
import ErrorState from "./ErrorState.vue";

const store = useNotesStore();

// State management
const noteDialog = ref(false);
const deleteDialog = ref(false);
const saving = ref(false);
const deleting = ref(false);
const loading = ref(false);
const error = ref<string | null>(null);
const editingNote = ref<Note | null>(null);
const noteToDelete = ref<Note | null>(null);

// Lifecycle hooks
onMounted(async () => {
  loading.value = true;
  try {
    await store.fetchNotes();
  } catch (err) {
    error.value = "Failed to load notes. Please try again later.";
  } finally {
    loading.value = false;
  }
});

// Methods
const openNoteDialog = (note?: Note) => {
  editingNote.value = note ?? null;
  noteDialog.value = true;
};

const closeNoteDialog = () => {
  noteDialog.value = false;
  editingNote.value = null;
};

const saveNote = async (noteData: Partial<Note>) => {
  saving.value = true;
  try {
    if (editingNote.value) {
      await store.updateNote(editingNote.value.id, noteData);
    } else {
      await store.createNote(noteData);
    }
    closeNoteDialog();
    await store.fetchNotes();
  } catch (err) {
    error.value = "Failed to save note. Please try again later.";
  } finally {
    saving.value = false;
  }
};

const confirmDelete = (note: Note) => {
  noteToDelete.value = note;
  deleteDialog.value = true;
};

const handleDeleteNote = async () => {
  if (!noteToDelete.value) return;

  deleting.value = true;
  try {
    await store.deleteNote(noteToDelete.value.id);
    deleteDialog.value = false;
    noteToDelete.value = null;
  } catch (err) {
    error.value = "Failed to delete note. Please try again later.";
  } finally {
    deleting.value = false;
  }
};
</script>

<style scoped>
.notes-page {
  background: linear-gradient(135deg, #0f172a 0%, #1e293b 100%);
  min-height: 100vh;
  padding: 2rem 0;
}
</style>
