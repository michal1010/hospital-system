import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import FormDiv from '../FormDiv.js'


const PatientUpdate = () => {

    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [isPending, setIsPending] = useState(false);
    const navigate = useNavigate();


    const { id } = useParams();
    console.log(id)
    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch(`/api/patients/${id}`)
                    .then(res => res.json())
                    .then(d =>{
                        setName(d.name);
                        setSurname(d.surname);
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
            const patient = { id, name, surname, email, password }
            setIsPending(true);
            const response = await fetch(`/api/patients/${id}`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(patient)
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
            <h1>Edit</h1>

            <h4>Doctor</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                    <form onSubmit={handleSubmit}>
                        <FormDiv label='Name' value={name} changeFunction={setName} />
                        <FormDiv label='Surname' value={surname} changeFunction={setSurname} />
                        <FormDiv label='Email' value={email} changeFunction={setEmail} />
                        <FormDiv label='Password' value={password} type="password" changeFunction={setPassword} />

                        <div className="form-group">
                            {!isPending && < input type="submit" value="Submit" className="btn btn-primary" />}
                            {isPending && <p>Adding patient to the data base...</p>}
                        </div>
                    </form>
               </div>
            </div>

            <div>
                < Link to="/patients">Back to List</Link>


            </div>
        </div>
    );
};


export default PatientUpdate;
