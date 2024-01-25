import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'
import Specializations from '../Parameters.js'


const AppointmentListDoctor = () => {

    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [specialization, setSpecialization] = useState(-1);
    const [startDate, setStartDate] = useState(new Date())
    const [endDate, setEndDate] = useState(new Date())


    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch('/api/appointments')
                    .then(res => res.json())
                    .then(d => {
                        setUsers(d)
                    })
                    .then(() => setLoading(false));
            } catch (error) {
                console.error('Error fetching data:', error);
                setLoading(false);
            }

            // Get the current date
            let currentDate = new Date();
            let currentDayOfWeek = currentDate.getDay();
            let diff = currentDate.getDate() - currentDayOfWeek;
            setStartDate(new Date(currentDate.setDate(diff)));
            setEndDate(new Date(currentDate.setDate(diff + 6)));
        };

        fetchData();

    }, []);

    const options = { year: 'numeric', month: '2-digit', day: '2-digit'};
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
                users.map((user) => {
                    const currentDate = new Date(user.startTime)
                    console.log(`current: ${currentDate}; start: ${startDate}`)
                    if ((specialization == user.doctor.specialization || specialization == -1) &&
                    (currentDate >= startDate && currentDate < endDate)) {
                            return (
                                <tr  key={user.id}>

                                    <td>{user.doctor.name}</td>
                                    <td>{user.doctor.surname}</td>
                                    <td>{new Date(user.startTime).toLocaleString('en-UK', options)}</td>
                                    <td>{Specializations[user.doctor.specialization]}</td>
                                    {/* <td><Link to={`/appointments/update/${user.id}`}>Edit</Link></td> */}
                                    <td><Link to={`/appointments/details/safe/${user.id}`}>Details</Link></td>

                                </tr>
                            );
                        }

                } )}
            </tbody>
        </table>
    );



    const decreaseWeek = () => {
        setStartDate(new Date(startDate.setDate(startDate.getDate() - 7)));
        setEndDate(new Date(endDate.setDate(endDate.getDate() - 7)));
    }

    const increaseWeek = () => {
        setStartDate(new Date(startDate.setDate(startDate.getDate() + 7)));
        setEndDate(new Date(endDate.setDate(endDate.getDate() + 7)))
    }

    const handleSelectChange = (event) =>{
        setSpecialization(event.target.value);
    }
    return (

        <div>
            <h1 id="tableLabel">Appointments</h1>
            <div className="form-group">
                <label>Specialization</label>
                <select value={specialization} onChange={handleSelectChange}>
                    {Specializations.map((spec, index) =>
                        <option key={index} value={index}>
                            {spec}
                        </option>
                    )}
                    <option value="-1">All</option>
                </select>
            </div>
            <div>
                <button onClick={decreaseWeek} className="btn btn-primary m-2">Previous week</button>
                <button onClick={increaseWeek} className="btn btn-primary m-2">Next week</button>
                <label className="m-2">{startDate.toLocaleString('en-UK', options)}</label>
                <label className="m-2">{endDate.toLocaleString('en-UK', options)}</label>
            </div>



            {loading ? <p><em>Loading...</em></p> : renderUsersTable(users)}
            <div>
                < Link to="/director">Back</Link>
            </div>
        </div>
    );
};


export default AppointmentListDoctor;
