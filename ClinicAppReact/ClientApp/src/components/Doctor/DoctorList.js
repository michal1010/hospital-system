import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'
import Specializations from './Parameters.js'
import useFetch from './useFetch.js';

const DoctorList = () => {

    // const {users, error} = useFetch('/api/doctors');

    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch('/api/doctors')
                    .then(res => res.json())
                    .then(d => setUsers(d))
                    .then(() => setLoading(false));
            } catch (error) {
                console.error('Error fetching data:', error);
                setLoading(false);
            }
        };

        fetchData();

    }, []); // Empty dependency array means this effect runs once after the initial render
    const renderUsersTable = (users) => (
        <table className="table table-striped" aria-labelledby="tableLabel">

            <thead>
                <tr>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Email</th>
                    <th>Specialization</th>
                    <th></th>

                </tr>
            </thead>
            <tbody>
                {users.map((user) => (
                    <tr  key={user.id}>

                        <td>{user.name}</td>
                        <td>{user.surname}</td>
                        <td>{user.email}</td>
                        <td>{Specializations[user.sepcialization]}</td>
                        <td><Link to={`/doctors/update/${user.id}`}>Edit</Link></td>
                        <td><Link to={`/doctors/details/${user.id}`}>Details</Link></td>

                    </tr>
                ))}
            </tbody>
        </table>
    );
    console.log(users)
    return (

        <div>
            <h1 id="tableLabel">Doctors</h1>
            <p><Link to="/doctors/create">Create</Link></p>
            {error ? <p><em>Error</em></p> : renderUsersTable(users)}
        </div>
    );
};


export default DoctorList;
