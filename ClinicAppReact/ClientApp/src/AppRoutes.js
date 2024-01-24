import Counter  from "./components/Counter";
import DoctorList from "./components/Doctor/DoctorList";
import DoctorUpdate from "./components/Doctor/DoctorUpdate";
import FetchData from "./components/FetchData";
import Home from "./components/Home";
import DoctorCreate from "./components/Doctor/DoctorCreate"
import DoctorDetails from "./components/Doctor/DocotrDetails";
import PatientCreate from "./components/Patient/PatientCreate"
import PatientDetails from "./components/Patient/PatientDetails";
import PatientList from "./components/Patient/PatientList";
import PatientUpdate from "./components/Patient/PatientUpdate";

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
        path: "/doctors/update/:id",
        element: <DoctorUpdate/>
    },
    {
        path: "/doctors/details/:id",
        element: <DoctorDetails/>
    },
    {
        path: '/doctors/create',
        element: <DoctorCreate />
    },
    {
        exact: true,
        path: '/patients',
        element: <PatientList />
    },
    {
        path: "/patients/update/:id",
        element: <PatientUpdate/>
    },
    {
        path: "/patients/details/:id",
        element: <PatientDetails/>
    },
    {
        path: '/patients/create',
        element: <PatientCreate />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
];

export default AppRoutes;
