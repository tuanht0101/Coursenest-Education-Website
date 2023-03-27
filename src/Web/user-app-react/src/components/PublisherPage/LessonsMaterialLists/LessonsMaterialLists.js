import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import axios from 'axios';
import classNames from 'classnames/bind';
import { useEffect } from 'react';
import { useContext, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import config from '~/config';
import CourseContext from '~/contexts/courseContext';

import styles from './LessonsMaterialLists.module.scss';

const cx = classNames.bind(styles);

function LessonsMaterialLists({ lessonsList, handleNextStep, handleTitleValue, getLessonsListOnAdd, onEdit }) {
    const [lessons, setLessons] = useState(lessonsList);
    // const { lessons, setLessons } = useContext(CourseContext);

    const navigate = useNavigate();
    let params = useParams();
    const accessToken = localStorage.getItem('accessToken');

    useEffect(() => {
        const accessToken = localStorage.getItem('accessToken');
        axios
            .get(`${config.baseUrl}/api/lessons`, {
                params: {
                    courseId: params.courseId,
                },
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            })
            .then((response) => {
                const sortedLessons = response.data.sort((a, b) => a.order - b.order);
                setLessons([...sortedLessons]);
            })
            .catch((error) => {
                console.log(error);
            });
    }, [lessonsList]);

    // const [lessonsList, setLessonsList] = useState([]);

    const moveItem = async (lessonId, direction, event) => {
        const newItems = [...lessons];
        const index = newItems.findIndex((item) => item.lessonId === lessonId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setLessons(newItems);
        event.preventDefault();
        if (direction < 0) {
            await axios
                .put(
                    `${config.baseUrl}/api/lessons/${lessonId}/order`,
                    {
                        toId: newItems[index].lessonId,
                        isBefore: true,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    console.log(`Da di chuyen lesson ${lessonId} direction buoc`);
                })
                .catch((error) => {
                    console.log(error);
                });
        } else if (direction > 0) {
            await axios
                .put(
                    `${config.baseUrl}/api/lessons/${lessonId}/order`,
                    {
                        toId: newItems[index].lessonId,
                        isBefore: false,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    console.log(`Da di chuyen lesson ${lessonId} direction buoc`);
                })
                .catch((error) => {
                    console.log(error);
                });
        }
    };

    const handleAddLessonClick = async (event) => {
        event.preventDefault();
        if (lessons.length === 0) {
            const defaultNewLesson = {
                title: `New item 1`,
                description: 'Description of item ',
            };
            await axios
                .post(
                    `${config.baseUrl}/api/lessons`,
                    {
                        title: defaultNewLesson.title,
                        description: defaultNewLesson.description,
                        courseId: params.courseId,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedLessonsList = [defaultNewLesson];
                    setLessons(addedLessonsList);
                    getLessonsListOnAdd(addedLessonsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        } else {
            const defaultNewLesson = {
                title: `New item ${lessons.length + 1}`,
                description: 'Description of item ',
            };
            await axios
                .post(
                    `${config.baseUrl}/api/lessons`,
                    {
                        title: defaultNewLesson.title,
                        description: defaultNewLesson.description,
                        courseId: params.courseId,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedLessonsList = [...lessons, defaultNewLesson];
                    setLessons(addedLessonsList);
                    getLessonsListOnAdd(addedLessonsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
        // setLessons(addedLessonsList);
        console.log(lessons);
    };

    const handleDeleteLesson = async (id) => {
        await axios.delete(`${config.baseUrl}/api/lessons/${id}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`,
            },
        });
        const newArrLesson = [...lessons.filter((item) => item.lessonId !== id)];
        setLessons(newArrLesson);
        console.log(newArrLesson);
    };

    const handleEditLesson = (lesson) => {
        onEdit(lesson);
        console.log(lesson);
        handleNextStep();
        handleTitleValue(lesson.title);
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

    let activeBtn = {
        opacity: '1',
        cursor: 'pointer',
    };

    let disableBtn = {
        opacity: '0.3',
        cursor: 'not-allowed',
    };

    return (
        <div className={cx('lessonsContainer')}>
            <div className={cx('topContainer')}>
                <p className={cx('topTitle')}>Lessons</p>
                <button className={cx('topTitleBtn')} onClick={handleAddLessonClick}>
                    {/* <Link className={cx('addLessonLink')} to="add-lesson"> */}
                    Add Lesson
                    {/* </Link> */}
                </button>
            </div>
            <ul className={cx('wrapper')}>
                {lessons.map((item, index) => (
                    <li className={cx('itemDiv')} key={index}>
                        <p className={cx('itemTitle')}>{item.title}</p>
                        <div className={cx('itemAction')}>
                            <p className={cx('btnAction')} onClick={() => handleEditLesson(item)}>
                                Edit
                            </p>
                            <p className={cx('btnAction')} onClick={() => handleDeleteLesson(item.lessonId)}>
                                Delete
                            </p>
                            <p className={cx('itemOrder')}>{index + 1}</p>
                            <div className={cx('moveBtnContainer')}>
                                <button
                                    className={cx('moveBtn')}
                                    style={lessons[lessons.indexOf(item) - 1] ? activeBtn : disableBtn}
                                    onClick={(event) =>
                                        lessons[lessons.indexOf(item) - 1]
                                            ? moveItem(item.lessonId, -1, event)
                                            : console.log('not allowed to click')
                                    }
                                >
                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronUp} />
                                </button>
                                <button
                                    className={cx('moveBtn')}
                                    style={lessons[lessons.indexOf(item) + 1] ? activeBtn : disableBtn}
                                    onClick={(event) =>
                                        lessons[lessons.indexOf(item) + 1]
                                            ? moveItem(item.lessonId, 1, event)
                                            : console.log('not allowed to click')
                                    }
                                >
                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronDown} />
                                </button>
                            </div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default LessonsMaterialLists;
