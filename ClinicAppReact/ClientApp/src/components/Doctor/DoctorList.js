import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'


const DoctorList = () => {

    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);

    // the order is essential
    const Specializations = [
        'Internal',
        'Pediartrics',
        'Neurology',
        'Oncology',
        'Dermatology',
        'Cariology',
        'Surgeon'
    ]

    useEffect(() => {
        const fetchData = async () => {
            try {
                fetch('/api/doctors')
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

                </tr>
            </thead>
            <tbody>
                {users.map((user) => (
                    <tr key={user.id}>

                        <td>{user.name}</td>
                        <td>{user.surname}</td>
                        <td>{user.email}</td>
                        <td>{Specializations[user.sepcialization]}</td>

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
            {loading ? <p><em>Loading...</em></p> : renderUsersTable(users)}
        </div>
    );
};


export default DoctorList;
