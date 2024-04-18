/* eslint-disable react/prop-types */
import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  CategoryScale,
  TimeSeriesScale,
  LinearScale,
  BarElement,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from "chart.js";

ChartJS.register(
  CategoryScale,
  TimeSeriesScale,
  LinearScale,
  BarElement,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

const ChartComponent = ({ data }) => {
  const config = {
    type: "bar",
    data: data,
    options: {
      plugins: {
        title: {
          display: true,
          text: "Chart.js Bar Chart - Stacked",
          font: {
            size: 18,
          },
        },
      },
      responsive: true,
      scales: {
        x: {
          stacked: true,
          grid: {
            color: "rgba(95, 95, 205, 0.3)",
            borderColor: "rgba(95, 95, 205, 0.6)",
            borderWidth: 1,
          },
          ticks: {
            font: {
              size: 14,
              weight: "bold",
            },
            color: "rgba(255, 255, 255, 0.8)",
          },
        },
        y: {
          stacked: true,
          grid: {
            color: "rgba(95, 95, 205, 0.3)",
            borderColor: "rgba(95, 95, 205, 0.6)",
            borderWidth: 1,
          },
          ticks: {
            font: {
              size: 14,
              weight: "bold",
            },
            color: "rgba(255, 255, 255, 0.8)",
          },
        },
      },
    },
  };

  return <Bar {...config} />;
};

export default ChartComponent;
