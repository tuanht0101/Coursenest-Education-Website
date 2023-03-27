import axiosClient from './axiosClient';

const topicsApi = {
    getAll(params) {
        const url = '/topics';
        return axiosClient.get(url, { params });
    },
    get(id) {
        const url = `/topics/${id}`;
        return axiosClient.get(url);
    },
    add(data) {
        const url = '/topics';
        return axiosClient.post(url, data);
    },
    update(data) {
        const url = `/topics/${data.id}`;
        return axiosClient.put(url, data);
    },
    remove(id) {
        const url = `/topics/${id}`;
        return axiosClient.delete(url);
    },
};

export default topicsApi;
