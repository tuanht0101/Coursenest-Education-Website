import styles from './Achievement.module.scss';
import images from '~/assets/images';

export default function Achievement(props) {
    const { achievement } = props;
    return (
        <div className={styles.achievement}>
            <img src={images.achievementIcon} alt="" />
            <div>
                <p className={styles.achievementTitle}>{achievement.title}</p>
                <p className={styles.achievementCreated}>Achieved {new Date(achievement.created).toDateString()}</p>
            </div>
        </div>
    );
}
