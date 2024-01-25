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
import DoctorListUnauthorized from "./components/Doctor/DoctorListUnauthorized";
import PatientListUnauthorized from "./components/Patient/PatientListUnauthorized";
import DirectorMenu from "./components/Director/DirectorMenu";
import AppointmentList from "./components/Appointment/AppointmentList";
import AppointmentCreate from "./components/Appointment/AppointmentCreate";
import AppointmentUpdate from "./components/Appointment/AppointmentUpdate";
import AppointmentDetails from "./components/Appointment/AppointmentDetails";
import AppointmentListDoctor from "./components/Appointment/AppointmentListDoctor";
import AppointmentDetailsSafe from "./components/Appointment/AppointmentDetailsSafe";



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
        path: '/doctors/unauthorized',
        element: <DoctorListUnauthorized />
    },
    {
        path: '/patients/unauthorized',
        element: <PatientListUnauthorized />
    },
    {
        path: '/director',
        element: <DirectorMenu />
    },
    {
        path: '/appointments',
        element: <AppointmentList />
    },
    {
        path: '/appointments/create',
        element: <AppointmentCreate />
    },
    {
        path: '/appointments/update/:id',
        element: <AppointmentUpdate />
    },
    {
        path: '/appointments/details/:id',
        element: <AppointmentDetails />
    },
    {
        path: '/appointments/doctor',
        element: <AppointmentListDoctor />
    },
    {
        path: '/appointments/details/safe/:id',
        element: <AppointmentDetailsSafe />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
];

export default AppRoutes;
