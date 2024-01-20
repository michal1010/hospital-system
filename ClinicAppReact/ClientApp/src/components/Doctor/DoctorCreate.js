import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import Specializations from './Parameters.js'
import FormDiv from '../FormDiv.js'


const DoctorCreate = () => {

    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [specialization, setSpecialization] = useState(Specializations[0]);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isPending, setIsPending] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        try {
            e.preventDefault();
            const doctor = { name, surname, email, password, specialization }
            setIsPending(true);
            const response = await fetch('/api/doctors', {
                method: 'POST',
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
            <h1>Create</h1>

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
                        <FormDiv label='Password' value={password} changeFunction={setPassword} />

                        <div className="form-group">
                            {!isPending && < input type="submit" value="Create" className="btn btn-primary" />}
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


export default DoctorCreate;
