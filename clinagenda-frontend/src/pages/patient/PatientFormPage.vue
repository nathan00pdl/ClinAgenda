<!-- eslint-disable @typescript-eslint/no-unused-vars -->
<script lang="ts">
import request from '@/engine/httpClient'
import { PageMode } from '@/enum'
import type { GetPatientListRequest, GetPatientResponse, PatientForm } from '@/interfaces/patient'
import type { GetStatusListResponse, IStatus } from '@/interfaces/status'
import router from '@/router'
import { useToastStore } from '@/stores'
import DefaultTemplate from '@/template/DefaultTemplate.vue'
import {
  clearMask,
  dateFormat,
  DateFormatEnum,
} from '@/utils'
import { computed, onMounted, ref } from 'vue'
import { useRoute } from 'vue-router'

const toastStore = useToastStore()

const isLoadingForm = ref<boolean>(false)

const route = useRoute()
const id = route.params.id
const pageMode = id ? PageMode.PAGE_UPDATE : PageMode.PAGE_INSERT

const form = ref<PatientForm>({
  name: '',
  documentNumber: '',
  phoneNumber: '',
  birthDate: '',
  statusId: null
})

const statusItems = ref<IStatus[]>([])

const pageTitle = computed(() => {
  return pageMode === PageMode.PAGE_UPDATE ? 'Patient Edit' : 'Register New Patient'
})

const submitForm = async () => {
  isLoadingForm.value = true

  const body = {
    ...form.value,
    documentNumber: clearMask(form.value.documentNumber),
    phoneNumber: clearMask(form.value.phoneNumber),
    birthDate: dateFormat(
      form.value.birthDate,
      DateFormatEnum.FullDateAmerican.value,
      DateFormatEnum.FullDate.value
    )
  }

  const response = await request<PatientForm, null>({
    method: pageMode == PageMode.PAGE_INSERT ? 'POST' : 'PUT',
    endpoint: pageMode == PageMode.PAGE_INSERT ? 'patient/insert' : `patient/update/${id}`,
    body
  })

  if (response.isError) return

  toastStore.setToast({
    type: 'success',
    text: `Patient ${pageMode == PageMode.PAGE_INSERT ? 'created' : 'changed'} Successfully!`
  })

  router.push({ name: 'patient-list' })

  isLoadingForm.value = false
}

const loadForm = async () => {
  isLoadingForm.value = true

  const statusRequest = request<undefined, GetStatusListResponse>({
    method: 'GET',
    endpoint: 'status/list'
  })

  const requests: Promise<any>[] = [statusRequest]

  if (pageMode === PageMode.PAGE_UPDATE) {
    const patientFormRequest = request<undefined, GetPatientResponse>({
      method: 'GET',
      endpoint: `patient/listById/${id}`
    })

    requests.push(patientFormRequest)
  }

  const [statusResponse, patientFormResonse] = await Promise.all(requests)

  if (statusResponse.isError || patientFormResonse?.isError) return

  statusItems.value = statusResponse.data.items

  if (pageMode === PageMode.PAGE_UPDATE) {
    form.value = patientFormResonse.data
    form.value.statusId = patientFormResonse.data.id
  }

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
      <v-btn :prepend-icon="mdiCancel" :to="{ name: 'patient-list' }"> Cancel </v-btn>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" @click.prevent="submitForm"> Save </v-btn>
    </template>

    <v-form :disabled="isLoadingForm" @submit.prevent="submitForm">
      <v-row>
        <v-col cols="4">
          <v-text-field v-model.trim="form.name" label="Name" hide-details />
        </v-col>
        <v-col cols="2">
          <v-select
            v-model="form.statusId"
            label="Patients"
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
        <v-col cols="4">
          <v-text-field
            v-model.trim="form.documentNumber"
            v-maska="documentNumberMask"
            label="CPF"
            hide-details
          />
        </v-col>
        <v-col cols="4">
          <v-text-field
            v-model.trim="form.phoneNumber"
            v-maska="phoneNumberMask"
            label="Phone Number"
            hide-details
          />
        </v-col>
        <v-col cols="4">
          <v-text-field
            v-model.trim="form.birthDate"
            v-maska="dateMask"
            label="Birthday Date"
            hide-details
          />
        </v-col>
      </v-row>
    </v-form>
  </DefaultTemplate>
</template>

