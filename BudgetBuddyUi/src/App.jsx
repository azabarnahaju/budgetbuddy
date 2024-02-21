import { Route, Routes } from "react-router-dom";
import Home from "./components/Home/Home";
import Account from "./components/Account/Account";
import Transaction from "./components/Transaction/Transaction";
import Achievement from "./components/Achievement/Achievement";

const App = () => {
  return (
    <div>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/account/:id" element={<Account />} />
        <Route path="/transaction/:id" element={<Transaction />} />
        <Route path="/achievement/:id" element={<Achievement />} />
      </Routes>
    </div>
  );
};

export default App;
