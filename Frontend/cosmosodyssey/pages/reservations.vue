<!-- <template>
    <div class="container">
      <h1 class="title"> Routes from {{ route.query.from }} to {{ route.query.to }}</h1>

      <div class="content-wrapper">
        <div v-if="routes.length === 0" class="loading">
            <div class="pulsar-loader"></div>
            <p>Loading...</p>
        </div>

        <div v-else-if="routes.length === 0" class="error">
            <p class="error-message"> Navigation failure - no routes found in this sector </p>
        </div>

        <div v-else class="table-container">
            <RouteTable :routes="routes" />
        </div>
      </div>
    </div>
  </template>

<script setup lang="ts">
import {useRouteInfoStore} from '~/stores/RouteInfoStore'

const routeInfoStore = useRouteInfoStore();
const route = useRoute();
const router = useRouter();

const origin = route.query.from as string
const destination = route.query.to as string

if (!route.query.from || !route.query.to) {
  router.push('/');
}

const routes = computed(() => routeInfoStore.routes);

onMounted(async () => {
  if (routes.value.length === 0) {
    const exists = await routeInfoStore.checkRouteExists(origin, destination);
    if (!exists) router.push('/')
  }
});


definePageMeta({
  layout: "reservation-layout",
});

</script>
  
  <style>
  .container {
    padding: 2rem;
    max-width: 1200px;
    margin: 0 auto;
    background: rgba(255, 255, 255, 0.1);
    backdrop-filter: blur(10px);
    border-radius: 20px;
    border: 1px solid rgba(255, 255, 255, 0.2);
    box-shadow: 0 0 30px rgba(255, 192, 203, 0.3);
    min-height: 80vh;
  }
  
  .title {
  font-size: 2.5rem;
  color: #ff69b4;
  text-align: center;
  font-family: 'Orbitron', sans-serif;
  text-shadow: 0 0 15px rgba(255, 105, 180, 0.5);
  position: sticky;
  top: 0;
  z-index: 3;
}

.content-wrapper {
  flex: 1;
  position: relative;
  overflow-y: auto; 
}

.table-container {
  max-height: 70vh; 
  overflow-y: auto;
  margin-top: 1rem;
}

.loading {
  height: 50vh;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 2rem;
}
  
.pulsar-loader {
width: 50px;
height: 50px;
border-radius: 50%;
background: radial-gradient(circle, #ff69b4 0%, #ff1493 100%);
animation: pulsate 1.5s ease-out infinite;
}

@keyframes pulsate {
0% { transform: scale(0.8); opacity: 0.8; }
50% { transform: scale(1.2); opacity: 1; }
100% { transform: scale(0.8); opacity: 0.8; }
}
</style> -->

<template>
  <Routes />
</template>

<script setup lang="ts">

definePageMeta({
  layout: "reservation-layout",
});
</script>