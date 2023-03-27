import axios from "axios";

const instance = axios.create({
	baseURL: `https://coursenest.corn207.loseyourip.com/api/`,
});

const token = localStorage.getItem("accessToken");

instance.defaults.headers.common["Authorization"] = `Bearer ${token}`;
instance.defaults.headers.post["Content-Type"] = "application/json";
instance.defaults.headers.put["Content-Type"] = "application/json";
// instance.defaults.headers.put['Content-Type'] = "multipart/form-data";

export default instance;
