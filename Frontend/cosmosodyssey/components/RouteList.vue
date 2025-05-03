<template>
  <div class="mb-4 table-container flex items-center justify-between">
    <h1 class="title"> Routes from {{ route.query.from }} to {{ route.query.to }}</h1>

  </div>
  <div class="mb-4 table-container left flex items-center justify-between">
    <input v-model="searchQuery" type="text" placeholder="Search company..." class="search-input" />
  </div>

  <div v-if="isLoading" class="loading">
    <div class="pulsar-loader"></div>
    Loading...
  </div>

  <div v-else-if="routes.length === 0" class="text-center text-red-500">
    No routes have been added
  </div>

  <div v-else-if="filteredRoutes.length === 0" class="text-center text-red-500">
    No routes match your search.
  </div>

  <div v-else>
    <div class="table-container">
      <Table class="Table">
        <TableHeader>
          <TableRow class="header-row">
            <TableCell class="header-cell">Company</TableCell>
            <TableCell class="header-cell">Price</TableCell>
            <TableCell class="header-cell">Distance</TableCell>
            <TableCell class="header-cell">Travel Time</TableCell>
            <TableCell class="header-cell">Register</TableCell>
          </TableRow>
        </TableHeader>
        <TableBody>
          <TableRow v-for="route in filteredRoutes" :key="route.providerId" class="border-b border-black">
            <TableCell>{{ route.companyName }}</TableCell>
            <TableCell>{{ route.price }}</TableCell>
            <TableCell>{{ route.distance }}</TableCell>
            <TableCell>{{ route.travelTime }}</TableCell>
            <TableCell>
              <UButton type="button" variant="ghost" icon="i-heroicons-plus-20-solid"
                @click="openReservationModal(route.providerId)"></UButton>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </div>
  </div>

  <template v-if="isReservationModalOpen" class=" fixed inset-0 flex items-center justify-center z-50">
    {{ console.log('Modal is open') }}
    <div class="fixed inset-0 bg-black opacity-50"></div>
    <UModal v-model:open="isReservationModalOpen" :portal="false" :overlay="true" :transition="true" class="z-50"
      aria-labelledby="dialog-title" aria-describedby="dialog-description">
      <template #header>
        <div class="flex items-center justify-between text-white p-4">
          <h2 class="text-xl font-bold" id="dialog-title">
            <title>Make a reservation</title>
          </h2>
          <UButton variant="ghost" icon="i-heroicons-x-mark-20-solid" @click="isReservationModalOpen = false" />
        </div>
      </template>

      <template #body>
        <div>
          <UFormField label="First Name" name="firstName">
            <UInput v-model="firstName" variant="outline" placeholder="Enter your first name" />
            <p v-if="errors.firstName">{{ errors.firstName }}</p>
          </UFormField>
          <UFormField label="Last Name" name="lastName">
            <UInput v-model="lastName" variant="outline" placeholder="Enter your last name" />
            <p v-if="errors.lastName">{{ errors.lastName }}</p>
          </UFormField>
        </div>
      </template>

      <template #footer>
        <div style="display: flex; justify-content: flex-end; gap: 8px;">
          <UButton label="Cancel" @click="isReservationModalOpen = false" />
          <UButton label="Submit" @click="submitReservation" />
        </div>
      </template>
    </UModal>
  </template>
</template>

<script setup lang="ts">

import { useRouteInfoStore } from '~/stores/RouteInfoStore'
import { ref, onMounted, computed, reactive } from "vue";
import { useReservationStore } from '~/stores/reservationStore';
import { useRoute, useRouter } from 'vue-router'
import { errorMessages } from 'vue/compiler-sfc';

const isReservationModalOpen = ref(false);
const selectedProviderId = ref<Guid | null>(null);
const routeInfoStore = useRouteInfoStore();
const reservationStore = useReservationStore();
const { routes } = storeToRefs(routeInfoStore);
const firstName = ref("");
const lastName = ref("");
const route = useRoute();
const router = useRouter();


const origin = route.query.from as string
const destination = route.query.to as string

if (!route.query.from || !route.query.to) {
  router.push('/');
}

const isLoading = ref(true);

onMounted(async () => {
  if (routes.value.length === 0) {
    const exists = await routeInfoStore.checkRouteExists(
      route.query.from as string,
      route.query.to as string
    )
    if (!exists) {
      return router.push('/')
    }
  }
  isLoading.value = false
});


const searchQuery = ref("");

const filteredRoutes = computed(() => {
  if (!searchQuery.value.trim()) return routes.value;
  const lowerCaseQuery = searchQuery.value.toLowerCase();
  return routes.value.filter((route) =>
    route.companyName.toLowerCase().includes(lowerCaseQuery)
  );
});

const openReservationModal = (providerId: Guid) => {
  console.log("Opening reservation modal for provider:", providerId);
  isReservationModalOpen.value = true;
  const route = routeInfoStore.routes.find((r) => r.providerId === providerId);
  if (route) {
    selectedProviderId.value = providerId;
    firstName.value = "";
    lastName.value = "";
    isReservationModalOpen.value = true;
  }
  else {
    console.error("Route not found for provider ID:", providerId);
    errors.firstName = "Route no longer exists, please try another one.";
  }
};

const submitReservation = async () => {
  if (!validate()) return;
  try {
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

const errors = reactive<{ firstName: string | null, lastName: string | null }>({
  firstName: null,
  lastName: null,
});

const validate = () => {
  let isValid = true;
  errors.firstName = firstName.value.trim() ? null : "Required";
  errors.lastName = lastName.value.trim() ? null : "Required";

  if (!firstName.value.trim() || !lastName.value.trim()) {
    isValid = false;
  }
  return isValid;
};


</script>

<style scoped>
.title {
  font-size: 2.5rem;
  color: #ff69b4;
  text-align: center;
  font-family: 'Orbitron', sans-serif;
  text-shadow: 0 0 15px rgba(255, 105, 180, 0.5);
  position: sticky;
  top: 0;
  z-index: 3;
}

.loading {
  height: 50vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 2rem;
  font-family: 'Orbitron', sans-serif;
  font-size: 2rem;
}

.pulsar-loader {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  background: radial-gradient(circle, #ff69b4 0%, #ff1493 100%);
  animation: pulsate 1.5s ease-out infinite;
}

@keyframes pulsate {
  0% {
    transform: scale(0.8);
    opacity: 0.8;
  }

  50% {
    transform: scale(1.2);
    opacity: 1;
  }

  100% {
    transform: scale(0.8);
    opacity: 0.8;
  }
}

.search-input {
  background-color: white;
  color: #333;
  width: 300px;
  padding: 8px 12px;
  border: 1px solid rgb(147, 4, 223);
  border-radius: 4px;
  box-shadow: 0 4px 4px rgb(147, 4, 223);
  font-size: 15px;
}

.search-input:focus {
  outline: none;
  box-shadow: 0 0px 5px rgba(147, 4, 223);
  z-index: 1;
}

.table-container {
  max-width: 90%;
  margin: 0 auto;
  padding: 20px;
}

.Table {
  width: 100%;
  border-spacing: 0;
  border-collapse: collapse;
  z-index: 1;
  font-family: 'Orbitron', sans-serif;
}

.Table tr:hover {
  background: none
}

.header-row {
  box-shadow: 0 5px 15px rgb(147, 4, 223);
}

.header-cell {
  font-weight: bold;
  color: #ae15e0;
  font-size: large;
  z-index: 1;
}

.Table th,
.Table td {
  padding: 10px;
  text-align: left;
  font-family: 'Orbitron', sans-serif;
}
</style>