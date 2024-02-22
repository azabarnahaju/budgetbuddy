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

  const navigateToCreateAccount = () => {
    navigate("/account/create");
  }

  return (
    <div className="mb-3 form-container">
      <h1>Account</h1>
      <form onSubmit={handleNavigateToAccount}>
        <label className="form-label mb-3" htmlFor="account">
          Account id
        </label>
        <input
          className="form-control mb-3"
          required
          onChange={(e) => handleSetAccountId(e)}
          type="number"
          id="account"
          placeholder="Enter the account id"
        />
        <div>
          <div className="mb-5">
            <button className="btn btn-info ms-4" type="submit">
              Get account by id
            </button>
          </div>
          <div className="mb-5">
            <button onClick={navigateToCreateAccount} className="btn btn-success ms-4">
              + Create account
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default AccountForm;
