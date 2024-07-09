// utils/api.ts
import axios from 'axios';

const axiosInstanceUserManagement = axios.create({
    baseURL: 'https://localhost:7014',
    timeout: 5000, // Timeout after 5 seconds
    headers: {
        'Content-Type': 'application/json',
    },
});

// Axios instance for Microservice B
const axiosInstanceB = axios.create({
    baseURL: 'https://microservice-b-url/api',
    timeout: 5000,
    headers: {
        'Content-Type': 'application/json',
    },
});

export { axiosInstanceUserManagement, axiosInstanceB };
