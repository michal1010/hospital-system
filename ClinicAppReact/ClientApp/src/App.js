import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'
import { LocalizationProvider } from '@mui/x-date-pickers';

// Functional component App
const App = () => {
    return (
        <LocalizationProvider dateAdapter={AdapterDayjs}>
            <Layout>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, exact, ...rest } = route;
                        return <Route key={index} exact={exact} {...rest} element={element} />;
                    })}
                </Routes>
            </Layout>
        </LocalizationProvider>

    );
};

export default App;
