import { Outlet } from 'react-router-dom';
import classNames from 'classnames/bind';

import InstructorSideBar from '~/components/InstructorSideBar';

import styles from './Instructor.module.scss';

const cx = classNames.bind(styles);

function Instructor() {
    return (
        <div className={cx('wrapper')}>
            <div className={cx('bodyWrapper')}>
                <InstructorSideBar />
                <Outlet />
            </div>
        </div>
    );
}

export default Instructor;
