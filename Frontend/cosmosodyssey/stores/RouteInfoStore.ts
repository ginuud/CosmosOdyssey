import { defineStore } from "pinia";
import { ref } from "vue";

export const useRouteInfoStore = defineStore("routeInfoStore", () => {
  const routes = ref([]);
  const config = useRuntimeConfig();

  const checkRouteExists = async (origin: string, destination: string): Promise<boolean> => {
    try {
      const response = await fetch(`${config.public.apiBase}RouteInfos/getRoutes/${origin}/${destination}`);
      if (!response.ok) {
        return false; 
      }
      const data = await response.json();
      return data.length > 0; 
    } catch (error) {
      console.error("Error checking route:", error);
      return false; 
    }
  };

  return { routes, checkRouteExists };
});