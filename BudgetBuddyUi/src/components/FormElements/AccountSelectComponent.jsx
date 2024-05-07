/* eslint-disable react/prop-types */
import "./FormElement.scss";

const AccountSelectComponent = ({
  account,
  id,
  text,
  array,
  handleSetAccount,
}) => {
  return (
    <select
      onChange={handleSetAccount}
      className="form-control mb-3 select-comp"
      value={account ? account.id : ""}
      required
      id={id}
      name={id}
    >
      {text && (
        <option disabled value="">
          {text}
        </option>
      )}
      {array.map((item) => (
        <option key={item.id} value={item.id}>
          {item.name}
        </option>
      ))}
    </select>
  );
};

export default AccountSelectComponent;
