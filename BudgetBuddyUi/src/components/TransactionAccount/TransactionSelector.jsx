import TransactionCreator from "../Create/TransactionCreator/TransactionCreator";

/* eslint-disable react/prop-types */
const TransactionSelector = ({ account, setAccount, accounts, handleSetAccount }) => {
  return (
    <div className="container mt-5">
      <div className="container mt-5">
        <h3>Select your account</h3>
        <div className="row">
          <div className="col-md-4">
            <select
              onChange={handleSetAccount}
              className="form-control mb-3"
              value={account ? account.id : ""}
              required
              id="account"
              name="account"
            >
              {accounts.map((acc) => (
                <option key={acc.id} value={acc.id}>
                  {acc.name}
                </option>
              ))}
            </select>
          </div>
          <div>
            <h4 className="my-4">Details:</h4>
            <h3>
              {account.name} - <span className="lead fs-4">{account.type}</span>
            </h3>
            <h5>Balance: {account.balance}$</h5>
          </div>
          
        </div>
      </div>
      <div className="row my-5">
            <div className="col-md-6">
              <TransactionCreator account={account} setAccount={setAccount} />
            </div>
          </div>
    </div>
  );
};

export default TransactionSelector;
