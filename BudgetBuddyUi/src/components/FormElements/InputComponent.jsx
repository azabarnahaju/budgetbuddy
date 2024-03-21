/* eslint-disable react/prop-types */

const InputComponent = ({text, value, name, type, onChange}) => {
  return (
    <>
      <label className="form-label" htmlFor={name}>
        {text}
      </label>
      <input
        className="form-control"
        value={value}
        id={name}
        name={name}
        type={type}
        required
        onChange={onChange}
        placeholder={name}
      />
    </>
  );
};

export default InputComponent;
