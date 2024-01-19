import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { Layout } from './components/Layout';
import './custom.css';


// Functional component App
const App = () => {
    return (
        <Layout>
            <Routes>
                {AppRoutes.map((route, index) => {
                    const { element, exact, ...rest } = route;
                    return <Route key={index} exact={exact} {...rest} element={element} />;
                })}
            </Routes>
        </Layout>
    );
};

export default App;
