import React, { Component } from 'react';
import ScreenLoaderModal from './CommonUI/ScreenLoaderModal';
import { CommonGet, CommonPost } from './Utils/CommonFetch';
import { Select, Empty, Table } from 'antd';
import { openNotification } from './Utils/CommonUI';
import NotificationStatusTypes from './Enums/NotificationStatusTypes';
import StockTypes from './Enums/StockTypes';

export class HistoricalData extends Component {
    constructor(props) {
        super(props);
        this.state = {
            screenLoading: false,
            selectedSymbol: "",
            selectedExchangeCode: "",
            period: 1,
            tableColumns: [],
            tableData: []
        }
    }

    componentWillReceiveProps(props) {
        var currentSymbol = props.currentSymbol;
        if (currentSymbol !== null && currentSymbol !== undefined && this.state.selectedSymbol !== currentSymbol.code) {
            this.setState({
                selectedSymbol: currentSymbol.code,
                selectedExchangeCode: currentSymbol.exchangeCode,
            }, () => {
                this.getStockData();
            })
        } else if (currentSymbol === null) {
            this.setState({
                selectedSymbol: "",
                selectedExchangeCode: "",
                tableColumns: [],
                tableData: []
            })
        }
    }

    //componentDidMount() {
    //    //this.getExchangeSymbols(this.state.selectedExchangeCode);
    //    this.getExchangeCodes();
    //}

    //getExchangeCodes = () => {
    //    this.setState({
    //        exchangeCodes: [],
    //        screenLoading: true,
    //        symbols: [],
    //        selectedSymbol: "",
    //        selectedExchangeCode: "",
    //    })

    //    CommonGet(`/api/v1/exchange/exchange-codes`)
    //        .then(res => {
    //            if (res != null) {
    //                let options = [];
    //                for (var item of res) {
    //                    options.push({
    //                        value: item.code,
    //                        label: `${item.name}`
    //                    })
    //                }
    //                this.setState({
    //                    screenLoading: false,
    //                    exchangeCodes: options
    //                });

    //            }
    //            else {
    //                this.setState({
    //                    screenLoading: false,
    //                    exchangeCodes: []
    //                })
    //                openNotification(NotificationStatusTypes.Error, "", "Exchange codes retrieve error!");
    //            }
    //        }).catch(err => {
    //            console.error(err);
    //            this.setState({
    //                screenLoading: false,
    //                exchangeCodes: []
    //            })
    //            openNotification(NotificationStatusTypes.Error, "", "Exchange codes retrieve error!");
    //        })
    //}

    //getExchangeSymbols = (exchangeCode) => {
    //    this.setState({
    //        screenLoading: true,
    //        symbols: [],
    //        selectedSymbol: ""
    //    })
    //    CommonGet(`/api/v1/exchange/symbols/${exchangeCode}`, `StockType=${StockTypes.CommonStock}`)
    //        .then(res => {
    //            if (res != null) {
    //                let options = [];
    //                for (var item of res) {
    //                    options.push({
    //                        value: item.code,
    //                        label: `${item.name} (${item.code}.${exchangeCode.toUpperCase()})`
    //                    })
    //                }
    //                this.setState({
    //                    screenLoading: false,
    //                    symbols: options
    //                });

    //            }
    //            else {
    //                this.setState({
    //                    screenLoading: false,
    //                    symbols: []
    //                })
    //                openNotification(NotificationStatusTypes.Error, "", "Exchange symbols retrieve error!");
    //            }
    //        }).catch(err => {
    //            console.error(err);
    //            this.setState({
    //                screenLoading: false,
    //                symbols: []
    //            })
    //            openNotification(NotificationStatusTypes.Error, "", "Exchange symbols retrieve error!");
    //        })
    //}

