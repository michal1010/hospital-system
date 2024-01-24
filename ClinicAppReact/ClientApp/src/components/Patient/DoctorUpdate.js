import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Specializations from './Parameters.js'
import FormDiv from '../FormDiv.js'


const PatientUpdate = () => {

    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [specialization, setSpecialization] = useState(Specializations[0]);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isPending, setIsPending] = useState(false);
    const navigate = useNavigate();


    const { id } = useParams();
    console.log(id)
    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch(`/api/doctors/${id}`)
                    .then(res => res.json())
                    .then(d =>{
                        setName(d.name);
                        setSurname(d.surname);
                        setSpecialization(d.specialization);
                        setEmail(d.email);
                        setPassword(d.password);
                        }
                    )
                //.then(() => setLoading(false));
            } catch (error) {
                console.error('Error fetching data:', error);
                // setLoading(false);
            };
        }
        fetchData();

    }, []); // Empty dependency array means this effect runs once after the



    const handleSubmit = async (e) => {
        try {
            e.preventDefault();
            const doctor = { id, name, surname, email, password, specialization }
            setIsPending(true);
            const response = await fetch(`/api/doctors/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(doctor)
            }).then((response) => {
                setIsPending(false);
                console.log(response.json())
                navigate('/doctors');
            })
        }
        catch (error) {
            console.error(error.message)
        }
    }


    const handleSelectChange = (event) => {
        setSpecialization(event.target.value);
    };

    return (

        <div>
            <h1>Edit</h1>

            <h4>Doctor</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                    <form onSubmit={handleSubmit}>
                        <FormDiv label='Name' value={name} changeFunction={setName} />
                        <FormDiv label='Surname' value={surname} changeFunction={setSurname} />
                        <div className="form-group">
                            <label>Specialization</label>
                            <select value={specialization} onChange={handleSelectChange}>
                                {Specializations.map((spec, index) =>
                                    <option key={index} value={spec}>
                                        {spec}
                                    </option>
                                )}
                            </select>
                        </div>
                        <FormDiv label='Email' value={email} changeFunction={setEmail} />
                        <FormDiv label='Password' value={password} type="password" changeFunction={setPassword} />

                        <div className="form-group">
                            {!isPending && < input type="submit" value="Submit" className="btn btn-primary" />}
                            {isPending && <p>Adding doctor to the data base...</p>}
                        </div>
                    </form>
               </div>
            </div>

            <div>
                < Link to="/doctors">Back to List</Link>


            </div>
        </div>
    );
};


export default PatientUpdate;
