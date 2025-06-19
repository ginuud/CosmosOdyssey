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
            <div v-if="filteredAndSortedRoutes.length === 0" class="no-routes">
                No trips where found.
            </div>

            <div v-for="route in filteredAndSortedRoutes" :key="`${route.price}-${route.travelTime}`"
                class="route-card">
                <div class="route-header">
                    <h2>{{ route.from }} → {{ route.to }}</h2>
                    <span class="price">{{ route.price }}$</span>
                </div>

                <div class="route-details">
                    <div class="detail-group">
                        <span class="label">Company:</span>
                        <span class="value">{{ route.companyNames.join(', ') }}</span>
                    </div>

                    <!-- testimiseks: -->
                    <div class="detail-group">
                        <span class="label">Price List Id:</span>
                        <span class="value">{{ route.priceListId }}</span>
                    </div>

                    <div class="detail-group">
                        <span class="label">route Info Ids:</span>
                        <span class="value">{{ route.routeInfoIds.join(', ') }}</span>
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
                    <p><span class="label">Company(s):</span> {{ selectedRoute?.companyNames?.join(', ') }}</p>
                    <p><span class="label">Price:</span> {{ selectedRoute?.price }}€ </p>
                    <p><span class="label">Travel time:</span> {{ formatTravelTime(selectedRoute?.travelTime ?? 0) }}
                    </p>
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
.travel-app {
    font-family: 'Orbitron', sans-serif;
    size: 16px;
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
}

h1 {
    color: #e4e9ee;
    font-size: 2.5rem;
    text-align: center;
    margin-bottom: 30px;
}

.filters {
    display: flex;
    justify-content: space-between;
    margin-bottom: 20px;
    padding: 15px;
    background-color: #f5f7fa;
    border-radius: 8px;
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
    border: 1px solid #ddd;
    background-color: white;
}

.routes-container {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    gap: 20px;
}

.route-card {
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 15px;
    background-color: white;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    transition: transform 0.2s ease;
}

.route-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.route-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
    padding-bottom: 10px;
    border-bottom: 1px solid #4c39f5;
}

.route-header h2 {
    margin: 0;
    font-size: 18px;
    color: #2c3e50;
}

.price {
    font-weight: bold;
    font-size: 18px;
    color: #4c39f5;
}

.route-details {
    margin-bottom: 15px;
}

.detail-group {
    display: flex;
    margin-bottom: 5px;
}

.label {
    width: 160px;
    font-weight: bold;
    color: #0b0b0b;
    font-family: 'orbitron', sans-serif;
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
    background-color: transparent;
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

.form-group {
    margin-bottom: 15px;
}

label {
    display: block;
    margin-bottom: 5px;
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
    border: none;
    border-radius: 4px;
    font-weight: bold;
    cursor: pointer;
}

.submit-button:hover {
    background-color: #0056b3;
}

.no-routes {
    grid-column: 1 / -1;
    text-align: center;
    padding: 30px;
    background-color: #f8f9fa;
    border-radius: 8px;
    color: #6c757d;
}

.value {
    color: #0b010c;
    font-family: 'funnel-web', sans-serif;

}

.route-header h2 {
    margin: 0;
    font-size: 18px;
    color: #2c3e50;
    font-weight: bold;
}

.modal-content h2 {
    font-size: 24px;
    font-weight: bold;
    color: #03111f;
}

.see-reservations-container {
    display: flex;
    justify-content: space-between;
    align-items: center;
    gap: 16px;
    margin-bottom: 20px;

}

.buttons {
    display: flex;
    gap: 10px;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 20px;
}
</style>