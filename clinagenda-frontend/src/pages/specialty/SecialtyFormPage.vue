<script setup lang="ts">
import request from '@/engine/httpClient'
import { PageMode } from '@/enum'
import type { SpecialtyForm } from '@/interfaces/specialty'
import router from '@/router'
import { useToastStore } from '@/stores'
import { DefaultTemplate } from '@/template'
import { mdiCancel, mdiPlusCircle } from '@mdi/js'
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'

const toastStore = useToastStore()

const isLoadingForm = ref<boolean>(false)

const route = useRoute()
const id = route.params.id
const pageMode = id ? PageMode.PAGE_UPDATE : PageMode.PAGE_INSERT

const form = ref<SpecialtyForm>({
    name: '',
    scheduleDuration: ''
})

const pageTitle = computed(() => {
  return pageMode === PageMode.PAGE_UPDATE ? 'Specialty Edit' : 'Register New Specialty'
})

const submitForm = async () => {
  isLoadingForm.value = true

  const response = await request<SpecialtyForm, null>({
    method: pageMode == PageMode.PAGE_INSERT ? 'POST' : 'PUT',
    endpoint: pageMode == PageMode.PAGE_INSERT ? 'specialty/insert' : `specialty/update/${id}`,
    body: form.value
  })

  if (response.isError) return

  toastStore.setToast({
    type: 'success',
    text: `Specialty ${pageMode == PageMode.PAGE_INSERT ? 'created' : 'changed'} Successfully!`
  })

  router.push({ name: 'specialty-list' })

  isLoadingForm.value = false
}

const loadForm = async () => {
  if (pageMode === PageMode.PAGE_INSERT) return

  isLoadingForm.value = true

  const specialtyFormResponse = await request<undefined, SpecialtyForm>({
    method: 'PUT',
    endpoint: `specialty/update/${id}`
  })

  if (specialtyFormResponse?.isError) return

  form.value = specialtyFormResponse.data

  isLoadingForm.value = false
}

onMounted(() => {
  loadForm()
})
</script>

<template>
  <DefaultTemplate>
    <template #title> {{ pageTitle }} </template>

    <template #action>
      <v-btn :prepend-icon="mdiCancel" :to="{ name: 'specialty-list' }"> Cancel </v-btn>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" @click.prevent="submitForm"> Save </v-btn>
    </template>

    <v-form :disabled="isLoadingForm" @submit.prevent="submitForm">
      <v-row>
        <v-col cols="6">
          <v-text-field v-model.trim="form.name" label="Name" hide-details />
        </v-col>
        <v-col cols="2">
          <v-text-field v-model.trim="form.scheduleDuration" label="Duration" hide-details />
        </v-col>
      </v-row>
    </v-form>
  </DefaultTemplate>
</template>
