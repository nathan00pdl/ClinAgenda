export const routes = [
  {
    path: '/',
    name: 'dashboard',
    component: () =>
      import(/* webpackChunkName: "dashboard" */ '@/pages/dashboard/DashboardPage.vue')
  },

  //STATUS
  {
    path: '/status',
    name: 'status-list',
    component: () => import(/* webpackChunkName: "status" */ '@/pages/status/StatusListPage.vue')
  },
  {
    path: '/status/insert',
    name: 'status-insert',
    component: () => import(/* webpackChunkName: "status" */ '@/pages/status/StatusFormPage.vue')
  },
  {
    path: '/status/update/:id',
    name: 'status-update',
    component: () => import(/* webpackChunkName: "status" */ '@/pages/status/StatusFormPage.vue')
  },

  //SPECIALTY
  {
    path: '/specialty',
    name: 'specialty-list',
    component: () => import(/* webpackChunkName: "status" */ '@/pages/specialty/SpecialtyListPage.vue')
  },
  {
    path: '/specialty/insert',
    name: 'specialty-insert',
    component: () => import(/* webpackChunkName: "status" */ '@/pages/specialty/SecialtyFormPage.vue')
  },
  {
    path: '/specialty/update/:id',
    name: 'specialty-update',
    component: () => import(/* webpackChunkName: "status" */ '@/pages/specialty/SecialtyFormPage.vue')
  }
]
