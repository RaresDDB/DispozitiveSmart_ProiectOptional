<script>
    import { Line } from 'vue-chartjs'
    export default {
        extends: Line,
        props: {
            chartDataProp: {
                labels: [],
                datasets: [{
                    label: 'Line Chart',
                    data: []
                }]
            },
        },
        data() {
            return {
                chartData: this.chartDataProp,
                options: {
                    legend: {
                        display: true
                    },
                    responsive: true,
                    maintainAspectRatio: false
                }
            }
        },
        mounted() {
            this.renderChart(this.chartData, this.options)
        }
    }
</script>