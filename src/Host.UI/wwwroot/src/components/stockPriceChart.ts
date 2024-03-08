import StockPriceService from '../services/stockPriceService';

import { Chart, CategoryScale, LinearScale, BarElement, LineController, PointElement, LineElement } from 'chart.js';

// Register the components
Chart.register(CategoryScale, LinearScale, BarElement, LineController, PointElement, LineElement);

const template = document.createElement('template');
template.innerHTML = `
    <style>
        .chart-container {
            width: 100%; /* Adjust this as needed */
            height: 100%; /* Adjust this as needed, or use a specific height like 400px */
            position: relative; /* Important for responsiveness */
        }

        .chart-container canvas {
            width: 100% !important;
            height: 100% !important;
        }
    </style>
    <div class="chart-container">
        <canvas id="myChart"></canvas>
    </div>
`;

class StockPriceChart extends HTMLElement {
    private shadow: ShadowRoot;
    private chart: Chart;

    constructor() {
        super();

        this.shadow = this.attachShadow({ mode: 'open' });

        const templateClone = template.content.cloneNode(true);
        this.shadow.appendChild(templateClone);

        const canvas = this.shadowRoot?.getElementById('myChart') as HTMLCanvasElement;
        if (!canvas) throw new Error('Canvas not found');

        const ctx = canvas.getContext('2d');
        if (!ctx) throw new Error('Context not found');

        this.chart = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: []
            },
            options: {
                scales: {
                    x: {
                        type: 'linear',
                        position: 'bottom',
                    },
                    y: {
                        beginAtZero: true
                    }   
                },
                plugins: {
                    legend: {
                        display: true,
                        labels: {
                            color: 'rgb(255, 99, 132)',
                            font: {
                                size: 16
                            },
                            boxWidth: 20,
                            padding: 20 
                        },
                        title: {
                            display: true,
                            text: 'Stock Price'
                        }
                    }
                },
                responsive: true,
                maintainAspectRatio: false,
                animation: {
                    duration: 0
                }
            }
        });

        this.refresh();

        setInterval(() => this.refresh(), 1000);
    }

    private async refresh() {
        const priceHistory = await StockPriceService.getStockPriceHistory();

        priceHistory.stockPrices.forEach((stockPrice, i) => {
            if (i >= this.chart.data.datasets.length) {
                this.chart.data.datasets.push({
                    label: stockPrice.ticker,
                    data: stockPrice.prices.map(entry => {
                        return {
                            x: entry.timestep,
                            y: entry.price
                        };
                    }),
                    borderColor: 'rgb(255, 99, 132)',
                    borderWidth: 1,
                    fill: false
                });
            }

            this.chart.data.datasets[i].data = stockPrice.prices.map(entry => {
                return {
                    x: entry.timestep,
                    y: entry.price
                };
            });
            
            this.chart.data.datasets[0].label = stockPrice.ticker;
        });

        this.chart.update();
    }
}

customElements.define('stock-price-chart', StockPriceChart);