import React, { Component } from 'react';
import ScreenLoaderModal from './CommonUI/ScreenLoaderModal';
import { CommonGet, CommonPost } from './Utils/CommonFetch';
import { Select, Radio, Table, Card, Col, Row, Button } from 'antd';
import { openNotification } from './Utils/CommonUI';
import NotificationStatusTypes from './Enums/NotificationStatusTypes';
import StockTypes from './Enums/StockTypes';
import ReactApexChart from 'react-apexcharts'
import  { createChart } from 'lightweight-charts';
import FrequencyTypes from './Enums/FrequencyTypes';
import ChartGridTypes from './Enums/ChartGridTypes';
import SymbolsPopupModal from './CommonUI/SymbolsPopupModal';


export class LiveData extends Component {
    constructor(props) {
        super(props);
        this.state = {
            screenLoading: false,
            selectedSymbol: [],
            selectedExchangeCode: [],
            seriesData: [],

            candlestickSeries: [],

            //liveChartContainer: null,
            //selectedInterval: 0,
            //differentIntervals: [],

            //isLiveChart: false,
            liveDataInterval: [],

            fetchingUpdateData: [false],

            selectedFrequencyType: [],
            chartDivs: [],

            viewDisplayLayoutType: props.viewDisplayLayoutType,

            symbolPopupModalIndex: null,
            symbolModalOpen: false,
            symbolsLoading: false,
            symbolsData: [],
            currentSymbolsPage: 1,
            searchKeyword: '',
            currentSymbol: null,

            isLiveData: []
        }
       
    }

    componentDidMount() {
        this.setState({
            chartDivs: ["livechart1"],
            selectedSymbol: ["Symbol"],
            selectedExchangeCode: [""],
            selectedFrequencyType: [FrequencyTypes.Daily],
            isLiveData: [false],
            seriesData: [[]],
            liveDataInterval: [null],
            candlestickSeriesList: [],
            fetchingUpdateData:[false]
        }, () => {
            this.configureCharts();
        });
    }

    componentWillReceiveProps(props) {
        if (this.state.viewDisplayLayoutType !== props.viewDisplayLayoutType) {
            this.setState({
                viewDisplayLayoutType: props.viewDisplayLayoutType
            }, () => {
                this.configureCharts();
            });
        }
    }

