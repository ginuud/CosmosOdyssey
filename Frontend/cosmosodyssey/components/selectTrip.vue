<script setup lang="ts">
import { ref} from 'vue';
import type { FormError, FormSubmitEvent } from "#ui/types";
import { usePlanetStore } from "@/stores/planetStore";
import { computed, reactive } from "vue";
import { useRouteInfoStore } from "~/stores/RouteInfoStore"
import { useRouter } from "vue-router";
const router = useRouter();
const routeInfoStore = useRouteInfoStore();
const planetStore = usePlanetStore();


const selectTripForm = reactive({
    origin: "",
    destination: "",
});

function resetForm() {
    selectTripForm.origin = "";
    selectTripForm.destination = "";
}

const validateSelection = (state: Partial<typeof selectTripForm>): FormError[] => {
    const errors: FormError[] = [];
    if (state.origin && state.destination) {
        if (state.origin === state.destination) {
            errors.push({ path: "origin", message: "Origin and destination cannot be the same" });
        }
    }
    return errors;
};

async function onSubmit(event: FormSubmitEvent<typeof selectTripForm>) { 
    event.preventDefault();
    const errors = validateSelection(selectTripForm);
    if (errors.length) {
        console.log("Validation errors:", errors);
        return;
    }
    const routeExists = await routeInfoStore.checkRouteExists(selectTripForm.origin, selectTripForm.destination);
  if (routeExists) {
    resetForm();
    router.push("/reservations"); 
  } else {
    alert("No route exists for the selected origin and destination."); 
  }
}

const planetOptions = computed(() =>{
const uniquePlanets = new Map();
    planetStore.planets.forEach((planet) => {
        if (!uniquePlanets.has(planet.name)) {
            uniquePlanets.set(planet.name, planet);
        }
    });
    return Array.from(uniquePlanets.values()).map((planet) => ({
        label: planet.name,
        value: planet,
    }));
});



</script>

<template>
  <UForm :validate="validateSelection" :state="selectTripForm" class="space-y-4" @submit="onSubmit">
    <UFormField label="Origin" name="origin">
      <USelectMenu v-model="selectTripForm.origin" 
      :options = "planetOptions"
      searchable
      searchable-placeholder="Search origin..."
      placeholder="Select origin"/>
    </UFormField>

    <UFormField label="Destination" name="destination">
      <USelectMenu v-model="selectTripForm.destination" 
      :options = "planetOptions"
      searchable
      searchable-placeholder="Search destination..."
      placeholder="Select destination"/>
    </UFormField>

    <UButton type="submit">
      Submit
    </UButton>
  </UForm>
</template>













<!-- <template>
    <div class="form-container">
        
    </div>
</template>


<style scoped>
.form-container {
  background-color: #433ea6; 
  border-radius: 1rem; /* Ümarad nurgad */
  padding: 2rem; 
  box-shadow: 0 4px 12px #9f57e2; /* Kerge vari  SIIIIAAAAA*/
  width: 100%;
  max-width: 300px; /* Maksimaalne laius */
  display: flex;
  flex-direction: column;
  align-items: center;
}
</style> -->