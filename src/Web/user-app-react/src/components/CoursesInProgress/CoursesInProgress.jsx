import { useState, useEffect } from "react";
import styles from './CourseInProgress.module.scss';
import axios from "axios";

export default function CoursesInProgress(props) {

    const [inprogressCourses, setInprogressCourses] = useState([]);

    // call api get courses in progress of userId
    useEffect(() => {
        axios
            .get(`http://localhost:21002/`)
            .then((res) => {
                setInprogressCourses(res.data);
            })
            .catch((err) => console.log(err));
    }, []);

    const handleClickReviewCourse = () => {

    }

    return (
        <div>
            <div>
                <h3>In-Progress Courses</h3>
                <p>See all</p>
                {/* click see all -> MyCourses page */}
            </div>
            <div>
                <button
                    className={styles.reviewButton}
                    onClick={() => handleClickReviewCourse()}
                >
                    Review
                </button>
            </div>
        </div>
    );
}