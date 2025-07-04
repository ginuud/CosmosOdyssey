<script setup lang="ts">
import type { FormError, FormSubmitEvent } from "#ui/types";
import { usePlanetStore } from "@/stores/planetStore";
import { useRouteStore } from "~/stores/RouteStore"
import { useRouter } from "vue-router";
import { UFormField } from "#components";
import { onMounted, ref, reactive } from "vue";

const router = useRouter();
const routeStore = useRouteStore();
const planetStore = usePlanetStore();


const selectTripForm = reactive({
  origin: "",
  destination: "",
});

const errors = reactive({
  origin: null as string | null,
  destination: null as string | null,
});

function resetForm() {
  selectTripForm.origin = "";
  selectTripForm.destination = "";
}

const validateSelection = (): FormError[] => {
  errors.origin = null;
  errors.destination = null;

  const validationErrors: FormError[] = [];

  if (!selectTripForm.origin) {
    errors.origin = "Origin is required.";
    validationErrors.push({ path: "origin", message: errors.origin });
  }

  if (!selectTripForm.destination) {
    errors.destination = "Destination is required.";
    validationErrors.push({ path: "destination", message: errors.destination });
  }

  if (!errors.origin && !errors.destination) {
    if (selectTripForm.origin === selectTripForm.destination) {
      errors.destination = "Origin and destination cannot be the same.";
      validationErrors.push({ path: "destination", message: errors.destination });
    }
  }

  return validationErrors;
};

const submitted = ref(false);

const onSubmit = async (event?: Event) => {
  submitted.value = true;

  const validationErrors = validateSelection();
  if (validationErrors.length > 0) {
    console.log("Validation errors:", validationErrors);
    return;
  }
  const selectedOrigin = selectTripForm.origin;
  const selectedDestination = selectTripForm.destination;

  const routeExists = await routeStore.checkRouteExists(selectedOrigin, selectedDestination);
  if (routeExists) {
    router.push({
      path: "/routes",
      query: {
        from: selectedOrigin,
        to: selectedDestination
      }
    });
    resetForm();
  }
  else { alert("No route exists for the selected origin and destination."); }
};

const planetOptions = ref<{ label: string; value: string }[]>([]);

onMounted(async () => {
  const names = await planetStore.getPlanetNames();
  console.log('Fetched planet names:', names);
  if (!Array.isArray(names)) {
    planetOptions.value = [];
    return;
  }
  planetOptions.value = names.map((name: string) => ({
    label: name,
    value: name,
  }));
});

const handleAnimatedSubmit = async (event?: Event) => {
  animate();
  setTimeout(() => {
    onSubmit(event);
  }, 400);
};

const animate = () => {
  const planeElement = document.getElementById('plane');
  if (planeElement) {
    planeElement.classList.add('animation');
    const bgElement = document.getElementById('bg');
    setTimeout(() => planeElement?.classList.remove('animation'), 400);
  }
  const bgElement = document.getElementById('bg');
  if (bgElement) {
    bgElement.className = 'animation2';
    setTimeout(() => bgElement.classList.remove('animation2'), 800);
  }
};
</script>

<template>
  <UForm :validate="validateSelection" :state="selectTripForm" @submit="onSubmit">
    <div class="fields-and-button-container">
      <div class="fields-container">
        <UFormField label="Origin" name="origin" class="field">
          <USelectMenu v-model="selectTripForm.origin" :items="planetOptions" value-key="value"
            placeholder="Select origin" class="custom-select" />
          <div v-if="submitted && errors.origin" class="error-message">{{ errors.origin }}</div>
        </UFormField>

        <UFormField label="Destination" name="destination" class="field">
          <USelectMenu v-model="selectTripForm.destination" highlight :items="planetOptions" value-key="value"
            placeholder="Select destination" class="custom-select" />
          <div v-if="submitted && errors.destination" class="error-message">{{ errors.destination }}</div>
        </UFormField>
      </div>

      <div class="button-container">
        <button type="button" class="btn btn-inside btn-boarder" @click="handleAnimatedSubmit">
          <img src="https://i.cloudup.com/gBzAn-oW_S-2000x2000.png" width="35px" height="35px" id="plane" />
        </button>
      </div>
    </div>
  </UForm>
</template>


<style>
@import url('https://fonts.googleapis.com/css2?family=Orbitron&display=swap');

html,
body {
  margin: 0;
  padding: 0;
  height: 100%;
}

.custom-select {
  border-radius: 0.5rem;
  font-size: 1.2rem;
  font-family: "Orbitron", sans-serif;
  color: #9acef0;
  padding: 0.5rem;
  padding-bottom: 1rem;
  width: 210px;
  cursor: pointer;
}

.field {
  backdrop-filter: blur(8px);
  display: flex;
  flex-direction: column;
  align-items: center;
  border: 1px solid #9acef0;
  border-radius: 0.5rem;
  padding: 0.5rem;
  color: #9acef0;
  font-size: 1.2rem;
  font-family: "Orbitron", sans-serif;
}

.field:hover {
  background-color: rgba(255, 255, 255, 0.1);
}

.p-1.isolate {
  padding: 0.5rem;
  font-family: "Orbitron", sans-serif;
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
  cursor: pointer;
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
  gap: 2rem;
  align-items: flex-end;
}

.error-message {
  color: red;
  font-size: 1rem;
  margin-top: 0.25rem;
}
</style>