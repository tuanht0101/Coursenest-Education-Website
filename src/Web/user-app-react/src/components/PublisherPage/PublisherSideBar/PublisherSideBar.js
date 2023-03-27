import { faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useEffect, useState } from 'react';
import { Link, NavLink, useParams } from 'react-router-dom';

import styles from './PublisherSideBar.module.scss';

const cx = classNames.bind(styles);

function PublisherSideBar() {
    let params = useParams();
    const [isSelected, setIsSlected] = useState(1);
    const [isAddCourseActive, setIsAddCourseActive] = useState(false);

    useEffect(() => {
        const isContain = window.location.href.includes(`add-course`);
        setIsAddCourseActive(isContain);
    }, [window.location.href]);

    const handleClick = (id) => {
        setIsSlected(id);
        console.log(params.PublisherUserId);
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('coursesPagesContainer')}>
                <p className={cx('tabTitle')}>Edit Courses</p>
                <div className={cx('tabsDiv')}>
                    <NavLink
                        to={`${params.PublisherUserId}`}
                        // className={cx('navLink')}
                        className={isAddCourseActive ? cx('noneActiveTab') : cx('activeTab')}
                    >
                        <p
                            className={cx('subTab')}
                            style={{ opacity: isSelected === 1 ? 1 : 0.3 }}
                            onClick={() => handleClick(1)}
                        >
                            Dashboard
                        </p>
                    </NavLink>
                    <NavLink
                        to={`${params.PublisherUserId}/add-course`}
                        // className={cx('navLink')}
                        className={isAddCourseActive ? cx('activeTab') : cx('noneActiveTab')}
                        // to="edit-course"
                    >
                        <p
                            className={cx('subTab')}
                            style={{ opacity: isSelected === 2 ? 1 : 0.3 }}
                            onClick={() => handleClick(2)}
                        >
                            Add Course
                        </p>
                    </NavLink>
                    {/* <NavLink to="/" className={({ isActive }) => (isActive ? cx('activeTab') : cx('noneActiveTab'))}>
                        <p className={cx('subTab')}>Function 2</p>
                    </NavLink> */}
                </div>
            </div>
            <div className={cx('notiTabContainer')}>
                <div className={cx('topNotification')}>
                    <p className={cx('tabTitle')}>Notification</p>
                    <div className={cx('rightTopNoti')}>
                        <button className={cx('notiAmount')}>5</button>
                        <button className={cx('deleteCourseButton')}>
                            <FontAwesomeIcon className={cx('deleteCourse')} icon={faXmark} />
                        </button>
                    </div>
                </div>
                <p className={cx('messageNoti')}>You’ve got an E-mail from your Coursenest.</p>
            </div>
        </div>
    );
}

export default PublisherSideBar;
