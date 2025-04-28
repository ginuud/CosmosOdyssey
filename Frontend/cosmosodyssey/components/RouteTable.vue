<script setup lang="ts">
import { h, resolveComponent } from 'vue'
import type { FormSubmitEvent, TableColumn } from '@nuxt/ui'
import type {Route} from '~/types/route'

defineProps<{
  routes: Route[]
}>()

const isReservationModalOpen = ref(false);
const selectedProviderId = ref<string | null>(null);
const routeInfoStore = useRouteInfoStore();
const reservationStore = useReservationStore();
const { routes } = storeToRefs(routeInfoStore);
const firstName = ref("");
const lastName = ref("");

const errors = reactive<{ firstName: string | null, lastName: string | null }>({
  firstName: null,
  lastName: null,
});

const openReservationModal = (providerId: Guid) => {
  const route = routeInfoStore.routes.find((r) => r.providerId === providerId);
  if (route) {
    selectedProviderId.value = providerId;
    firstName.value = "";
    lastName.value = "";
    isReservationModalOpen.value = true;
  }
};

const validate = () => {
  let isValid = true;
  errors.firstName = firstName.value.trim() ? null : "Required";
  errors.lastName = lastName.value.trim() ? null : "Required";
  
  if (!firstName.value.trim() || !lastName.value.trim()) {
    isValid = false;
  }
  return isValid;
};

// const submitReservation = () => {
//   if (validate()) {
//     if (selectedProviderId.value !== null) {
//         reservationStore.makeReservation(
//           selectedProviderId.value,
//           firstName.value,
//           lastName.value
//         );
//         isReservationModalOpen.value = false;
//         selectedProviderId.value = null;
//         navigateTo("/reservations");
//     }
//   }
// };

const submitReservation = async () => {
  if (!validate()) return;
  try{
    await reservationStore.makeReservation({
      id: reservationStore.generateId(),
      firstName: firstName.value,
      lastName: lastName.value,
      providerId: selectedProviderId.value!,
    });
    isReservationModalOpen.value = false;
    firstName.value = "";
    lastName.value = "";
    selectedProviderId.value = null;
  } catch (error) {
    console.error("Error making reservation:", error);
  }
};

const columns: TableColumn<Route>[] = [
{
  accessorKey: 'companyName',
  header: () => h('div', { class: 'text-right nebula-text' }, 'Company'), 
  cell: ({ row }) => h('div', { class: 'text-right stardust-text' }, row.getValue('companyName'))
  
},
{
  accessorKey: 'price',
  header: () => h('div', { class: 'text-right nebula-text' }, 'Price'),
  cell: ({ row }) => {
  const amount = Number.parseFloat(row.getValue('price'))

  const formatted = new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'EUR'
  }).format(amount)

  return h('div', { class: 'text-right font-medium stardust-text' }, formatted)
}},
{
  accessorKey: 'distance',
  header: () => h('div', { class: 'text-right nebula-text' }, 'Distance'),
  cell: ({ row }) => {
  const amount = Number.parseFloat(row.getValue('distance'))

  return h('div', { class: 'text-right font-medium stardust-text' }, `${amount} km`)
}},
{
  accessorKey: 'travelTime',
  header: () => h('div', { class: 'text-right nebula-text' }, 'Travel Time'),
  cell: ({ row }) => {
  const amount = Number.parseFloat(row.getValue('travelTime'))

  return h('div', { class: 'text-right font-medium stardust-text' }, `${amount} hrs`)
}},
{
  accessorKey: 'providerId',
  header: () => h('div', { class: 'text-right nebula-text' }, 'Register'),
  cell: ({ row }) => {
  const providerId = row.getValue('providerId') as Guid
      return h(
        'div',
        { class: 'flex justify-center' },
        [
          h(resolveComponent('UButton'),
            {
              icon: 'i-heroicons-plus-20-solid',
              class: 'register-button',
              onClick: () => openReservationModal(providerId)
            },
            () => 'Book Now'
          )
        ]
      )
    }
  }
]


</script>

