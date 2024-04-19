export const formatDate = (dateString) => {
  const date = new Date(dateString);
  const currentDate = new Date();
  const timeDifference = currentDate.getTime() - date.getTime();
  const secondsDifference = Math.floor(timeDifference / 1000);

  if (secondsDifference < 60) {
    return `${secondsDifference} second${
      secondsDifference !== 1 ? "s" : ""
    } ago`;
  } else if (secondsDifference < 3600) {
    const minutesDifference = Math.floor(secondsDifference / 60);
    return `${minutesDifference} minute${
      minutesDifference !== 1 ? "s" : ""
    } ago`;
  } else if (secondsDifference < 86400) {
    const hoursDifference = Math.floor(secondsDifference / 3600);
    return `${hoursDifference} hour${hoursDifference !== 1 ? "s" : ""} ago`;
  } else if (secondsDifference < 2592000) {
    const daysDifference = Math.floor(secondsDifference / 86400);
    return `${daysDifference} day${daysDifference !== 1 ? "s" : ""} ago`;
  } else if (secondsDifference < 31536000) {
    const monthsDifference = Math.floor(secondsDifference / 2592000);
    return `${monthsDifference} month${monthsDifference !== 1 ? "s" : ""} ago`;
  } else {
    const yearsDifference = Math.floor(secondsDifference / 31536000);
    return `${yearsDifference} year${yearsDifference !== 1 ? "s" : ""} ago`;
  }
};

export const stringToDate = (dateToFormat) =>
  new Date(dateToFormat).toLocaleDateString("en-gb", {
    weekday: "long",
    year: "numeric",
    month: "short",
    day: "numeric",
  });

export const calculatePercentage = (current, target) => {
  return ((current * 100) / target).toFixed(1);
};

export const prepareChartData = (rawData, startDate, endDate) => {
  const data = extractTransactionData(rawData);
  const chartData = {
    labels: [],
    datasets: [
      {
        label: "Expense",
        backgroundColor: "rgba(255, 99, 132, 0.6)",
        borderColor: "rgba(255, 99, 132, 1)",
        borderWidth: 1,
        hoverBackgroundColor: "rgba(255, 99, 132, 0.8)",
        hoverBorderColor: "rgba(255, 99, 132, 1)",
        data: [],
      },
      {
        label: "Income",
        backgroundColor: "rgba(75, 192, 192, 0.6)",
        borderColor: "rgba(75, 192, 192, 1)",
        borderWidth: 1,
        hoverBackgroundColor: "rgba(75, 192, 192, 0.8)",
        hoverBorderColor: "rgba(75, 192, 192, 1)",
        data: [],
      },
    ],
  };

  for (
    let date = new Date(startDate);
    date < new Date(endDate);
    date.setDate(date.getDate() + 1)
  ) {

    const dateString = date.toISOString().split("T")[0];
    chartData.labels.push(dateString);

    let totalIncome = 0;
    let totalExpense = 0;

    data.forEach((entry) => {
      if (entry.date === dateString) {
        if (entry.type === "Income") {
          totalIncome += entry.amount;
        } else {
          totalExpense += entry.amount;
        }
      }
    });

    chartData.datasets[0].data.push(-totalExpense);
    chartData.datasets[1].data.push(totalIncome);
  }

  return chartData;
};

const extractTransactionData = (data) => {
  const extractedData = [];
  data.forEach((element) => {
    extractedData.push({
      amount: element.amount,
      type: element.type,
      date: element.date.split("T")[0],
    });
  });

  return extractedData;
};
