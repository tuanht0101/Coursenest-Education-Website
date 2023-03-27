import { faMinus } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';

import styles from './FollowingSubject.module.scss';

const cx = classNames.bind(styles);

function FollowingSubject({ title, items, type }) {
    return (
        <div className={cx('wrapper')}>
            <p className={cx('tabTitle')}>{title}</p>
            <div className={cx('itemsContainer')}>
                <ul className={cx('listItems')}>
                    {items.map((item, index) => (
                        <div className={cx('itemContainer')} key={index}>
                            <li className={cx('item')}>
                                <p>{item.Content}</p>
                                <div className={cx('rightItem')}>
                                    <p>{type}</p>
                                    <button className={cx('deleteButton')}>
                                        <FontAwesomeIcon className={cx('delete')} icon={faMinus} />
                                    </button>
                                </div>
                            </li>
                        </div>
                    ))}
                </ul>
            </div>
        </div>
    );
}

export default FollowingSubject;
