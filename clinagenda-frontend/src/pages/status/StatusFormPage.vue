<script setup lang="ts">
import request from '@/engine/httpClient'
import { PageMode } from '@/enum'
import type { StatusForm } from '@/interfaces/status'
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

const form = ref<StatusForm>({
  name: ''
})

const pageTitle = computed(() => {
  return pageMode === PageMode.PAGE_UPDATE ? 'Status Edit' : 'Register New Status'
})

const submitForm = async () => {
  isLoadingForm.value = true

  const response = await request<StatusForm, null>({
    method: pageMode == PageMode.PAGE_INSERT ? 'POST' : 'PUT',
    endpoint: pageMode == PageMode.PAGE_INSERT ? 'status/insert' : `status/update/${id}`,
    body: form.value
  })

  if (response.isError) return

  toastStore.setToast({
    type: 'success',
    text: `Status ${pageMode == PageMode.PAGE_INSERT ? 'created' : 'changed'} Successfully!`
  })

  router.push({ name: 'status-list' })
  
  isLoadingForm.value = false
}

const loadForm = async () => {
  if (pageMode === PageMode.PAGE_INSERT) return 

  isLoadingForm.value = true

  const statusFormResponse = await request<undefined, StatusForm>({
    method: 'PUT',
    endpoint: `status/update/${id}`
  })

  if (statusFormResponse?.isError) return

  form.value = statusFormResponse.data

  isLoadingForm.value = false
}

onMounted(() => {
  loadForm()
})
</script>

<template>
  <default-template>
    <template #title> {{ pageTitle }} </template>

    <template #action>
      <v-btn :prepend-icon="mdiCancel" :to="{ name: 'status-list' }"> Cancel </v-btn>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" @click.prevent="submitForm"> Save </v-btn>
    </template>

    <v-form :disabled="isLoadingForm" @submit.prevent="submitForm">
      <v-row>
        <v-col cols="6">
          <v-text-field v-model.trim="form.name" label="Name" hide-details />
        </v-col>
      </v-row>
    </v-form>
  </default-template>
</template>