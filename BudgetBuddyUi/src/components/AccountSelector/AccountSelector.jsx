import React from 'react'
import "./AccountSelector.scss";

const AccountSelector = ({
  selectedAccountIndex,
  setSelectedAccountIndex,
  accounts,
  setAccounts,
  navigate,
}) => {
    const handleSetAccount = (e) => {
      const id = e.target.value;
      setSelectedAccountIndex(id);
    };

  return (
    <div className="container mt-5">
      <div className="container mt-5">
        <div className="selector">
          <h3>Select your account</h3>
          <select
            onChange={handleSetAccount}
            className="form-control mb-3"
            value={selectedAccountIndex}
            required
            id="account"
            name="account"
          >
            {accounts.map((acc, index) => (
              <option key={acc.id} value={index}>
                {acc.name}
              </option>
            ))}
          </select>
        </div>
        <div className="details-container">
          <h4>Details:</h4>
          <table className="table table-responsive  table-acc-details">
            <tbody className="table-success">
              <tr>
                <th>Name</th>
                <td>{accounts[selectedAccountIndex].name}</td>
              </tr>
              <tr>
                <th>Type</th>
                <td>{accounts[selectedAccountIndex].type}</td>
              </tr>
              <tr>
                <th>Balance</th>
                <td>${accounts[selectedAccountIndex].balance}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default AccountSelector