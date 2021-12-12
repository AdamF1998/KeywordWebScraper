<template>
    <div class="app">
        <KeywordSearch msg="Welcome to Your Vue.js + TypeScript App" />
    </div>
</template>

<script lang="ts">
import Vue from 'vue'
import KeywordSearch from './components/KeywordSearch.vue'

@KeywordSearch
export default Vue.extend({
  name: 'App',
  components: {
    KeywordSearch

  }
});
</script>
