import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { PriceList } from '~/types/priceList';

export const usePriceListStore = defineStore('priceList', () => {
  const priceLists = ref<PriceList[]>([]);
  const config = useRuntimeConfig();

    //vist võib ära võtta
  // const loadPriceLists = async () => {
  //   try {
  //     const response = await fetch('/api/PriceLists');
  //     const data = await response.json();
  //     priceLists.value = data.map((item: any) => ({ 
  //       ...item,
  //       validUntil: new Date(item.validUntil), // Convert validUntil to a Date object
  //     }));
  //   } catch (error) {
  //     console.error('Error loading price lists:', error);
  //   }
  // };

  const getLatestPriceList = async () => {
    try {
      const response = await fetch(`${config.public.apiBase}PriceLists/latest`);
      const data = await response.json();
      return {
        ...data,
        validUntil: new Date(data.validUntil), 
      };
    } catch (error) {
      console.error('Error loading latest price list:', error);
      return null;
    }
  }
  return { priceLists, getLatestPriceList};
});
