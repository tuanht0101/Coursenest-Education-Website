import { React } from 'react';
import classNames from 'classnames/bind';

import styles from './InstructorSearchResult.module.scss';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPlus } from '@fortawesome/free-solid-svg-icons';

const cx = classNames.bind(styles);

function InstructorSearchResult({ courses, topics, categories, subCategories, children, onChose }) {
    const handleItemClick = (id) => {
        onChose(id);
    };

    return (
        <div>
            <ul className={cx('items-list')}>
                {courses.map((item, index) => (
                    <li className={cx('item')} key={index} onClick={() => handleItemClick(item.id)}>
                        <div className={cx('itemContent')}>
                            {item.Title}
                            <div className={cx('itemType')}>Course</div>
                        </div>
                    </li>
                ))}
                {topics.map((item, index) => (
                    <li className={cx('item')} key={index} onClick={() => handleItemClick(item.id)}>
                        <div className={cx('itemContent')}>
                            {item.Content}
                            <div className={cx('itemType')}>Topic</div>
                        </div>
                    </li>
                ))}
                {categories.map((item, index) => (
                    <li className={cx('item')} key={index} onClick={() => handleItemClick(item.id)}>
                        <div className={cx('itemContent')}>
                            {item.Content}
                            <div className={cx('itemType')}>Category</div>
                        </div>
                    </li>
                ))}
                {subCategories.map((item, index) => (
                    <li className={cx('item')} key={index} onClick={() => handleItemClick(item.id)}>
                        <div className={cx('itemContent')}>
                            {item.Content}
                            <div className={cx('itemType')}>Sub-Category</div>
                        </div>
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default InstructorSearchResult;
