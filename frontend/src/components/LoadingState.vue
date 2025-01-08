<template>
    <v-row class="my-12">
      <v-col cols="12" class="text-center">
        <v-progress-circular
          indeterminate
          color="green-accent-4"
          size="64"
        />
        <div class="mt-4 text-body-1 text-grey">
          <slot>Loading notes...</slot>
        </div>
      </v-col>
    </v-row>
  </template>