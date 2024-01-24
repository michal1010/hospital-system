import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Specializations from './Parameters.js'
import useFetch from '../useFetch';


const DoctorDetails = () => {
    const { id } = useParams()
    //const { doctor, error, isPending } = useFetch(`/apki/doctors/${id}`);
    const navigate = useNavigate()
    const [doctor, setDoctors] = useState([]);
    const [loading, setLoading] = useState([]);


    useEffect(() => {
        const fetchData = async () => {
            try {
                await fetch(`/api/doctors/${id}`)
                    .then(res => res.json())
                    .then(d => setDoctors(d))
                    .then(() => setLoading(false));
            } catch (error) {
                console.error('Error fetching data:', error);
                setLoading(false);
            }
        };

        fetchData();

    }, []); // Empty dependency array means this effect runs once after the initial render

    const handleClick = async () => {
        await fetch(`api/doctors/${id}`, {
            method: 'DELETE',
        }).then(() => {
            navigate('/doctors');
        })
    }

    return (
        <div>
            <h1>Details</h1>

            <div>
                <h4>Doctor</h4>
                <hr />
                {doctor && (
                        <dl key={doctor.id} className="row">
                            <dt className = "col-sm-2">Name</dt>
                            <dd className="col-sm-10">{doctor.name}</dd>
                            <dt className = "col-sm-2">Surname</dt>
                            <dd className="col-sm-10">{doctor.surname}</dd>
                            <dt className = "col-sm-2">Specialization:</dt>
                            <dd className="col-sm-10">{Specializations[doctor.sepcialization]}</dd>
                            <dt className = "col-sm-2">Email</dt>
                            <dd className="col-sm-10">{doctor.email}</dd>
                        </dl>
                        )
                }
            </div>
            <div>
                <button className="btn btn-danger " onClick={handleClick}>Delete</button>
            </div>
            <div>
                <button className="btn btn-primary my-2" onClick={() => navigate('/doctors')}>Back</button>
            </div>
        </div>

    );
}

export default DoctorDetails;