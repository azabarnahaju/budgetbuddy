import { Route, Routes } from "react-router-dom";
import Home from "./components/Home/Home";
import Transaction from "./components/Transaction/Transaction";
import Achievement from "./components/Achievement/Achievement";
import TransactionCreator from "./components/Create/TransactionCreator/TransactionCreator";
import AchievementCreator from "./components/Create/AchievementCreator/AchievementCreator";
import AccountCreator from "./components/Create/AccountCreator/AccountCreator";
import Login from "./components/Authentication/Login/Login";
import Register from "./components/Authentication/Register/Register";
import UserProfile from "./components/UserProfile/UserProfile";
import Goal from "./components/Goals/Goal";
import TransactionAccount from "./components/TransactionAccount/TransactionAccount";

const App = () => {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/goal" element={<Goal />} />
        <Route path="/transaction/:id" element={<Transaction />} />
        <Route path="/achievement/:id" element={<Achievement />} />
        <Route path="/account" element={<TransactionAccount />} />
        <Route path="/transaction/create" element={<TransactionCreator />} />
        <Route path="/achievement/create" element={<AchievementCreator />} />
        <Route path="account/update/:id" element={<AccountCreator />} />
        <Route path="transaction/update/:id" element={<TransactionCreator />} />
        <Route path="achievement/update/:id" element={<AchievementCreator />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/profile/:id" element={<UserProfile />} />
      </Routes>
    </div>
  );
};

export default App;
