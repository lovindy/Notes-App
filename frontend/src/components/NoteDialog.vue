<template>
  <v-dialog
    :model-value="modelValue"
    @update:model-value="emit('update:modelValue', $event)"
    max-width="600"
    transition="dialog-bottom-transition"
  >
    <v-card>
      <v-card-title class="text-h5 pa-4">
        {{ note ? "Edit Note" : "Create Note" }}
        <v-btn
          icon="mdi-close"
          variant="text"
          @click="emit('close')"
          class="float-right"
        />
      </v-card-title>
      <v-card-text class="pa-4">
        <v-form @submit.prevent="handleSubmit" ref="form">
          <v-text-field
            v-model="formData.title"
            label="Title"
            variant="outlined"
            :rules="[(v) => !!v || 'Title is required']"
            required
            class="mb-4"
            bg-color="grey-darken-4"
          />
          <v-textarea
            v-model="formData.content"
            label="Content"
            variant="outlined"
            rows="5"
            :rules="[(v) => !!v || 'Content is required']"
            required
            bg-color="grey-darken-4"
          />
        </v-form>
      </v-card-text>
      <v-card-actions class="pa-4">
        <v-spacer />
        <v-btn
          color="grey-darken-1"
          variant="text"
          @click="emit('close')"
          :disabled="loading"
        >
          Cancel
        </v-btn>
        <v-btn
          color="green-accent-4"
          @click="handleSubmit"
          :loading="loading"
          :disabled="loading"
        >
          Save
        </v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script setup lang="ts">
import { ref, reactive, watch } from "vue";
import type { Note, NoteFormData } from "../interfaces/types";

const props = defineProps<{
  modelValue: boolean;
  note: Note | null;
  loading?: boolean;
}>();

const emit = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "save", data: NoteFormData): void;
  (e: "close"): void;
}>();

const form = ref<any>(null);
const formData = reactive<NoteFormData>({
  title: "",
  content: "",
});

// Watch for note changes to update form data
watch(
  () => props.note,
  (newNote) => {
    if (newNote) {
      formData.title = newNote.title;
      formData.content = newNote.content;
    } else {
      formData.title = "";
      formData.content = "";
    }
  },
  { immediate: true }
);

const handleSubmit = async () => {
  if (!form.value) return;

  const { valid } = await form.value.validate();
  if (!valid) return;

  emit("save", { ...formData });
};
</script>
