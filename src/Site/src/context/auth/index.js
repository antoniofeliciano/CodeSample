import React, { useState, createContext, useContext, useEffect } from "react";
import { useNavigate, Navigate } from "react-router-dom";
import PropTypes from 'prop-types';
import jwt_decode from 'jwt-decode';
import { LinearProgress } from '@material-ui/core';
import { SignInRequest } from "pages/api/authApi";
import { ProfilePicture } from "pages/api/userApi";
import { TenantData } from "pages/api/tenantApi";


const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [user, setUser] = useState();
    const [userProfilePicture, setUserProfilePicture] = useState();
    const [tenantData, setTenantData] = useState();

    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    async function fetchContextData() {
        await ProfilePicture((res) => setUserProfilePicture(res.data.data));
        await TenantData((res) => setTenantData(res.data.data));
    }

    useEffect(() => {
        try {
            const credential = localStorage.getItem("credentials");
            if (credential) {
                const recoveredCredential = JSON.parse(credential);
                const decodedToken = jwt_decode(recoveredCredential.accessToken);
                setUser(decodedToken);
                fetchContextData().then();
            }
            else{
                navigate("/signIn");
            }
        } catch (error) {
            console.log("Error fetching user data:", error);
        } finally {
            setLoading(false);
        }
        
    }, []);

    const signIn = (email, password) => {
        SignInRequest({ email, password })
            .then((res) => {
                localStorage.setItem("credentials", JSON.stringify(res.data.data));
                const decodedToken = jwt_decode(res.data.data.accessToken);
                setUser(decodedToken);
                fetchContextData();
                navigate("/");
            })
            .catch((err) => {
                console.log(err);
            });
    };

    const signOut = () => {
        localStorage.removeItem("credentials")
        setUser(null);
        navigate("/signIn");
    };

    return (
        <AuthContext.Provider
            value={{ authenticated: !!user, user, userProfilePicture, tenantData, loading, signIn, signOut }}
        >
            {children}
        </AuthContext.Provider>
    );
};
const Private = ({ children }) => {
    const { authenticated, loading, tenantData } = useContext(AuthContext);
    if (loading || tenantData == undefined) {
        return <LinearProgress style={{backgroundColor: "blue"}}/>
    }
    if (!authenticated && window.location.pathname != '/signIn') {
        return <Navigate to={"/signIn"} />
    }

    return children
}

Private.propTypes = {
    children: PropTypes.node,
};

AuthProvider.propTypes = {
    children: PropTypes.node,
};

export {
    AuthContext, AuthProvider, Private
}