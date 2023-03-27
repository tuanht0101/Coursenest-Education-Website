import { React } from 'react';
import classNames from 'classnames/bind';

import styles from './TopicsList.module.scss';

const cx = classNames.bind(styles);

function TopicsList({ courses, children, onChose }) {
    const handleItemClick = (id) => {
        onChose(id);
    };

    return (
        <ul className={cx('course-list')}>
            {courses.map((item, index) => (
                <li className={cx('course-item')} key={index} onClick={() => handleItemClick(item.topicId)}>
                    {item.content}
                    {children}
                </li>
            ))}
        </ul>
    );
}

export default TopicsList;
