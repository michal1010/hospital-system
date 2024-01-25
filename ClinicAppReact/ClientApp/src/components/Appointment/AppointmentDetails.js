import React, { useState, useEffect } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import Specializations from '../Parameters.js'
import useFetch from './useFetch.js';


const AppointmentDetails = () => {
    const { id } = useParams()
    const navigate = useNavigate()
    const [data, setData] = useState(null)
    const [error, setError] = useState(null)
    let counter = 1
    console.log(counter)

    useEffect(() => {
        const fetchData = async () => {
            await fetch(`/api/appointments/${id}`)
            .then(res => {

                if (!res.ok){
                   throw Error('Could not fetch the data for that resource')
                }
                return res.json()
            })
            .then((data) => {
                setData(data)
                setError(null)
            })
            .catch(err => {
                if (err.name === 'AbortError'){
                    console.log('fetch aborted')
                } else {
                    setError(err.message)
                }
            })
        }

        fetchData()
    }, [])



    const handleClick = async () => {
        await fetch(`api/appointments/${id}`, {
            method: 'DELETE',
        }).then(() => {
            navigate('/appointments');
        })
    }
    const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour12: false, hour: 'numeric', minute: 'numeric' };

    return (
        <div>
            <h1>Details</h1>

            <div>
                <h4>Doctor</h4>
                <hr />
                {data && (
                        <dl key={data.doctor.id} className="row">
                            <dt className = "col-sm-2">Name</dt>
                            <dd className="col-sm-10">{data.doctor.name}</dd>
                            <dt className = "col-sm-2">Surname</dt>
                            <dd className="col-sm-10">{data.doctor.surname}</dd>
                            <dt className = "col-sm-2">Specialization:</dt>
                            <dd className="col-sm-10">{Specializations[data.doctor.specialization]}</dd>
                            <dt className = "col-sm-2">Start Date</dt>
                            <dd className="col-sm-10">{new Date(data.startTime).toLocaleString('en-UK', options)}</dd>
                            <dt className = "col-sm-2">Description</dt>
                            <dd className="col-sm-10">{data.description}</dd>

                        </dl>
                        )
                }
            </div>
            <div>
                <button className="btn btn-danger " onClick={handleClick}>Delete</button>
            </div>
            <div>
                <button className="btn btn-primary my-2" onClick={() => navigate('/appointments')}>Back</button>
            </div>
        </div>

    );
}

export default AppointmentDetails;