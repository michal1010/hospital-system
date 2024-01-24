import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import FormDiv from '../FormDiv.js'


const PatientCreate = () => {

    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isPending, setIsPending] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        try {
            e.preventDefault();
            const doctor = { name, surname, email, password }
            setIsPending(true);
            const response = await fetch('/api/patients', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(doctor)
            }).then((response) => {
                setIsPending(false);
                console.log(response.json())
                navigate('/patients');
            })
        }
        catch (error) {
            console.error(error.message)
        }


    }


    return (

        <div>
            <h1>Create</h1>

            <h4>Patient</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                    <form onSubmit={handleSubmit}>
                        <FormDiv label='Name' value={name} changeFunction={setName} />
                        <FormDiv label='Surname' value={surname} changeFunction={setSurname} />
                        <FormDiv label='Email' value={email} changeFunction={setEmail} />
                        <FormDiv label='Password' value={password} changeFunction={setPassword} />

                        <div className="form-group">
                            {!isPending && < input type="submit" value="Create" className="btn btn-primary" />}
                            {isPending && <p>Adding patient to the data base...</p>}
                            </div>
                    </form>
                </div>
            </div>

            <div>
                < Link to="/patietns">Back to List</Link>


            </div>
        </div>
    );
};


export default PatientCreate;
