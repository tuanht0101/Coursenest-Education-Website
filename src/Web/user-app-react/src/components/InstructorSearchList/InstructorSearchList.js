import { React } from 'react';
import classNames from 'classnames/bind';

import styles from './InstructorSearchList.module.scss';

const cx = classNames.bind(styles);

function InstructorSearchList({ courses, categories, subCategories, topics, children, onChose }) {
    const handleItemClick = (id) => {
        onChose(id);
    };

    return (
        <ul className={cx('course-list')}>
            {courses &&
                courses.map((item, index) => (
                    <li className={cx('course-item')} key={index} onClick={() => handleItemClick(item.id)}>
                        {item.title}
                        {children}
                    </li>
                ))}
            {categories &&
                categories.map((item, index) => (
                    <li className={cx('course-item')} key={index} onClick={() => handleItemClick(item.id)}>
                        {item.Content}
                        {children}
                    </li>
                ))}
            {subCategories &&
                subCategories.map((item, index) => (
                    <li className={cx('course-item')} key={index} onClick={() => handleItemClick(item.id)}>
                        {item.Content}
                        {children}
                    </li>
                ))}
            {topics &&
                topics.map((item, index) => (
                    <li className={cx('course-item')} key={index} onClick={() => handleItemClick(item.id)}>
                        {item.Content}
                        {children}
                    </li>
                ))}
        </ul>
    );
}

export default InstructorSearchList;
