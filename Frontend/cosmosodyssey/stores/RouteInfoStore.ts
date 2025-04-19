import { defineStore } from "pinia";
import { ref } from "vue";

export const useRouteInfoStore = defineStore("routeInfoStore", () => {
  const routes = ref([]);

  const checkRouteExists = async (origin: string, destination: string): Promise<boolean> => {
    try {
      const response = await fetch(`/api/RouteInfos/getRoutes?from=${origin}&to=${destination}`);
      const data = await response.json();
      routes.value = data; // Optionally store the routes if needed
      return data.length > 0; 
    } catch (error) {
      console.error("Error checking route:", error);
      return false; 
    }
  };

  return { routes, checkRouteExists };
});