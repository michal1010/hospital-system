import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom'


const DoctorCreate = () => {
 
    const [name, setName] = useState('');
    const [surname, setSurname] = useState('');
    const [specialization, setSpecialization] = useState([]);
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const id = 123;
    const handleSubmit = async (e) => {
        try {
            e.preventDefault();
            const doctor = { id, name, surname, email, password, specialization }
            //setIsPending(true);
            const response = fetch('/api/doctors', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(doctor)
            }).then(() => {
                console.log('new blog added')
                //setIsPending(false);
                history.push('/')
            })
            console.log(response.json())
        }
        catch (error) {
            console.error('Fethc error:' + error.message)
        }
      

    }

    return (
       
        <div>
            <h1>Create</h1>

            <h4>Doctor</h4>
            <hr />
            <div className="row">
                <div className="col-md-4">
                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label>Name</label>
                            <input
                                type="text"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                className="form-control"
                            />
                        </div>
                        <div className="form-group">
                            <label>Surname</label>
                            <input
                                type="text"
                                value={surname}
                                onChange={(e) => setSurname(e.target.value)}
                                className="form-control"
                            />
                        </div>
                        <div className="form-group">
                            <label>Specialization</label>
                            <input
                                type="text"
                                value={specialization}
                                onChange={(e) => setSpecialization(e.target.value)}
                                className="form-control"
                            />
                        </div>
                        <div className="form-group">
                            <label>Email</label>
                            <input
                                type="text"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                className="form-control"
                            />
                        </div>
                        <div className="form-group">
                            <label>Password</label>
                            <input
                                type="text"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                className="form-control"
                            />
                        </div>
                           
                            <div className="form-group">
                                <input type="submit" value="Create" className="btn btn-primary" />
                            </div>
                    </form>
                </div>
            </div>

            <div>
                <Link to="/doctors">Back to List</Link>

            </div>
        </div>
    );
};


export default DoctorCreate;
