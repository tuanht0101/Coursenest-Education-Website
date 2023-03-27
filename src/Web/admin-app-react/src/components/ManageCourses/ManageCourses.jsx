import React, { useState, useEffect, useMemo } from "react";
import instance from '../../api/request';
import { Pagination } from "rsuite/";
import Search from "../Search";
import styles from "./ManageCourses.module.css";
import DisplayListCourses from "../DisplayListCourses/DisplayListCourses";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import { useNavigate } from 'react-router-dom';
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";

export default function ManageCourses() {

    const [listCourses, setListCourses] = useState([]);
    const [countCourses, setCountCourses] = useState();
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(5);
    const [keyword, setKeyWord] = useState();
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        fetchListCourses();
    }, [page, pageSize]);

    const fetchListCourses = () => {
        setIsLoading(true);
        instance
            .get(`/courses?IsApproved=${false}&PageNumber=${page}&PageSize=${pageSize}`)
            .then((res) => {
                setListCourses(res.data.queried);
                setCountCourses(res.data.total)
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    };

    
    function getFilteredListCourse() {
        if (!keyword) {
            return listCourses;
        }
        return listCourses.filter((course) => {
            return (
                course.title.toLowerCase().indexOf(keyword.toLowerCase()) !== -1
            );
        });
    }

    var filteredListCourse = useMemo(getFilteredListCourse, [keyword, listCourses]);


    const handleOnChangePage = (event) => {
        setPage(parseInt(event));
    };

    const handleOnChangePageSize = event => {
        setPageSize(parseInt(event.target.value));
    };

    const navigate = useNavigate();
    const handleClickSeeCourseDetail = (course) => {
        navigate(`/courses/${course.courseId}`);
    };

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
                fetchListCourses();
            })
            .catch((err) => {
                console.log(err);
            })
    }

    const options = [
        { value: '5', text: '5 courses/page' },
        { value: '50', text: '50 courses/page' },
        { value: '100', text: '100 courses/page' }
    ];
    
    if(isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <Search setKeyWord={setKeyWord} />
                <div className={styles.courseCount}>
                    <span>Total: {countCourses} Courses</span>
                </div>
                <div>
                    <select value={pageSize} onChange={handleOnChangePageSize}>
                        {options.map(option => (
                            <option key={option.value} value={option.value}>
                                {option.text}
                            </option>
                        ))}
                    </select>
                </div>
            </div>
            <DisplayListCourses
                listCourses={filteredListCourse}
                handleClickApproveCourse={handleClickApproveCourse}
                handleClickSeeCourseDetail={handleClickSeeCourseDetail}
            />
            <div className={styles.pagination}>
                <div className={styles.center}>
                    <Pagination
                        prev
                        last
                        next
                        first
                        size="lg"
                        total={countCourses}
                        limit={pageSize}
                        maxButtons={5}
                        activePage={page}
                        onChangePage={handleOnChangePage}
                    />
                </div>
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
    )
}