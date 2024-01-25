import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'
import Specializations from '../Parameters.js'


const AppointmentList = () => {

    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    // the order is essential



    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch('/api/appointments')
                    .then(res => res.json())
                    .then(d => {
                        console.log('break')
                        setUsers(d)
                    })
                    .then(() => setLoading(false));
            } catch (error) {
                console.error('Error fetching data:', error);
                setLoading(false);
            }
        };

        fetchData();

    }, []); // Empty dependency array means this effect runs once after the initial render


    // const DisplayDate = ({user}) => {
    //     console.log(user)
    //     const formattedStartTime = new Date(user.starttime).toLocaleString(); // Adjust to your desired format
    //     console.log(formattedStartTime)
    //     return (
    //         <td>{formattedStartTime}</td>
    //     );
    // }
    const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour12: false, hour: 'numeric', minute: 'numeric' };

    const renderUsersTable = (users) => (
        <table className="table table-striped" aria-labelledby="tableLabel">

            <thead>
                <tr>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Start Time</th>
                    <th>Specialization</th>
                    <th></th>

                </tr>
            </thead>
            <tbody>
                {
                users.map((user) => (
                    <tr  key={user.id}>

                        <td>{user.doctor.name}</td>
                        <td>{user.doctor.surname}</td>
                        <td>{new Date(user.startTime).toLocaleString('en-UK', options)}</td>
                        <td>{Specializations[user.doctor.sepcialization]}</td>
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
            <h1 id="tableLabel">Appointments</h1>
            <p><Link to="/appointments/create">Create</Link></p>
            {loading ? <p><em>Loading...</em></p> : renderUsersTable(users)}
        </div>
    );
};


export default AppointmentList;
