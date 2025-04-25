<script setup lang="ts">
import type { FormError, FormSubmitEvent } from "#ui/types";
import { usePlanetStore } from "@/stores/planetStore";
import { ref, computed, reactive } from "vue";
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

// const validateSelection = (state: Partial<typeof selectTripForm>): FormError[] => {
//     const errors: FormError[] = [];
//     if (state.origin && state.destination) {
//         if (state.origin === state.destination) {
//             errors.push({ path: "origin", message: "Origin and destination cannot be the same" });
//         }
//     }
//     return errors;
// };

const validateSelection = (state: Partial<typeof selectTripForm>): FormError[] => {
  const errors: FormError[] = [];
  if (state.origin && state.destination) {
    if (state.origin === state.destination) {
      errors.push({ path: "origin", message: "Origin and destination cannot be the same" });
    }
  }
  return errors;
};

// async function onSubmit(event: FormSubmitEvent<typeof selectTripForm>) { 
//     event.preventDefault();
//     const errors = validateSelection(selectTripForm);
//     if (errors.length) {
//         console.log("Validation errors:", errors);
//         return;
//     }
//     const routeExists = await routeInfoStore.checkRouteExists(selectTripForm.origin, selectTripForm.destination);
//   if (routeExists) {
//     resetForm();
//     router.push("/reservations"); 
//   } else {
//     alert("No route exists for the selected origin and destination."); 
//   }
// }

const onSubmit = async (event?: Event) => {
  if (event && "preventDefault" in event) {
    event.preventDefault();
  }
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
};

// const planetOptions = computed(() =>{
// const uniquePlanets = new Map();
//   planetStore.planets.forEach((planet) => {
//       if (!uniquePlanets.has(planet.name)) {
//           uniquePlanets.set(planet.name, planet);
//       }
//   });
//   return Array.from(uniquePlanets.values()).map((planet) => ({
//       label: planet.name,
//       value: planet,
//   }));
// });

const planetOptions = ref(['Earth', 'Mars', 'Venus', 'Jupiter', 'Saturn', 'Neptune', 'Pluto']);


const animate = () => {
  const planeElement = document.getElementById('plane');
  if (planeElement) {
    planeElement.className = 'animation';
  }
  const bgElement = document.getElementById('bg');
  if (bgElement) {
    bgElement.className = 'animation2';
  }
};
</script>

<template>
  <UForm 
  :validate="validateSelection" 
  :state="selectTripForm" 
  class="space-y-4 p-6 bg-white rounded-2xl" 
  @submit="onSubmit">
    <div class="fields-and-button-container">
      <div class="fields-container">
        <UFormField label="Origin" name="origin" class="field">
          <USelectMenu v-model="selectTripForm.origin" 
          :options = "planetOptions"
          searchable
          searchable-placeholder="Search origin..."
          placeholder="Select origin"
          class="custom-select"
          />
        </UFormField>

        <UFormField label="Destination" name="destination" class="field">
          <USelectMenu v-model="selectTripForm.destination"           
          :options = "planetOptions"
          searchable
          searchable-placeholder="Search destination..."
          placeholder="Select destination"
          class="custom-select"
          :ui-menu="{ style: 'background: rgba(255, 255, 255, 0.95); color: #1e3a8a;' }" />
        </UFormField>
      </div>

      <div class="button-container">
        <button class="btn btn-inside btn-boarder"  @click="onSubmit">
          <img src="https://i.cloudup.com/gBzAn-oW_S-2000x2000.png" width="35px" height="35px" id="plane" />
        </button>
        <div class="bg">
          <img src="https://i.cloudup.com/2ZAX3hVsBE-3000x3000.png" id="bg" width="32px" height="32px" style="opacity:0;" />
        </div>
        <div class="around around-boarder" @click="animate"></div>
      </div>
    </div>
  </UForm>
</template>


<style>
html, body {
  margin: 0;
  padding: 0;
  height: 100%; 
}

