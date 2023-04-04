import React, { Component } from 'react';
import { Button, Divider, Card } from 'antd';
import { CloseCircleOutlined } from '@ant-design/icons';
import { CommonGet } from './Utils/CommonFetch';
import { HistoricalData } from './HistoricalData';
import StockTypes from './Enums/StockTypes';
import SymbolsPopupModal from './CommonUI/SymbolsPopupModal';

class DataLayout extends Component {
    constructor(props) {
        super(props);
        this.state = {
            symbolModalOpen: false,
            symbolsLoading: false,
            symbolsData: [],
            currentSymbolsPage: 1,
            searchKeyword: '',
            selectedSymbols: [],
            currentSymbol: null
        }
    }

    toggleSymbolModal = (status) => {
        this.setState({
            symbolModalOpen: status,
            currentSymbolsPage: 1,
            symbolsData: [],
            searchKeyword: ''
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
        CommonGet("api/v1/exchange/all-symbols", `currentSymbolsPage=${this.state.currentSymbolsPage}&&searchKeyword=${this.state.searchKeyword}&&stockType=${StockTypes.CommonStock}`)
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

    searchKeywordOnClick = () => {
        this.setState({
            symbolsData:[],
            currentSymbolsPage: 1
        }, () => {
            this.getSymbolsData();
        });
    }

    onChangeSelectedSymbol = (symbol) => {
        var selectedSymbols = this.state.selectedSymbols;
        var exists = selectedSymbols.find(x => x.code == symbol.code);
        if (exists === undefined) {
            selectedSymbols.push(symbol);
        }
        this.setState({
            currentSymbol: symbol,
            selectedSymbols,
            symbolModalOpen: false
        })
    }
    
    removeSelectedSymbol = (index, e) => {
        e.stopPropagation()
        var selectedSymbols = this.state.selectedSymbols;
        selectedSymbols.splice(index, 1);
        this.setState({
            selectedSymbols,
            currentSymbol: null
        })
    }

    getSymbolData = (data) => {
        this.setState({
            currentSymbol: data
        })
    }

    onSeachKeyWordChange = (e) => {
        this.setState({
            searchKeyword: e.target.value
        })
    }

    render() {
        return (
            <div className="d-flex flex-wrap">
                <div className="symbols-selection-container">
                    <div className="row">
                        <div className="col-md-12" align="center">
                            <Button className="btn-custom" onClick={() => this.toggleSymbolModal(true)}>Symbol</Button>
                        </div>
                    </div>
                    <Divider className="m-0 custom-divider" />
                    <div className="select-symbols-main-container">
                        {
                            this.state.selectedSymbols.map((symbol, index) => {
                                var selectedCss = this.state.currentSymbol !== null && symbol.code === this.state.currentSymbol.code ? "select-symbols-card-selected" : "";
                                return (
                                    <Card hoverable={true} className={`mt-2 select-symbols-card ${selectedCss}`} key={index} onClick={() => this.getSymbolData(symbol)} >
                                        <div className="row">
                                            <div className="col-md-12" align="right">
                                                <CloseCircleOutlined className="" onClick={(e)=>this.removeSelectedSymbol(index, e)} />
                                            </div>
                                        </div>
                                        <div className="row mt-2">
                                            <div className="col-md-12">
                                                <span className="symbol-code">{symbol.code}</span><br />
                                                <span className="selected-symbol-name">{symbol.name}</span>
                                            </div>
                                        </div>
                                    </Card>
                                )
                            })
                        }
                    </div>
                </div>

                <div className="symbols-selection-container-mobile w-100" align="center">
                    <div className="row m-0">
                        <div className="col-md-12" align="center">
                            <Button className="btn-custom" onClick={() => this.toggleSymbolModal(true)}>Symbol</Button>
                        </div>
                    </div>
                    <Divider className="m-0 custom-divider" />
                    <div className="select-symbols-main-container-mobile black-3-bg-color">
                       
                        {
                            this.state.selectedSymbols.map((symbol, index) => {
                                var selectedCss = this.state.currentSymbol !== null && symbol.code === this.state.currentSymbol.code ? "select-symbols-card-selected" : "";
                                return (
                                    <div hoverable={true} className={`${selectedCss} mt-2 select-symbols-card-mobile `} key={index} onClick={() => this.getSymbolData(symbol)} >
                                        <div className="row">
                                            <div className="col-md-12" align="right">
                                                <CloseCircleOutlined className="" onClick={(e) => this.removeSelectedSymbol(index, e)} />
                                            </div>
                                        </div>
                                        <div className="row mt-2">
                                            <div className="col-md-12">
                                                <span className="symbol-code">{symbol.code}</span><br />
                                                <span className="selected-symbol-name">{symbol.name}</span>
                                            </div>
                                        </div>
                                    </div>
                                )
                            })
                            }
                    </div>
                </div>

                <div className="main-content-container">
                    <HistoricalData currentSymbol={this.state.currentSymbol} />
                </div>

                <SymbolsPopupModal
                    symbolModalOpen={this.state.symbolModalOpen}
                    toggleSymbolModal={this.toggleSymbolModal}
                    onSeachKeyWordChange={this.onSeachKeyWordChange}
                    searchKeyword={this.state.searchKeyword}
                    searchKeywordOnClick={this.searchKeywordOnClick}
                    symbolsLoading={this.state.symbolsLoading}
                    symbolsData={this.state.symbolsData}
                    getNextSymbolsData={this.getNextSymbolsData}
                    onChangeSelectedSymbol={this.onChangeSelectedSymbol }
                />

            </div>
        )
    }
}

export default DataLayout;