import { Outlet, useParams } from "react-router";
import { useState, useEffect } from "react";
import styles from "./EnrolledCourse.module.scss";
import LoadingSpinner from "~/components/LoadingSpinner/LoadingSpinner";
import config from '~/config';
import axios from 'axios';
import images from "~/assets/images";
import Badge from 'react-bootstrap/Badge';
import { useLocation } from "react-router-dom";
import Units from "../Units/Units";
import { useNavigate } from "react-router";
import _ from "lodash";
import ModalConfirm from "../Exam/ModalConfirm";

export default function EnrolledCourse() {
    const { enrollementId } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const tokenStr = localStorage.getItem('accessToken');
    const [enrollement, setEnrollement] = useState({});
    const [courseInfo, setCourseInfo] = useState({});
    const [listLessons, setListLessons] = useState([]);
    const [show, setShow] = useState(false);
    const [unit, setUnit] = useState({});
    const pathname = useLocation().pathname;

    useEffect(() => {
        setIsLoading(true);
        axios.get(`${config.baseUrl}/api/enrollments/${enrollementId}`, {
            headers: { Authorization: `Bearer ${tokenStr}` }
        })
        .then((res) => {
            setEnrollement(res.data);
            setIsLoading(false);
            return axios.get(`${config.baseUrl}/api/courses/${res.data.courseId}`);
        })
        .then((res) => {
            setCourseInfo(res.data);
            return axios.get(`${config.baseUrl}/api/lessons?courseId=${res.data.courseId}`)
        })
        .then((res) => {
            return res.data;
        })
        .then(async (listLessons) => {
                const newListLessons = [];
                await Promise.all(
                    (listLessons).map(async (lesson) => {
                        const response = await axios.get(`${config.baseUrl}/api/units?lessonId=${lesson.lessonId}`);
                        const units = response.data;
                        const newLesson = { ...lesson, "units": units }
                        newListLessons.push(newLesson);
                    }),
                );
                setListLessons(newListLessons);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, [enrollementId]);

     const [open, setOpen] = useState(0);
    const handleToggleButton = (lesson) => {
        setOpen(lesson.lessonId)
    }
    
    const navigate = useNavigate();
    const handleOpenDetailUnit = (unit) => {
        setUnit(unit);
        if(unit.isExam) {
            setShow(true);
        }
        else
            navigate(`material/${unit.unitId}`)
    }
    
    if (isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div className={styles.sideBar}>
                <div style={{ display: "flex", gap: 20, marginBottom: 40, alignItems: "flex-start" }}>
                    <h5>{courseInfo.title}</h5>
                    <Badge style={{ padding: 8 }} bg="success">
                        {courseInfo.tier === 0 ? 'Free' : courseInfo.tier === 1 ? 'Premium' : ''}
                    </Badge>
                </div>
                <div>
                    {listLessons && listLessons.map((lesson) => {
                        return (
                            <>
                                <div key={lesson.lessonId} style={{ marginBottom: 20 }}>
                                    <div
                                        style={{ cursor: "pointer", display: "flex", alignItems: "center", justifyContent: "space-between" }}
                                        onClick={() => { handleToggleButton(lesson) }}
                                    >
                                        <h6>{lesson.title}</h6>
                                        <img src={images.dropDownIcon} alt="" style={{ width: 20, height: 10 }} />
                                    </div>
                                    {open == lesson.lessonId && lesson.units && lesson.units.map((unit) => (
                                        <div key={unit.unitId} style={{ paddingLeft: 20, marginTop: 12 }}>
                                            {console.log(_.includes(enrollement.completedUnitIds, unit.unitId))}
                                            <div style={{display: "flex", gap: 15}}>
                                                {(_.includes(enrollement.completedUnitIds, unit.unitId)) ? <img src={images.tickIcon} alt="done unit" style={{width: 20, height: 20}}/> : <span style={{paddingLeft: 20}}></span>}
                                                <p onClick={() => handleOpenDetailUnit(unit)} style={{ cursor: "pointer" }}>{unit.title}</p>
                                            </div>
                                        </div>
                                    ))}
                                </div>
                            </>
                        )
                    })}
                </div>
            </div>
            <div className={styles.courseInfo}>
                {
                    (pathname === `/enrolled-course/${enrollementId}`) ? 
                        <Units courseInfo={courseInfo} listLessons={listLessons} handleOpenDetailUnit={handleOpenDetailUnit} /> 
                        : <Outlet/>
                }
            </div>
            <ModalConfirm show={show} setShow={setShow} unit={unit}/>
        </div>
    );
}
