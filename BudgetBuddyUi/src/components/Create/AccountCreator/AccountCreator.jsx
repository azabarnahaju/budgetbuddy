import { useState } from "react";
import { fetchData } from "../../../service/connectionService";
import SnackBar from "../../Snackbar/Snackbar";
import Loading from "../../Loading/Loading";
import { useNavigate } from "react-router-dom";

const currentUser = 1;

const sampleAccount = {
  id: 0,
  balance: 0,
  date: new Date(),
  name: "",
  type: "",
  userId: currentUser,
  transactions: [],
};

const AccountCreator = () => {
  const [account, setAccount] = useState(sampleAccount);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const [localSnackbar, setLocalSnackbar] = useState({
    open: false,
    message: "",
    type: "",
  });

  const handleAccountChange = (e) => {
    const key = e.target.name;
    const value = e.target.value;
    setAccount({ ...account, [key]: value });
  };

  const handleBack = () => {
    navigate("/");
  }

  const handleCreateAccount = async (e) => {
    e.preventDefault();
    try {
      setLoading(true);
      const response = await fetchData(account, "/Account", "POST");
      if (response.ok) {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "success",
        });
      } else {
        setLocalSnackbar({
          open: true,
          message: response.message,
          type: "error",
        });
      }
    } catch (error) {
      setLocalSnackbar({
        open: true,
        message: "Server not responding.",
        type: "error",
      });
    }
    setLoading(false);
    setAccount(sampleAccount);
  };

  if (loading) {
    return <Loading />;
  }

  return (
    <div className="container mt-5">
      <SnackBar
        {...localSnackbar}
        setOpen={() => setLocalSnackbar({ ...localSnackbar, open: false })}
      />
      <h1>Create new account:</h1>
      <form onSubmit={handleCreateAccount}>
        <label className="form-label mb-3" htmlFor="id">
          Account id
        </label>
        <input
          onChange={handleAccountChange}
          className="form-control mb-3"
          value={account.id}
          required
          type="number"
          id="id"
          name="id"
          placeholder="Enter the account id"
        />
        <label className="form-label mb-3" htmlFor="balance">
          Balance
        </label>
        <input
          onChange={handleAccountChange}
          className="form-control mb-3"
          value={account.balance}
          required
          step={0.00001}
          type="number"
          id="balance"
          name="balance"
          placeholder="Enter the balance"
        />
        <label className="form-label mb-3" htmlFor="name">
          Name
        </label>
        <input
          onChange={handleAccountChange}
          className="form-control mb-3"
          required
          value={account.name}
          type="text"
          id="name"
          name="name"
          placeholder="Enter the name of the account"
        />
        <label className="form-label mb-3" htmlFor="type">
          Type
        </label>
        <input
          onChange={handleAccountChange}
          className="form-control mb-3"
          value={account.type}
          required
          type="text"
          id="type"
          name="type"
          placeholder="Enter the type of the account"
        />
        <div>
          <div className="mb-5">
            <button className="btn btn-info ms-4" type="submit">
              Submit
            </button>
            <button onClick={handleBack} className="btn btn-dark ms-4">
              Back
            </button>
          </div>
        </div>
      </form>
    </div>
  );
};

export default AccountCreator;
