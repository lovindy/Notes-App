<template>
  <v-container>
    <v-row justify="center" align="center" style="height: 80vh">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="elevation-12">
          <v-card-title class="text-h5 text-center pt-6">
            <h2>Create Account</h2>
          </v-card-title>

          <v-card-text>
            <v-form @submit.prevent="handleSubmit" class="mt-4">
              <v-text-field
                v-model="formData.username"
                prepend-icon="mdi-account"
                label="Username"
                type="text"
                variant="outlined"
                required
              ></v-text-field>

              <v-text-field
                v-model="formData.password"
                prepend-icon="mdi-lock"
                label="Password"
                type="password"
                variant="outlined"
                required
              ></v-text-field>

              <v-text-field
                v-model="formData.confirmPassword"
                prepend-icon="mdi-lock-check"
                label="Confirm Password"
                type="password"
                variant="outlined"
                required
              ></v-text-field>

              <v-btn
                type="submit"
                color="primary"
                size="large"
                block
                class="mt-4"
                :loading="loading"
              >
                Register
              </v-btn>
            </v-form>

            <v-row class="mt-4">
              <v-col class="text-center">
                Already have an account?
                <router-link to="/login" class="text-decoration-none">
                  Login here
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

const router = useRouter();
const loading = ref(false);

const formData = reactive({
  username: "",
  password: "",
  confirmPassword: "",
});

const handleSubmit = async () => {
  loading.value = true;
  try {
    // API call here
    await new Promise((resolve) => setTimeout(resolve, 1000));
    router.push("/login");
  } catch (error) {
    console.error("Registration failed:", error);
  } finally {
    loading.value = false;
  }
};
</script>
