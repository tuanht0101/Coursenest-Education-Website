import { Outlet, useParams } from "react-router";
import instance from "../../../api/request";
import LoadingSpinner from "../../LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";
import styles from "./UnapprovedCourse.module.css";
import Badge from 'react-bootstrap/Badge';
import arrowDown from "../../../assets/black-arrow-down.png"
import { useNavigate } from "react-router-dom";
import Modal from "react-bootstrap/Modal";
import Button from "react-bootstrap/Button";

export default function UnapprovedCourse() {
    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [courseInfo, setCourseInfo] = useState({});
    const [listLessons, setListLessons] = useState([])

    useEffect(() => {
       setIsLoading(true);
       instance
            .get(`courses/${id}`)
            .then((res) => {
                setCourseInfo(res.data);
                return instance.get(`lessons?courseId=${id}`)
            })
            .then((res) => {
                return res.data;
            })
            .then(async (listLessons) => {
                const newListLessons = [];
                await Promise.all(
                    (listLessons).map(async (lesson) => {
                        const response = await instance.get(`units?lessonId=${lesson.lessonId}`);
                        const units = response.data;
                        const newLesson = {...lesson, "units": units}
                        newListLessons.push(newLesson);
                    }),
                );
                setListLessons(newListLessons);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, [id]);

    const [open, setOpen] = useState(0);
    const handleToggleButton = (lesson) => {
        setOpen(lesson.lessonId)
    }

    const navigate = useNavigate();
    const handleOpenDetailUnit = (unit) => {
        if(unit.isExam)
            navigate(`exam/${unit.unitId}`)
        else
            navigate(`material/${unit.unitId}`)
    }

    const [courseNeedApprove, setCourseNeedApprove] = useState(0);
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);

    const handleClickApproveCourse = (courseId) => {
        setShow(true);
        setCourseNeedApprove(courseId);
    }

    const handleConfirmApproveCourse = () => {
        instance
            .put(`courses/${courseNeedApprove}/approve`)
            .then(() => {
                handleClose();
                navigate(`/courses`);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    if(isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div className={styles.sideBar}>
                <div style={{display: "flex", gap: 20, marginBottom: 40}}>
                    <h5>{courseInfo.title}</h5>
                    <Badge style={{padding: 8}} bg="success">
                        {courseInfo.tier === 0 ? 'Free' : courseInfo.tier === 1 ? 'Premium' : ''}
                    </Badge>
                </div>
                <div>
                    {listLessons && listLessons.map((lesson) => {
                        return (
                            <>
                                <div key={lesson.lessonId} style={{marginBottom: 20}}>
                                    <div 
                                        style={{cursor: "pointer", display: "flex", alignItems: "center", justifyContent: "space-between"}}  
                                        onClick={() => {handleToggleButton(lesson)}}>
                                        <h6>{lesson.title}</h6>
                                        <img src={arrowDown} alt="" style={{width: 30}}/>
                                    </div>
                                    {open == lesson.lessonId && lesson.units && lesson.units.map((unit) => (
                                        <div key={unit.unitId} style={{paddingLeft: 20, marginTop: 12}}>
                                            <p onClick={() => handleOpenDetailUnit(unit)} style={{cursor: "pointer"}}>{unit.title}</p>
                                        </div>
                                    ))}
                                </div>
                            </>
                        )
                    })}
                    <div style={{marginTop: 20}}>
                        <button
                            className={`btn btn-primary btn-sm`}
                            onClick={() => handleClickApproveCourse(id)}
                            title="Approve this course"
                        >
                            Approve
                        </button>
                    </div>
                </div>
            </div>
            <div className={styles.courseInfo}>
                <Outlet />
            </div>
            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
                size="md"
            >
                <Modal.Header closeButton>
                    <Modal.Title>Approve this course ?</Modal.Title>
                </Modal.Header>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancel
                    </Button>
                    <Button
                        variant="success"
                        onClick={() => {
                            handleConfirmApproveCourse();
                        }}
                    >
                        Approve
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}
