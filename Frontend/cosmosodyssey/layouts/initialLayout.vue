<template>
    <div class="page-wrapper">
      <div class="form-container">
        <slot />
      </div>
    </div>
  </template>
  

  
  <style scoped>
  .page-wrapper {
    height: 100vh;
    background-image: url('/assets/pexels-krisof-1252890.jpg'); /* Put your image path here */
    background-size: cover;
    background-position: center;
    background-repeat: no-repeat;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  .form-container {
    background-color: rgba(255, 255, 255, 0.9); /* Optional: for readability */
    padding: 2rem;
    border-radius: 1rem;
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
  }
  </style>
  