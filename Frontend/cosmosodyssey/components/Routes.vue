<template>
    <div class="travel-app">
        <h1>Available Routes</h1>
        <div class="see-reservations-container">
            <Timer :reservation-modal-open="showReservationModal" @close-reservation="showReservationModal = false"
                @session-expired="handleSessionExpired" />
            <div class="buttons">
                <SeeReservationsButton />
                <SelectTripButton />
            </div>
        </div>


        <div class="filters">
            <div class="filter-group">
                <label for="company-filter">Filter by company:</label>
                <select id="company-filter" v-model="filters.companyName">
                    <option value="">All companies</option>
                    <option v-for="company in uniqueCompanies" :key="company" :value="company">
                        {{ company }}
                    </option>
                </select>

            </div>

            <div class="filter-group">
                <label for="sort-by">Sort by:</label>
                <select id="sort-by" v-model="sortBy">
                    <option value="price">Price</option>
                    <option value="distance">Distance</option>
                    <option value="travelTime">Total travel time</option>
                </select>

                <select v-model="sortDirection">
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                </select>
            </div>
        </div>

        <div class="routes-container">
            <div v-if="filteredAndSortedRoutes.length === 0" class="no-data">
                No trips where found.
            </div>

            <div v-for="route in filteredAndSortedRoutes" :key="`${route.price}-${route.travelTime}`" class="card">
                <div class="header">
                    <h2>{{ route.from }} → {{ route.to }}</h2>
                    <span class="important-info">{{ route.price }}$</span>
                </div>

                <div class="details">
                    <div class="detail-group">
                        <span class="label">Company:</span>
                        <span class="value">{{ route.companyNames.join(', ') }}</span>
                    </div>

                    <div class="detail-group">
                        <span class="label">Distance:</span>
                        <span class="value">{{ route.distance }} km</span>
                    </div>

                    <div class="detail-group">
                        <span class="label">Total travel time:</span>
                        <span class="value">{{ formatTravelTime(route.travelTime) }}</span>
                    </div>
                </div>




                <button @click="openReservationModal(route)" class="reserve-button">Make a reservation</button>
            </div>
        </div>

        <div v-if="showReservationModal" class="modal">
            <div class="modal-content">
                <span class="close-button" @click="showReservationModal = false">&times;</span>
                <h2>Plan a trip</h2>
                <div class="selected-route-info">
                    <h3>{{ selectedRoute?.from }} → {{ selectedRoute?.to }}</h3>
                    <div class="selected-trip-details">
                        <span class="label">Company(s):</span>
                        <span class="value">{{ selectedRoute?.companyNames?.join(', ') }}</span>
                        <span class="label">Price:</span>
                        <span class="value">{{ selectedRoute?.price }}€ </span>
                        <span class="label">Travel time:</span>
                        <span class="value">{{ formatTravelTime(selectedRoute?.travelTime ?? 0) }} </span>
                    </div>
                </div>

                <form @submit.prevent="submitReservation">
                    <div class="form-group">
                        <label for="firstName">First name:</label>
                        <input type="text" id="firstName" v-model="reservation.firstName" required>
                    </div>

                    <div class="form-group">
                        <label for="lastName">Last name:</label>
                        <input type="text" id="lastName" v-model="reservation.lastName" required>
                    </div>

                    <button type="submit" class="submit-button">Make reservation</button>
                </form>
            </div>
        </div>

    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import { useRoute } from 'vue-router';
import { useRouteStore } from '~/stores/RouteStore';
import { useRouteInfoStore } from '~/stores/RouteInfoStore';
import { useReservationStore } from '~/stores/reservationStore';
import type { Route } from '~/types/route';
import { watch } from 'vue';


const route = useRoute();
const router = useRouter();
const routeStore = useRouteStore();
const routeInfoStore = useRouteInfoStore();
const { routes } = storeToRefs(routeStore);
const { routeInfos } = storeToRefs(routeInfoStore);
const reservationStore = useReservationStore();
const showTimerWarning = ref(false);


const handleSessionExpired = () => {
    showTimerWarning.value = false;
    window.location.reload();
};


const filters = ref({
    companyName: ""
});
const sortBy = ref("price");
const sortDirection = ref("asc");
const showReservationModal = ref(false);
const selectedRoute = ref<Route | null>(null);
const reservation = ref({
    firstName: "",
    lastName: ""
});

watch(showReservationModal, (val) => {
    if (val) {
        document.body.classList.add('modal-open');
    } else {
        document.body.classList.remove('modal-open');
    }
});


const uniqueCompanies = computed(() => {
    return [...new Set(routes.value.flatMap(route => route.companyNames))];
});


