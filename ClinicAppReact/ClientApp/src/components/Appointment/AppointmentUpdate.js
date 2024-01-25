import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Specializations from '../Parameters.js'
import FormDiv from '../FormDiv.js'


const AppointmentUpdate = () => {

    const [idApp, setIdApp] = useState(null)
    const [startTime, setStartTime] = useState(new Date());
    const [docId, setDocId] = useState(null);
    const [isPending, setIsPending] = useState(false);
    const [doctors, setDoctors] = useState([]);
    const [selectedDoctorId, setSelectedDoctorsId] = useState([]);

    const navigate = useNavigate();


    const { id } = useParams();
    console.log(id)
    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch(`/api/appointments/${id}`)
                    .then(res => res.json())
                    .then(d =>{
                        setId(d.id)
                        setIdApp(d.id)
                        setStartTime(d.startTime);
                        setDocId(d.doctor.id)
                        }
                    )
            } catch (error) {
                console.error('Error fetching data:', error);
            };
        }
        fetchData();

    }, []); // Empty dependency array means this effect runs once after the


    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch('/api/doctors')
                    .then(res => res.json())
                    .then(d => {
                        setDoctors(d)
                        setSelectedDoctorsId(d[0].id) // TODO: dlaczego id ustawia mi się na początku na 0????
                        console.log(id)
                    })
            } catch (error) {
                console.error('Error fetching data:', error);
            }
        };

        fetchData();

    }, []);


    const handleSubmit = async (e) => {
        try {
            e.preventDefault();
            const doctorId = selectedDoctorId
            const id = idApp
            const appointment = { id, doctorId, startTime }
            setIsPending(true);
            const response = await fetch(`/api/appointemnts/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(appointment)
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
        setSelectedDoctorsId(event.target.value)
    };


    return (

        <div>
            <h1>Edit</h1>

            <h4>Doctor</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label className="control-label">Doctor</label>
                            <select className="form-control" value={docId} onChange={handleSelectChange}>
                                {doctors.map((doc, index) =>
                                    <option key={index} value={doc.id}>
                                        {`${doc.name} ${doc.surname}`}
                                    </option>
                                )}
                            </select>
                        </div>
                        <FormDiv type="datetime-local" label='Start Time' value={startTime} changeFunction={setStartTime} />
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


export default AppointmentUpdate;
