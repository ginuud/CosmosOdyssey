import { defineStore } from "pinia";
import { ref } from "vue";

export const useRouteInfoStore = defineStore("routeInfoStore", () => {
  const routes = ref<Route[]>([]);
  const config = useRuntimeConfig();

  const checkRouteExists = async (origin: string, destination: string): Promise<boolean> => {
    try {
      const response = await fetch(`${config.public.apiBase}RouteInfos/getRoutes/${origin}/${destination}`);
      if (!response.ok) {
        routes.value = []; 
        return false; 
      }
      const data = await response.json();
      routes.value = data; 
      return data.length > 0; 
    } catch (error) {
      console.error("Error checking route:", error);
      routes.value = [];
      return false; 
    }
  };

  return { routes, checkRouteExists };
});