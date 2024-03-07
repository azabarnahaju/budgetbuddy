import { Route, Routes } from "react-router-dom";
import Home from "./components/Home/Home";
import Account from "./components/Account/Account";
import Transaction from "./components/Transaction/Transaction";
import Achievement from "./components/Achievement/Achievement";
import TransactionCreator from "./components/Create/TransactionCreator/TransactionCreator";
import AchievementCreator from "./components/Create/AchievementCreator/AchievementCreator";
import AccountCreator from "./components/Create/AccountCreator/AccountCreator";
import Authentication from "./components/Authentication/Authentication/Authentication";
import UserProfile from "./components/UserProfile/UserProfile";

const App = () => {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/account/:id" element={<Account />} />
        <Route path="/transaction/:id" element={<Transaction />} />
        <Route path="/achievement/:id" element={<Achievement />} />
        <Route path="/account/create" element={<AccountCreator />} />
        <Route path="/transaction/create" element={<TransactionCreator />} />
        <Route path="/achievement/create" element={<AchievementCreator />} />
        <Route path="account/update/:id" element={<AccountCreator />} />
        <Route path="transaction/update/:id" element={<TransactionCreator />} />
        <Route path="achievement/update/:id" element={<AchievementCreator />} />
        <Route path="/authentication" element={<Authentication />} />
        <Route path="/profile/:id" element={<UserProfile />} />
      </Routes>
    </div>
  );
};

export default App;
