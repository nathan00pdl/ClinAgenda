<!-- eslint-disable @typescript-eslint/no-unused-vars -->
<script lang="ts">
import request from '@/engine/httpClient'
import { PageMode } from '@/enum'
import type { DoctorForm } from '@/interfaces/doctor'
import type { GetSpecialtyListResponse, ISpecialty } from '@/interfaces/specialty'
import type { GetStatusListResponse, IStatus } from '@/interfaces/status'
import router from '@/router'
import { useToastStore } from '@/stores'
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'

const toastStore = useToastStore()

const isLoadingForm = ref<boolean>(false)

const route = useRoute()
const id = route.params.id
const pageMode = id ? PageMode.PAGE_UPDATE : PageMode.PAGE_INSERT

const form = ref<DoctorForm>({
  name: '',
  specialty: [],
  statusId: null
})

const specialtyItems = ref<ISpecialty[]>([])
const statusItems = ref<IStatus[]>([])

const pageTitle = computed(() => {
  return pageMode === PageMode.PAGE_UPDATE ? 'Professional Edit' : 'Register New Professional'
})

const submitForm = async () => {
  try {
    isLoadingForm.value = true

    const response = await request<DoctorForm, null>({
      method: pageMode == PageMode.PAGE_INSERT ? 'POST' : 'PUT',
      endpoint: pageMode == PageMode.PAGE_INSERT ? 'doctor/insert' : `doctor/update/${id}`,
      body: form.value
    })

    if (response.isError) return

    toastStore.setToast({
      type: 'success',
      text: `Professional ${pageMode == PageMode.PAGE_INSERT ? 'created' : 'changed'} Successfully!`
    })

    router.push({ name: 'doctor-list' })

    isLoadingForm.value = false
  } catch (e) {
    console.error('Error Saving Form', e)
  }
}

const loadForm = async () => {
  isLoadingForm.value = true

  const specialtyRequest = request<undefined, GetSpecialtyListResponse>({
    method: 'GET',
    endpoint: 'specialty/list'
  })

  const statusRequest = request<undefined, GetStatusListResponse>({
    method: 'GET',
    endpoint: 'status/list'
  })

  const requests: Promise<any>[] = [specialtyRequest, statusRequest]

  if (pageMode === PageMode.PAGE_UPDATE) {
    const doctorFormRequest = request<undefined, DoctorForm>({
      method: 'GET',
      endpoint: `doctor/listById/${id}`
    })

    requests.push(doctorFormRequest)
  }

  try {
    const [specialtyResponse, statusResponse, doctorFormResonse] = await Promise.all(requests)

    if (specialtyResponse.isError || statusResponse.isError || doctorFormResonse?.isError) return

    specialtyItems.value = specialtyResponse.data.items
    statusItems.value = statusResponse.data.items

    if (pageMode === PageMode.PAGE_UPDATE) {
      form.value = doctorFormResonse.data
    }

    form.value.statusId = doctorFormResonse.data.status.id
    form.value.specialty = doctorFormResonse.data.specialty.map((specialty: ISpecialty) => specialty.id)
  } catch (e) {
    console.error('Error Fetching Form Data', e)
  } finally {
    isLoadingForm.value = false
  }
}

onMounted(() => {
  loadForm()
})
</script>

<template>
  <DefaultTemplate>
    <template #title> {{ pageTitle }} </template>

    <template #action>
      <v-btn :prepend-icon="mdiCancel" :to="{ name: 'doctor-list' }"> Cancel </v-btn>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" @click.prevent="submitForm"> Save </v-btn>
    </template>

    <v-form :disabled="isLoadingForm" @submit.prevent="submitForm">
      <v-row>
        <v-col cols="6">
          <v-text-field v-model.trim="form.name" label="Name" hide-details />
        </v-col>
        <v-col cols="2">
          <v-select
            v-model="form.statusId"
            label="Status"
            :loading="isLoadingForm"
            :items="statusItems"
            item-value="id"
            item-title="name"
            clearable
            hide-details
          />
        </v-col>
      </v-row>
      <v-row>
        <v-col>
          <v-label>Specialties</v-label>
          <v-checkbox
            v-for="specialty in specialtyItems"
            :key="specialty.id"
            v-model="form.specialty"
            :label="specialty.name"
            :value="specialty.id"
          />
        </v-col>
      </v-row>
    </v-form>
  </DefaultTemplate>
</template>