    getStockData = () => {


        if (this.state.selectedExchangeCode == null || this.state.selectedExchangeCode == undefined || this.state.selectedExchangeCode == '') {
            openNotification(NotificationStatusTypes.Warning, "", "Exchange code not selected");
            return;
        }

        if (this.state.selectedSymbol == null || this.state.selectedSymbol == undefined || this.state.selectedSymbol == '') {
            openNotification(NotificationStatusTypes.Warning, "", "Exchange symbol not selected");
            return;
        }
        this.setState({
            tableColumns: [],
            tableData: [],
            screenLoading: true
        })

        var data = {
            Symbol: this.state.selectedSymbol,
            ExchangeCode: this.state.selectedExchangeCode,
            Period: this.state.period
        }
        CommonPost("/api/v1/stockdata", null, data)
            .then(res => {
                if (res != null) {
                    var dataColumns = res.columns;
                    const columns = [
                        {
                            title: '',
                            dataIndex: 'historicalData',
                            key: 'historicalData',
                            fixed: 'left',
                        }
                    ];
                    for (var column of dataColumns) {
                        columns.push({
                            title: column,
                            dataIndex: column,
                            key: column,
                            columnWidth: "150px"
                        })
                    }
                    var keys = Object.keys(res);
                    var rowKeys = res.rowKeys;

                    var data = [];
                    for (var index in keys) {
                        var key = keys[index];
                        if (key.toLowerCase() != "columns" && key.toLowerCase() != "rowkeys") {

                            data.push({ key: index, historicalData: rowKeys[key.toLowerCase()], ...res[key] })
                        }
                    }
                    for (var record of data) {
                        var keys = Object.keys(record);
                        for (var key of keys) {
                            if (key !== "key" && key !== "historicalData") {
                                record[key] = this.formatRecordToDiv(record[key]);
                            }
                        }
                    }

                    this.setState({
                        tableColumns: columns,
                        tableData: data
                    })
                    setTimeout(() => {
                        this.setState({
                            screenLoading: false
                        })
                    }, 1000)
                }
                else {
                    this.setState({
                        tableColumns: [],
                        tableData: [],
                        screenLoading: false
                    })
                    //openNotification(NotificationStatusTypes.Error, "", "Stock data retrieve error!");
                }
            }).catch(err => {
                this.setState({
                    screenLoading: false,
                    tableColumns: [],
                    tableData: []
                })
                //openNotification(NotificationStatusTypes.Error, "", "Stock data retrieve error!");
            })
    }

    formatRecordToDiv = (value) => {
        var data = value.replace("%", "")

        if (data > 0) {
            return (
                <span className="value-positive-text">
                    {value}
                </span>
            )
        } else if (data < 0) {
            return (
                <span className="value-negative-text">
                    {value}
                </span>
            )
        } else {
            data = value.replace("-", "");
            return (
                <span>
                    {data}
                </span>
            )
        }

    }

    //exchangeCodeOnChange = (value) => {
    //    this.setState({
    //        selectedExchangeCode: value,
    //        symbols: [],
    //        selectedSymbol: "",
    //        tableColumns: [],
    //        tableData: []
    //    }, () => {
    //        this.getExchangeSymbols(value);
    //    })
    //}

    //symbolOnChange = (value) =>{
    //    this.setState({
    //        selectedSymbol: value,
    //        tableColumns: [],
    //        tableData: []
    //    },() => {
    //        this.getStockData();
    //    })

    //}

    periodOnChange = (value) => {
        this.setState({
            period: value
        }, () => {
            this.getStockData();
        })
    }

    render() {
        var quarterlySelected = this.state.period === 1 ? "custom-radio-btn-active" : "custom-radio-btn-inactive";
        var yearlySelected = this.state.period !== 1 ? "custom-radio-btn-active" : "custom-radio-btn-inactive";

        var displayTable = this.state.tableData.length > 0 ? "display-block" : "display-none";
        var dsiplayEmpty = this.state.tableData.length == 0 ? "display-block" : "display-none";
        return (
            <ScreenLoaderModal loading={this.state.screenLoading}>
                <div className="historical-data-container">
                    <div className="d-flex justify-content-start custom-radio-btn-group p-3">
                        <div className={`custom-radio-btn flex-size-1 custom-radio-btn-left-side ${quarterlySelected}`}
                            onClick={() => this.periodOnChange(1)}>
                            <span>Quarterly</span>
                        </div>
                        <div className={`custom-radio-btn  flex-size-1 custom-radio-btn-right-side ${yearlySelected}`}
                            onClick={() => this.periodOnChange(2)}
                        >
                            <span>Yearly</span>
                        </div>
                    </div>
                    <Table
                        className={`stock-table-main-container ${displayTable} stock-table`}
                        dataSource={this.state.tableData}
                        columns={this.state.tableColumns}
                        scroll={{ x: true, y: "calc(100vh - 64px - 2em - 6em)" }}
                        pagination={false}
                    />
                    <div className={`${dsiplayEmpty}`}>
                        <Empty />
                    </div>
                </div>
            </ScreenLoaderModal>
        );
    }
}
