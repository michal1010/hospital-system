import Counter  from "./components/Counter";
import DoctorList from "./components/Doctor/DoctorList";
import FetchData from "./components/FetchData";
import Home from "./components/Home";
import DoctorCreate from "./components/Doctor/DoctorCreate"

const AppRoutes = [
    {
        index: true,
        element: <Home />
    },
    {
        path: '/counter',
        element: <Counter />
    },
    {
        exact: true,
        path: '/doctors',
        element: <DoctorList />
    },
    {
        path: '/doctors/create',
        element: <DoctorCreate />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
];

export default AppRoutes;
