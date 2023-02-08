<template>
    <div>
        <div>
            <input type="date" v-model:value="date" required />
            <button type="button" v-on:click="getConsumption">Submit</button>
        </div>
        <div v-if="showLineChart" id="chartId">
            <linechart :chart-data-prop=dataLineChart></linechart>
        </div>
    </div>
</template>

<script>
    import linechart from './linechart.vue';
    export default ({
        components: {
            linechart
        },
        props: {
            mdevice: Object,
        },
        data() {
            return {
                dataLineChart: {
                    labels: [],
                    datasets: [{
                        label: 'kW/h',
                        data: [],
                        backgroundColor: '#00FF7F',
                    }]
                },
                device: mdevice,
                date: "",
                showLineChart: false,
            }
        },
        methods: {
            getConsumption() {
                this.showLineChart = false;
                var my = this;
                $.ajax({
                    url: "/api/MeasurementApi/GraphicMeasurements?deviceId=" + this.device.id + "&timeStamp=" + this.date,
                    method: "GET",
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    success: function (data) {
                        var parsedDates = [];
                        var consumption = [];
                        data.forEach(d => {
                            parsedDates.push(new Date(d.timestamp).toTimeString().slice(0, 5));
                            consumption.push(d.consumption);
                        });
                        consumption.push(1);
                        my.dataLineChart.labels = parsedDates;
                        my.dataLineChart.datasets[0].data = consumption;
                        my.showLineChart = true;
                    }
                });
            }
        },
    })
</script>