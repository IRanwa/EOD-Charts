// TradingViewWidget.jsx

import React, { useEffect, useRef } from 'react';
import { createChart } from 'lightweight-charts';

let tvScriptLoadingPromise;

export default function TradingViewWidget() {
    const onLoadScriptRef = useRef();
    
    useEffect(
        () => {

            const chart = createChart(document.getElementById('tradingview_cbbe8'));

            const candlestickSeries = chart.addCandlestickSeries({
                upColor: '#26a69a', downColor: '#ef5350', borderVisible: false,
                wickUpColor: '#26a69a', wickDownColor: '#ef5350',
            });
            candlestickSeries.setData([
                { time: '2018-12-22', open: 75.16, high: 82.84, low: 36.16, close: 45.72 },
                { time: '2018-12-23', open: 45.12, high: 53.90, low: 45.12, close: 48.09 },
                { time: '2018-12-24', open: 60.71, high: 60.71, low: 53.39, close: 59.29 },
                { time: '2018-12-25', open: 68.26, high: 68.26, low: 59.04, close: 60.50 },
                { time: '2018-12-26', open: 67.71, high: 105.85, low: 66.67, close: 91.04 },
                { time: '2018-12-27', open: 91.04, high: 121.40, low: 82.70, close: 111.40 },
                { time: '2018-12-28', open: 111.51, high: 142.83, low: 103.34, close: 131.25 },
                { time: '2018-12-29', open: 131.33, high: 151.17, low: 77.68, close: 96.43 },
                { time: '2018-12-30', open: 106.33, high: 110.20, low: 90.39, close: 98.10 },
                { time: '2018-12-31', open: 109.87, high: 114.69, low: 85.66, close: 111.26 },
            ]);
            chart.timeScale().fitContent();
            //onLoadScriptRef.current = createWidget;

            //if (!tvScriptLoadingPromise) {
            //    tvScriptLoadingPromise = new Promise((resolve) => {
            //        const script = document.createElement('script');
            //        script.id = 'tradingview-widget-loading-script';
            //        script.src = 'https://s3.tradingview.com/tv.js';
            //        script.type = 'text/javascript';
            //        script.onload = resolve;

            //        document.head.appendChild(script);
            //    });
            //}

            //tvScriptLoadingPromise.then(() => onLoadScriptRef.current && onLoadScriptRef.current());

            //return () => onLoadScriptRef.current = null;

            //function createWidget() {
            //    if (document.getElementById('tradingview_cbbe8') && 'TradingView' in window) {
            //        new window.TradingView.widget({
            //            autosize: true,
            //            symbol: "NASDAQ:AAPL",
            //            timezone: "Etc/UTC",
            //            theme: "light",
            //            style: "1",
            //            locale: "en",
            //            toolbar_bg: "#f1f3f6",
            //            withdateranges: true,
            //            range: "YTD",
            //            hide_side_toolbar: false,
            //            allow_symbol_change: true,
            //            details: true,
            //            hotlist: true,
            //            calendar: true,
            //            show_popup_button: true,
            //            popup_width: "1000",
            //            popup_height: "650",
            //            container_id: "tradingview_cbbe8"
            //        });
            //    }
            //}
        },
        []
    );

    return (
        <div className='tradingview-widget-container' style={{height:"100%"}}>
            <div id='tradingview_cbbe8' className="tradingview-chart" />
        </div>
    );
}
