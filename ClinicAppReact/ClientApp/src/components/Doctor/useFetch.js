import { useState, useEffect } from "react"

const useFetch = (url) => {

    const [data, setData] = useState(null)
    const [error, setError] = useState(null)

    useEffect(() => {
        const fetchData = async () => {

            await fetch(url)
            .then(res => {

                if (!res.ok){
                   throw Error('Could not fetch the data for that resource')
                }
                return res.json()
            })
            .then((data) => {
                setData(data)
                console.log(data)
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
    }, [url])

    return {data, error}
}

export default useFetch;