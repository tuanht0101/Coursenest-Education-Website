import axiosClient from './axiosClient';

const coursesApi = {
    getAll(params) {
        const url = '/courses';
        return axiosClient.get(url, { params });
    },
    get(id) {
        const url = `/courses/${id}`;
        return axiosClient.get(url);
    },
    add(data) {
        const url = '/courses';
        return axiosClient.post(url, data);
    },
    update(data) {
        const url = `/courses/${data.id}`;
        return axiosClient.put(url, data);
    },
    remove(id) {
        const url = `/courses/${id}`;
        return axiosClient.delete(url);
    },
};

export default coursesApi;
