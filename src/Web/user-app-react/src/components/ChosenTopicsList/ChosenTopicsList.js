import { React } from 'react';
import classNames from 'classnames/bind';

import styles from './ChosenTopicsList.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMinus } from '@fortawesome/free-solid-svg-icons';

const cx = classNames.bind(styles);

function ChosenTopicsList({ courses, onChose }) {
    const handleRemove = (id) => {
        onChose(id);
    };

    return (
        <ul className={cx('course-list')}>
            {courses.map((item, index) => (
                <li className={cx('course-item')} key={index}>
                    {item.content}
                    <button className={cx('deleteCourseButton')} onClick={() => handleRemove(item.topicId)}>
                        <FontAwesomeIcon className={cx('deleteCourse')} icon={faMinus} />
                    </button>
                </li>
            ))}
        </ul>
    );
}

export default ChosenTopicsList;
