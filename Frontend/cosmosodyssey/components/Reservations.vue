<template>
    <div class="travel-app">
        <h1>Made Reservations</h1>
        <div class="top-bar">
            <div class="see-routes-container">
                <MoveToRoutesPageButton />
                <SelectTripButton />
            </div>
            <div class="filter-group">
                <UInput v-model="searchQuery" type="text" placeholder="Search by name..." class="search-input" />
            </div>
        </div>



        <div class="reservations-container">
            <div v-if="filteredReservations.length === 0" class="no-data">
                No reservations where found.
            </div>

            <div v-for="reservation in filteredReservations" :key="`${reservation.id}`" class="card">
                <div class="header">
                    <h2>{{ reservation.firstName }} {{ reservation.lastName }}</h2>
                    <span class="important-info" v-if="reservation.routes && reservation.routes.length > 0">
                        {{ reservation.routes[0].from.name }} → {{ reservation.routes[reservation.routes.length -
                            1].to.name }}
                    </span>
                    <span class=".important-info" v-else>
                        No route info
                    </span>
                </div>

                <div class="details">
                    <div class="detail-group">
                        <span class="label">Route(s):</span>
                        <span class="value">{{ Layovers(reservation) }}</span>
                    </div>

                    <div class="detail-group">
                        <span class="label">Price:</span>
                        <span class="value">{{ reservation.totalQuotedPrice }} $</span>
                    </div>
                    <div class="detail-group">
                        <span class="label">Travel time:</span>
                        <span class="value">{{ formatTravelTime(reservation.totalQuotedTravelTime) }}</span>
                    </div>

                    <div class="detail-group">
                        <span class="label">Company(s):</span>
                        <span class="value">{{ reservation.transportationCompanyNames.join(', ') }}</span>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { storeToRefs } from 'pinia';
import { useReservationStore } from '~/stores/reservationStore';
import type { Reservation } from '~/types/reservation';
import MoveToRoutesPageButton from './MoveToRoutesPageButton.vue';

const router = useRouter();
const reservationStore = useReservationStore();
const routeStore = useRouteStore();
const { reservations } = storeToRefs(reservationStore);
const { routes } = storeToRefs(routeStore);
const searchQuery = ref("");


const Layovers = (reservation: Reservation) => {
    if (!reservation.routes || reservation.routes.length === 0) return '';
    const stops = [reservation.routes[0].from.name];
    for (const route of reservation.routes) {
        stops.push(route.to.name);
    }
    return stops.join(' → ');
};


const filteredReservations = computed(() => {
    let result = [...reservations.value];

    if (searchQuery.value) {
        result = result.filter(reservation =>
            reservation.firstName.toLowerCase().includes(searchQuery.value.toLowerCase()) ||
            reservation.lastName.toLowerCase().includes(searchQuery.value.toLowerCase())
        );
    }
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


onMounted(async () => {
    if (reservations.value.length === 0) {
        await reservationStore.getReservations();

    }
});
</script>


<style scoped>
@import "@/assets/css/cardStyle.css";

.filter-group {
    gap: 10px;
    align-items: center;
    color: #e2e6ea;
}

.search-input {
    padding: 10px 20px;
    border-radius: 999px;
    border: 1.5px solid #fdfdfd;
}

.reservations-container {
    display: grid;
    grid-template-columns: repeat(1, 1fr);
    gap: 20px;
}

.top-bar {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
    gap: 20px;
}

.see-routes-container {
    display: flex;
    gap: 10px;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 20px;
}
</style>