import InterestedTopics from '~/components/InterestedTopics/InterestedTopics';
import styles from './Home.module.scss';
import { Link } from 'react-router-dom';
import Typical from 'react-typed';

export default function Home(props) {
    const { logged } = props;
    return (
        <div className={styles.container}>
            <div className={styles.slogan}>
                <h1 className={styles.title}># ONLINE COURSE EXAM</h1>
                <span className={styles.subTitle}>BY TEAM 3</span>
            </div>
            {!logged && (
                    <div className={styles.content}>
                        <h2>IT - courses</h2>
                        <h2>Online studying has never been{' '}
                            <Typical strings={[' easier.', ' faster.']} typeSpeed={150} backSpeed={120} loop />
                        </h2>
                        <p>Python, C++, C# or Java?</p>
                        <p>Which programming language do you want to start your journey as a programmer?</p>
                        <div className={styles.links}>
                            <Link to="/sign-in">
                                <button className={styles.bstudy}>Study now</button>
                            </Link>
                            <Link to="/sign-up">
                                <button className={styles.bregister}>Register</button>
                            </Link>
                        </div>
                    </div>
                )}
            <div>
                {logged && <InterestedTopics logged={logged}/>}
            </div>
        </div>
    );
}
