import { defineStore } from "pinia";
import { ref } from "vue";

export const useRouteInfoStore = defineStore("routeInfoStore", () => {
  const routeInfos = ref<RouteInfo[]>([]);
  const config = useRuntimeConfig();

  const getRouteInfos = async () => {
    try {
      const data = await $fetch(`${config.public.apiBase}RouteInfos`, {
        method: 'GET',
        headers: {
          'Content-Type': 'application/json'
        }
        });
        if (!data || !Array.isArray(data)) {
        routeInfos.value = [];
        return false;
      }

      routeInfos.value = data;
      return data.length > 0;
    } catch (error) {
      console.error("Error checking route:", error);
      routeInfos.value = [];
      return false;
    }
  };

  return { routeInfos, getRouteInfos };
});

 