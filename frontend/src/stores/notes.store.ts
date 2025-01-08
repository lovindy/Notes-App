// notes.store.ts
import { defineStore } from "pinia";
import { ref, computed } from "vue";
import api from "../services/api";
import type { Note } from "../interfaces/types";

export const useNotesStore = defineStore("notes", () => {
  const notes = ref<Note[]>([]);
  const searchQuery = ref("");
  const sortField = ref<"title" | "createdAt" | "updatedAt">("createdAt");
  const sortDirection = ref<"asc" | "desc">("desc");

  const filteredAndSortedNotes = computed(() => {
    const query = searchQuery.value.toLowerCase().trim();
    
    return [...notes.value]
      .filter((note) => {
        if (!note) return false;
        
        const titleMatch = note.title?.toLowerCase()?.includes(query) || false;
        const contentMatch = note.content?.toLowerCase()?.includes(query) || false;
        
        return titleMatch || contentMatch;
      })
      .sort((a, b) => {
        if (!a || !b) return 0;
        
        const modifier = sortDirection.value === "asc" ? 1 : -1;
        const aValue = a[sortField.value]?.toString() || "";
        const bValue = b[sortField.value]?.toString() || "";
        
        return modifier * aValue.localeCompare(bValue);
      });
  });

  async function fetchNotes() {
    try {
      const response = await api.get("/notes");
      // Ensure we're setting an array, even if empty
      notes.value = Array.isArray(response.data) ? response.data : [];
      return response.data;
    } catch (error) {
      console.error("Failed to fetch notes:", error);
      notes.value = []; // Reset to empty array on error
      throw error;
    }
  }

  async function createNote(note: Partial<Note>) {
    try {
      const response = await api.post("/notes", note);
      const newNote = response.data;
      notes.value = [...notes.value, newNote];
      return newNote;
    } catch (error) {
      console.error("Failed to create note:", error);
      throw error;
    }
  }

  async function updateNote(id: number, note: Partial<Note>) {
    try {
      const response = await api.put(`/notes/${id}`, note);
      const updatedNote = response.data;

      // Find and replace the note in the array
      const index = notes.value.findIndex((n) => n?.id === id);
      if (index !== -1) {
        const updatedNotes = [...notes.value];
        updatedNotes[index] = updatedNote;
        notes.value = updatedNotes;
      }

      return updatedNote;
    } catch (error) {
      console.error("Failed to update note:", error);
      throw error;
    }
  }

/*************  ✨ Codeium Command ⭐  *************/
  /**
   * Resets the store to its initial state.
   * This is useful when the user logs out and we want to clear all data.
   */
/******  8276179d-12d0-431e-b4b2-0a03d9c12f10  *******/  async function deleteNote(id: number) {
    try {
      await api.delete(`/notes/${id}`);
      notes.value = notes.value.filter((note) => note?.id !== id);
    } catch (error) {
      console.error("Failed to delete note:", error);
      throw error;
    }
  }

  function resetStore() {
    notes.value = [];
    searchQuery.value = "";
    sortField.value = "createdAt";
    sortDirection.value = "desc";
  }

  return {
    notes,
    searchQuery,
    sortField,
    sortDirection,
    filteredAndSortedNotes,
    fetchNotes,
    createNote,
    updateNote,
    deleteNote,
    resetStore,
  };
});