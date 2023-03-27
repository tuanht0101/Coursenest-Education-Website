const dev = {
    baseUrl: 'https://coursenest.corn207.loseyourip.com',
};

const prod = {
    baseUrl: '',
};

const config = process.env.NODE_ENV === 'development' ? dev : prod;

export default config;
