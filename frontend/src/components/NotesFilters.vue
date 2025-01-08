<template>
  <v-card class="mb-8 bg-surface pa-4" elevation="1">
    <v-row>
      <v-col cols="12" md="6">
        <v-text-field
          :model-value="search"
          @update:model-value="emit('update:search', $event)"
          prepend-inner-icon="mdi-magnify"
          label="Search notes"
          variant="outlined"
          density="comfortable"
          hide-details
          clearable
          bg-color="grey-darken-4"
          class="search-field"
        >
          <template v-slot:append>
            <v-fade-transition>
              <v-progress-circular
                v-if="loading"
                indeterminate
                color="green-accent-4"
                size="24"
              />
            </v-fade-transition>
          </template>
        </v-text-field>
      </v-col>
      <v-col cols="12" md="4">
        <v-select
          :model-value="sortField"
          @update:model-value="emit('update:sortField', $event)"
          :items="sortOptions"
          label="Sort by"
          variant="outlined"
          density="comfortable"
          hide-details
          bg-color="grey-darken-4"
        />
      </v-col>
      <v-col cols="12" md="2" class="d-flex align-center">
        <v-btn-toggle
          :model-value="sortDirection"
          @update:model-value="emit('update:sortDirection', $event)"
          mandatory
          rounded="lg"
          class="w-100"
          density="comfortable"
        >
          <v-btn value="asc" variant="tonal">
            <v-icon>mdi-sort-ascending</v-icon>
          </v-btn>
          <v-btn value="desc" variant="tonal">
            <v-icon>mdi-sort-descending</v-icon>
          </v-btn>
        </v-btn-toggle>
      </v-col>
    </v-row>
  </v-card>
</template>

<script setup lang="ts">
import type { SortField, SortDirection } from "../interfaces/types";

const props = defineProps<{
  search: string;
  sortField: SortField;
  sortDirection: SortDirection;
  loading: boolean;
}>();

const emit = defineEmits<{
  (e: "update:search", value: string): void;
  (e: "update:sortField", value: SortField): void;
  (e: "update:sortDirection", value: SortDirection): void;
}>();

const sortOptions = [
  { title: "Created Date", value: "createdAt" },
  { title: "Title", value: "title" },
  { title: "Updated Date", value: "updatedAt" },
] as const;
</script>
