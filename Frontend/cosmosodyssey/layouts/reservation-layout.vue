<template>
    <div class="page-wrapper">
        <div class="form-container">
            <slot />
      </div>
    </div>
  </template>
  
  
  <style>
  
  @import url('https://fonts.googleapis.com/css2?family=Orbitron&display=swap');
  
  .page-wrapper {
    height: 100vh;
    background-image: url('/assets/pink-image.jpeg'); 
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
    border-radius: 1rem;
    box-shadow: 0 8px 24px rgba(0, 0, 0, 0.2);
  }
   
  </style>