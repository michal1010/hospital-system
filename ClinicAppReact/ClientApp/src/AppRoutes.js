import Counter  from "./components/Counter";
import DoctorList from "./components/Doctor/DoctorList";
import FetchData from "./components/FetchData";
import Home from "./components/Home";

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
      path: '/doctors',
      element: <DoctorList />
    },
    {
        path: '/fetch-data',
        element: <FetchData />
    }
];

export default AppRoutes;
