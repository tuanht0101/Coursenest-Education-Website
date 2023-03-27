import { useState, useEffect } from 'react';
import axios from 'axios';
import styles from './MyCourses.module.scss';
import config from '~/config';
import AllEnrolledCourses from '~/components/AllEnrolledCourses/AllEnrolledCourses';
import LoadingSpinner from '~/components/LoadingSpinner/LoadingSpinner';

export default function MyCourses() {
    const tokenStr = localStorage.getItem('accessToken');
    const [inProgressCourses, setInProgressCourses] = useState([]);
    const [completedCourses, setCompletedCourses] = useState([]);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        setIsLoading(true);
        axios
            .get(`${config.baseUrl}/api/enrollments`, {
                headers: { Authorization: `Bearer ${tokenStr}` },
            })
            .then((res) => {
                return res.data;
            })
            .then(async (enrollments) => {
                const allCoursesEnrollments = [];
                await Promise.all(
                    enrollments.map(async (enrollment) => {
                        const response = await axios.get(`${config.baseUrl}/api/enrollments/${enrollment.enrollmentId}`, {
                            headers: { Authorization: `Bearer ${tokenStr}` },
                        });
                        const enrollmentDetail = response.data;
                        allCoursesEnrollments.push(enrollmentDetail);
                    }),
                );
                return allCoursesEnrollments;
            })
            .then(async (allCoursesEnrollments) => {
                const allCoursesEnrollmentsDetail = [];
                await Promise.all(
                    allCoursesEnrollments.map(async (coursesEnrollment) => {
                        const response = await axios.get(`${config.baseUrl}/api/courses/${coursesEnrollment.courseId}`, {
                            headers: { Authorization: `Bearer ${tokenStr}` },
                        });
                        const courseInfo = response.data;
                        const detail = {...coursesEnrollment, ...courseInfo}
                        allCoursesEnrollmentsDetail.push(detail);
                    }),
                );
                return allCoursesEnrollmentsDetail;
                // từng enroll -> call api get course info -> sau đó mới chia 2 [] complete course và inprogress
                // nếu complete != null -> 100% 
                // nếu complete = null thì call api get count unit by course (display progress bar)
            })
            .then((allCoursesEnrollmentsDetail) => {
                const inprogress = [];
                const complete = [];
                allCoursesEnrollmentsDetail &&
                    allCoursesEnrollmentsDetail.map((coursesEnrollment) => {
                        if (coursesEnrollment.completed != null) {
                            complete.push(coursesEnrollment);
                        }
                        else {
                            inprogress.push(coursesEnrollment);
                        }
                    });
                setInProgressCourses(inprogress);
                setCompletedCourses(complete);
                return inprogress;
            })
            .then(async(res) => {
                const inprogress = [];
                await Promise.all(
                    res.map(async (item) => {
                        const completeUnits = (item.completedUnitIds).length;
                        const response = await axios.get(`${config.baseUrl}/api/units/count?courseId=${item.courseId}`, {
                            headers: { Authorization: `Bearer ${tokenStr}` },
                        })
                        const countUnits = response.data;
                        const percent = (completeUnits/countUnits)*100;
                        const newItem = {...item, "progress": percent}
                        inprogress.push(newItem);
                    }),
                );
                setInProgressCourses(inprogress);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, []);

    
    if(isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div>
                <h3>My courses</h3>
            </div>
            <div className={styles.inProgress}>
                <h4>In-Progress Courses</h4>
                <div>
                    {(inProgressCourses.length == 0) ? <p>0 result</p> : (
                        <>
                            <p>{inProgressCourses.length} results</p>
                            <AllEnrolledCourses coursesEnrollments={inProgressCourses}/>
                        </>
                    )}
                </div>
            </div>
            <div className={styles.completed}>
                <h4>Completed Courses</h4>
                <div>
                    {(completedCourses.length == 0) ? <p>0 result</p> : (
                        <>
                            <p>{completedCourses.length} results</p>
                            <AllEnrolledCourses coursesEnrollments={completedCourses}/>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
}