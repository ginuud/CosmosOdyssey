import { defineStore } from "pinia";
import { ref } from "vue";
import type { Reservation } from "~/types/reservation";
import { useRouteInfoStore} from "~/stores/RouteInfoStore";

export const useReservationStore = defineStore("reservationStore", () => {
    let currentId: number = 0;

  function generateId(): number {
    return ++currentId;
  }

  const reservations = ref<Reservation[]>([]);
  const config = useRuntimeConfig();

  const makeReservation = async (reservation: Reservation) => { 
    try {
      const response = await $fetch(`${config.public.apiBase}Reservations`,{
        method: 'POST',
        body: JSON.stringify(reservation),
        headers: {
          'Content-Type': 'application/json'
        }
      });
      reservations.value.push(response);
      return response;
    }
    catch (error) {
      console.error("Error making reservation:", error);
      throw error; 
    }
  };

  const getReservations = async () => {
    try {
      const data = await $fetch(`${config.public.apiBase}Reservations`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
    });
      if (!data || !Array.isArray(data)) {
        reservations.value = [];
        return false;
      }
      const routeInfoStore = useRouteInfoStore();
    if (routeInfoStore.routeInfos.length === 0) {
      await routeInfoStore.getRouteInfos();
    }
    const allRouteInfos = routeInfoStore.routeInfos;

    data.forEach((reservation: any) => {
  console.log('routeInfoIds:', reservation.routeInfoIds);
  console.log('allRouteInfos ids:', allRouteInfos.map(i => i.id));
});


reservations.value = data.map((reservation: any) => ({
  ...reservation,
  routes: reservation.routes ?? []
}));

      return data.length > 0;
    } catch (error) {
      console.error("Error fetching reservations:", error);
      reservations.value = [];
      return false;
    }
  };

  return { reservations, makeReservation, generateId, getReservations };
});