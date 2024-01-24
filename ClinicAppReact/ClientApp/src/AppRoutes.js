import Counter  from "./components/Counter";
import DoctorList from "./components/Doctor/DoctorList";
import DoctorUpdate from "./components/Doctor/DoctorUpdate";
import FetchData from "./components/FetchData";
import Home from "./components/Home";
import DoctorCreate from "./components/Doctor/DoctorCreate"
import DoctorDetails from "./components/Doctor/DocotrDetails";
import PatientCreate from "./components/Doctor/DoctorCreate"
import PatientDetails from "./components/Doctor/DocotrDetails";
import PatientList from "./components/Doctor/DoctorList";
import PatientUpdate from "./components/Doctor/DoctorUpdate";

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
        path: "/patient/update/:id",
        element: <PatientUpdate/>
    },
    {
        path: "/patient/details/:id",
        element: <PatientDetails/>
    },
    {
        path: '/patient/create',
        element: <PatientCreate />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
];

export default AppRoutes;
