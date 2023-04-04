import React, { Component } from 'react';
import ScreenLoaderModal from './CommonUI/ScreenLoaderModal';
import SymbolsPopupModal from './CommonUI/SymbolsPopupModal';
import { LiveData } from './LiveData';
import { CommonGet } from './Utils/CommonFetch';
import { TableOutlined } from '@ant-design/icons';
import { Dropdown } from 'antd';

import { ReactComponent as Grid1Img } from '../assets/images/grid-1.svg';
import { ReactComponent as Grid2HorizontalImg } from '../assets/images/grid-2-horizontal.svg';
import { ReactComponent as Grid2VerticalImg } from '../assets/images/grid-2-vertical.svg';
import { ReactComponent as Grid4HorizontalImg } from '../assets/images/grid-4-horizontal.svg';
import { ReactComponent as Grid4VerticalImg } from '../assets/images/grid-4-vertical.svg';
import { ReactComponent as Grid6Img } from '../assets/images/grid-6-horizontal.svg';
import { ReactComponent as Grid8Img } from '../assets/images/grid-8-horizontal.svg';
import ChartGridTypes from './Enums/ChartGridTypes';

class ChartLayout extends Component {
    constructor(props) {
        super(props);
        this.state = {
            viewDisplayLayoutType: ChartGridTypes.Grid1,
            dropdownOpen: false
        }
    }

    onChangeGridLayout = (type) => {
        this.setState({
            viewDisplayLayoutType: type
        })
    }

    dropdownDisplay = () => {
        this.setState({
            dropdownOpen: true
        })
    }

    render() {
        var width = window.innerWidth;
        var mobileEnable = width >= 992 ? "display-content" : "display-none-content";
        const items = [
            {
                key: '1',
                label: (
                    <div className="">
                        <div align="left" className="col-md-12 col-sm-12 ">
                            <Grid1Img className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid1)} />
                        </div>
                    </div>
                ),
            },
            {
                key: '2',
                label: (
                    <div className="row">
                        <div className={`col-md-6 col-sm-6 ${mobileEnable}`} >
                            <Grid2HorizontalImg className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid2Horizontal)} />
                        </div>
                        <div className="col-md-6 col-sm-6">
                            <Grid2VerticalImg className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid2Vertical)} />
                        </div>
                    </div>
                )
            },
            {
                key: '3',
                label: (
                    <div className="row">
                        <div className={`col-md-6 col-sm-6 ${mobileEnable}`} >
                            <Grid4HorizontalImg className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid4Horizontal)} />
                        </div>
                        <div className="col-md-6 col-sm-6">
                            <Grid4VerticalImg className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid4Vertical)} />
                        </div>
                    </div>
                )
            },
            {
                key: '4',
                label: (
                    <div className="row">
                        <div align="left" className="col-md-12 col-sm-12 ">
                            <Grid6Img className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid6)} />
                        </div>
                    </div>
                ),
            },
            {
                key: '5',
                label: (
                    <div className="row">
                        <div align="left" className="col-md-12 col-sm-12 ">
                            <Grid8Img className="chart-grid-layout-icon blue-icon cursor-pointer" onClick={() => this.onChangeGridLayout(ChartGridTypes.Grid8)} />
                        </div>
                    </div>
                ),
            },
        ];

        var selectedGridLayout = <Grid1Img className="blue-icon cursor-pointer" />;
        if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid2Horizontal) {
            selectedGridLayout = <Grid2HorizontalImg className="blue-icon cursor-pointer" />
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid2Vertical) {
            selectedGridLayout = <Grid2VerticalImg className="blue-icon cursor-pointer" />
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid4Horizontal) {
            selectedGridLayout = <Grid4HorizontalImg className="blue-icon cursor-pointer" />
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid4Vertical) {
            selectedGridLayout = <Grid4VerticalImg className="blue-icon cursor-pointer" />
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid6) {
            selectedGridLayout = <Grid6Img className="blue-icon cursor-pointer" />
        } else if (this.state.viewDisplayLayoutType === ChartGridTypes.Grid8) {
            selectedGridLayout = <Grid8Img className="blue-icon cursor-pointer" />
        }
        return (
            <div>
                <div className="row m-0">
                    <div className="col-md-12 chart-top-nav" align="left">
                        <Dropdown
                            menu={{
                                items,
                            }}
                            className="mt-3 mb-3 chart-grid-layout-dropdown"
                            onClick={this.dropdownDisplay}
                            open={this.dropdownOpen}
                        >
                            {
                                selectedGridLayout
                            }

                        </Dropdown>
                    </div>
                </div>
                <LiveData
                    currentSymbol={this.state.currentSymbol}
                    viewDisplayLayoutType={this.state.viewDisplayLayoutType}
                />


            </div>
        )
    }
}

export default ChartLayout;