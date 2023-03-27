import axios from 'axios';
import classNames from 'classnames/bind';
import { useState } from 'react';
import MultiChoicesQuesTion from '~/components/MultiChoicesQuesTion';
import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import config from '~/config';

import styles from './EditExam.module.scss';
import EditQuestion from './EditQuestion';

const cx = classNames.bind(styles);

function EditExam({ chosenExam, handleBackStep }) {
    const [examTitle, setExamTitle] = useState(chosenExam.title);
    const [isEditingTitle, setIsEditingTitle] = useState(false);
    const [isEditingTime, setIsEditingTime] = useState(false);
    const [timeLimit, setTimeLimit] = useState(chosenExam.requiredMinutes);
    const [questionList, setQuestionList] = useState([]);
    const [chosenQuestion, setChosenQuestion] = useState(null);
    const [step, setStep] = useState(0);

    const accessToken = localStorage.getItem('accessToken');

    const handleEditQuestionClick = (item) => {
        setStep(1);
        setChosenQuestion(item);
    };

    const handleQuestion = (arr) => {
        setQuestionList(arr);
        console.log(arr);
    };

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleTitleChange = (event) => {
        setExamTitle(event.target.value);
        // setMaterial({ ...material, Title: event.target.value });
    };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleTimeClick = () => {
        setIsEditingTime(true);
    };

    const handleTimeChange = (event) => {
        const time = parseInt(event.target.value);
        if (!isNaN(time)) {
            setTimeLimit(time);
        }
        // setMaterial({ ...material, Time: event.target.value }); <----- To-do
    };

    const handleTimeBlur = () => {
        setIsEditingTime(false);
    };

    const handleCancel = () => {
        handleBackStep();
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

    const handleCancelQuestion = () => {
        setStep(0);
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

    const handleConfirm = async () => {
        await axios
            .put(
                `${config.baseUrl}/api/units/${chosenExam.unitId}/exam`,
                {
                    title: examTitle,
                    requiredMinutes: timeLimit,
                },
                {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                },
            )
            .then((res) => {
                handleBackStep();
                console.log(chosenExam.unitId);
                console.log({
                    title: examTitle,
                    requiredMinutes: timeLimit,
                });
            })
            .catch((err) => {
                console.log(err);
                console.log(chosenExam);
                console.log({
                    title: examTitle,
                    requiredMinutes: timeLimit,
                });
            });
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

    return (
        <>
            {step === 0 && (
                <div className={cx('wrapper')}>
                    <div className={cx('topContent')}>
                        {isEditingTitle ? (
                            <input
                                type="text"
                                className={cx('titleInput')}
                                value={examTitle}
                                onChange={handleTitleChange}
                                onBlur={handleTitleBlur}
                                autoFocus
                            />
                        ) : (
                            <p className={cx('examTitle')} onClick={handleTitleClick}>
                                {examTitle}
                            </p>
                        )}
                        {isEditingTime ? (
                            <input
                                type="text"
                                className={cx('TimeInput')}
                                value={timeLimit}
                                onChange={handleTimeChange}
                                onBlur={handleTimeBlur}
                                autoFocus
                            />
                        ) : (
                            <p className={cx('timeLimit')} onClick={handleTimeClick}>
                                Time: {timeLimit}:00
                            </p>
                        )}
                    </div>
                    <div className={cx('bodyContainer')}>
                        <MultiChoicesQuesTion
                            editingExam={chosenExam}
                            title={'Question'}
                            addBtnName={'Add Question'}
                            onHandleQuestionList={handleQuestion}
                            onEditClick={handleEditQuestionClick}
                        />
                    </div>
                    <CancelConfirmBtns onCancel={handleCancel} onConfirm={handleConfirm} />
                </div>
            )}
            {step === 1 && (
                <EditQuestion
                    chosenExam={chosenExam}
                    chosenQuestion={chosenQuestion}
                    handleBackStep={handleCancelQuestion}
                />
            )}
        </>
    );
}

export default EditExam;
