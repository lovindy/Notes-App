<template>
  <v-menu location="bottom end" transition="slide-y-transition">
    <template v-slot:activator="{ props }">
      <v-btn
        icon="mdi-dots-vertical"
        variant="text"
        v-bind="props"
        density="comfortable"
      />
    </template>
    <v-list>
      <v-list-item @click="emit('edit')" class="action-item">
        <template v-slot:prepend>
          <v-icon color="green-accent-4">mdi-pencil</v-icon>
        </template>
        <v-list-item-title>Edit</v-list-item-title>
      </v-list-item>
      <v-list-item @click="emit('delete')" class="action-item">
        <template v-slot:prepend>
          <v-icon color="error">mdi-delete</v-icon>
        </template>
        <v-list-item-title class="text-error">Delete</v-list-item-title>
      </v-list-item>
    </v-list>
  </v-menu>
</template>

<script setup lang="ts">
const emit = defineEmits<{
  (e: "edit"): void;
  (e: "delete"): void;
}>();
</script>

<style scoped>
.action-item {
  transition: background-color 0.2s ease;
}

.action-item:hover {
  background-color: rgba(74, 222, 128, 0.1);
}
</style>
