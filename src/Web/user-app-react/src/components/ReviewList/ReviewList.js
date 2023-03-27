import classNames from 'classnames/bind';
import styles from './ReviewList.module.scss';

import ReviewItem from '../ReviewItem';
import images from '~/assets/images';

const cx = classNames.bind(styles);

function ReviewList() {
    return (
        <div className={cx('reviewContainer')}>
            <ReviewItem
                text="Simply unbelievable! I am really satisfied with my projects in TEAMs courses. This is absolutely wonderful!  "
                avatar={images.reviewer1}
                name="Timson K"
                role="Freelancer"
            />
            <ReviewItem
                text="Simply unbelievable! I am really satisfied with my projects in TEAMs courses. This is absolutely wonderful!  "
                avatar={images.reviewer2}
                name="Michael Dam"
                role="Student"
            />
            <ReviewItem
                text="Simply unbelievable! I am really satisfied with my projects in TEAMs courses. This is absolutely wonderful!  "
                avatar={images.reviewer3}
                name="Nicolas Horn"
                role="Student"
            />
        </div>
    );
}

export default ReviewList;
