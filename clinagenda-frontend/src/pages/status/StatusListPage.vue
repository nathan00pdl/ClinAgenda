<script setup lang="ts">
import request from '@/engine/httpClient'
import type { GetStatusListRequest, GetStatusListResponse, IStatus } from '@/interfaces/status'
import { useToastStore } from '@/stores'
import { DefaultTemplate } from '@/template'
import { mdiPlusCircle, mdiTrashCan, mdiSquareEditOutline } from '@mdi/js'
import { ref } from 'vue'

const toastStore = useToastStore()

const isLoadingList = ref<boolean>(false)

const items = ref<IStatus[]>([])
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
    title: 'Actions',
    key: 'actions',
    sortable: false,
    width: 0,
    cellProps: { class: 'text-no-wrap' }
  }
]

const loadDataTable = async () => {
  isLoadingList.value = true

  const { isError, data } = await request<GetStatusListRequest, GetStatusListResponse>({
    method: 'GET',
    endpoint: 'status/list',
    body: {
      itemsPerPage: itemsPerPage.value,
      page: page.value
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

const deleteListItem = async (item: IStatus) => {
  const shouldDelete = confirm(`Do You Really Want Delete ${item.name}?`)
  
  if (!shouldDelete) return

  const response = await request<null, null>({
    method: 'DELETE',
    endpoint: `status/delete/${item.id}`
  })

  if (response.isError) return

  toastStore.setToast({
    type: 'success',
    text: 'Status Deleted Successfully.'
  })

  loadDataTable()
}
</script>

<template>
  <default-template>
    <template #title> List of Status </template>

    <template #action>
      <v-btn color="primary" :prepend-icon="mdiPlusCircle" :to="{ name: 'status-insert' }"> Add Status </v-btn>
    </template>

    <template #default>
      <v-data-table-server
        v-model:items-per-page="itemsPerPage"
        :headers="headers"
        :items-length="total"
        :items="items"
        :loading="isLoadingList"
        item-value="id"
        @update:options="handleDataTableUpdate"
      >
        <template #[`item.actions`]="{ item }">
          <v-tooltip text="Delete Status" location="left">
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
          <v-tooltip text="Edit Status" location="left">
            <template #activator="{ props }">
              <v-btn
                v-bind="props"
                :icon="mdiSquareEditOutline"
                size="small"
                color="primary"
                class="mr-2"
                :to="{ name: 'status-update', params: { id: item.id } }"
              />
            </template>
          </v-tooltip>
        </template>
      </v-data-table-server>
    </template>
  </default-template>
</template>

