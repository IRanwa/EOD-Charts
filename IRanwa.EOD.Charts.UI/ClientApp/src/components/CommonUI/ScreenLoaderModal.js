﻿import { Spin } from 'antd';
import React, { Component } from 'react';
import { loadingIcon } from '../Utils/CommonUI';

class ScreenLoaderModal extends Component {
    render() {
        return (
            <Spin className="ui-loader" spinning={this.props.loading} indicator={loadingIcon} >
                {this.props.children}
            </Spin>
        )
    }
}

export default ScreenLoaderModal;