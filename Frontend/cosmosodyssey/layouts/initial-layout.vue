<template>
  <div class="page-wrapper">
    <h1 class="title">Start your journey!</h1>

    <div class="form-container">
      <slot />
    </div>
  </div>
</template>


<style>
@import url('https://fonts.googleapis.com/css2?family=Orbitron&display=swap');

.page-wrapper {
  height: 100vh;
  background-image: url('/assets/image.jpeg');
  background-size: cover;
  display: flex;
  flex-direction: column;
  align-items: center;
  padding-top: 110px;
}

.form-container {
  padding: 2rem;
}

.title {
  font-size: 4rem;
  font-weight: 500;
  color: #81c1ec;
  font-family: "Orbitron", sans-serif;
}
</style>