import { SummarizeStockResponseModel } from '../generated/apiClient';
import StocksService from '../services/stocksService';

const template = document.createElement('template');
template.innerHTML = `
    <style>
        :host  {
            --primary: #007acc;
            --accent: #3e3e42;
            --light: #3e3e42;
            --background: #2d2d30;
            --dark: #252526;
            --darker: #1e1e1e;
            --text: #fff;
        }

        .stock-summary-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        .stock-summary-table th {
            background-color: var(--dark);
            color: var(--text);
            padding: 10px;
            text-align: left;
        }

        .stock-summary-table td {
            padding: 10px;
            text-align: left;
            border-bottom: 1px solid var(--light);
        }

        .stock-summary-table tr:hover {
            background-color: var(--darker);
            font-weight: bold;
            color: var(--text);
        }
    </style>
    <table class="stock-summary-table">
        <thead>
            <tr>
                <th>Symbol</th>
                <th>Price</th>
                <th>Change</th>
                <th>% Change</th>
            </tr>
        </thead>
        <tbody id="stock-summary-table-body">
        </tbody>
    </table>
`;

class StockSummaryTable extends HTMLElement {
    private shadow: ShadowRoot;
    private tableBody: HTMLElement;

    constructor() {
        super();

        this.shadow = this.attachShadow({ mode: 'open' });

        const table = template.content.cloneNode(true);
        this.shadow.appendChild(table);

        const tableBody = this.shadow.getElementById('stock-summary-table-body');
        if (!tableBody) throw new Error('Table body not found');

        this.tableBody = tableBody;

        this.refresh();
    }

    refresh() {
        StocksService.getStockSummaries().then(summarizeStocksModel => {
            this.tableBody.innerHTML = '';
            summarizeStocksModel.stocks.forEach(stock => {
                const row = this.getRow(stock);
                this.tableBody.appendChild(row);
            });
        }).catch(error => {
            console.error('Error getting stock summaries', error);
            this.tableBody.innerHTML = '';

            const row = document.createElement('tr');
            row.innerHTML = '<td colspan="4">Error getting stock summaries</td>';
            this.tableBody.appendChild(row);
        });
    }

    getRow(stock: SummarizeStockResponseModel) {
        const row = document.createElement('tr');

        row.addEventListener('click', () => {
            console.log('Stock clicked', stock);
        });
        const changeColor = 'green';
        row.innerHTML = `
            <td>${stock.ticker}</td>
            <td>$${stock.price.toFixed(2)}</td>
            <td style="color: ${changeColor};">${0}</td>
            <td style="color: ${changeColor};">${0}</td>
        `;
        
        return row;
    }
}

customElements.define('stock-summary-table', StockSummaryTable);