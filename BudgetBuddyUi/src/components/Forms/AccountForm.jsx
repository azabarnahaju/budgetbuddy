import { useNavigate } from "react-router-dom";
import { useState } from "react";

const AccountForm = () => {
  const [accountId, setAccountId] = useState(false);
  const navigate = useNavigate();

  const handleSetAccountId = (e) => {
    setAccountId(e.target.value);
  };

  const handleNavigateToAccount = (e) => {
    e.preventDefault();
    accountId && navigate(`/account/${accountId}`);
  };
  return (
    <div className="mb-3">
      <form onSubmit={handleNavigateToAccount}>
        <label className="form-label mb-3" htmlFor="account">Account id</label>
        <input
        className="form-control mb-3"
          required
          onChange={(e) => handleSetAccountId(e)}
          type="number"
          id="account"
          placeholder="Enter the account id"
        />
        <button className="btn btn-dark ms-4" type="submit">
          Get account by id
        </button>
      </form>
    </div>
  );
};

export default AccountForm;
