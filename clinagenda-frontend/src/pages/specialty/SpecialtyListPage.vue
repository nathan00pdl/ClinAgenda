<script setup lang="ts">
import request from '@/engine/httpClient'
import type {
  GetSpecialtyListRequest,
  GetSpecialtyListResponse,
  ISpecialty
} from '@/interfaces/specialty'
import { useToastStore } from '@/stores'
import { DefaultTemplate } from '@/template'
import { mdiPlusCircle, mdiTrashCan, mdiSquareEditOutline } from '@mdi/js'
import { ref } from 'vue'

const toastStore = useToastStore()

const isLoadingList = ref<boolean>(false)

const filterName = ref<GetSpecialtyListRequest['name']>('')

const items = ref<ISpecialty[]>([])
const itemsPerPage = ref<number>(10)
const page = ref<number>(1)
const total = ref<number>(0)

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
    title: 'Duration',
    key: 'scheduleDuration',
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
  isLoadingList.value = true

  const { isError, data } = await request<GetSpecialtyListRequest, GetSpecialtyListResponse>({
    method: 'GET',
    endpoint: 'specialty/list',
    body: {
      itemsPerPage: itemsPerPage.value,
      page: page.value,
      name: filterName.value
    }
  })

  if (isError) return

  items.value = data.items
  total.value = data.total

  isLoadingList.value = false
}

const handleDataTableUpdate = async ({ page: tablePage, itemsPerPage: tableItemsPerPage }: any) => {
  page.value = tablePage
  itemsPerPage.value = tableItemsPerPage

  loadDataTable()
}

const deleteListItem = async (item: ISpecialty) => {
  const shouldDelete = confirm(`Do You Really Want Delete ${item.name}?`)

  if (!shouldDelete) return

  const response = await request<null, null>({
    method: 'DELETE',
    endpoint: `specialty/delete/${item.id}`
  })

  if (response.isError) return

  toastStore.setToast({
    type: 'success',
    text: 'Specialty Deleted Successfully.'
  })

  loadDataTable()
}
</script>

<template>
  <default-template>
    <template #title> List of Specialties </template>

    <template #action>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" :to="{ name: 'specialty-insert' }"> Add Specialties </v-btn>
    </template>

    <template #default>
      <v-sheet class="pa-4 mb-4">
        <v-form @submit.prevent="loadDataTable">
          <v-row>
            <v-col>
              <v-text-field v-model.trim="filterName" label="Name" hide-details />
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
        <template #[`item.scheduleDuration`]="{ item }"> {{ item.scheduleDuration }} minutes </template>
        <template #[`item.actions`]="{ item }">
          <v-tooltip text="Delete Specialties" location="left">
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
          <v-tooltip text="Edit Specialties" location="left">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                :icon="mdiSquareEditOutline"
                size="small"
                color="primary"
                class="mr-2"
                :to="{ name: 'specialty-update', params: { id: item.id } }"
              />
            </template>
          </v-tooltip>
        </template>
      </v-data-table-server>
    </template>
  </default-template>
</template>

