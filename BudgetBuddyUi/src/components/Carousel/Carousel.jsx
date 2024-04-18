import React, { useState, useEffect } from "react";
import { fetchData } from "../../service/connectionService";
import SnackBar from "../Snackbar/Snackbar";
import "./Carousel.scss";

const Carousel = () => {
  const [news, setNews] = useState([]);
  //const [currentSlide, setCurrentSlide] = useState(0);

  const fetchNews = async () => {
    try {
      const response = await fetchData(null, "/news", "GET");
      console.log(response);
      if (response.ok) {
        setNews(response.data.content);
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    // setNews([
    //   {
    //     title: "Johnson & Johnson Reports Mixed Q1 Results, Adjusts Outlook",
    //     date: "2024-04-16 11:33:00",
    //     content:
    //       "<p><a href='https://financialmodelingprep.com/financial-summary/JNJ'>Johnson & Johnson (NYSE:JNJ)</a> shares fell more than 1% intra-day today after the company delivered mixed financial results for the first quarter of 2024, with its adjusted earnings per share (EPS) slightly exceeding expectations, while its revenue slightly missed forecasts. The company announced an adjusted EPS of $2.71, topping the consensus estimate by $0.07, but its quarterly revenue fell just short at $21.38 billion, slightly below the expected $21.4 billion.</p>\n<p>Johnson & Johnson also adjusted its full-year 2024 guidance, raising the midpoint for both operational sales and adjusted operational EPS, indicating optimism about its growth trajectory. The company now anticipates sales between $88 billion and $88.4 billion, with adjusted EPS forecasted to range from $10.57 to $10.72.</p>\n<p>Additionally, Johnson & Johnson declared a quarterly dividend of $1.24 per share, a 4.2% increase from the previous $1.19, resulting in an annualized yield of 3.4%.</p>\n",
    //     tickers: "NYSE:JNJ",
    //     image:
    //       "https://cdn.financialmodelingprep.com/images/fmp-1713281594960.jpg",
    //     link: "https://financialmodelingprep.com/market-news/fmp-johnson-&-johnson-reports-mixed-q1-results-adjusts-outlook",
    //     author: "Davit Kirakosyan",
    //     site: "Financial Modeling Prep",
    //   },
    //   {
    //     title: "Morgan Stanley Stock Gains 3% Following Q1 Earnings Beat",
    //     date: "2024-04-16 11:30:00",
    //     content:
    //       "<p><a href='https://financialmodelingprep.com/financial-summary/MS'>Morgan Stanley (NYSE:MS)</a> reported a significant increase in its first-quarter earnings and revenue, outperforming analyst forecasts, which led to a 3% rise in its shares intra-day today. The financial services firm posted net revenues of $15.1 billion for the quarter, marking a 4.1% increase from $14.5 billion in the same period the previous year. The adjusted earnings per share (EPS) was $2.02, well above the analyst prediction of $1.67.</p>\n<p>Ted Pick, the Chief Executive Officer, attributed the firm's strong performance to the substantial growth in client assets across Wealth and Investment Management, which now stand at $7 trillion, and robust activities in Institutional Securities, especially in equity and underwriting.</p> \n<p>Institutional Securities reported net revenues of $7.0 billion, up from $6.8 billion year-over-year, with pre-tax income increasing to $2.4 billion from $1.9 billion. Wealth Management saw its net revenues rise to $6.9 billion from $6.6 billion the previous year, with a pre-tax margin of 26.3%. Furthermore, Investment Management's net revenues grew to $1.4 billion from $1.3 billion year-over-year, with pre-tax income climbing to $241 million from $166 million.</p>\n<p>Morgan Stanley also reported an expense efficiency ratio of 71%, demonstrating operating leverage in an improving market environment, while the standardized Common Equity Tier 1 capital ratio stood at 15.1%.</p>\n",
    //     tickers: "NYSE:MS",
    //     image:
    //       "https://cdn.financialmodelingprep.com/images/fmp-1713281422674.jpg",
    //     link: "https://financialmodelingprep.com/market-news/fmp-morgan-stanley-stock-gains-3-following-q1-earnings-beat",
    //     author: "Davit Kirakosyan",
    //     site: "Financial Modeling Prep",
    //   },
    //   {
    //     title: "UnitedHealth Shares Gain 5% on Q1 Beat",
    //     date: "2024-04-16 11:27:00",
    //     content:
    //       "<p>Shares of <a href='https://financialmodelingprep.com/financial-summary/UNH'>UnitedHealth Group (NYSE:UNH)</a> saw a 5% increase intra-day today following the announcement of Q1/24 earnings that surpassed expectations. The health insurer reported earnings per share (EPS) of $6.91, exceeding consensus estimates of $6.62, with revenue reported at $99.8 billion, higher than the expected $99.26 billion.</p>\n<p>UnitedHealth estimates the total impact of the cyberattack on 2024 earnings will range between $1.15 and $1.35 per share. Looking forward, UnitedHealth has set its full-year 2024 adjusted EPS guidance between $27.50 and $28.00, compared to the market consensus of $27.52. This outlook accounts for costs associated with the cyberattack and the sale of the Brazil operations but includes an anticipated $0.30 to $0.40 per share impact from service disruptions in Change Healthcare. Additionally, the company revised its 2024 net earnings forecast to between $17.60 and $18.20 per share.</p>\n",
    //     tickers: "NYSE:UNH",
    //     image:
    //       "https://cdn.financialmodelingprep.com/images/fmp-1713281266983.jpg",
    //     link: "https://financialmodelingprep.com/market-news/fmp-unitedhealth-shares-gain-5-on-q1-beat",
    //     author: "Davit Kirakosyan",
    //     site: "Financial Modeling Prep",
    //   },
    //   {
    //     title: "Cisco Systems Earns an Upgrade at BofA Securities",
    //     date: "2024-04-16 11:24:00",
    //     content:
    //       "<p>BofA Securities upgraded <a href='https://financialmodelingprep.com/financial-summary/CSCO'>Cisco Systems (NASDAQ:CSCO)</a> to Buy from Neutral, raising their price target to $60 from $55 per share. They outlined three primary growth drivers for Cisco, emphasizing the potential in Splunk, Security, and artificial intelligence to counterbalance weaknesses in networking. Analysts predict that networking will normalize, projecting growth spurred by Cisco's market share gains in Ethernet-based AI infrastructure expansions by major cloud service providers.</p>\n<p>The bank also anticipates an acceleration in Cisco's security segment, supported by stabilization in firewall performance and recent product launches. They further noted the positive growth synergies expected from the acquisition of Splunk.</p>\n<p>While acknowledging that Cisco may face challenges in the upcoming quarters, Bank of America believes these pressures are already accounted for in current market expectations and that Cisco's guidance remains conservatively realistic. The bank views the current setup for Cisco's stock as favorable, with improved fundamentals likely to drive growth moving forward.</p>\n",
    //     tickers: "NASDAQ:CSCO",
    //     image:
    //       "https://cdn.financialmodelingprep.com/images/fmp-1713281043986.jpg",
    //     link: "https://financialmodelingprep.com/market-news/fmp-cisco-systems-earns-an-upgrade-at-bofa-securities-",
    //     author: "Davit Kirakosyan",
    //     site: "Financial Modeling Prep",
    //   },
    //   {
    //     title: "Netflix Price Target Raised at Several Firms Ahead of Earnings",
    //     date: "2024-04-16 11:21:00",
    //     content:
    //       "<p>Several Wall Street firms updated their price targets for <a href='https://financialmodelingprep.com/financial-summary/NFLX'>Netflix (NASDAQ:NFLX)</a>, demonstrating optimism ahead of the streaming company's earnings announcement on April 18. As a result, the company’s shares rose nearly 2% intra-day today. </p>\n<p>Guggenheim increased its target to $700 from $600 and maintained a Buy rating, citing high investor confidence in Netflix's upcoming financial results. The firm adjusted its first-quarter net member addition forecast to 6.8 million, slightly below buy-side expectations of 8 million but above the Visible Alpha consensus of 4.8 million. Guggenheim noted strong download metrics in the U.S. and Canada, although other international regions showed some slowing. </p>\n<p>Despite uncertainties over core membership growth and the impact of new paid-sharing policies, Guggenheim sees considerable potential for Netflix's continued global membership expansion.</p>\n<p>Meanwhile, Macquarie raised its Netflix target to $685 from $595 and retained an Outperform rating. The firm highlighted the significant boost in subscriptions, nearly 30 million in fiscal 2023, attributed to Netflix's initiatives on password sharing and the introduction of its advertising tier.</p>\n",
    //     tickers: "NASDAQ:NFLX",
    //     image:
    //       "https://cdn.financialmodelingprep.com/images/fmp-1713280901496.jpg",
    //     link: "https://financialmodelingprep.com/market-news/fmp-netflix-price-target-raised-at-several-firms-ahead-of-earnings",
    //     author: "Davit Kirakosyan",
    //     site: "Financial Modeling Prep",
    //   },
    // ]);
    //  const intervalId = setInterval(() => {
    //    setCurrentSlide((prevSlide) => {
    //      prevSlide >= news.length - 1  ? 0 : prevSlide + 1;
    //      console.log(prevSlide);
    //    });
    // }, 5000);

    //return () => clearInterval(intervalId);
    fetchNews();
  }, []);

  const extractLinkFromHTML = (htmlString) => {
    const match = htmlString.match(/<a\s+(?:[^>]*?\s+)?href=(["'])(.*?)\1/);
    return match ? match[2] : "";
  };

  // const prevSlide = () => {
  //   setCurrentSlide((prevSlide) =>
  //     prevSlide === 0 ? news.length - 1 : prevSlide - 1
  //   );
  // };

  // const nextSlide = () => {
  //   setCurrentSlide((prevSlide) => (prevSlide === news.length - 1 ? 0 : prevSlide + 1));
  // };

  // return (
  //   <div className="container mt-1" style={{ borderRadius: "10px", marginBottom: "40px" }}>
  //     <div id="carouselExampleIndicators" className="carousel slide" data-ride="carousel">
  //       <ol className="carousel-indicators">
  //         {news.map((item, index) => (
  //           <li
  //             key={index}
  //             data-target="#carouselExampleIndicators"
  //             data-slide-to={index}
  //             className={index === currentSlide ? "active" : ""}
  //           ></li>
  //         ))}
  //       </ol>
  //       <div className="carousel-inner">
  //         {news.map((item, index) => (
  //           <div
  //             key={index}
  //             className={`carousel-item ${index === currentSlide ? "active" : ""} c-item`}
  //             data-bs-interval="5000"
  //           >
  //             <img
  //               src={item.image}
  //               alt={item.title}
  //               className="d-block w-100 c-image"
  //             />
  //             <div className="carousel-caption d-none d-md-block" style={{border: "solid 1px"}}>
  //               <h5>{item.title}</h5>
  //               <a href={extractLinkFromHTML(item.content)}>Read More</a>
  //             </div>
  //           </div>
  //         ))}
  //       </div>
  //       <a
  //         className="carousel-control-prev"
  //         role="button"
  //         onClick={prevSlide}
  //       >
  //         <span
  //           className="carousel-control-prev-icon"
  //           aria-hidden="true"
  //         ></span>
  //         <span className="sr-only">Previous</span>
  //       </a>
  //       <a
  //         className="carousel-control-next"
  //         role="button"
  //         onClick={nextSlide}
  //       >
  //         <span
  //           className="carousel-control-next-icon"
  //           aria-hidden="true"
  //         ></span>
  //         <span className="sr-only">Next</span>
  //       </a>
  //     </div>
  //   </div>
  // );

  return (
    <div
      id="carouselExampleInterval"
      className="carousel slide"
      data-bs-ride="carousel"
    >
      <div className="carousel-inner">
        {news.map((n, i) => (
          <div className={`carousel-item ${i === 0 ? "active" : ""} c-item`} data-bs-interval="5000">
            <img src={n.image} className="d-block w-100 c-image" alt="..." />
          </div>
        ))}
      </div>
      <button
        className="carousel-control-prev"
        type="button"
        data-bs-target="#carouselExampleInterval"
        data-bs-slide="prev"
      >
        <span className="carousel-control-prev-icon" aria-hidden="true"></span>
        <span className="visually-hidden">Previous</span>
      </button>
      <button
        className="carousel-control-next"
        type="button"
        data-bs-target="#carouselExampleInterval"
        data-bs-slide="next"
      >
        <span className="carousel-control-next-icon" aria-hidden="true"></span>
        <span className="visually-hidden">Next</span>
      </button>
    </div>
  );
};

export default Carousel;
