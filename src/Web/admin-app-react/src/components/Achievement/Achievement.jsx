import styles from './Achievement.module.css';
import achievementIcon from '../../assets/achievementIcon.png';

export default function Achievement (props) {
    const { achievement } = props;
    return (
        <div className={styles.achievement}>
            <img src={achievementIcon} alt="" />
            <div>
                <p className={styles.achievementTitle}>{achievement.title}</p>
                <p className={styles.achievementCreated}>
                    Achieved {new Date(achievement.created).toDateString()}
                </p>
            </div>
        </div>
    );
}