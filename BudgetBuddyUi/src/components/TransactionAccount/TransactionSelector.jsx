import TransactionCreator from "../Create/TransactionCreator/TransactionCreator";

/* eslint-disable react/prop-types */
const TransactionSelector = ({ selectedAccountIndex, setSelectedAccountIndex, accounts, setAccounts }) => {

  const handleSetAccount = (e) => {
    const id = e.target.value;
    console.log(id);
    setSelectedAccountIndex(id);
  }

  return (
    <div className="container mt-5">
      <div className="container mt-5">
        <h3>Select your account</h3>
        <div className="row">
          <div className="col-md-4">
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
          <div>
            <h4 className="my-4">Details:</h4>
            <h3>
              {accounts[selectedAccountIndex].name} - <span className="lead fs-4">{accounts[selectedAccountIndex].type}</span>
            </h3>
            <h5>Balance: {accounts[selectedAccountIndex].balance}$</h5>
          </div>
          
        </div>
      </div>
      <div className="row my-5">
            <div className="col-md-6">
              <TransactionCreator selectedAccountIndex={selectedAccountIndex} accounts={accounts} setAccounts={setAccounts} />
            </div>
          </div>
    </div>
  );
};

export default TransactionSelector;
