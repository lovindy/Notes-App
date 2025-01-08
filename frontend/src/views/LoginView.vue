<template>
  <v-container>
    <v-row justify="center" align="center" style="height: 80vh">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="elevation-12">
          <v-card-title class="text-h5 text-center pt-6">
            <h2>Welcome Back</h2>
          </v-card-title>

          <v-card-text>
            <v-alert v-if="error" type="error" variant="tonal" class="mb-4">
              {{ error }}
            </v-alert>

            <v-form @submit.prevent="handleSubmit" class="mt-4">
              <v-text-field
                v-model="credentials.username"
                prepend-icon="mdi-account"
                label="Username"
                type="text"
                variant="outlined"
                required
                :disabled="loading"
              ></v-text-field>

              <v-text-field
                v-model="credentials.password"
                prepend-icon="mdi-lock"
                label="Password"
                type="password"
                variant="outlined"
                required
                :disabled="loading"
              ></v-text-field>

              <v-btn
                type="submit"
                color="primary"
                size="large"
                block
                class="mt-4"
                :loading="loading"
              >
                Sign In
              </v-btn>
            </v-form>

            <v-row class="mt-4">
              <v-col class="text-center">
                Don't have an account?
                <router-link to="/register" class="text-decoration-none">
                  Register here
                </router-link>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import { useAuthStore } from "../stores/auth.store";

const router = useRouter();
const authStore = useAuthStore();
const loading = ref(false);
const error = ref("");

const credentials = reactive({
  username: "",
  password: "",
});

const handleSubmit = async () => {
  loading.value = true;
  error.value = "";

  try {
    await authStore.login(credentials);
    router.push("/");
  } catch (err: any) {
    error.value =
      err.response?.data?.message || "Login failed. Please try again.";
    console.error("Login failed:", err);
  } finally {
    loading.value = false;
  }
};
</script>
  