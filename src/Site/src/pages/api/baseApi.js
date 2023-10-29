import axios from "axios";
import { toast } from 'react-toastify';
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import { useNavigate } from "react-router-dom";

const baseURL = 'http://netbassessment.eastus.cloudapp.azure.com:8080/';
// const baseURL = 'https://localhost:7294/';

export const handleCatch = (error) => {
    if (error.response?.data?.errors) {

        toast.error(() => (
            <ul style={{ listStyle: "none"}}>
                {error.response?.data?.errors.map((item, index) => (
                    <li key={index}>
                        <span style={{
                            fontWeight: 'bold',
                            fontSize: '10px',
                            marginLeft: '10px',
                        }}>{item}</span>

                    </li>
                ))}
            </ul>
        ));
    } else {
        toast.error(error.message);
    }
}
export const handleSuccess = (success) => {
    toast.success(() => (
        <span style={{
            fontWeight: 'bold',
            fontSize: '10px',
            marginLeft: '10px',
        }}>{success}</span>
    ));
}
export const api = axios.create({
    baseURL: baseURL
})

api.interceptors.request.use(
    async config => {
        const credential = localStorage.getItem("credentials");

        if (credential) {

            var recoveredCredential = JSON.parse(credential)
            var expired = new Date(recoveredCredential.expiresIn) <= new Date()
            if (expired) 
            {
                axios.create({
                    baseURL: baseURL
                }).post('/Authentication/RefreshToken', { accessToken: recoveredCredential.accessToken, refreshToken: recoveredCredential.refreshToken })
                    .then((res) => {
                        localStorage.setItem("credentials", JSON.stringify(res.data.data));
                        config.headers['Authorization'] = `Bearer ${res.data.data.accessToken}`;
                        return config;

                    })
                    .catch((err) => {
                        handleCatch(err.message);
                        localStorage.removeItem("credentials");
                        window.location.href = "/signIn"
                    })
            }
            config.headers['Authorization'] = `Bearer ${recoveredCredential.accessToken}`;
        }
        return config;
    },
    error => Promise.reject(handleCatch(err.message))
);