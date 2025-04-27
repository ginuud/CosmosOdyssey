<template>
  <div class="page-wrapper">
    <div class="text-section">
      <h1 class="title">Start your journey!</h1>
  </div>

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
  background-position: center;
  background-repeat: no-repeat;
  display: flex; 
  flex-direction: column; 
  justify-content: center;
  align-items: center;
}

.form-container {
  background-color: transparent; 
  padding: 2rem;
  border-radius: 1rem;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
}

.text-section {
display: flex;
flex-direction: column;
justify-content: center;
align-items: flex-start;
}

.title {
font-size: 4rem;
font-weight: 500;
color: #2d8fd0;
margin: 0;
font-family: "Orbitron", sans-serif;
font-optical-sizing: auto;
font-style: normal;
}


</style>