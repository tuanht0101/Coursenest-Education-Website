import Badge from 'react-bootstrap/Badge';
import styles from './AllCoursesByTopic.module.scss';
import StarRatings from 'react-star-ratings';
import { useNavigate } from 'react-router-dom';

export default function AllCoursesByTopic(props) {
    const { listCourses } = props;

    const navigate = useNavigate();
    const handleShowCourseDetail = (courseId) => {
        navigate(`/courses/${courseId}`);
    }

    return (
        <div className={styles.container}>
            {listCourses &&
                listCourses.map((course) => {
                    return (
                        <div className={styles.courseCard} key={course.courseId} onClick={() => handleShowCourseDetail(course.courseId)}>
                            <div>
                                <img
                                    className={styles.imageCourse}
                                    src={course?.cover?.uri == null ? '' : course.cover.uri}
                                    alt=""
                                />
                            </div>
                            <div className={`styles.child ${styles.childThree}`}>
                                <h4>{course.title}</h4>
                                <p>{course.description}</p>
                                <div className={styles.rating}>
                                    <span style={{ color: '#FFC069' }}>{course.ratingAverage}</span>
                                    <span>
                                        <StarRatings
                                            starRatedColor="#FFC069"
                                            rating={course.ratingAverage}
                                            starDimension="20px"
                                            starSpacing="4px"
                                            numberOfStars={5}
                                        />
                                    </span>
                                    <span>({course.ratingTotal})</span>
                                </div>
                            </div>
                            <div className={styles.tier}>
                                <Badge className={styles.badge} bg="success">
                                    {course.tier === 0 ? 'Free' : course.tier === 1 ? 'Premium' : ''}
                                </Badge>
                            </div>
                        </div>
                    );
                })}
        </div>
    );
}
