import { defineStore } from 'pinia';
import { ref } from 'vue';
import type {Planet} from '~/types/planet';

export const usePlanetStore = defineStore('planet', () => {
  const planets = ref<Planet[]>([]);
  const config = useRuntimeConfig();
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

  const getPlanetNames = async () => {
    try {
      const response = await fetch(`${config.public.apiBase}Planets/names`);
      const data = await response.json();
      return Array.isArray(data) ? data : [];
    } catch (error) {
      console.error('Error loading planet names:', error);
      return [];
    }
  }

  return { planets, loadPlanets, getPlanetNames };
});