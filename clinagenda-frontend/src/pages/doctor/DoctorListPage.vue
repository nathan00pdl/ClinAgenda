<script setup lang="ts">
import request from '@/engine/httpClient'
import type { GetDoctorListRequest, GetDoctorListResponse, IDoctor } from '@/interfaces/doctor'
import type {
    GetSpecialtyListResponse,
    ISpecialty
} from '@/interfaces/specialty'
import type { GetStatusListResponse, IStatus } from '@/interfaces/status'
import { useToastStore } from '@/stores'
import DefaultTemplate from '@/template/DefaultTemplate.vue'
import { mdiPlusCircle, mdiSquareEditOutline, mdiTrashCan } from '@mdi/js'
import { onMounted, ref } from 'vue'

const toastStore = useToastStore()

const isLoadingList = ref<boolean>(false)
const isLoadingFilter = ref<boolean>(false)

const filterName = ref<GetDoctorListRequest['name']>('')
const filterSpecialtyId = ref<ISpecialty['id'] | null>(null)
const filterStatusId = ref<IStatus['id'] | null>(null)

const items = ref<IDoctor[]>([])
const itemsPerPage = ref<number>(10)
const page = ref<number>(1)
const total = ref<number>(0)

const specialtyItems = ref<ISpecialty[]>([])
const statusItems = ref<IStatus[]>([])

const headers = [
  {
    title: 'ID',
    key: 'id',
    sortable: false,
    width: 0,
    cellProps: { class: 'text-no-wrap' }
  },
  {
    title: 'Name',
    key: 'name',
    sortable: false
  },
  {
    title: 'Specialties',
    key: 'specialty',
    sortable: false
  },
  {
    title: 'Status',
    key: 'status',
    sortable: false
  },
  {
    title: 'Actions',
    key: 'actions',
    sortable: false,
    width: 0,
    cellProps: { class: 'text-no-wrap' }
  }
]

const loadDataTable = async () => {
  try {
    isLoadingList.value = true

    const { isError, data } = await request<GetDoctorListRequest, GetDoctorListResponse>({
      method: 'GET',
      endpoint: 'doctor/list',
      body: {
        itemsPerPage: itemsPerPage.value,
        page: page.value,
        name: filterName.value,
        specialtyId: filterSpecialtyId.value,
        statusId: filterStatusId.value
      }
    })

    if (isError) return

    items.value = data.items
    total.value = data.total

    isLoadingList.value = false
  } catch (e) {
    console.error('Error Fetching Item From List', e)
  }
}

const handleDataTableUpdate = async ({ page: tablePage, itemsPerPage: tableItemsPerPage }: any) => {
  page.value = tablePage
  itemsPerPage.value = tableItemsPerPage

  loadDataTable()
}

const loadFilters = async () => {
  isLoadingFilter.value = true

  const specialtyRequest = await request<undefined, GetSpecialtyListResponse>({
    method: 'GET',
    endpoint: 'specialty/list'
  })

  const statusRequest = await request<undefined, GetStatusListResponse>({
    method: 'GET',
    endpoint: 'status/list'
  })

  try {
    const [specialtyResponse, statusResponse] = await Promise.all([specialtyRequest, statusRequest])

    if (specialtyResponse.isError || statusResponse.isError) return

    specialtyItems.value = specialtyResponse.data.items
    statusItems.value = statusResponse.data.items
  } catch (e) {
    console.error('Error Fetching Filter Items', e)
  }

  isLoadingFilter.value = false
}

const deleteListItem = async (item: IDoctor) => {
  const shouldDelete = confirm(`Do You Really Want Delete ${item.name}?`)

  if (!shouldDelete) return

  try {
    const response = await request<null, null>({
      method: 'DELETE',
      endpoint: `doctor/delete/${item.id}`
    })

    if (response.isError) return

    toastStore.setToast({
      type: 'success',
      text: 'Professional Deleted Successfully.'
    })

    loadDataTable()
  } catch (e) {
    console.error('Failed To Delete Item From List', e)
  }
}

onMounted(() => {
  loadFilters()
})
</script>

<template>
  <DefaultTemplate>
    <template #title> List of Professionals </template>

    <template #action>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" :to="{ name: 'doctor-insert' }"> Add Professional </v-btn>
    </template>

    <template #default>
      <v-sheet class="pa-4 mb-4">
        <v-form @submit.prevent="loadDataTable">
          <v-row>
            <v-col>
              <v-text-field v-model.trim="filterName" label="Name" hide-details />
            </v-col>
            <v-col>
              <v-select
                v-model="filterSpecialtyId"
                label="Specialties"
                :loading="isLoadingFilter"
                :items="specialtyItems"
                item-value="id"
                item-title="name"
                clearable
                hide-details
              />
            </v-col>
            <v-col>
              <v-select
                v-model="filterStatusId"
                label="Status"
                :loading="isLoadingFilter"
                :items="statusItems"
                item-value="id"
                item-title="name"
                clearable
                hide-details
              />
            </v-col>
            <v-col cols="auto" class="d-flex align-center">
              <v-btn color="primary" type="submit">Filter</v-btn>
            </v-col>
          </v-row>
        </v-form>
      </v-sheet>

      <v-data-table-server
        v-model:items-per-page="itemsPerPage"
        :headers="headers"
        :items-length="total"
        :items="items"
        :loading="isLoadingList"
        item-value="id"
        @update:options="handleDataTableUpdate"
      >
        <template #[`item.specialty`]="{ item }">
          <v-chip v-for="specialty of item.specialty" :key="specialty.id" class="mr-2"> {{ specialty.name }} </v-chip>
        </template>
        <template #[`item.status`]="{ item }">
          <v-chip> {{ item.status.name }} </v-chip>
        </template>
        <template #[`item.actions`]="{ item }">
          <v-tooltip text="Delete Professional" location="left">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                :icon="mdiTrashCan"
                size="small"
                color="error"
                class="mr-2"
                @click="deleteListItem(item)"
              />
            </template>
          </v-tooltip>
          <v-tooltip text="Edit Professional" location="left">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                :icon="mdiSquareEditOutline"
                size="small"
                color="primary"
                :to="{ name: 'doctor-update', params: { id: item.id } }"
              />
            </template>
          </v-tooltip>
        </template>
      </v-data-table-server>
    </template>
  </DefaultTemplate>
</template>

