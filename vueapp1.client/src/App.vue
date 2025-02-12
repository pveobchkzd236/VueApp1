<script setup>
  import ChatterMain from './components/ChatterMain.vue'
</script>

<template>
  <div class="container">
    <div class="left-pane">
      <ChatterMain />
    </div>
    <div class="right-pane">
      222
    </div>
  </div>
</template>

<style scoped>
  html, body {
    height: 100%;
    margin: 0;
    padding: 0;
  }

  .container {
    display: flex;
    height: 100vh;
    width: 100%;
    gap: 0; /* 可选，添加间隙控制 */
  }

  .left-pane, .right-pane {
    flex: 1; /* 确保两个 div 各占 50% 宽度 */
    overflow: auto;
  }

</style>
