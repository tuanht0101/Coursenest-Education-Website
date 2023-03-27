import { faChevronDown, faChevronRight, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import axios from 'axios';
import classNames from 'classnames/bind';
import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import config from '~/config';
import EditExam from './EditExam';

import styles from './EditLesson.module.scss';
import EditMaterial from './EditMaterial';

const cx = classNames.bind(styles);

function EditLesson({ chosenLesson, titleValue, lessonTitle, handleLessonUpdate, handleBackStep }) {
    const [lesson, setLesson] = useState(chosenLesson);
    const [chosenUnit, setChosenUnit] = useState(null);
    const [chosenExam, setChosenExam] = useState(null);
    const [materials, setMaterials] = useState([]);
    const [exams, setExams] = useState([]);
    const [lessonEditTitle, setLessonEditTitle] = useState(lessonTitle);
    const [isEditingTitle, setIsEditingTitle] = useState(false);
    const [stepLesson, setStepLesson] = useState(0);
    const [lessonDesc, setLessonDesc] = useState(chosenLesson.description);
    const [isEditingDesc, setIsEditingDesc] = useState(false);
    const [timeDefault, setTimeDefault] = useState(45);

    const fetchUnits = async () => {
        await axios
            .get(`${config.baseUrl}/api/units`, {
                params: {
                    lessonId: lesson.lessonId,
                },
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            })
            .then((response) => {
                const filteredDataMate = response.data.filter((item) => !item.isExam);
                const filteredDataExam = response.data.filter((item) => item.isExam);
                const orderedData = response.data.sort((a, b) => a.order - b.order);
                setMaterials(orderedData);
                // setExams(filteredDataExam);
                // console.log(filteredDataMate);
            })
            .catch((error) => {
                console.log(error);
            });
    };

    useEffect(() => {
        fetchUnits();
    }, [lesson]);

    const navigate = useNavigate();
    let params = useParams();
    const accessToken = localStorage.getItem('accessToken');

    const handleEditMaterialClick = () => {
        setStepLesson(1);
    };

    const handleEditExamClick = () => {
        setStepLesson(2);
    };

    const handleAddExamClick = async () => {
        // setStepLesson(2);
        if (exams.length === 0) {
            const defaultNewExam = {
                title: `New exam 1`,
                requiredMinutes: timeDefault,
                lessonId: lesson.lessonId,
            };
            await axios
                .post(
                    `${config.baseUrl}/api/units/exam`,
                    {
                        title: defaultNewExam.title,
                        requiredMinutes: defaultNewExam.requiredMinutes,
                        lessonId: defaultNewExam.lessonId,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedExamsList = [defaultNewExam];
                    setExams(addedExamsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        } else {
            const defaultNewExam = {
                title: `New exam ${exams.length + 1}`,
                requiredMinutes: timeDefault,
                lessonId: lesson.lessonId,
            };
            await axios
                .post(
                    `${config.baseUrl}/api/units/exam`,
                    {
                        title: defaultNewExam.title,
                        requiredMinutes: defaultNewExam.requiredMinutes,
                        lessonId: defaultNewExam.lessonId,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedMaterialsList = [...exams, defaultNewExam];
                    setExams(addedMaterialsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
        fetchUnits();
        console.log(materials);
    };

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleTitleChange = (event) => {
        setLessonEditTitle(event.target.value);
        setLesson({ ...lesson, title: event.target.value });
    };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleDescClick = () => {
        setIsEditingDesc(true);
    };

    const handleDescChange = (event) => {
        setLessonDesc(event.target.value);
        setLesson({ ...lesson, description: event.target.value });
    };

    const handleDescBlur = () => {
        setIsEditingDesc(false);
    };

    const handleCancelClick = () => {
        setStepLesson(0);
    };

    const moveItem = async (unitId, direction, event) => {
        const newItems = [...materials];
        const index = newItems.findIndex((item) => item.unitId === unitId);
        const temp = newItems[index];
        newItems[index] = newItems[index + direction];
        newItems[index + direction] = temp;
        setMaterials(newItems);
        event.preventDefault();

        if (direction < 0) {
            await axios
                .put(
                    `${config.baseUrl}/api/units/${unitId}/order`,
                    {
                        toId: newItems[index].unitId,
                        isBefore: true,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    console.log(`Da di chuyen lesson ${unitId} direction buoc`);
                })
                .catch((error) => {
                    console.log(error);
                });
        } else if (direction > 0) {
            await axios
                .put(
                    `${config.baseUrl}/api/units/${unitId}/order`,
                    {
                        toId: newItems[index].unitId,
                        isBefore: false,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    console.log(`Da di chuyen lesson ${unitId} direction buoc`);
                })
                .catch((error) => {
                    console.log(error);
                });
        }
    };

    const handleAddMaterialClick = async () => {
        if (materials.length === 0) {
            const defaultNewMaterial = {
                title: `New item 1`,
                requiredMinutes: timeDefault,
                lessonId: lesson.lessonId,
                content: 'Content of item 1',
            };
            await axios
                .post(
                    `${config.baseUrl}/api/units/material`,
                    {
                        title: defaultNewMaterial.title,
                        requiredMinutes: defaultNewMaterial.requiredMinutes,
                        lessonId: defaultNewMaterial.lessonId,
                        content: defaultNewMaterial.content,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedMaterialsList = [defaultNewMaterial];
                    setMaterials(addedMaterialsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        } else {
            const defaultNewMaterial = {
                title: `New item ${materials.length + 1}`,
                requiredMinutes: timeDefault,
                lessonId: lesson.lessonId,
                content: `Content of item ${materials.length + 1}`,
            };
            await axios
                .post(
                    `${config.baseUrl}/api/units/material`,
                    {
                        title: defaultNewMaterial.title,
                        requiredMinutes: defaultNewMaterial.requiredMinutes,
                        lessonId: defaultNewMaterial.lessonId,
                        content: defaultNewMaterial.content,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedMaterialsList = [...materials, defaultNewMaterial];
                    setMaterials(addedMaterialsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
        fetchUnits();
        console.log(materials);
    };

    const handleDeleteMaterial = async (id) => {
        await axios
            .delete(`${config.baseUrl}/api/units/${id}`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            })
            .then((response) => {
                const newArrLesson = [...materials.filter((item) => item.unitId !== id)];
                setMaterials(newArrLesson);
                console.log(newArrLesson);
                console.log(`da~ xoa unit co id la ${id}`);
            })
            .catch((error) => {
                console.error(error);
            });
    };

    const handleEditMaterial = async (item) => {
        if (item.isExam) {
            handleEditExamClick();
            setChosenExam(item);
        } else {
            handleEditMaterialClick();
            setChosenUnit(item);
            console.log(item);
        }

        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson/edit-material`);
    };

    // const handleEditExam = (item) => {
    //     handleEditExamClick();
    //     setChosenExam(item);
    //     console.log(item);
    //     // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson/edit-material`);
    // };

    const handleCancel = () => {
        handleBackStep();
        // navigate(`/publisher/${params.PublisherUserId}/add-course`);
    };

    const handleConfirm = async () => {
        await axios
            .put(
                `${config.baseUrl}/api/lessons/${lesson.lessonId}`,
                {
                    title: lessonEditTitle,
                    description: lessonDesc,
                },
                {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                },
            )
            .then((response) => {
                handleBackStep();
                handleLessonUpdate(lesson);
                console.log(`da~ cap nhat lesson co id la ${lesson.lessonId}`);
            })
            .catch((error) => {
                console.error(error);
            });
        // navigate(`/publisher/${params.PublisherUserId}/add-course`);
    };

    const handleMaterialUpdate = (updatedMaterial) => {
        // setChosenMaterial(updatedMaterial);
        const updatedMaterials = materials.map((material) => {
            if (material.unitId === updatedMaterial.unitId) {
                return updatedMaterial;
            } else {
                return material;
            }
        });
        setMaterials(updatedMaterials);
        console.log(updatedMaterials);
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
        <>
            {stepLesson === 0 && (
                <div className={cx('wrapper')}>
                    <div className={cx('topTitle')}>
                        <p className={cx('courseTitle')}>{titleValue}</p>
                        <div>
                            <FontAwesomeIcon className={cx('icon')} icon={faChevronRight} />
                        </div>
                        <p className={cx('lessonTitle')}>{lessonEditTitle}</p>
                    </div>
                    <div className={cx('lessonBody')}>
                        {isEditingTitle ? (
                            <input
                                type="text"
                                className={cx('titleInput')}
                                value={lessonEditTitle}
                                onChange={handleTitleChange}
                                onBlur={handleTitleBlur}
                                autoFocus
                            />
                        ) : (
                            <p className={cx('lessonTitleDetail')} onClick={handleTitleClick}>
                                {'Title: ' + lessonEditTitle}
                            </p>
                        )}
                        {isEditingDesc ? (
                            <input
                                type="text"
                                className={cx('titleInput')}
                                value={lessonDesc}
                                onChange={handleDescChange}
                                onBlur={handleDescBlur}
                                autoFocus
                            />
                        ) : (
                            <p className={cx('lessonDesc')} onClick={handleDescClick}>
                                {lessonDesc}
                            </p>
                        )}
                        <div className={cx('unitsBody')}>
                            <div className={cx('unitsTopBody')}>
                                <p className={cx('unitsTitle')}>Units</p>
                                <div className={cx('unitsBtns')}>
                                    <button className={cx('unitsBtn')} onClick={handleAddMaterialClick}>
                                        Add Material
                                    </button>
                                    <button className={cx('unitsBtn')} onClick={handleAddExamClick}>
                                        Add Exam
                                    </button>
                                </div>
                            </div>
                            <p className={cx('listTitle')}>Material</p>
                            <ul className={cx('listWrapper')}>
                                {materials.map((item, index) => (
                                    <li className={cx('itemDiv')} key={index}>
                                        <p className={cx('itemTitle')}>{item.title}</p>
                                        <div className={cx('itemAction')}>
                                            <p className={cx('btnAction')} onClick={() => handleEditMaterial(item)}>
                                                Edit
                                            </p>
                                            <p
                                                className={cx('btnAction')}
                                                onClick={() => handleDeleteMaterial(item.unitId)}
                                            >
                                                Delete
                                            </p>
                                            <p className={cx('itemOrder')}>{index + 1}</p>
                                            <div className={cx('moveBtnContainer')}>
                                                <button
                                                    className={cx('moveBtn')}
                                                    style={
                                                        materials[materials.indexOf(item) - 1] ? activeBtn : disableBtn
                                                    }
                                                    onClick={(event) =>
                                                        materials[materials.indexOf(item) - 1]
                                                            ? moveItem(item.unitId, -1, event)
                                                            : console.log('not allowed to click')
                                                    }
                                                >
                                                    <FontAwesomeIcon className={cx('fontIcon')} icon={faChevronUp} />
                                                </button>
                                                <button
                                                    className={cx('moveBtn')}
                                                    style={
                                                        materials[materials.indexOf(item) + 1] ? activeBtn : disableBtn
                                                    }
                                                    onClick={(event) =>
                                                        materials[materials.indexOf(item) + 1]
                                                            ? moveItem(item.unitId, 1, event)
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
                    </div>
                    <CancelConfirmBtns onConfirm={handleConfirm} onCancel={handleCancel} />
                </div>
            )}
            {stepLesson === 1 && (
                <EditMaterial
                    chosenMaterial={chosenUnit}
                    handleBackStep={handleCancelClick}
                    handleMaterialsUpdate={handleMaterialUpdate}
                />
            )}
            {stepLesson === 2 && <EditExam chosenExam={chosenExam} handleBackStep={handleCancelClick} />}
        </>
    );
}

export default EditLesson;
