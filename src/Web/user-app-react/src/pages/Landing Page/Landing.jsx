import React from 'react';
import { Link } from 'react-router-dom';
import Typical from 'react-typed';

import logowhite from '~/assets/images/logo-white.png';
import tickicon from '~/assets/images/shield-tick.png';
import styles from './Landing.module.css';
import GuestActions from '~/components/UserAction/GuestActions';

export default function Landing() {
    return (
        <div className={styles.landingpg}>
            <div className={styles.nav}>
                <div className={styles.navleft}>
                    <img src={logowhite} className={styles.logowhite} alt="" />
                    <Link to="/home" className={`textlink ${styles.home}`}>
                        <p>Home</p>
                    </Link>
                </div>
                <div className={styles.navright}>
                    <GuestActions />
                </div>
            </div>
            <div className={styles.content}>
                <h1 className={styles.title}>IT - courses</h1>
                <h2 className={styles.slogan}>
                    Online studying has never been{' '}
                    <Typical strings={[' easier.', ' faster.']} typeSpeed={150} backSpeed={120} loop />
                </h2>
                <p className={styles.intro}>Python, C++, C# or Java?</p>
                <p className={styles.intro}>
                    Which programming language do you want <br /> to start your journey as a <br /> programmer?
                </p>
                <Link to="/sign-in">
                    <button className={styles.bstudy}>Study now</button>
                </Link>
                <Link to="/sign-up">
                    <button className={styles.bregister}>Register</button>
                </Link>
                <div className={styles.advantages}>
                    <img src={tickicon} alt="" />
                    <span>Free register</span>
                    <img src={tickicon} alt="" />
                    <span>Great service</span>
                </div>
            </div>
        </div>
    );
}
