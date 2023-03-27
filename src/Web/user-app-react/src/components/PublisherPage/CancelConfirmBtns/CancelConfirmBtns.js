import classNames from 'classnames/bind';
import styles from './CancelConfirmBtns.module.scss';

const cx = classNames.bind(styles);

function CancelConfirmBtns({ onCancel, onConfirm }) {
    const handleCancelClick = () => {
        onCancel();
    };

    const handleConfirmClick = () => {
        onConfirm();
    };

    return (
        <div className={cx('confirmBtnsDiv')}>
            <button className={cx('cancelBtn')} onClick={handleCancelClick}>
                Cancel
            </button>
            <button className={cx('confirmBtn')} onClick={handleConfirmClick}>
                Confirm
            </button>
        </div>
    );
}

export default CancelConfirmBtns;
