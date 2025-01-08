<template>
  <v-container>
    <!-- Top Bar with Search and Add Button -->
    <v-row class="mb-6">
      <v-col cols="12" sm="6" md="8">
        <v-text-field
          v-model="store.searchQuery"
          prepend-icon="mdi-magnify"
          label="Search notes"
          variant="outlined"
          density="comfortable"
          hide-details
          clearable
        ></v-text-field>
      </v-col>
      <v-col cols="12" sm="6" md="4" class="d-flex align-center justify-end">
        <v-btn
          color="primary"
          prepend-icon="mdi-plus"
          @click="openNoteDialog()"
        >
          Add Note
        </v-btn>
      </v-col>
    </v-row>

    <!-- Filter and Sort Bar -->
    <v-row class="mb-6">
      <v-col cols="12" sm="6" md="4">
        <v-select
          v-model="store.sortField"
          :items="[
            { title: 'Created Date', value: 'createdAt' },
            { title: 'Title', value: 'title' },
            { title: 'Updated Date', value: 'updatedAt' },
          ]"
          label="Sort by"
          variant="outlined"
          density="comfortable"
        ></v-select>
      </v-col>
      <v-col cols="12" sm="6" md="4" class="d-flex align-center">
        <v-btn-toggle v-model="store.sortDirection" mandatory>
          <v-btn value="asc" icon="mdi-sort-ascending"></v-btn>
          <v-btn value="desc" icon="mdi-sort-descending"></v-btn>
        </v-btn-toggle>
      </v-col>
    </v-row>

    <!-- Loading State -->
    <v-row v-if="loading">
      <v-col cols="12" class="text-center">
        <v-progress-circular
          indeterminate
          color="primary"
        ></v-progress-circular>
      </v-col>
    </v-row>

    <!-- Error State -->
    <v-row v-else-if="error">
      <v-col cols="12">
        <v-alert type="error" title="Error" :text="error"></v-alert>
      </v-col>
    </v-row>

    <!-- Notes Grid -->
    <v-row v-else>
      <v-col
        v-for="note in store.filteredAndSortedNotes"
        :key="note.id"
        cols="12"
        sm="6"
        md="4"
      >
        <v-card class="h-100">
          <v-card-title class="d-flex justify-space-between align-center">
            {{ note.title }}
            <v-menu>
              <template v-slot:activator="{ props }">
                <v-btn
                  icon="mdi-dots-vertical"
                  variant="text"
                  v-bind="props"
                ></v-btn>
              </template>
              <v-list>
                <v-list-item @click="openNoteDialog(note)">
                  <v-list-item-title>
                    <v-icon start>mdi-pencil</v-icon>
                    Edit
                  </v-list-item-title>
                </v-list-item>
                <v-list-item @click="confirmDelete(note)">
                  <v-list-item-title class="text-red">
                    <v-icon start color="red">mdi-delete</v-icon>
                    Delete
                  </v-list-item-title>
                </v-list-item>
              </v-list>
            </v-menu>
          </v-card-title>

          <v-card-text>
            <p class="text-body-1">{{ note.content }}</p>
            <div class="mt-4 text-caption text-grey">
              Created: {{ new Date(note.createdAt).toLocaleDateString() }}
              <br />
              Updated: {{ new Date(note.updatedAt).toLocaleDateString() }}
            </div>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Create/Edit Note Dialog -->
    <v-dialog v-model="noteDialog" max-width="600">
      <v-card>
        <v-card-title>{{
          editingNote ? "Edit Note" : "Create Note"
        }}</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="saveNote" ref="form">
            <v-text-field
              v-model="noteForm.title"
              label="Title"
              variant="outlined"
              :rules="[(v) => !!v || 'Title is required']"
              required
            ></v-text-field>
            <v-textarea
              v-model="noteForm.content"
              label="Content"
              variant="outlined"
              rows="5"
              :rules="[(v) => !!v || 'Content is required']"
              required
            ></v-textarea>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey-darken-1" variant="text" @click="closeNoteDialog">
            Cancel
          </v-btn>
          <v-btn color="primary" @click="saveNote" :loading="saving">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation Dialog -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>Delete Note</v-card-title>
        <v-card-text>
          Are you sure you want to delete this note? This action cannot be
          undone.
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn
            color="grey-darken-1"
            variant="text"
            @click="deleteDialog = false"
          >
            Cancel
          </v-btn>
          <v-btn color="error" @click="handleDeleteNote" :loading="deleting">
            Delete
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, reactive, onMounted } from "vue";
import { useNotesStore } from "../stores/notes.store";
import type { Note } from "../interfaces/types";

// Store
const store = useNotesStore();

// State
const noteDialog = ref(false);
const deleteDialog = ref(false);
const saving = ref(false);
const deleting = ref(false);
const loading = ref(false);
const error = ref<string | null>(null);
const editingNote = ref<Note | null>(null);
const noteToDelete = ref<Note | null>(null);
const form = ref<any>(null);

const noteForm = reactive({
  title: "",
  content: "",
});

// Fetch notes on component mount
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
  if (note) {
    editingNote.value = note;
    noteForm.title = note.title;
    noteForm.content = note.content;
  } else {
    editingNote.value = null;
    noteForm.title = "";
    noteForm.content = "";
  }
  noteDialog.value = true;
};

const closeNoteDialog = () => {
  noteDialog.value = false;
  editingNote.value = null;
  noteForm.title = "";
  noteForm.content = "";
  if (form.value) {
    form.value.resetValidation();
  }
};

const saveNote = async () => {
  if (!form.value) return;

  const { valid } = await form.value.validate();
  if (!valid) return;

  saving.value = true;
  try {
    if (editingNote.value) {
      // Wait for the update to complete and get the updated note
      const updatedNote = await store.updateNote(editingNote.value.id, {
        title: noteForm.title,
        content: noteForm.content,
      });

      // Refresh the notes list to ensure we have the latest data
      await store.fetchNotes();

      // Log the update for debugging
      console.log("Note updated:", updatedNote);
      console.log("Current notes:", store.notes);
    } else {
      await store.createNote(noteForm);
    }
    closeNoteDialog();
  } catch (err) {
    error.value = "Failed to save note. Please try again later.";
    console.error("Save error:", err);
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
