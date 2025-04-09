export const routes = [
  {
    path: '/',
    name: 'dashboard',
    component: () =>
      import(/* webpackChunkName: "dashboard" */ '@/pages/dashboard/DashboardPage.vue')
  },

  // STATUS
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
  
  // SPECIALTY
  {
    path: '/specialty',
    name: 'specialty-list',
    component: () =>
      import(/* webpackChunkName: "specialty" */ '@/pages/specialty/SpecialtyListPage.vue')
  },
  {
    path: '/specialty/insert',
    name: 'specialty-insert',
    component: () =>
      import(/* webpackChunkName: "specialty" */ '@/pages/specialty/SecialtyFormPage.vue')
  },
  {
    path: '/specialty/update/:id',
    name: 'specialty-update',
    component: () =>
      import(/* webpackChunkName: "specialty" */ '@/pages/specialty/SecialtyFormPage.vue')
  },

  // PATIENT
  {
    path: '/patient/list',
    name: 'patient-list',
    component: () => import(/* webpackChunkName: "patient" */ '@/pages/patient/PatientListPage.vue')
  },
  {
    path: '/patient/insert',
    name: 'patient-insert',
    component: () => import(/* webpackChunkName: "patient" */ '@/pages/patient/PatientFormPage.vue')
  },
  {
    path: '/patient/update/:id',
    name: 'patient-update',
    component: () => import(/* webpackChunkName: "patient" */ '@/pages/patient/PatientFormPage.vue')
  }
]