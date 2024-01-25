import React, { useState, useEffect } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import FormDiv from '../FormDiv.js'
// import DateTimePicker from 'react-datetime-picker';
// import Datetime from 'react-datetime';
import { DateTimePicker } from '@mui/x-date-pickers';

const AppointmentCreate = () => {

    const [doctors, setDoctors] = useState([]);
    const [startTime, setStartTime] = useState(new Date());
    const [endTime, setEndTime] = useState(new Date());
    const [isPending, setIsPending] = useState(false);
    const navigate = useNavigate();
    const [id, setId] = useState(0);

    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch('/api/doctors')
                    .then(res => res.json())
                    .then(d => {
                        setDoctors(d)
                        setId(d[0].id) // TODO: dlaczego id ustawia mi się na początku na 0????
                        console.log(id)
                    })
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchData();

    }, []);

    const [selectedDoctor, setSelectedDoctor] = useState(doctors[0]);



    const handleSubmit = async (e) => {
        try {
            e.preventDefault();
            console.log('point')
            console.log('startTime ' + startTime)
            const doctorId = id; // TODO: Dlaczego jak dawałem dwa razy id to nie działało
            const shift = {id, doctorId, startTime, endTime}
            setIsPending(true);
            const response = await fetch('/api/appointments', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(shift)
            }).then((response) => {
                setIsPending(false);
                console.log(response.json())
                navigate('/appointments');
            })
        }
        catch (error) {
            console.error(error.message)
        }


    }

    const handleSelectChange = (event) => {
        setId(event.target.value)
        console.log(id)
    };

    const handleSetStartTime = (event) => {
        const selectedDate = new Date(event.target.value);
        // const formattedDate = selectedDate.toISOString().slice(0, 16); // Format: "yyyy-MM-ddThh:mm"

        // console.log(formattedDate); // This logs the formatted date
        setStartTime(selectedDate); // Update state with the formatted date
    };

    const handleSetEndTime = (event) => {
        const selectedDate = new Date(event.target.value);
        // const formattedDate = selectedDate.toISOString().slice(0, 16); // Format: "yyyy-MM-ddThh:mm"

        // console.log(formattedDate); // This logs the formatted date
        setEndTime(selectedDate); // Update state with the formatted date
    };

    return (

        <div>
            <h1>Create</h1>

            <h4>Doctor</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label className="control-label">Doctor</label>
                            <select className="form-control" onChange={handleSelectChange}>
                                {doctors.map((doc, index) =>
                                    <option key={index} value={doc.id}>
                                        {`${doc.name} ${doc.surname}`}
                                    </option>
                                )}
                            </select>
                        </div>
                        {/*<div className="mt-2">*/}
                        {/*<DateTimePicker ampm={false} views={['day', 'month', 'year', 'hours', 'minutes']}*/}
                        {/*label="Start Time" onChange={setStartTime}/>*/}
                        {/*</div>*/}
                        {/*<div className="mt-2">*/}
                        {/*<DateTimePicker ampm={false} views={['day', 'month', 'year', 'hours', 'minutes']}*/}
                        {/*label="End Time" onChange={setEndTime}/>*/}
                        {/*</div>*/}



                        <FormDiv type="datetime-local" label='Start Time' value={startTime} changeFunction={setStartTime} />
                        <FormDiv  type="datetime-local" label='End Time' value={endTime} changeFunction={setEndTime} />

                        <div className="form-group">
                            {!isPending && < input type="submit" value="Create" className="btn btn-primary" />}
                            {isPending && <p>Adding doctor to the data base...</p>}
                            </div>
                    </form>
                </div>
            </div>

            <div>
                < Link to="/appointments">Back to List</Link>
            </div>
        </div>
    );
};


export default AppointmentCreate;
