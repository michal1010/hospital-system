import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import useFetch from './useFetch.js';


const PatientDetails = () => {
    const { id } = useParams()
    const navigate = useNavigate()
    console.log('This is id ' + id)
    const {data, error} =  useFetch(`/api/patients/${id}`);
    const doctor = data;


    const handleClick = async () => {
        await fetch(`api/patients/${id}`, {
            method: 'DELETE',
        }).then(() => {
            navigate('/patients');
        })
    }

    return (
        <div>
            <h1>Details</h1>

            <div>
                <h4>Patient</h4>
                <hr />
                {data && (
                        <dl key={doctor.id} className="row">
                            <dt className = "col-sm-2">Name</dt>
                            <dd className="col-sm-10">{data.name}</dd>
                            <dt className = "col-sm-2">Surname</dt>
                            <dd className="col-sm-10">{data.surname}</dd>
                            <dt className = "col-sm-2">Email</dt>
                            <dd className="col-sm-10">{data.email}</dd>
                        </dl>
                        )
                }
            </div>
            <div>
                <button className="btn btn-danger " onClick={handleClick}>Delete</button>
            </div>
            <div>
                <button className="btn btn-primary my-2" onClick={() => navigate('/patients')}>Back</button>
            </div>
        </div>

    );
}

export default PatientDetails;