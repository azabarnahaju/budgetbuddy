export const getSideChartData = (transactions) => {
    const daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    const firstDay = getDateXDaysAgo(6);
    const result = getChartDataFormatForWeek();

    console.log(transactions)
    transactions.forEach((transaction) => {
        if (new Date(transaction.date) > firstDay) {
            const day = daysOfWeek[new Date(transaction.date).getDay()]
            const obj = result.find(o => o.name === day);
            obj.amount += transaction.amount;
        }
    });

    return result;
}

export const getBalance = (accounts) => {
  return accounts.reduce((acc, cv) => acc + cv.balance, 0);
};

export const getOverviewChartData = (transactions, balance) => {
    const firstDay = getDateXDaysAgo(6);
    const lastWeekTransactions = transactions.filter(
      (t) => new Date(t.date) > firstDay
    );
    const daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
    const result = getChartDataFormatForWeek();
    result[result.length-1].amount = balance;
    let currBalance = balance;
    for (let i = result.length-1; i >= 0; i--) {
        const dayToFind = result[i]["name"];
        const filtered = lastWeekTransactions.filter(
          (t) => daysOfWeek[new Date(t.date).getDate()] === dayToFind
        );
        const sum = filtered.reduce((acc, cv) => acc + cv.amount, 0);
        const obj = result.find((o) => o.name === dayToFind);
        currBalance -= sum;
        obj.amount = currBalance;
    }

    return result;

}

const getChartDataFormatForWeek = () => {
    const result = [];
    const firstDay = getDateXDaysAgo(6);
    const daysOfWeek = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];

    for (let i = 0; i < 7; i++) {
      let key = new Date(firstDay);
      key.setDate(key.getDate() + i);
      result.push({ name: daysOfWeek[key.getDay()], amount: 0 });
    }

    return result;
}

const getDateXDaysAgo = (days) => {
  var date = new Date();
  var last = new Date(date.getTime() - days * 24 * 60 * 60 * 1000);
  var day = last.getDate();
  var month = last.getMonth();
  var year = last.getFullYear();
  return new Date(year, month, day);
};