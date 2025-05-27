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
  height: 100vh;
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
}

.form-contain {
  background-color: transparent;
  border-radius: 1rem;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
  width: 100%;
  max-width: 1200px;
  height: 100%;
}
</style>