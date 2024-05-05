<!-- Improved compatibility of back to top link: See: https://github.com/othneildrew/Best-README-Template/pull/73 -->
<a name="readme-top"></a>

[![Stargazers][stars-shield]][stars-url]
[![Issues][issues-shield]][issues-url]
[![Forks][forks-shield]][forks-url]
[![LinkedIn][linkedin-shield]][linkedin-url]



<br />
<div align="center">
<h3 align="center">SolarWatch</h3>
</div>



<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#main-features">Main features</a></li>
        <li><a href="#built-with">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li><a href="#installation">Installation</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#contact">Contact</a></li>
  </ol>
</details>



<!-- ABOUT THE PROJECT -->
## About The Project

<!--[![Product Name Screen Shot][product-screenshot]](https://example.com) -->

BudgetBuddy is a web application built with ASP.NET and React.js that enables users to take control of their finances, set financial goals, and gain insights into their financial habits. 

<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Main features

*  **Tracking Financial Actions**: Users can log their spending and income transactions, categorize them, and view their financial history to monitor their cash flow.
* **Goal Setting and Tracking**: Set financial goals such as saving for a vacation or paying off debt, and track progress towards these goals over time.
* **Insightful Reports**: Generate detailed reports and visualizations to gain insights into spending patterns, identify areas for improvement, and make informed financial decisions.
* **Achievement System**: Users can earn achievements for utilizing various features of the app, providing motivation and encouragement to engage with their finances.
* **User Authentication**: Secure user authentication allows users to sign up and log in to their accounts, ensuring their financial data remains confidential.
* **Admin Panel**: For administrative users, an admin panel is available, providing additional capabilities for managing users, data, and settings within the app.


<p align="right">(<a href="#readme-top">back to top</a>)</p>



### Built With

* [![ASP.NET Core][ASP.NET Core]][dotnetcore-url]
* [![React][React.js]][React-url]
* [![MicrosoftSQLServer][Microsoft SQL Server]][sql-server-url]
* [![Bootstrap][Bootstrap.com]][Bootstrap-url]
* [![Docker][Docker]][docker-url]


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

### Prerequisites

* **Docker Desktop**

    Download via https://www.docker.com/products/docker-desktop/

### Installation

1. Clone the repo
   ```sh
   git clone https://github.com/azabarnahaju/budgetbuddy
   ```
2. Enter your user secrets in `dotnet user-secrets` and `.env` files
    * `dotnet user-secrets`
    ```sh
        cd BudgetBuddy
        dotnet user-secrets init
        dotnet user-secrets set "JwtSettings:ValidIssuer" "YOUR_VALID_ISSUER"
        dotnet user-secrets set "JwtSettings:validAudience" "YOUR_VALID_AUDIENCE"
        dotnet user-secrets set "JwtSettings:IssuerSigningKey" "YOUR_ISSUER_SIGNING_KEY"
        dotnet user-secrets set "AdminInfo:adminEmail" "YOUR_CUSTOM_ADMIN_EMAIL_ADDRESS"
        dotnet user-secrets set "AdminInfo:adminPassword" "YOUR_CUSTOM_ADMIN_PASSWORD"
    ```
    * `.env` files:
        
        * Create `db.env`:
            ```
            ACCEPT_EULA=Y
            MSSQL_SA_PASSWORD=YOUR_DATABASE_PASSWORD
            ``` 
        * Create `server.db`:
            ```
            DB_CONNECTION_STRING="Server=localhost,1433;Database=BudgetBuddy;User Id=sa;Password=YOUR_DB_PASSWORD;Encrypt=false;TrustServerCertificate=true"
            ``` 
    
3. Build and run Docker Compose:
    ```sh
    docker-compose up --build
    ```
4. Access frontend and backend:

    * Backend: http://localhost:7204
    * Frontend: http://localhost:8080 

<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- USAGE EXAMPLES -->
## Usage

### 1. Sign up/Log in 
* As a new user, navigate to the authentication page and sign up
* If you already have an account, navigate to the authentication page and log in with your credentials

### 2. Track finances
* Once you're signed in, navigate to the Accounts page where you can create accounts such as _Credit card, Cash, Savings, etc_.
* Choosing an account, you can add transactions to it by providing a name, a date of the transactions, choosing a type (_Income / Expense_) and putting a tag on it (e.g. _Bills, Grocieries, Entertainment, etc_)

### 3. Create financial reports 
* Navigate to the Reports page and look at your existing reports' details by clicking on the See details button or create new ones by providing a timeframe (_weekly, monthly, custom, etc._) and an account. 

### 4. Gain achievements
* By creating accounts, transactions and creating and / or achieving goals you can earn achievements 

### 5. Read financial news
* By clicking on each picture on the bottom of the main page you can read the latest financial news


<p align="right">(<a href="#readme-top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

<p align="right">(<a href="#readme-top">back to top</a>)</p>




<!-- CONTACT -->
## Contact

Alexandra Kanik - [Linkedin](https://www.linkedin.com/in/alexandrakanik/) - kanikalexandra@gmail.com

Project Link: [https://github.com/azabarnahaju/budgetbuddy](https://github.com/azabarnahaju/budgetbuddy)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


[forks-shield]: https://img.shields.io/github/forks/azabarnahaju/budgetbuddy.svg?style=for-the-badge
[forks-url]: https://github.com/azabarnahaju/budgetbuddy/network/members
[stars-shield]: https://img.shields.io/github/stars/azabarnahaju/budgetbuddy.svg?style=for-the-badge
[stars-url]: https://github.com/azabarnahaju/budgetbuddy/stargazers
[issues-shield]: https://img.shields.io/github/issues/azabarnahaju/budgetbuddy.svg?style=for-the-badge
[issues-url]: https://github.com/azabarnahaju/budgetbuddy/issues
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://linkedin.com/in/alexandrakanik
[product-screenshot]: images/screenshot.png
[React.js]: https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB
[React-url]: https://reactjs.org/
[Bootstrap.com]: https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white
[Bootstrap-url]: https://getbootstrap.com
[ASP.NET Core]: https://img.shields.io/badge/asp.net_core-6d409d?style=for-the-badge&logo=dotnet&logoColor=white
[dotnetcore-url]: https://dotnet.microsoft.com/en-us/apps/aspnet
[Microsoft SQL Server]: https://img.shields.io/badge/sql_server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white
[sql-server-url]: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
[Docker]: https://img.shields.io/badge/docker-2496ED?style=for-the-badge&logo=docker&logoColor=white
[docker-url]: https://www.docker.com 