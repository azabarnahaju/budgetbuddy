import { useContext, useEffect } from "react";
import { SnackbarContext } from "../../context/snackbarContext";
import SnackBar from "../Snackbar/Snackbar";
import "./Home.scss";
import Navbar from "../Navbar/Navbar";
import Footer from "../Footer/Footer";
import { useNavigate, Link } from "react-router-dom";
import { UserContext } from "../../context/userContext";

const Home = () => {
  const { snackbar, setSnackbar } = useContext(SnackbarContext);
  const { currentUser } = useContext(UserContext);
  const navigate = useNavigate();

  useEffect(() => {
    setTimeout(() => {
      setSnackbar({
        open: false,
        message: "",
        type: "",
      });
    }, 6000);
  }, [setSnackbar]);

  const handleNavigateTo = (route) => {
    navigate(route);
  };

  // useEffect(() => {
  //   const fetchGoals = async () => {
  //     const response = await fetchData(null, "/Goal/1", "GET");
  //     if (response.ok) {
  //       setGoals(response.data.data);
  //     }
  //   };
  //   fetchGoals();
  // }, []);

  return (
    <div className="home-container vh-100">
      <SnackBar
        {...snackbar}
        setOpen={() => setSnackbar({ ...snackbar, open: false })}
      />
      <Navbar />
      <div className="home-content">
        <div className="container mt-5">
          <div className="text-center">
            <h1 className="title mb-4">
              Take Control of Your Finances with{" "}
              <span className="brand-name">{" BudgetBuddy "}</span>!
            </h1>
            <h2 className="lead-text mb-4">
              Ready to manage your expenses, track your income, and stay on top
              of your financial goals? BudgetBuddy has you covered.
            </h2>
            <h2 className="lead-text">
              Say goodbye to financial stress and hello to financial freedom.
              With MoneyFlow, you can effortlessly monitor your expenses, set
              budgets, track your bills, and stay informed about your overall
              financial health - all in one place.
            </h2>
          </div>
          <hr className="my-5" />
        </div>
        <div className="features">
          <div className="container mt-5">
            <div className="row">
              <div className="col-md-4">
                <div className="feature-item text-center mb-3">
                  <img
                    className="img-fluid w-25"
                    src="/assets/expense.png"
                    alt="Track Expense"
                  />
                  <h3 className="mt-3">Track Expenses</h3>
                  <p>Record your expenses with ease and accuracy.</p>
                </div>
              </div>
              <div className="col-md-4">
                <div className="feature-item text-center mb-3">
                  <img
                    className="img-fluid w-25"
                    src="/assets/income.png"
                    alt="Track Income"
                  />
                  <h3 className="mt-3">Track Income</h3>
                  <p>Keep track of your earnings and sources of income.</p>
                </div>
              </div>
              <div className="col-md-4">
                <div className="feature-item text-center mb-3">
                  <img
                    className="img-fluid w-25"
                    src="/assets/budget.png"
                    alt="Set Budget"
                  />
                  <h3 className="mt-3">Set Goals</h3>
                  <p>
                    Set realistic goals and stay within your financial limits.
                  </p>
                </div>
              </div>
              <hr className="my-5" />
              {currentUser ? (
                <div className="row d-flex justify-content-center text-center mb-5">
                  <div className="col-md-4 mb-3">
                    <button className="btn btn-lg btn-outline-light">
                      Manage your accounts
                    </button>
                  </div>
                  <div className="col-md-4 mb-3">
                    <Link to="/reports/">
                      <button className="btn btn-lg btn-outline-light">
                        Show reports
                      </button>
                    </Link>
                  </div>
                  <div className="col-md-4 mb-3">
                    <button className="btn btn-lg btn-outline-light">
                      Look at your goals
                    </button>
                  </div>
                </div>
              ) : (
                <div className="mb-5 row text-center">
                  <div className="col-md-6">
                    <h3>Welcome back!</h3>
                    <p>
                      Log in to your account to access your financial dashboard.
                    </p>
                    <button
                      onClick={() => handleNavigateTo("/login")}
                      className="btn btn-outline-light mx-3"
                    >
                      Log In
                    </button>
                  </div>
                  <div className="col-md-6">
                    <h3>New here?</h3>
                    <p>
                      Create an account to start managing your finances with
                      ease.
                    </p>
                    <button
                      onClick={() => handleNavigateTo("/register")}
                      className="btn btn-outline-light mx-3"
                    >
                      Register
                    </button>
                  </div>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default Home;
3;
