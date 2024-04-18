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
import Reports from "./components/Report/Reports";
import ReportDetails from "./components/Report/ReportDetails";
import ReportCreator from "./components/Create/ReportCreator/ReportCreator";
import Goal from "./components/Goals/Goal";
import TransactionAccount from "./components/TransactionAccount/TransactionAccount";
import AdminHome from "./components/AdminPages/AdminHome/AdminHome";
import AchievementList from "./components/AdminPages/Achievements/AchievementList/AchievementList";
import CreateAchievement from "./components/AdminPages/CreateAchievement/CreateAchievement";
import UpdateAchievement from "./components/AdminPages/UpdateAchievement/UpdateAchievement";
import AchievementDashBoard from "./components/Achievement/AchievementDashBoard";
import Transactions from "./components/TransactionAccount/Transactions";


const App = () => {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/goal" element={<Goal />} />
        <Route path="/transaction/:id" element={<Transaction />} />
        <Route path="/achievement/:id" element={<Achievement />} />
        <Route path="/account" element={<TransactionAccount />} />
        <Route path="/account/transactions/:id" element={<Transactions />} />
        <Route path="account/update/:id" element={<AccountCreator />} />
        <Route path="transaction/update/:id" element={<TransactionCreator />} />
        <Route path="achievements" element={<AchievementDashBoard />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/profile/:id" element={<UserProfile />} />
        <Route path="/reports/" element={<Reports />} />
        <Route path="/reports/:reportId" element={<ReportDetails />} />
        <Route path="/reports/add" element={<ReportCreator />} />
        <Route path="/admin" element={<AdminHome />} />
        <Route path="/achievements/create/" element={<CreateAchievement />} />
        <Route path="/achievements/update/:achievementId" element={<UpdateAchievement />}/>
      </Routes>
    </div>
  );
};

export default App;
