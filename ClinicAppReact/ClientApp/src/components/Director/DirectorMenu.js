import React from 'react';
import { Link } from 'react-router-dom';

const Home = () => {
    return (
        <div>
            <h1>Choose the sector</h1>

               <Link className="btn btn-primary" to="/doctors">Doctors</Link>
               <Link className="btn btn-success p-2" to="/patients">Patients</Link>
               <Link className="btn btn-info p-2" to="/appointments">Appointments</Link>

        </div>
    );
};

export default Home;
