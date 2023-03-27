import React from 'react';
import { Link } from 'react-router-dom';
import appstoreImg from '~/assets/images/appstore.png';
import chplayImg from '~/assets/images/chplay.png';
import styles from './Footer.module.scss';

export default function Footer() {
    return (
        <div className={styles.footer}>
            <div className={`${styles.footercol} ${styles.footercol3}`}>
                <div>
                    <h3>Information</h3>
                    <Link to="#" className="textlink">
                        <p>About Us</p>
                    </Link>
                </div>
                <div>
                    <h3>Contact</h3>
                    <Link to="#" className="textlink">
                        <span>Contact Us</span>
                    </Link>
                    <Link to="#" className="textlink">
                        <p>Help and Support</p>
                    </Link>
                </div>
                <div>
                    <h3>Term</h3>
                    <Link to="#" className="textlink">
                        <p>Privacy policy</p>
                    </Link>
                </div>
            </div>
            <div className={`${styles.footercol} ${styles.footercol2}`}>
                <div>
                    <Link to="#">
                        <img src={appstoreImg} alt="" />
                    </Link>
                    <Link to="#">
                        <img className={styles.chplayimg} src={chplayImg} alt="" />
                    </Link>
                </div>
                <p>2022.Coursenest Copyright*</p>
            </div>
        </div>
    );
}
