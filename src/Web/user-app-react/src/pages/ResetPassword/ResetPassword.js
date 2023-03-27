import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faEye, faEyeSlash } from '@fortawesome/free-solid-svg-icons';
import classNames from 'classnames/bind';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
import { useRef, useState } from 'react';

import styles from './ResetPassword.module.scss';
import images from '~/assets/images';
import Image from '~/components/Image';

const cx = classNames.bind(styles);

function ResetPassword() {
    const {
        register,
        handleSubmit,
        watch,
        formState: { errors },
    } = useForm({
        defaultValues: {
            email: '',
        },
    });

    const [isShow, setIsShow] = useState(false);
    const [isHidden, setIsHidden] = useState(false);
    const passwordValue = useRef({});
    const emailValue = useRef({});

    // const usernameValue = watch('username');
    emailValue.current = watch('email');
    passwordValue.current = watch('password', '');
    // const retypePassword = watch('retypePassword');

    const handleShowPassword = (event) => {
        event.preventDefault();

        setIsShow(!isShow);
    };

    const onSubmit = (data) => {
        setIsHidden(true);
        console.log(data);
    };

    const successBackground = (
        <div className={cx('success-wrapper')}>
            <div className={cx('container')}>
                <div className={cx('reviewer-info')}>
                    <Image className={cx('avatar')} src={images.locationTick} alt={'TEAM'} />
                    <div className={cx('user-info')}>
                        <div className={cx('name')}>
                            <p>TEAM</p>
                        </div>
                        <div className={cx('role')}>
                            <p>Success</p>
                        </div>
                    </div>
                </div>
            </div>
            <p>Password has been changed successfully!</p>
            <p className={cx('login-text')}>
                Please{' '}
                <Link className={cx('login-direct')} to={'/sign-in'}>
                    Log-in
                </Link>{' '}
                again
            </p>
        </div>
    );

    const form = (
        <div className={cx('forgot-form-container')}>
            <div className={cx('forgot-form')}>
                <div className={cx('form-header')}>
                    <h1>TEAM</h1>
                    <p>Forgot the password</p>
                </div>
                <form className={cx('password-fill-form')} onSubmit={handleSubmit(onSubmit)}>
                    <div>
                        <label>New password</label>
                        <span className={cx('form-message')}>{errors.password?.message}</span>
                        <div
                            className={cx('passwordInput')}
                            style={{ border: errors.password?.message ? '1px solid red' : '' }}
                        >
                            <input
                                type={isShow ? 'text' : 'password'}
                                className={cx('passwordEnter')}
                                {...register('password', {
                                    required: 'This input is required.',
                                    // validate: (value) =>
                                    //     value !== emailValue.current || "Password can't be the same as email!",
                                    pattern: {
                                        // value: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?@!$%^&*-./\\';:,]).{8,}$/,
                                        value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
                                        message: `Must be more than 8 character with 1 uppercase, 1 lowercase, 1 number, only 1 special character and no space`,
                                    },
                                })}
                                placeholder="Enter your password..."
                            />
                            <button className={cx('showButton')} onClick={(event) => handleShowPassword(event)}>
                                <FontAwesomeIcon className={cx('eye-icon')} icon={isShow ? faEyeSlash : faEye} />
                            </button>
                        </div>
                    </div>

                    <div>
                        <label>Retype password</label>
                        {/* {passwordValue === retypePassword ? ( */}
                        {errors.retypePassword && (
                            <span className={cx('form-message')}>{errors.retypePassword.message}</span>
                        )}
                        {/* ) : (
            <span className={cx()}"form-message">Password doesn't match!</span>
        )} */}
                        <div
                            className={cx('passwordInput')}
                            style={{ border: errors.retypePassword?.message ? '1px solid red' : '' }}
                        >
                            <input
                                type={isShow ? 'text' : 'password'}
                                className={cx('passwordEnter')}
                                {...register('retypePassword', {
                                    required: 'This input is required.',
                                    validate: (value) => value === passwordValue.current || "Password doesn't match!",
                                })}
                                placeholder="Retype your password..."
                            />
                            <button className={cx('showButton')} onClick={(event) => handleShowPassword(event)}>
                                <FontAwesomeIcon className={cx('eye-icon')} icon={isShow ? faEyeSlash : faEye} />
                            </button>
                        </div>
                    </div>
                    <button className={cx('confirmButton')}>Confirm</button>
                </form>
                <div className={cx('member-yet')}>
                    <p>Already have account?</p>
                    <p className={cx('signInUp-btn')}>
                        <Link className={cx('login-link')} to={'/sign-in'}>
                            Login?
                        </Link>
                    </p>
                    <p className={cx('signInUp-btn')}>
                        <Link className={cx('signUp-link')} to={'/sign-up'}>
                            Sign Up?
                        </Link>
                    </p>
                </div>
            </div>
        </div>
    );

    return <div className={cx('wrapper')}>{isHidden ? successBackground : form}</div>;
}

export default ResetPassword;
