import ChartLayout from "./components/ChartLayout";
import DataLayout from "./components/DataLayout";
import { HistoricalData } from "./components/HistoricalData";
import { Layout } from './components/Layout';
import { LiveData } from "./components/LiveData";
import LoginRegistration from "./components/LoginRegistration";
import TradingViewWidget from "./components/TradingViewWidget";
import Unauthorize from "./components/Utils/Unauthorize";

const AppRoutes = [
    {
        index: true,
        element: <LoginRegistration />
    },
    {
        path: '/historical',
        index: false,
        element: <Unauthorize> <Layout><DataLayout></DataLayout></Layout></Unauthorize>
    },
    {
        path: '/live',
        index: false,
        element: <Unauthorize> <Layout><ChartLayout></ChartLayout> </Layout></Unauthorize>
    }
];

export default AppRoutes;
