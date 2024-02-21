import AccountForm from "../Forms/AccountForm";
import AchievementForm from "../Forms/AchievementForm";
import TransactionForm from "../Forms/TransactionForm";
import "./Home.css";

const Home = () => {
  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-5 mx-2">
          <AccountForm />
          <AchievementForm />
          <TransactionForm />
        </div>
      </div>
    </div>
  );
};

export default Home;
