import { faPlus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import axios from 'axios';
import classNames from 'classnames/bind';
import { useEffect, useState } from 'react';
import config from '~/config';
import EditQuestion from '~/pages/Publisher/AddCourses/EditLesson/EditExam/EditQuestion/EditQuestion';

import styles from './MultiChoicesQuesTion.module.scss';

const cx = classNames.bind(styles);

function MultiChoicesQuesTion({ editingExam, title, addBtnName, onHandleQuestionList, onEditClick }) {
    const [questions, setQuestions] = useState([]);
    const [answers, setAnswers] = useState([]);

    const accessToken = localStorage.getItem('accessToken');

    const fetchExam = async () => {
        axios
            .get(`${config.baseUrl}/api/units/${editingExam.unitId}/exam`, {
                headers: {
                    Authorization: `Bearer ${accessToken}`,
                },
            })
            .then((response) => {
                setQuestions(response.data.questions);
            })
            .catch((error) => {
                console.log(error);
            });
    };

    useEffect(() => {
        fetchExam();
    }, [editingExam]);

    // const handleEditQuestionClick = () => {
    //     setStep(1);
    // };

    const handleEditQuestion = (item) => {
        onEditClick(item);
        // setChosenQuestion(item);
        console.log(item);
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson/edit-material`);
    };

    const handleAnswerChange = (questionIndex, answerIndex) => {
        setAnswers((prevAnswers) => {
            const newAnswers = [...prevAnswers];
            newAnswers[questionIndex] = answerIndex;
            return newAnswers;
        });
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            const newQuestion = { ...newQuestionList[questionIndex] };
            newQuestion.choices = newQuestion.choices.map((choice, index) => {
                return {
                    ...choice,
                    isCorrect: index === answerIndex,
                };
            });
            newQuestionList[questionIndex] = newQuestion;
            return newQuestionList;
        });
    };

    const handleAddQuestionClick = async (event) => {
        event.preventDefault();
        if (questions.length === 0) {
            const defaultNewQuestion = {
                // LessonId: 1,
                content: `New question 1`,
                point: 1,
                choices: [
                    {
                        content: 'answer 1',
                        isCorrect: true,
                    },
                    {
                        content: 'answer 2',
                        isCorrect: false,
                    },
                    {
                        content: 'answer 3',
                        isCorrect: false,
                    },
                ],
            };

            await axios
                .post(
                    `${config.baseUrl}/api/units/questions`,
                    {
                        content: defaultNewQuestion.content,
                        point: defaultNewQuestion.point,
                        examUnitId: editingExam.unitId,
                        choices: defaultNewQuestion.choices,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedQuestionsList = [defaultNewQuestion];
                    setQuestions(addedQuestionsList);
                    onHandleQuestionList(addedQuestionsList);
                })
                .catch((error) => {
                    console.error(error);
                    console.error(defaultNewQuestion.choices);
                });
        } else {
            const defaultNewQuestion = {
                // LessonId: lessons[lessons.length - 1].LessonId + 1,
                content: `New question ${questions.length + 1}`,
                point: 1,
                choices: [
                    {
                        content: 'answer 1',
                        isCorrect: true,
                    },
                    {
                        content: 'answer 2',
                        isCorrect: false,
                    },
                    {
                        content: 'answer 3',
                        isCorrect: false,
                    },
                ],
            };
            await axios
                .post(
                    `${config.baseUrl}/api/units/questions`,
                    {
                        content: defaultNewQuestion.content,
                        point: defaultNewQuestion.point,
                        examUnitId: editingExam.unitId,
                        choices: defaultNewQuestion.choices,
                    },
                    {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    },
                )
                .then((response) => {
                    const addedQuestionsList = [...questions, defaultNewQuestion];
                    setQuestions(addedQuestionsList);
                    onHandleQuestionList(addedQuestionsList);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
        fetchExam();
        // setLessons(addedLessonsList);
    };

    const handleDeleteQuestion = async (question, index) => {
        await axios.delete(`${config.baseUrl}/api/units/questions/${question.questionId}`, {
            headers: {
                Authorization: `Bearer ${accessToken}`,
            },
        });
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            newQuestionList.splice(index, 1);
            return newQuestionList;
        });
        fetchExam();
    };

    const handleAddAnswer = (questionIndex) => {
        setQuestions((prevQuestionList) => {
            const newQuestionList = [...prevQuestionList];
            newQuestionList[questionIndex] = {
                ...newQuestionList[questionIndex],
                choices: [
                    ...newQuestionList[questionIndex].choices,
                    {
                        content: 'new answer',
                        isCorrect: false,
                    },
                ],
            };
            return newQuestionList;
        });
        fetchExam();
    };

    const handleChoiceContentEdit = (questionIndex, choiceIndex, newContent) => {
        setQuestions((prevQuestionList) => {
            const questionToUpdate = prevQuestionList[questionIndex];
            const updatedChoices = [...questionToUpdate.choices];
            const choiceToUpdate = updatedChoices[choiceIndex];
            choiceToUpdate.content = newContent;
            questionToUpdate.choices = updatedChoices;
            const updatedQuestionList = [...prevQuestionList];
            updatedQuestionList[questionIndex] = questionToUpdate;
            return updatedQuestionList;
        });
    };

    const handleChoiceClick = (questionIndex, choiceIndex) => {
        const newContent = window.prompt(
            'Enter new content of the answer:',
            questions[questionIndex].choices[choiceIndex].content,
        );
        if (newContent !== null) {
            handleChoiceContentEdit(questionIndex, choiceIndex, newContent);
        }
    };

    return (
        <>
            <div className={cx('wrapper')}>
                <div className={cx('top')}>
                    <p className={cx('title')}>{title}</p>
                    <button className={cx('addQuestionBtn')} onClick={handleAddQuestionClick}>
                        {addBtnName}
                    </button>
                </div>
                <ul className={cx('body')}>
                    {questions.map((item, questionIndex) => (
                        <li className={cx('question')} key={questionIndex}>
                            <div className={cx('questionHeader')}>
                                <p className={cx('questionName')}>
                                    Question {questionIndex + 1}: {item.content}
                                </p>
                                <div className={cx('rightHeader')}>
                                    <p className={cx('point')}>{item.point} Point</p>
                                    <p className={cx('edit')} onClick={() => handleEditQuestion(item)}>
                                        Edit
                                    </p>
                                    <button
                                        className={cx('deleteBtn')}
                                        onClick={() => handleDeleteQuestion(item, questionIndex)}
                                    >
                                        X
                                    </button>
                                </div>
                            </div>
                            {/* <ul className={cx('answerList')}>
                                {item.choices.map((choice, answerIndex) => (
                                    <li key={answerIndex} className={cx('answerLi')}>
                                        <label className={cx('answerLabel')}>
                                            <input
                                                className={cx('answerInput')}
                                                type="radio"
                                                name={`question-${questionIndex}`}
                                                value={choice.content}
                                                checked={answers[questionIndex] === answerIndex}
                                                onChange={() => handleAnswerChange(questionIndex, answerIndex)}
                                            />
                                            <span onClick={() => handleChoiceClick(questionIndex, answerIndex)}>
                                                {choice.content}
                                            </span>
                                        </label>
                                    </li>
                                ))}
                            </ul>
                            <button className={cx('addAnswerBtn')} onClick={() => handleAddAnswer(questionIndex)}>
                                <FontAwesomeIcon icon={faPlus} />
                            </button> */}
                            {/*  */}
                        </li>
                    ))}
                </ul>
            </div>
        </>
    );
}

export default MultiChoicesQuesTion;
