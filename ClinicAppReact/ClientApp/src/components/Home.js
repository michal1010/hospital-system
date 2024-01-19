import React from 'react';
import { Link } from 'react-router-dom';

const Home = () => {
    return (
        <div>
            <h1>Hello, world!</h1>
            <p>Welcome to your new single-page application, built with:</p>
            
               <Link className="btn btn-primary" to="/Director">Director</Link>
               <Link className="btn btn-success p-2" to="/doctors">Doctor</Link>
               <Link className="btn btn-info p-2" to="/Patient">Patient</Link>

        </div>
    );
};

export default Home;
