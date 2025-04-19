function handleHotUpdate(_router, _generateRoutes) {
}
const _routes = [
  {
    name: "index",
    path: "/",
    component: () => import("../pages/index.vue.mjs")
  },
  {
    name: "reservations",
    path: "/reservations",
    component: () => import("../pages/reservations.vue.mjs")
  },
  {
    name: "tripSelection",
    path: "/tripSelection",
    component: () => import("../pages/tripSelection.vue.mjs")
  }
];
export {
  _routes as default,
  handleHotUpdate
};
//# sourceMappingURL=virtual_nuxt_C__Users_Kiviselg_CosmosOdyssey_CosmosOdyssey_frontend_cosmosodyssey_.nuxt_routes.mjs.map
