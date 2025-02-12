<script setup>
  import ChatterMain from './components/ChatterMain.vue'
</script>

<template>
  <div class="container">
    <div class="left-pane">
      <ChatterMain />
    </div>
    <div class="right-pane">
      <!-- 右半边内容 -->111
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
    width: 100vw;
    gap: 0; /* 可选，添加间隙控制 */
  }

  .left-pane, .right-pane {
    flex: 1;
    overflow: auto;
  }

  /* 可选：给容器设置背景颜色，便于检查布局 */
  .left-pane {
    background-color: lightblue;
  }

  .right-pane {
    background-color: lightgreen;
  }
</style>
