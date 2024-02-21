import AccountForm from "../Forms/AccountForm";
import AchievementForm from "../Forms/AchievementForm";
import TransactionForm from "../Forms/TransactionForm";

const Home = () => {
  return (
    <div className="container mt-4">
      <div className="row">
        <div className="col-md-5">
          <AccountForm />
          <AchievementForm />
          <TransactionForm />
        </div>
      </div>
    </div>
  );
};

export default Home;
