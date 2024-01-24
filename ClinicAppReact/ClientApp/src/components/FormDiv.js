const FormDiv = ({ label, value, changeFunction, type }) => {

    return (
        <div className="form-group">
            <label>{label}</label>
            <input
                type= {type ? type : 'text'}
                value={value}
                onChange={(e) => changeFunction(e.target.value)}
                className="form-control"
            />
        </div>
    )
}

export default FormDiv;