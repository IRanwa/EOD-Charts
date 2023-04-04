import React, { Component } from 'react';
import { Modal, Input, Button } from 'antd';
import { CloseCircleOutlined, SearchOutlined } from '@ant-design/icons';
import ScreenLoaderModal from './ScreenLoaderModal';
import InfiniteScroll from 'react-infinite-scroll-component'

class SymbolsPopupModal extends Component {
    render() {
        return (
            <Modal
                title="Symbol Search"
                className="popup-modal"
                open={this.props.symbolModalOpen}
                onCancel={() => this.props.toggleSymbolModal(false)}
                closeIcon={<CloseCircleOutlined className="" />}
                footer={[]}>
                <div className="row">
                    <div className="col align-self-center">
                        <Input
                            placeholder="Search"
                            prefix={<SearchOutlined className="gray-icon" />}
                            className="w-100"
                            onChange={this.props.onSeachKeyWordChange}
                            value={this.props.searchKeyword}
                        />
                    </div>
                    <div className="col-md-auto">
                        <Button className="btn-custom" onClick={this.props.searchKeywordOnClick}>Search</Button>
                    </div>
                </div>
                <ScreenLoaderModal loading={this.props.symbolsLoading}>
                    <div className="mt-2 symbols-search-main-container" id="symbols-search-main-container" >
                        <InfiniteScroll
                            dataLength={this.props.symbolsData.length} //This is important field to render the next data
                            next={this.props.getNextSymbolsData}
                            hasMore={true}
                            scrollableTarget="symbols-search-main-container"
                            className="symbols-search-scroll"
                            height={"55vh"}
                        >
                            {
                                this.props.symbolsData.map((data, index) => {
                                    return (
                                        <div className="row p-2 symbol-container" key={index} onClick={() => this.props.onChangeSelectedSymbol(data)}>
                                            <div className="col-md-3 col-sm-12 symbol-code" align="center">{data.code} <span className="symbol-type"> {data.country}</span></div>
                                            <div className="col-md-6 col-sm-12" align="center">{data.name}</div>
                                            <div className="col-md-3 col-sm-12" align="center" >
                                                <span className="symbol-type">{data.type}</span>
                                                <span> {data.exchange}</span>
                                            </div>
                                        </div>
                                    )
                                })
                            }
                        </InfiniteScroll>
                    </div>
                </ScreenLoaderModal>
            </Modal>
        )
    }
}

export default SymbolsPopupModal;