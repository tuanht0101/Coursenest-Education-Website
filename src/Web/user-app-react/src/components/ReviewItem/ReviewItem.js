import classNames from 'classnames/bind';
import Image from '~/components/Image';

import styles from './ReviewItem.module.scss';

const cx = classNames.bind(styles);

function ReviewItem({ text = '', avatar, name, role }) {
    return (
        <div className={cx('container')}>
            <p className={cx('comment')}>{text}</p>
            <div className={cx('reviewer-info')}>
                <Image className={cx('avatar')} src={avatar} alt={name} />
                <div className={cx('user-info')}>
                    <div className={cx('name')}>
                        <p>{name}</p>
                    </div>
                    <div className={cx('role')}>
                        <p>{role}</p>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ReviewItem;
