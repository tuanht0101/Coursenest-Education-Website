import styles from "./DisplayListCourses.module.css";
import "font-awesome/css/font-awesome.min.css";
import courseAvatar from "../../assets/courseAvatarDefault.png";
import Badge from 'react-bootstrap/Badge';

function DisplayListCourses(props) {

    const {listCourses, handleClickApproveCourse, handleClickSeeCourseDetail} = props;
   
    return(
        <div>
            <table className="table table-hover">
                <thead>
                    <tr>
                        <th scope="col">Title</th>
                        <th scope="col">Description</th>
                        <th scope="col">Status</th>
                        <th scope="col">Topic</th>
                        <th scope="col">Course Tier</th>
                        <th scope="col">Actions</th>
                    </tr>
                </thead>
                <tbody>
                   { listCourses && listCourses.map((course) => {
                        return (
                            <tr className={styles.tableRow} key={course.courseId}>
                                <td className={styles.avatarWrapper}>
                                    <img src={course.cover?.uri ?? courseAvatar} className={styles.avatar} alt=""/>
                                    <div>
                                        <p>{course.title}</p>
                                    </div>
                                </td>
                                <td style={{maxWidth: 300, paddingRight: 20, paddingTop: 20}}>
                                    <p>{course.description}</p>
                                </td>
                                <td style={{paddingTop: 20}}>
                                    <Badge style={{padding: 8, width: 60}}>Pending</Badge>
                                </td>
                                <td style={{paddingTop: 20}}>
                                    <p>{course.topicTitle}</p>
                                </td>
                                <td style={{paddingTop: 20}}>
                                    {course.tier == 0 ? <Badge style={{padding: 8, width: 60}} bg="success">Free</Badge > : <Badge style={{padding: 8, width: 60}} bg="danger">Premium</Badge>}
                                </td>
                                <td>
                                    <button
                                        className={`btn btn-secondary btn-sm ${styles.action}`}
                                        onClick={() => handleClickApproveCourse(course.courseId)}
                                        title="Approve this course"
                                    >
                                        <i className="fa fa-edit"></i>
                                    </button>
                                    <button 
                                        title="See course detail"
                                        className={`btn btn-secondary btn-sm ${styles.action}`}
                                        onClick={() => handleClickSeeCourseDetail(course)}>
                                            <i className="fa fa-eye" aria-hidden="true"></i>
                                    </button>
                                </td>
                            </tr>
                        )
                   })}
                </tbody>
            </table>
        </div>
    );
}

export default DisplayListCourses;