.custom-select {
  border-radius: 0.5rem;
  backdrop-filter: blur(8px);
  min-width: 18rem ; 
  min-height: 3rem ;
  font-size: 1.125rem;
}

/* Add these styles */
.custom-select :deep(.ui-select-menu-button) {
  display: flex !important;
  flex-direction: row-reverse !important;
  justify-content: space-between !important;
  align-items: center !important;
  gap: 0.5rem !important;
}

.custom-select :deep(.ui-trailing) {
  order: 2 !important;
  margin-left: auto !important;
}

.custom-select :deep(.ui-value) {
  order: 1 !important;
  flex-grow: 1 !important;
}

/* Icon rotation animation */
.custom-select :deep(.ui-trailing-icon) {
  transform: rotate(0deg) !important;
  transition: transform 0.2s ease !important;
}

.custom-select[open] :deep(.ui-trailing-icon) {
  transform: rotate(180deg) !important;
}

/* Force icon color */
.custom-select :deep(.ui-trailing-icon) {
  color: #a26626 !important;
}

.custom-select .ui-select-menu-options {
  background: #f7fafc ;
  border: 1px solid #cbd5e0 ;
}

.custom-select .ui-select-menu-option {
  color: #9eafcc;
}

.field {
  backdrop-filter: blur(8px);
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  border: 1px solid #9acef0; 
  border-radius: 0.5rem;
  padding: 0.5rem;
  font-family: sans-serif;
  color: #9acef0;
}

.field select:focus,
.field input:focus {
  outline: none;
  box-shadow: 0 0 0 2px #3b82f6; 
  color: #9acef0;
}

.button-container {
  position: relative;
  margin-top: 10px;
  margin-bottom: 4px;
}

.btn {
  width: 56px;
  height: 56px;
  color: inherit;
  background: none;
  border-radius: 50%;
  transition: all 0.3s ease;
  display: flex;
  align-items: center;
  justify-content: center;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
}

.bg {
  height: 32px;
  width: 32px;
  position: absolute;
  margin: auto;
}

.btn-boarder {
  border: 2px solid #9acef0;
  border-radius: 50%;
}

.animation {
  -webkit-transform: translate(100px, -100px);
  -moz-transform: translate(100px, -100px);
  -o-transform: translate(100px, -100px);
  -ms-transform: translate(100px, -100px);
  transform: translate(100px, -100px);
  -webkit-transition: all 0.3s linear;
  -moz-transition: all 0.3s linear;
  -o-transition: all 0.3s linear;
  transition: all 0.3s linear;
}

.animation2 {
  -webkit-animation: fadeIn 0.4s ease-out;
  -webkit-animation-iteration-count: 1;
  -webkit-animation-fill-mode: forwards;
  -webkit-animation-delay: 0.4s;
  -moz-animation: fadeIn 0.4s ease-out;
  -moz-animation-iteration-count: 1;
  -moz-animation-fill-mode: forwards;
  -moz-animation-delay: 0.4s;
  animation: fadeIn 0.4s ease-out;
  animation-iteration-count: 1;
  animation-fill-mode: forwards;
  animation-delay: 0.4s;
}

.around {
  width: 5px;
  height: 50px;
  border: none;
  color: inherit;
  background: none;
  cursor: pointer;
  display: inline-block;
  text-transform: uppercase;
  letter-spacing: 1px;
  font-weight: 300;
  outline: none;
  position: absolute;
  -webkit-transition: all 0.3s;
  -moz-transition: all 0.3s;
  transition: all 0.3s;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  margin: auto;
}

.around:after {
  content: '';
  position: absolute;
  z-index: -1;
}

.around-boarder {
  border-radius: 50%;
}

.fields-container {
  display: flex;
  gap: 1.5rem; 
}

.fields-and-button-container {
  display: flex;
  gap: 1.5rem;
  align-items: flex-end;
}
</style>