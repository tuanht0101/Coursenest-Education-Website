
import { useState } from "react";
import React from 'react';
import instance from "../../api/request";
import styles from './Login.module.css';

export default function Login(props) {

    const { setAccessToken } = props;

    const [info, setInfo] = useState({
        username: "",
        password: ""
    })

    const handleChange = (event) => {
        setInfo(prevState => ({
            ...prevState,
            [event.target.name]: event.target.value
        }));
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        userLoginFetch(info);
    }

    const userLoginFetch = () => {
        instance.post(`authenticate/login`, info)
            .then((res) => {
                return res.data;
            })
            .then(async (res) => { 
                const token = res.accessToken;
                const getRoles = await instance.get(`roles/me`, {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });

                if (getRoles.data.find(item => item.type === 3)) {
                    setAccessToken(token);
                    localStorage.setItem('userId', res.userId);
                    window.location.href = '/'
                }          
            })
            .catch((err) => {
                console.log("fail:", err);
                alert("The Username or Password is Incorrect");
            });
    }

    return (
        <div className={styles.container}>
            <div className={styles.loginForm}>
                <h2>Login</h2>

                <form onSubmit={handleSubmit}>

                    <div className={styles.formGroup}>
                        <label className={styles.formGroupLabel}>Username</label><br />
                        <input
                            className={styles.formGroupInput}
                            name='username'
                            value={info.username}
                            onChange={handleChange}
                            required
                        /><br />
                    </div>

                    <div className={styles.formGroup}>
                        <label className={styles.formGroupLabel}>Password</label><br />
                        <input
                            className={styles.formGroupInput}
                            type='password'
                            name='password'
                            value={info.password}
                            placeholder="Password"
                            onChange={handleChange}
                            required
                        /><br />
                    </div>

                    <input type='submit' className={styles.button} />
                </form>
            </div>
        </div>
    );
}