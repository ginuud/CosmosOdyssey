import { defineStore } from 'pinia';
import { ref } from 'vue';
import type {Planet} from '~/types/planet';

export const usePlanetStore = defineStore('planet', () => {
  const planets = ref<Planet[]>([]);
  const isLoaded = ref(false);

  const loadPlanets = async () => {
    if (isLoaded.value) return;
    try {
        const response = await fetch('/api/Planets');
        const data = await response.json();
        planets.value = data as Planet[];
        isLoaded.value = true;
    } catch (error) {
      console.error('Error loading planets:', error);
    }
  };

  return { planets, loadPlanets };
});