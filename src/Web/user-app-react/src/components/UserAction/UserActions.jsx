import axios from 'axios';
import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import config from '~/config';
import images from '~/assets/images';
import styles from './UserAction.module.scss';

export default function UserActions() {
    const [userInfo, setUserInfo] = useState({});
    const [isOpen, setIsOpen] = useState(false);

    const handleSignOut = () => {
        localStorage.clear();
        axios
            .post(`authenticate/logout`)
            .catch((err) => {
                console.log(err);
            })
    }

    const getInfoUser = () => {
        let userId = localStorage.getItem('userId');
        axios
            .get(`${config.baseUrl}/api/users/${userId}`)
            .then((res) => {
                setUserInfo(res.data);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    useEffect(() => {
        getInfoUser();
    }, []);

    return (
        <div className={styles.container}>
            <style>{`
                .hidden {
                    visibility: hidden;
                }
                .show {
                    display: block;
                }
            `}</style>

            <div onClick={() => setIsOpen(!isOpen)} className={styles.bdropdown}>
                <img className={styles.avatarImg} src={userInfo.avatar == null ? images.userAvatar : userInfo.avatar.uri} alt="" />
                <p>{userInfo?.fullName}</p>
                <img className={styles.dropDownImg} src={images.dropDownIcon} alt="" />
            </div>

            <div className={isOpen ? 'show' : 'hidden'}>
                <div className={styles.ddcontent}>
                    <Link to="/profile" className={`textlink ${styles.dditem}`}>
                        <p>My Profile</p>
                    </Link>
                    <Link to="/sign-in" onClick={() => handleSignOut()} className={`textlink ${styles.dditem}`}>
                        <p>Sign Out</p>
                    </Link>
                </div>
            </div>
        </div>
    );
}
