<template>
    <div class="travel-app">
        <h1>Made Reservations</h1>
        <div class="see-routes-container">
            <MoveToRoutesPageButton />
            <SelectTripButton />
        </div>

        <div class="filters">
            <div class="filter-group">
                <UInput v-model="searchQuery" type="text" placeholder="Search by name..."
                    class="search-input rounded-full" />
            </div>
        </div>

        <div class="reservation-container">
            <div v-if="filteredReservations.length === 0" class="no-reservations">
                No reservations where found.
            </div>

            <div v-for="reservation in filteredReservations" :key="`${reservation.id}`" class="reservation-card">
                <div class="reservation-header">
                    <h2>{{ reservation.firstName }} {{ reservation.lastName }}</h2>
                    <span class="reservation" v-if="reservation.routes && reservation.routes.length > 0">
                        {{ reservation.routes[0].from.name }} → {{ reservation.routes[reservation.routes.length -
                            1].to.name }}
                    </span>
                    <span class="reservation" v-else>
                        No route info
                    </span>
                </div>

                <div class="reservation-details">
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
                        <span class="label">Company name(s):</span>
                        <span class="value">{{ reservation.transportationCompanyNames }}</span>
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
    background-color: transparent;
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

.reservations-container {
    display: grid;
    grid-template-columns: repeat(1, 1fr);
    gap: 20px;
}

.reservation-card {
    border: 1px solid #ddd;
    border-radius: 8px;
    padding: 15px;
    background-color: white;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    transition: transform 0.2s ease;
}

.reservation-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
}

.reservation-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 15px;
    padding-bottom: 10px;
    border-bottom: 1px solid #4c39f5;
}

.reservation-header h2 {
    margin: 0;
    font-size: 18px;
    color: #2c3e50;
}

.reservation {
    font-weight: bold;
    font-size: 18px;
    color: #4c39f5;
}

.reservation-details {
    margin-bottom: 15px;
}

.detail-group {
    display: flex;
    margin-bottom: 5px;
}

.label {
    width: 160px;
    font-weight: bold;
    color: #666;
}

.reserve-button {
    display: block;
    width: 100%;
    padding: 10px;
    background-color: #4c39f5;
    color: white;
    border: none;
    border-radius: 4px;
    font-weight: bold;
    cursor: pointer;
    transition: background-color 0.2s ease;
}

.reserve-button:hover {
    background-color: #2b8fad;
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
}

.close-button {
    position: absolute;
    top: 10px;
    right: 15px;
    font-size: 24px;
    cursor: pointer;
    color: #666;
}

/* .selected-reservation-info {
    background-color: transparent;
    padding: 10px;
    border-radius: 4px;
    margin-bottom: 20px;
}

.selected-route-info h3 {
    margin: 0 0 10px 0;
    color: #2c3e50;
}

.selected-route-info p {
    margin: 0;
    color: #2a4ba3;
} */

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


.no-reservations {
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

.reservation-header h2 {
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

.see-routes-container {
    display: flex;
    gap: 10px;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 20px;
}
</style>