const filteredAndSortedRoutes = computed(() => {
    let result = [...routes.value];

    if (filters.value.companyName) {
        result = result.filter(route =>
            route.companyNames.includes(filters.value.companyName)
        );
    }

    result.sort((a, b) => {
        let modifier = sortDirection.value === "asc" ? 1 : -1;
        const aValue = a[sortBy.value as keyof Route];
        const bValue = b[sortBy.value as keyof Route];

        if (typeof aValue === 'number' && typeof bValue === 'number') {
            return (aValue - bValue) * modifier;
        }
        return String(aValue).localeCompare(String(bValue)) * modifier;
    });

    return result;
});

const formatTravelTime = (hours: number) => {

    const totalMinutes = Math.round(hours * 60);
    const days = Math.floor(totalMinutes / (60 * 24));
    const hoursPart = Math.floor((totalMinutes % (60 * 24)) / 60);
    const minutesPart = totalMinutes % 60;

    let result = "";
    if (days > 0) result += `${days} day(s) `;
    if (hoursPart > 0 || days > 0) result += `${hoursPart}h `;
    result += `${minutesPart}min`;
    return result.trim();
};

const openReservationModal = (route: Route) => {
    selectedRoute.value = route;
    showReservationModal.value = true;
    reservation.value = { firstName: "", lastName: "" };
};

const submitReservation = async () => {
    if (!validate()) return;
    try {
        await reservationStore.makeReservation({
            id: reservationStore.generateId(),
            firstName: reservation.value.firstName.trim(),
            lastName: reservation.value.lastName.trim(),
            routeInfoIds: selectedRoute.value?.routeInfoIds ?? [],
            totalQuotedPrice: selectedRoute.value?.price ?? 0,
            totalQuotedTravelTime: selectedRoute.value?.travelTime ?? 0,
            transportationCompanyNames: selectedRoute.value?.companyNames ?? []

        });
        await reservationStore.getReservations();
        showReservationModal.value = false;
        reservation.value.firstName = "";
        reservation.value.lastName = "";
        selectedRoute.value = null;
    } catch (error) {
        console.error("Error making reservation:", error);
    }
};

const validate = () => {
    let isValid = true;
    errors.firstName = reservation.value.firstName.trim() ? null : "Required";
    errors.lastName = reservation.value.lastName.trim() ? null : "Required";

    if (!reservation.value.firstName.trim() || !reservation.value.lastName.trim()) {
        isValid = false;
    }
    return isValid;
};

const errors = reactive<{ firstName: string | null, lastName: string | null }>({
    firstName: null,
    lastName: null,
});

onMounted(async () => {
    if (routes.value.length === 0) {
        const exists = await routeStore.checkRouteExists(
            route.query.from as string,
            route.query.to as string
        );
    }
});
</script>

<style scoped>
@import "@/assets/css/cardStyle.css";

.filters {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
    padding: 15px;
    background-color: #bdd1f0;
    border-radius: 30px;
}

.filter-group {
    display: flex;
    align-items: center;
    gap: 10px;
    color: #2c3e50;
}

select {
    padding: 8px;
    border-radius: 4px;
    background-color: white;
}

.routes-container {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
    align-items: flex-start;
}

.reserve-button {
    width: 100%;
    padding: 10px;
    background-color: #4c39f5;
    color: white;
    border-radius: 4px;
    font-weight: bold;
    cursor: pointer;
}

.reserve-button:hover {
    background-color: #0056b3;
}

.modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.5);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.modal-content {
    width: 90%;
    max-width: 500px;
    background-color: white;
    border-radius: 8px;
    padding: 20px;
    position: relative;
    color: #111213;
}

.close-button {
    position: absolute;
    top: 10px;
    right: 15px;
    font-size: 24px;
    cursor: pointer;
    color: #666;
}

.selected-route-info {
    padding: 10px;
    border-radius: 4px;
    margin-bottom: 20px;
}

.selected-route-info h3 {
    margin: 0 0 10px 0;
    color: #0c0c0c;
}

.selected-route-info p {
    margin: 0;
    color: #3b3b3c;
    font-family: 'funnel-web', sans-serif;
}

.selected-trip-details {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.form-group {
    margin-bottom: 15px;
}

label {
    font-weight: bold;
    color: #111213;
}

input {
    width: 100%;
    padding: 8px;
    border: 1px solid #ddd;
    border-radius: 4px;
}

.submit-button {
    width: 100%;
    padding: 10px;
    background-color: #0c66cc;
    color: white;
    border-radius: 4px;
    font-weight: bold;
    cursor: pointer;
}

.submit-button:hover {
    background-color: #0056b3;
}

.modal-content h2 {
    font-size: 24px;
    font-weight: bold;
    color: #03111f;
}

.see-reservations-container {
    display: flex;
    justify-content: space-between;
    gap: 16px;
}

.buttons {
    display: flex;
    gap: 10px;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 20px;
}
</style>

<style>
body.modal-open {
    overflow: hidden;
}
</style>