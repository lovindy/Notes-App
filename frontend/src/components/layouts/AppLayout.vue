<!-- AppLayout.vue -->
<template>
  <v-app>
    <v-app-bar elevation="1">
      <template v-slot:prepend>
        <div class="d-flex align-center">
          <v-img
            src="https://cdn2.iconfinder.com/data/icons/business-office-icons/256/To-do_List-512.png"
            alt="Notes Logo"
            width="40"
            height="40"
            class="me-3"
          ></v-img>
          <v-app-bar-title>Notes App</v-app-bar-title>
        </div>
      </template>

      <template v-slot:append>
        <div class="d-flex align-center me-4">
          <v-icon icon="mdi-account" class="me-2"></v-icon>
          <span class="text-body-2">{{ authStore.user?.username || 'Guest' }}</span>
        </div>
        <v-btn
          color="primary"
          variant="text"
          prepend-icon="mdi-logout"
          @click="handleLogout"
          :loading="isLoggingOut"
        >
          Logout
        </v-btn>
      </template>
    </v-app-bar>

    <v-main>
      <v-container fluid>
        <slot></slot>
      </v-container>
    </v-main>

    <!-- Logout Error Snackbar -->
    <v-snackbar
      v-model="showError"
      color="error"
      timeout="3000"
    >
      {{ errorMessage }}
      <template v-slot:actions>
        <v-btn
          color="white"
          variant="text"
          @click="showError = false"
        >
          Close
        </v-btn>
      </template>
    </v-snackbar>
  </v-app>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../../stores/auth.store';

const router = useRouter();
const authStore = useAuthStore();

const isLoggingOut = ref(false);
const showError = ref(false);
const errorMessage = ref('');

const handleLogout = async () => {
  isLoggingOut.value = true;
  try {
    await authStore.logout();
    
    // Clear any stored tokens
    localStorage.removeItem('authToken');
    
    // Redirect to login page
    router.push('/login');
  } catch (error) {
    console.error('Logout failed:', error);
    errorMessage.value = 'Failed to logout. Please try again.';
    showError.value = true;
  } finally {
    isLoggingOut.value = false;
  }
};
</script>

<style scoped>
.v-app-bar {
  border-bottom: 1px solid rgba(0, 0, 0, 0.12);
}
</style>