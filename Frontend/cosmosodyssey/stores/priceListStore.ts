import { defineStore } from 'pinia';
import { ref } from 'vue';
import type { PriceList } from '~/types/priceList';

export const usePriceListStore = defineStore('priceList', () => {
  const priceLists = ref<PriceList[]>([]);

  const loadPriceLists = async () => {
    try {
      const response = await fetch('/api/PriceLists');
      const data = await response.json();
      priceLists.value = data.map((item: any) => ({
        ...item,
        validUntil: new Date(item.validUntil), // Convert validUntil to a Date object
      }));
    } catch (error) {
      console.error('Error loading price lists:', error);
    }
  };

  return { priceLists, loadPriceLists };
});
