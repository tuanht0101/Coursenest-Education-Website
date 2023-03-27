import axios from 'axios';
import classNames from 'classnames/bind';
import { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';

import CancelConfirmBtns from '~/components/PublisherPage/CancelConfirmBtns';
import config from '~/config';

import styles from './EditMaterial.module.scss';

const cx = classNames.bind(styles);

// To-do: Content
function EditMaterial({ chosenMaterial, handleBackStep, handleMaterialsUpdate }) {
    const [content, setContent] = useState(chosenMaterial.content);
    const [material, setMaterial] = useState(chosenMaterial);
    const [materialTitle, setMaterialTitle] = useState(chosenMaterial.title);
    // const [lessonDesc, setLessonDesc] = useState(chosenMaterial.content);
    const [isEditingTitle, setIsEditingTitle] = useState(false);
    const [isEditingDesc, setIsEditingDesc] = useState(false);
    const [isEditingTime, setIsEditingTime] = useState(false);
    const [approximateTime, setApproximateTime] = useState(45);

    const navigate = useNavigate();
    let params = useParams();
    const accessToken = localStorage.getItem('accessToken');

    const handleOnChangeContent = (event) => {
        setContent(event.target.value);
    };

    const handleTitleClick = () => {
        setIsEditingTitle(true);
    };

    const handleTitleChange = (event) => {
        setMaterialTitle(event.target.value);
        setMaterial({ ...material, title: event.target.value });
        console.log(event.target.value);
    };

    const handleTitleBlur = () => {
        setIsEditingTitle(false);
    };

    const handleDescClick = () => {
        setIsEditingDesc(true);
    };

    // const handleDescChange = (event) => {
    //     setLessonDesc(event.target.value);
    //     setMaterial({ ...material, Description: event.target.value });
    // };

    const handleDescBlur = () => {
        setIsEditingDesc(false);
    };

    const handleTimeClick = () => {
        setIsEditingTime(true);
    };

    const handleTimeChange = (event) => {
        setApproximateTime(event.target.value);
        setMaterial({ ...material, requiredMinutes: event.target.value }); // <----- To-do
    };

    const handleTimeBlur = () => {
        setIsEditingTime(false);
    };

    // const navigate = useNavigate();
    // let params = useParams();

    const handleCancel = () => {
        handleBackStep();
        // navigate(`/publisher/${params.PublisherUserId}/add-course/add-lesson`);
    };

    // To-do: Update value of content when confirm
    const handleConfirm = async () => {
        await axios
            .put(
                `${config.baseUrl}/api/units/${material.unitId}/material`,
                {
                    title: materialTitle,
                    requiredTime: approximateTime,
                    content: content,
                },
                {
                    headers: {
                        Authorization: `Bearer ${accessToken}`,
                    },
                },
            )
            .then((response) => {
                handleBackStep();
                handleMaterialsUpdate(material);
                console.log(material.unitId);
            })
            .catch((error) => {
                // console.error(error);
                console.log(material.unitId);
            });
        // navigate(`/publisher/${params.PublisherUserId}/add-course`);
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('topContent')}>
                {isEditingTitle ? (
                    <input
                        type="text"
                        className={cx('titleInput')}
                        value={materialTitle}
                        onChange={handleTitleChange}
                        onBlur={handleTitleBlur}
                        autoFocus
                    />
                ) : (
                    <p className={cx('title')} onClick={handleTitleClick}>
                        {materialTitle}
                    </p>
                )}
                {isEditingTime ? (
                    <input
                        type="text"
                        className={cx('TimeInput')}
                        value={approximateTime}
                        onChange={handleTimeChange}
                        onBlur={handleTimeBlur}
                        autoFocus
                    />
                ) : (
                    <p className={cx('approximateTime')} onClick={handleTimeClick}>
                        Time: {approximateTime}:00
                    </p>
                )}
            </div>
            {/* {isEditingDesc ? (
                <input
                    type="text"
                    className={cx('titleInput')}
                    value={lessonDesc}
                    onChange={handleDescChange}
                    onBlur={handleDescBlur}
                    autoFocus
                />
            ) : (
                <p className={cx('description')} onClick={handleDescClick}>
                    {lessonDesc}
                </p>
            )} */}
            <div className={cx('contentContainer')}>
                <p className={cx('content')}>Content</p>
                <textarea
                    className={cx('contentText')}
                    value={content}
                    onChange={handleOnChangeContent}
                    placeholder="“Give any additional context on what happened.”"
                ></textarea>
            </div>
            <CancelConfirmBtns onCancel={handleCancel} onConfirm={handleConfirm} />
        </div>
    );
}

export default EditMaterial;
