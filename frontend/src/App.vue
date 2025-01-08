<!-- App.vue -->
<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useAuthStore } from "./stores/auth.store";
import { useRouter } from "vue-router";

const authStore = useAuthStore();
const router = useRouter();
const error = ref<string | null>(null);

onMounted(async () => {
  try {
    await authStore.checkAuth();
    // If not authenticated, redirect to login
    if (!authStore.isAuthenticated) {
      router.push("/login");
    }
  } catch (err) {
    error.value = "Failed to check authentication status";
    console.error("Auth check failed:", err);
    router.push("/login");
  }
});
</script>

<template>
  <div>
    <div v-if="authStore.isLoading" class="loading">Loading...</div>
    <div v-else-if="error" class="error">
      {{ error }}
    </div>
    <router-view v-else></router-view>
  </div>
</template>

<style scoped>
.loading {
  text-align: center;
  padding: 20px;
}
.error {
  color: red;
  text-align: center;
  padding: 20px;
}
</style>
