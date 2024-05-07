/* eslint-disable react/prop-types */
import "./FormElement.scss";

const InputComponent = ({text, value, name, type, onChange}) => {
  return (
    <>
      <label className="form-label" htmlFor={name}>
        {text}
      </label>
      <input
        className="form-control mb-3"
        value={value}
        id={name}
        name={name}
        type={type}
        required
        onChange={onChange}
      />
    </>
  );
};

export default InputComponent;
