import React, { useState, useEffect } from 'react';

const FetchData = () => {
    const [forecasts, setForecasts] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const populateWeatherData = async () => {
            try {
                const response = await fetch('weatherforecast');
                const data = await response.json();
                setForecasts(data);
                setLoading(false);
            } catch (error) {
                console.error('Error fetching weather data:', error);
                setLoading(false);
            }
        };

        populateWeatherData();
    }, []); // Empty dependency array means this effect runs once after the initial render

    const renderForecastsTable = (forecasts) => (
        <table className="table table-striped" aria-labelledby="tableLabel">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>
                    <th>Summary</th>
                </tr>
            </thead>
            <tbody>
                {forecasts.map((forecast) => (
                    <tr key={forecast.date}>
                        <td>{forecast.date}</td>
                        <td>{forecast.temperatureC}</td>
                        <td>{forecast.temperatureF}</td>
                        <td>{forecast.summary}</td>
                    </tr>
                ))}
            </tbody>
        </table>
    );

    return (
        <div>
            <h1 id="tableLabel">Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {loading ? <p><em>Loading...</em></p> : renderForecastsTable(forecasts)}
        </div>
    );
};

export default FetchData;
