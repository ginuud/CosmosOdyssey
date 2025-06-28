<template>
  <div class="page-wrap">
    <div class="form-contain">
      <slot />
    </div>
  </div>
</template>


<style>
@import url('https://fonts.googleapis.com/css2?family=Orbitron&display=swap');

.page-wrap {
  display: flex;
  justify-content: center;
}

.form-contain {
  width: 100%;
  max-width: 1200px;
}
</style>