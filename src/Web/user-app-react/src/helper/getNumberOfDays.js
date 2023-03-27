const getNumberOfDays = (date) => {
    let expiry = new Date(date);
    expiry.toISOString().substring(0, 10);
    let now = new Date();
    let difference = expiry.getTime() - now.getTime();
    let days = Math.ceil(difference / (1000 * 3600 * 24));
    if(days < 0) {
        return 0;
    }
    return days;
}
export default getNumberOfDays;