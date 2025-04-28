import { defineStore } from "pinia";
import { ref } from "vue";
import type { Reservation } from "~/types/reservation";

export const useReservationStore = defineStore("reservationStore", () => {
    let currentId: number = 0;

  function generateId(): number {
    return ++currentId;
  }

  const reservations = ref<Reservation[]>([]);
  const config = useRuntimeConfig();

  const makeReservation = async (reservation: Reservation) => { 
    try {
      const response = await fetch(`${config.public.apiBase}Reservations`,{
        method: 'POST',
        body: JSON.stringify(reservation),
        headers: {
          'Content-Type': 'application/json',
        }
      });
      if(response) {
      reservations.value.push(response);
      return response;
      } 
    }
    catch (error) {
      console.error("Error making reservation:", error);
      throw error; 
    }
  };

  return { reservations, makeReservation, generateId };
});