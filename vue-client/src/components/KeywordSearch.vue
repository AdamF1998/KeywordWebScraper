<template>
    <div class="post">
        <div class="search-wrapper">
            <form @submit.prevent="submitForm">
                <div v-if="modelstate.errors" class="row user-input">
                    <p style="background-color:red; border-radius:25px;">
                        <ul style="padding:0;">
                            <li style="list-style:none; text-align:center;" v-for="error in modelstate.errors">{{error.join(' ')}}</li>
                        </ul>
                    </p>
                </div>
                <div class="row user-input">
                    <div class="column">
                        <input type="text" id="url-textbox" class="input" placeholder="Url" v-model="url" required />
                    </div>
                    <div class="column">
                        <input type="text" id="keyword-textbox" class="input" placeholder="Keywords" v-model="keywords" required />
                    </div>
                    <div class="column">
                        <button type="submit" class="button-3">Search</button>
                    </div>
                </div>
                <div class="row result">
                    <div class="column">
                        <h5 class="secondary-text">Positions</h5>
                        <h4 class="results-text">{{response.resultPositions}}</h4>
                    </div>
                    <div class="column">
                        <h5 class="secondary-text">Occurences</h5>
                        <h4 v-if="state == 0" class="results-text">{{response.occurences}}</h4>
                        <h4 v-if="state == 1" class="results-text found-results-text">{{response.occurences}}</h4>
                        <h4 v-if="state == 2" class="results-text no-results-text">{{response.occurences}}</h4>
                    </div>
                </div>
            </form>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import axios, { AxiosError, AxiosResponse } from 'axios';

    enum resultState {
        default,
        found,
        none
    }

    type PostWebScrapeResponse = {
        occurences: number,
        resultPositions: string
    };

    interface Data {
        modelstate: {};
        url: string;
        keywords: string;
        response: PostWebScrapeResponse;
        state: number;
    }

    export default Vue.extend({
        data(): Data {
            return {
                modelstate: {},
                url: '',
                keywords: '',
                response: { occurences: 0, resultPositions: 'None' },
                state: resultState.default
            };
        },
        methods: {
            submitForm() {

                const request = { url: this.url, keywords: this.keywords };
                const headers = {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                };

                axios.post('KeywordWebScraper', request, { headers })
                    .then(response => {
                        this.modelstate = {};

                        this.response = response.data.response;

                        if (this.response.occurences > 0) {
                            this.state = resultState.found;
                        }
                        else {
                            this.state = resultState.none;
                        }

                        return;
                    })
                    .catch((reason: AxiosError<{ additionalInfo: string }>) => {
                        if (reason.response!.status === 400) {
                            // client received an error response (5xx, 4xx)
                            this.modelstate = reason.response.data;
                            console.log(reason.response.data);
                        }
                        else {
                            // anything else
                            alert(reason.message);
                        }
                    });
            }
        }
    });
</script>