    configureCharts = () => {
        var chartDivsList = this.state.chartDivs;
        var selectedSymbolList = this.state.selectedSymbol;
        var selectedExchangeCodeList = this.state.selectedExchangeCode;
        var selectedFrequencyTypeList = this.state.selectedFrequencyType;
        var isLiveDataList = this.state.isLiveData;
        var seriesDataList = this.state.seriesData;
        var liveDataIntervalList = this.state.liveDataInterval;
        var candlestickSeriesList = this.state.candlestickSeries;
        var fetchingUpdateDataList = this.state.fetchingUpdateData;

        var newChartCount = 1;
        if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid1) {
            newChartCount = 1;
        }
        else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid2Horizontal || this.state.viewDisplayLayoutType === ChartGridTypes.Grid2Vertical) {
            newChartCount = 2;
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid4Horizontal || this.state.viewDisplayLayoutType === ChartGridTypes.Grid4Vertical) {
            newChartCount = 4;
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid6) {
            newChartCount = 6;
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid8) {
            newChartCount = 8;
        }
        var diff = newChartCount - chartDivsList.length;
        if (diff > 0) {
            var newChartNames = [];
            var startCount = 0;
            if (chartDivsList.length > 0)
                startCount = parseInt(chartDivsList[chartDivsList.length - 1].replace('livechart', ""));
            for (var index = 1; index <= diff; index++) {

                var chartName = 'livechart' + (startCount + index);

                newChartNames.push(chartName);

                chartDivsList.push(chartName);
                selectedSymbolList.push("Symbol");
                selectedExchangeCodeList.push("");
                selectedFrequencyTypeList.push(FrequencyTypes.Daily);
                isLiveDataList.push(false);
                seriesDataList.push([]);
                liveDataIntervalList.push(null);
                fetchingUpdateDataList.push(false);
            }

            this.setState({
                chartDivs: chartDivsList,
                candlestickSeries: candlestickSeriesList,
                selectedSymbol: selectedSymbolList,
                selectedExchangeCode: selectedExchangeCodeList,
                selectedFrequencyType: selectedFrequencyTypeList,
                isLiveData: isLiveDataList,
                seriesData: seriesDataList,
                liveDataInterval: liveDataIntervalList,
                fetchingUpdateData: fetchingUpdateDataList
            }, () => {
                this.configureChartDiv();
            });
        } else if (diff < 0) {
            var diff = Math.abs(diff);

            //chartDivsList.slice(0, -diff);
            //candlestickSeriesList.slice(0, -diff);
            //selectedSymbolList.slice(0, -diff);
            //selectedExchangeCodeList.slice(0, -diff);
            //selectedFrequencyTypeList.slice(0, -diff);
            //isLiveDataList.slice(0, -diff);
            //seriesDataList.slice(0, -diff);
            //liveDataIntervalList.slice(0, -diff);

            chartDivsList.splice(-diff);
            candlestickSeriesList.splice(-diff);
            selectedSymbolList.splice(-diff);
            selectedExchangeCodeList.splice(-diff);
            selectedFrequencyTypeList.splice(-diff);
            isLiveDataList.splice(-diff);
            seriesDataList.splice(-diff);
            liveDataIntervalList.splice(-diff);
            fetchingUpdateDataList.splice(-diff);

            this.setState({
                chartDivs: chartDivsList,
                candlestickSeries: candlestickSeriesList,
                selectedSymbol: selectedSymbolList,
                selectedExchangeCode: selectedExchangeCodeList,
                selectedFrequencyType: selectedFrequencyTypeList,
                isLiveData: isLiveDataList,
                seriesData: seriesDataList,
                liveDataInterval: liveDataIntervalList,
                fetchingUpdateData: fetchingUpdateDataList
            }, () => {
                this.configureChartDiv();
            });
        } else {
            this.configureChartDiv();
        }
        
    }

    configureChartDiv = () => {
        var chartNames = this.state.chartDivs;
        var chartsList = [];
        for (const chartName of chartNames) {
            const container = document.getElementById(chartName)
            if (container.children.length > 0) {
                container.removeChild(container.children[0]);
            }
            const chart = createChart(container, {
                autoSize: true,
                layout: {
                    background: {
                        color: '#070c15',
                    },
                    textColor: "#fff"
                },
                grid: {
                    vertLines: {
                        visible: true,
                        color: '#121728',
                    },
                    horzLines: {
                        visible: true,
                        color: '#121728',
                    },
                },
                crosshair: {
                    mode: 0,
                }
            });
            
            const candlestickChart = chart.addCandlestickSeries({
                upColor: '#26a69a', downColor: '#ef5350', borderVisible: true
            });

            chart.subscribeCrosshairMove(param => {
                this.updateChartValues(param, chartName, candlestickChart)
            });

            const timeScale = chart.timeScale();
            timeScale.subscribeVisibleLogicalRangeChange((logicalRange) => {
                if (this.state.screenLoading === false) {
                    this.updateHistoryData(logicalRange, chartName, candlestickChart);
                }
            });
            chartsList.push(candlestickChart);
        }

        //var currentChartsList = this.state.candlestickSeries;
        //currentChartsList.push(...chartsList);
        this.setState({
            candlestickSeries: chartsList
        }, () => {
            for (var index in chartNames) {
                this.getStockData(index);
            }
        });
    }

    updateChartValues = (param, chartName, candlestickChart) => {
        const data = param.seriesData.get(candlestickChart);
        if (data !== undefined && data !== null) {
            var container = document.getElementsByClassName(chartName);
            var dataContainer = container[0].getElementsByClassName("stock-chart-display-values")[0];
            if (data.open < data.close) {
                var displayData = `
                <span>O: <span class="value-positive-text">${data.open}</span> </span>
                <span> H: <span class="value-positive-text">${data.high}</span> </span>
                <span>L: <span class="value-positive-text">${data.low}</span> </span>
                <span>C: <span class="value-positive-text">${data.close}</span> </span>`;
                dataContainer.innerHTML = displayData;
            } else if (data.open > data.close){
                var displayData = `
                <span>O: <span class="value-negative-text">${data.open}</span> </span>
                <span> H: <span class="value-negative-text">${data.high}</span> </span>
                <span>L: <span class="value-negative-text">${data.low}</span> </span>
                <span>C: <span class="value-negative-text">${data.close}</span> </span>`;
                dataContainer.innerHTML = displayData;
            } else {
                var displayData = `
                <span>O: <span >${data.open}</span> </span>
                <span> H: <span >${data.high}</span> </span>
                <span>L: <span >${data.low}</span> </span>
                <span>C: <span >${data.close}</span> </span>`;
                dataContainer.innerHTML = displayData;
            }
        }
    }

    //componentDidMount() {



    //    //var chartDivs = this.state.chartDivs;
    //    //var chartsList = [];
    //    //for (var index in chartDivs) {
    //    //    var container = document.getElementById(chartName)
    //    //    const chart = createChart(container, {
    //    //        autoSize: true,
    //    //        layout: {
    //    //            background: {
    //    //                color: '#070c15',
    //    //            },
    //    //            textColor: "#fff"
    //    //        },
    //    //        grid: {
    //    //            vertLines: {
    //    //                visible: true,
    //    //                color: '#121728',
    //    //            },
    //    //            horzLines: {
    //    //                visible: true,
    //    //                color: '#121728',
    //    //            },
    //    //        },
    //    //        crosshair: {
    //    //            mode: 0,
    //    //        }
    //    //    });

    //    //    const candlestickChart = chart.addCandlestickSeries({
    //    //        upColor: '#26a69a', downColor: '#ef5350', borderVisible: true
    //    //    });;

    //    //    const toolTip = document.createElement('div');
    //    //    toolTip.style = `width: auto; height: auto; position: absolute; display: none; padding: 8px; 
    //    //box-sizing: border-box; font-size: 12px; text-align: left; z-index: 1000; top: 12px;
    //    //left: 12px; pointer-events: none; border: 1px solid; border-radius: 2px;font-family: -apple-system,
    //    //BlinkMacSystemFont, 'Trebuchet MS', Roboto, Ubuntu, sans-serif; -webkit-font-smoothing: antialiased;
    //    //-moz-osx-font-smoothing: grayscale;`;
    //    //    toolTip.style.background = 'white';
    //    //    toolTip.style.color = 'black';
    //    //    toolTip.style.borderColor = '#F28E33';
    //    //    toolTip.style.borderRadius = '10px';
    //    //    toolTip.style.boxShadow = '2px 2px 2px #F28E33';
    //    //    container.appendChild(toolTip);

    //    //    chart.subscribeCrosshairMove(param => {
    //    //        this.chartTooltip(param, container, toolTip, candlestickChart)
    //    //    });

    //    //    var timeScale = chart.timeScale();
    //    //    timeScale.subscribeVisibleLogicalRangeChange((logicalRange) => {
    //    //        this.updateHistoryData(logicalRange, candlestickChart);
    //    //    });

    //    //    chartsList.push(candlestickChart);
            
            
    //    //    //this.getExchangeSymbols(this.state.selectedExchangeCode);
            
            
    //    //}
    //    //console.log(chartsList)
    //    //this.setState({
    //    //    candlestickSeries: chartsList
    //    //}, () => {
            
    //    //    for (var index in chartDivs) {
    //    //        this.getStockData(index);
    //    //    }
    //    //});
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

    updateHistoryData = (logicalRange, chartName, candlestickSeries) => {
        var chartIndex = this.state.chartDivs.findIndex(x => x == chartName);
        
        var selectedSymbol = this.state.selectedSymbol[chartIndex];
        var selectedExchangeCode = this.state.selectedExchangeCode[chartIndex];
        var selectedFrequencyType = this.state.selectedFrequencyType[chartIndex];
        var candlestickSeries = this.state.candlestickSeries[chartIndex];


        var fetchingUpdateDataList = this.state.fetchingUpdateData;
        var symbolSeriesDataList = this.state.seriesData;

        var symbolSeriesData = this.state.seriesData[chartIndex];

        if (logicalRange !== null) {
            var barsInfo = candlestickSeries.barsInLogicalRange(logicalRange);
            if (barsInfo !== null && barsInfo.barsBefore < 50) {
                var firstTimeStamp = symbolSeriesData[0];
                if (fetchingUpdateDataList[chartIndex] == false && firstTimeStamp != undefined && firstTimeStamp != null) {
                    fetchingUpdateDataList[chartIndex] = true;
                    this.setState({
                        fetchingUpdateData: fetchingUpdateDataList
                    })
                    var data = {
                        Symbol: selectedSymbol,
                        ExchangeCode: selectedExchangeCode,
                        LastTimeStamp: firstTimeStamp.time,
                        FrequencyType: selectedFrequencyType
                    }
                    CommonPost("/api/v1/stockdata/history", null, data)
                        .then(res => {
                            if (res != null) {
                                var seriesData = [];
                                for (var data of res) {
                                    seriesData.push({ time: data.timeStamp, open: data.data[0], high: data.data[1], low: data.data[2], close: data.data[3] })
                                }
                                seriesData.push(...symbolSeriesData);
                                candlestickSeries.setData(seriesData);
                                symbolSeriesDataList[chartIndex] = seriesData;
                                fetchingUpdateDataList[chartIndex] = false;
                                this.setState({
                                    symbolSeriesData: symbolSeriesDataList,
                                    fetchingUpdateData: fetchingUpdateDataList
                                })


                            }
                            else {
                                fetchingUpdateDataList[chartIndex] = false;
                                this.setState({
                                    fetchingUpdateData: fetchingUpdateDataList
                                })
                                //openNotification(NotificationStatusTypes.Error, "", "Stock history data retrieve error!");
                            }
                        }).catch(err => {
                            console.log(err)
                            fetchingUpdateDataList[chartIndex] = false;
                            this.setState({
                                fetchingUpdateData: fetchingUpdateDataList
                            })
                            //openNotification(NotificationStatusTypes.Error, "", "Stock history data retrieve error!");
                        })
                }
            }
        }
    }

    //chartTooltip = (param, container, toolTip, candlestickSeries) => {
    //    if (
    //        param.point === undefined ||
    //        !param.time ||
    //        param.point.x < 0 ||
    //        param.point.x > container.clientWidth ||
    //        param.point.y < 0 ||
    //        param.point.y > container.clientHeight
    //    ) {
    //        toolTip.style.display = 'none';
    //    } else {

    //        const date = new Date(parseFloat(param.time + "000"));

    //        //var year = date.getUTCFullYear();
    //        //var month = date.getUTCMonth();
    //        //var day = date.getUTCDate();

    //        //if (month == 0)
    //        //    month = "12"
    //        //else if (month < 10)
    //        //    month = "0" + month
    //        //if (day < 10)
    //        //    day = "0" + day


    //        //const year = date.getFullYear();
    //        //const month = date.getMonth() + 1;
    //        //const day = date.getDate();

    //        var year = date.getUTCFullYear();
    //        var month = date.getUTCMonth() + 1;
    //        var day = date.getUTCDate();

    //        if (month < 10)
    //            month = "0" + month
    //        if (day < 10)
    //            day = "0" + day

    //        var dateStr = year + "-" + month + "-" + day;

    //        toolTip.style.display = 'block';
    //        const data = param.seriesData.get(candlestickSeries);
    //        toolTip.innerHTML = `<div style="">${this.state.selectedSymbol}.${this.state.selectedExchangeCode}</div><div style="font-size: 12px; margin: 4px 0px; width: 100%; color: ${'black'}">
			 //                        <span>Open : ${data.open}</span><br/>
    //                                 <span>High : ${data.high}</span><br/>
    //                                 <span>Low : ${data.low}</span><br/>
    //                                 <span>Close : ${data.close}</span><br/>
			 //                       </div>
    //                                <div style="color: ${'black'}">
		  //                          ${dateStr}
    //                       </div>`;
    //        //toolTip.innerHTML = `<div>Test</div>`;
    //        toolTip.style.left = param.point.x - toolTip.clientWidth / 2 + 'px';
    //        toolTip.style.top = param.point.y - (toolTip.clientHeight + 10) + 'px';
    //    }
    //}

    //getExchangeSymbols = (exchangeCode) => {
    //    this.setState({
    //        screenLoading: true,
    //        symbols: [],
    //        selectedSymbol: ""
    //    })
    //    CommonGet(`/api/v1/exchange/symbols/${exchangeCode}`, null)
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

    getStockData = (chartIndex) => {
        this.setState({
            screenLoading: true
        })
        var chart = this.state.candlestickSeries[chartIndex];
        var selectedSymbol = this.state.selectedSymbol[chartIndex];
        var selectedExchangeCode = this.state.selectedExchangeCode[chartIndex];
        var selectedFrequencyType = this.state.selectedFrequencyType[chartIndex];
        var isLiveData = this.state.isLiveData[chartIndex];
        var seriesDataList = this.state.seriesData;

        if (selectedSymbol === "" || selectedExchangeCode === "") {
            this.setState({
                screenLoading: false
            })
            return;
        }
        var data = {
            Symbol: selectedSymbol,
            ExchangeCode: selectedExchangeCode,
            FrequencyType: selectedFrequencyType
        }
        CommonPost("/api/v1/stockdata/history", null, data)
            .then(res => {
                if (res != null) {
                    var seriesData = [];
                    for (var data of res) {
                        seriesData.push({ time: data.timeStamp, open: data.data[0], high: data.data[1], low: data.data[2], close: data.data[3] })
                    }
                    chart.setData(seriesData);

                    //if (this.state.isLiveData) {
                    //    this.getLiveCurrentData();
                    //    var liveDataInterval = this.state.liveDataInterval;
                    //    if (liveDataInterval != null) {
                    //        clearInterval(liveDataInterval);
                    //    }
                    //    liveDataInterval = setInterval(() => {
                    //        this.getLiveCurrentData();
                    //    }, 1000 * 5)
                    //    this.setState({
                    //        liveDataInterval
                    //    })
                    //}
                    //else {
                    //    var liveDataInterval = this.state.liveDataInterval;
                    //    if (liveDataInterval != null) {
                    //        clearInterval(liveDataInterval);
                    //        this.setState({
                    //            liveDataInterval: null
                    //        })
                    //    }
                    //}
                    seriesDataList[chartIndex] = seriesData;
                    this.setState({
                        seriesData: seriesDataList,
                        screenLoading: false
                    }, () => {
                        if(isLiveData)
                            this.getLiveCurrentData(chartIndex);
                    })

                    
                }
                else {
                    this.setState({
                        screenLoading: false
                    })
                    //openNotification(NotificationStatusTypes.Error, "", "Stock history data retrieve error!");
                }
            }).catch(err => {
                console.log(err)
                this.setState({
                    screenLoading: false
                })
                //openNotification(NotificationStatusTypes.Error, "", "Stock history data retrieve error!");
            })
    }

    getLiveCurrentData = (chartIndex) => {
        var seriesDataList = this.state.seriesData;
        var liveDataIntervalList = this.state.liveDataInterval;

        var selectedSymbol = this.state.selectedSymbol[chartIndex];
        var selectedExchangeCode = this.state.selectedExchangeCode[chartIndex];
        var symbolSeriesData = this.state.seriesData[chartIndex];
        var selectedFrequencyType = this.state.selectedFrequencyType[chartIndex];
        var candlestickSeries = this.state.candlestickSeries[chartIndex];
        var liveDataInterval = this.state.liveDataInterval[chartIndex];
        var isLiveData = this.state.isLiveData[chartIndex];

        var data = {
            Symbol: selectedSymbol,
            ExchangeCode: selectedExchangeCode
        }
        CommonPost("/api/v1/stockdata/live", null, data)
            .then(res => {
                if (res != null) {
                    var seriesData = [];
                    seriesData.push(...symbolSeriesData);
                    if (seriesData[seriesData.length - 1].time !== res.timeStamp) {

                        var replaceLast = null;

                        if (selectedFrequencyType === FrequencyTypes.Weekly || selectedFrequencyType === FrequencyTypes.Monthly) {
                            var lastRecord = seriesData[seriesData.length - 1];
                            var diff = (res.timeStamp + "000") - (lastRecord.time + "000");
                            var diffDays = diff / (1000 * 60 * 60 * 24);

                            if (
                                (selectedFrequencyType === FrequencyTypes.Weekly && diffDays < 7) ||
                                (selectedFrequencyType === FrequencyTypes.Monthly && diffDays < 31)
                            ) {
                                var open = lastRecord.open;
                                var low = res.data[2] < lastRecord.low ? res.data[2] : lastRecord.low;
                                var high = res.data[1] > lastRecord.high ? res.data[1] : lastRecord.high;
                                var close = res.data[3] > lastRecord.close ? res.data[3] : lastRecord.close;
                                replaceLast = { time: lastRecord.time, open, high, low, close };
                            }
                        }

                        if (replaceLast == null) {
                            seriesData.push({ time: res.timeStamp, open: res.data[0], high: res.data[1], low: res.data[2], close: res.data[3] });
                            candlestickSeries.setData(seriesData);
                        } else {
                            seriesData[seriesData.length - 1] = replaceLast;
                            candlestickSeries.setData(seriesData);
                            seriesDataList[chartIndex] = seriesData;
                            this.setState({
                                seriesData: seriesDataList
                            })
                        }
                    }

                    var diff = new Date().getTime() - (res.timeStamp + "000");
                    var diffInMin = diff / (1000);
                    var minimumCheckingDuration = 1000 * 60;
                    if (diffInMin < 60)
                        minimumCheckingDuration = 1000 * diffInMin;

                    if (liveDataInterval != null) {
                        clearInterval(liveDataInterval);
                    }

                    if (isLiveData) {
                        liveDataInterval = setInterval(() => {
                            this.getLiveCurrentData();
                        }, minimumCheckingDuration)
                    }
                    liveDataIntervalList[chartIndex] = liveDataInterval;
                    this.setState({
                        liveDataInterval: liveDataIntervalList
                    })


                }
                else {
                    //openNotification(NotificationStatusTypes.Error, "", "Stock live data retrieve error!");
                }
            }).catch(err => {
                console.log(err)
                //openNotification(NotificationStatusTypes.Error, "", "Stock live data retrieve error!");
            })
    }

    //exchangeCodeOnChange = (value) => {
    //    this.setState({
    //        selectedExchangeCode: value,
    //        symbols: [],
    //        selectedSymbol: ""
    //    }, () => {
    //        this.getExchangeSymbols(value);
    //    })
    //}

    //symbolOnChange = (value) => {
    //    this.setState({
    //        selectedSymbol: value
    //    }, () => {
    //        this.getStockData();
    //    })

    //}

    setIsLiveData = (value, index) => {
        var liveDataInterval = this.state.liveDataInterval;
        var isLiveData = this.state.isLiveData;

        if (liveDataInterval != null) {
            clearInterval(liveDataInterval);
        }

        isLiveData[index] = value;
        liveDataInterval[index] = null;
        this.setState({
            isLiveData,
            liveDataInterval
        }, () => {
            this.getStockData(index);
        })
    }

    frequencyOnChange = (value, index) => {
        let selectedFrequencyTypeList = this.state.selectedFrequencyType;
        selectedFrequencyTypeList[index] = value;
        this.setState({
            selectedFrequencyType: selectedFrequencyTypeList
        }, () => {
            this.getStockData(index);
        })
    }

    componentWillUnmount() {
        var liveDataInterval = this.state.liveDataInterval;
        for (var liveDataInterval of this.state.liveDataInterval) {
            if (liveDataInterval !== null) {
                clearInterval(liveDataInterval);
            }
        }
    }



    toggleSymbolModal = (status, index) => {
        this.setState({
            symbolModalOpen: status,
            currentSymbolsPage: 1,
            symbolsData: [],
            searchKeyword: '',
            symbolPopupModalIndex: index
        }, () => {
            if (status) {
                this.getSymbolsData();
            }
        });
    }

    getSymbolsData = () => {
        this.setState({
            symbolsLoading: true
        })
        CommonGet("api/v1/exchange/all-symbols", `currentSymbolsPage=${this.state.currentSymbolsPage}&&searchKeyword=${this.state.searchKeyword}`)
            .then(res => {
                var data = this.state.symbolsData;
                data.push(...res);
                this.setState({
                    symbolsData: data,
                    symbolsLoading: false
                })
            }).catch(err => {
                console.error(err);
                this.setState({
                    symbolsLoading: false
                })
            })
    }

    getNextSymbolsData = () => {
        this.setState({
            currentSymbolsPage: this.state.currentSymbolsPage + 1
        }, () => {
            this.getSymbolsData();
        });
    }

    searchKeyword = () => {
        this.setState({
            symbolsData: [],
            currentSymbolsPage: 1
        }, () => {
            this.getSymbolsData();
        });
    }

    searchKeywordOnClick = () => {
        this.setState({
            symbolsData: [],
            currentSymbolsPage: 1
        }, () => {
            this.getSymbolsData();
        });
    }

    onChangeSelectedSymbol = (symbol) => {
        var index = this.state.symbolPopupModalIndex;
        var selectedSymbols = this.state.selectedSymbol;
        var selectedExchangeCode = this.state.selectedExchangeCode;

        selectedSymbols[index] = symbol.code;
        selectedExchangeCode[index] = symbol.exchangeCode;
        this.setState({
            selectedSymbols,
            selectedExchangeCode,
            symbolModalOpen: false,
            symbolPopupModalIndex: null
        }, () => {
            this.getStockData(index);
        });
    }

    onSeachKeyWordChange = (e) => {
        this.setState({
            searchKeyword: e.target.value
        })
    }

    render() {
        var width = "100%";
        var height = "50vh";
        var reduceHeight = "7em"
        if (this.state.viewDisplayLayoutType !== null) {
            if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid1 || this.state.viewDisplayLayoutType === ChartGridTypes.Grid2Horizontal) {
                height = '100vh';
                reduceHeight = "11em";
            } 
        }
        if (this.state.viewDisplayLayoutType !== null) {
            if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid2Horizontal || this.state.viewDisplayLayoutType === ChartGridTypes.Grid4Horizontal) {
                width = "50%";
            } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid6) {
                width = `${100 / 3}%`;
            } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid8) {
                width = `${100 / 4}%`;
            }
        }

        let lineBreak = false;
        if (parseInt(width.replace("%","")) < 50) {
            lineBreak = true;
        }
        
        return (
            <ScreenLoaderModal loading={this.state.screenLoading}>
                    <Row >
                        {
                        this.state.chartDivs.map((divName, index) => {
                                var symbol = this.state.selectedSymbol[index];
                            var historicalDataSelected = !this.state.isLiveData[index] ? "custom-radio-btn-active" : "custom-radio-btn-inactive";
                            var liveDataSelected = this.state.isLiveData[index] ? "custom-radio-btn-active" : "custom-radio-btn-inactive";

                            var frequencyDaySelected = this.state.selectedFrequencyType[index] == FrequencyTypes.Daily ? "custom-radio-btn-active" : "custom-radio-btn-inactive";
                            var frequencyWeekSelected = this.state.selectedFrequencyType[index] == FrequencyTypes.Weekly ? "custom-radio-btn-active" : "custom-radio-btn-inactive";
                            var frequencyMonthlySelected = this.state.selectedFrequencyType[index] == FrequencyTypes.Monthly ? "custom-radio-btn-active" : "custom-radio-btn-inactive";

                                return (
                                    <Col flex={`${width}`} key={index} className={divName}>
                                        <div className="row single-chart-top-nav">
                                            <div className="col-md-9 col-sm-12 col-xs-12">
                                                <span >
                                                    <span >
                                                        <Button className="btn-custom m-0 btn-square" onClick={() => this.toggleSymbolModal(true, index)}>{symbol}</Button>
                                                    </span>
                                                    <span style={{ alignSelf: lineBreak? 'center':'' }}>
                                                        <span className="">
                                                            <span className={`custom-radio-btn-2 ${frequencyDaySelected}`}
                                                                onClick={() => this.frequencyOnChange(FrequencyTypes.Daily, index)}>
                                                                <span>1D</span>
                                                            </span>
                                                        </span>
                                                        <span>
                                                            <span className={`custom-radio-btn-2 ${frequencyWeekSelected}`}
                                                                onClick={() => this.frequencyOnChange(FrequencyTypes.Weekly, index)}>
                                                                <span>1W</span>
                                                            </span>
                                                        </span>
                                                        <span>
                                                            <span className={`custom-radio-btn-2 ${frequencyMonthlySelected}`}
                                                                onClick={() => this.frequencyOnChange(FrequencyTypes.Monthly, index)}>
                                                                <span>1M</span>
                                                            </span>
                                                        </span>
                                                    </span>
                                                </span>
                                            </div>
                                            <div className='col-md-3 d-flex justify-content-end custom-radio-btn-group'>
                                                <div className={`custom-radio-btn flex-size-1 custom-radio-btn-left-side ${historicalDataSelected}`}
                                                    onClick={() => this.setIsLiveData(false, index)}>
                                                    <span>Historical</span>
                                                </div>
                                                <div className={`custom-radio-btn  flex-size-1 custom-radio-btn-right-side ${liveDataSelected}`}
                                                    onClick={() => this.setIsLiveData(true, index)}>

                                                    <span>Live</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div id={divName} style={{ width: "100%", height: `calc(${height} - ${reduceHeight})` }} />
                                        <span className="stock-chart-display-values"/>
                                    </Col>
                                )
                            })
                        }
                </Row>

                <SymbolsPopupModal
                    symbolModalOpen={this.state.symbolModalOpen}
                    toggleSymbolModal={this.toggleSymbolModal}
                    onSeachKeyWordChange={this.onSeachKeyWordChange}
                    searchKeyword={this.state.searchKeyword}
                    searchKeywordOnClick={this.searchKeywordOnClick}
                    symbolsLoading={this.state.symbolsLoading}
                    symbolsData={this.state.symbolsData}
                    getNextSymbolsData={this.getNextSymbolsData}
                    onChangeSelectedSymbol={this.onChangeSelectedSymbol}
                />
            </ScreenLoaderModal>
        );
    }
}
