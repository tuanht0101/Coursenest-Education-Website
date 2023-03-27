import React, { useState } from 'react';
import { Link } from 'react-router-dom';

import userAvatar from "../../assets/images/user-avatar.png"
import dropDownIcon from "../../assets/images/downDropIcon.png";

import styles from './UserAction.module.scss';

export default function GuestActions() {

    const [isOpen, setIsOpen] = useState(false);
    return (
        <div className={styles.container}>
            <div onClick={() => setIsOpen(!isOpen)} className={styles.bdropdown}>
                <img className={styles.avatarImg} src={userAvatar} alt="" />
                <p>Guest</p>
                <img className={styles.dropDownImg} src={dropDownIcon} alt="" />
            </div>
            <style>{`
                .hidden {
                    visibility: hidden;
                }
                .show {
                    display: block;
                }
            `}</style>
            <div className={isOpen ? 'show' : 'hidden'}>
                <div className={styles.ddcontent}>
                    <Link to="/sign-up" className={`textlink ${styles.dditem}`}>
                        <p>Sign Up</p>
                    </Link>
                    <Link to="/sign-in" className={`textlink ${styles.dditem}`}>
                        <p>Sign In</p>
                    </Link>
                </div>
            </div>
        </div>
    );
}
