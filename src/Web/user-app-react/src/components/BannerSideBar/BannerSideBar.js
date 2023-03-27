import classNames from 'classnames/bind';
import PropTypes from 'prop-types';

import styles from './BannerSideBar.module.scss';

const cx = classNames.bind(styles);

function BannerSideBar({ teamName, title, body, children }) {
    return (
        <div className={cx('banner')}>
            <div className={cx('banner-intro')}>
                <h1>{teamName}</h1>
                <h2>{title}</h2>
                <p>{body}</p>
                <div>{children}</div>
            </div>
        </div>
    );
}

BannerSideBar.propTypes = {
    header: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired,
    body: PropTypes.string.isRequired,
    children: PropTypes.node,
};

export default BannerSideBar;