<template>
  <div class="route-table-container">
    <UTable 
      sticky 
      :data="routes" 
      :columns="columns" 
      class="space-table"
      :ui="{
        th: { base: 'nebula-bg px-6 py-4' },
        td: { base: 'stardust-bg px-6 py-4' },
        thead: 'space-x-4', 
        tbody: 'space-x-4' 
      }"
    >
      <template #loading-state>
        <div class="loading-overlay">
          <div class="pulsar"></div>
        </div>
      </template>
    </UTable>

  <UModal v-model="isReservationModalOpen" prevent-close
    :ui="{ 
          container: 'flex min-h-full items-center justify-center text-center',
          inner: 'inline-block text-left align-middle transition-all min-w-[400px]'
        }">
    <UCard class="space-y-4 nebula-bg text-white">
      <template #header>
        <div class="flex items-center justify-between">
          <h3 class="text-lg font-bold nebula-text">Space Travel Registration</h3>
            <UButton
              variant="ghost"
              icon="i-heroicons-x-mark-20-solid"
              @click="isReservationModalOpen = false"
          />
        </div>
      </template>
      
        <UFormField label="First Name" name="firstName"> required>
          <UInput v-model="firstName" variant="outline" placeholder="Enter your first name" />
          <p v-if="errors.firstName" class="text-red-500 text-sm mt-1">
            {{ errors.firstName }}
          </p>
        </UFormField>

        <UFormField label="Last Name" name="lastName" required class="mt-4">
          <UInput v-model="lastName" variant="outline" placeholder="Enter your last name" />
          <p v-if="errors.lastName" class="text-red-500 text-sm mt-1">
            {{ errors.lastName }}
          </p>
        </UFormField>

        <template #footer>
          <div class="flex justify-end gap-3">
            <UButton label="Cancel" @click="isReservationModalOpen = false" />
            <UButton
              @click="submitReservation" 
              type="submit" 
              label="Confirm Booking" 
              class="register-button"
            />
          </div>
        </template>
    </UCard>
  </UModal>
  </div>
</template>

<style>

@import url('https://fonts.googleapis.com/css2?family=Orbitron&display=swap');

.route-table-container {
  background: linear-gradient(45deg, #0a0a2e, #1a1a4a);
  border: 1px solid rgba(90, 200, 250, 0.2);
  border-radius: 12px;
  padding: 2rem;
  position: relative;
  overflow: hidden;
  box-shadow: 0 0 40px rgba(90, 200, 250, 0.1);
  backdrop-filter: blur(5px);
}

.route-table-container::before {
  content: '';
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: url('data:image/svg+xml,<svg xmlns="http://www.w3.org/2000/svg" width="100" height="100" viewBox="0 0 100 100"><circle cx="50" cy="50" r="1" fill="%237ED4FD" opacity="0.5"/></svg>');
  opacity: 0.1;
}

.route-table-container::after {
  content: '';
  position: absolute;
  top: -50%;
  left: -50%;
  width: 200%;
  height: 200%;
  background: linear-gradient(45deg, transparent, #007FFF, transparent);
  animation: rotate 20s linear infinite;
  opacity: 0.1;
  z-index: 0;
}

@keyframes rotate {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

:deep(.space-table) {
  --ui-table-border-color: rgba(0, 127, 255, 0.2);
  position: relative;
  z-index: 2;
  border-spacing: 1rem;
  border-collapse: separate;
}

:deep(.nebula-bg) {
  background: linear-gradient(45deg, #0f3460, #1a1a4a) ;
  color: #7ED4FD ;
  font-family: 'Orbitron', sans-serif ;
  font-weight: 700 ;
  border-bottom: 2px solid #007FFF ;
  text-shadow: 0 0 10px rgba(126, 212, 253, 0.5);
}

:deep(.stardust-bg) {
  background: rgba(16, 24, 39, 0.5) ;
  color: #B0E0FF ;
  border-bottom: 1px solid rgba(0, 127, 255, 0.1) ;
}

:deep(.space-table tr:hover td) {
  background: rgba(0, 127, 255, 0.05) ;
  box-shadow: inset 0 0 30px rgba(0, 127, 255, 0.1) ;
}

.loading-overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(10, 10, 46, 0.9);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 3;
}

.pulsar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: radial-gradient(circle, #007FFF 0%, #00BFFF 100%);
  animation: pulsate 1.5s ease-out infinite;
}

@keyframes pulsate {
  0% { transform: scale(0.8); opacity: 0.8; }
  50% { transform: scale(1.2); opacity: 1; }
  100% { transform: scale(0.8); opacity: 0.8; }
}

.nebula-text {
  color: #7ED4FD !important;
  font-family: 'Orbitron', sans-serif !important;
}

.stardust-text {
  color: #B0E0FF !important;
  font-family: monospace !important;
  letter-spacing: 1px;
}

::-webkit-scrollbar {
  width: 8px;
  background: rgba(0, 0, 0, 0.2);
}

::-webkit-scrollbar-thumb {
  background: #007FFF;
  border-radius: 4px;
  box-shadow: inset 0 0 6px rgba(0, 127, 255, 0.5);
}

.register-button {
  background: linear-gradient(45deg, #007FFF, #00BFFF);
  color: white;
  border: 1px solid rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
  &:hover {
    transform: scale(1.05);
    box-shadow: 0 0 15px rgba(0, 127, 255, 0.5);
  }
}

.nebula-text {
  color: #7ED4FD;
  font-family: 'Orbitron', sans-serif;
  text-shadow: 0 0 10px rgba(126, 212, 253, 0.5);
}
</style>