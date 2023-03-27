import classNames from 'classnames/bind';
import { useForm } from 'react-hook-form';
import { Link } from 'react-router-dom';
// import { ToastContainer, toast } from 'react-toastify';
// import 'react-toastify/dist/ReactToastify.css';
import { useState } from 'react';

import { adImages } from '~/mockupData/AdsData/AdsData';
import ImageSliders from '~/components/ImageSliders';
import styles from './Forgot.module.scss';
import axios from 'axios';
import config from '~/config';

const cx = classNames.bind(styles);

function Forgot() {
    const [userName, setUserName] = useState('');
    const [email, setEmail] = useState('');
    const [error, setError] = useState('');
    const [newPassword, setNewPassword] = useState('');

    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm({
        defaultValues: {
            email: '',
        },
    });

    const onSubmit = async () => {
        setNewPassword('');
        setError('');
        await axios
            .put(`${config.baseUrl}/api/authenticate/forgot-password`, {
                username: userName,
                email: email,
            })
            .then((response) => {
                setNewPassword('Password mới là: ' + response.data);
            })
            .catch(() => {
                setError('Username or email incorrect');
            });
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('forgot-form-container')}>
                <div className={cx('forgot-form')}>
                    <div className={cx('form-header')}>
                        <h1>TEAM</h1>
                        <p>Forgot the password</p>
                    </div>
                    <form className={cx('email-fill-form')} onSubmit={handleSubmit(onSubmit)}>
                        <div className={cx('email-fill')}>
                            <label className={cx('inputTitle')}>Username</label>
                            <span className={cx('form-message')}>{errors.userName?.message}</span>
                            <input
                                className={cx('registerInput')}
                                {...register('userName', {
                                    required: 'This input is required',
                                })}
                                value={userName}
                                onChange={(e) => setUserName(e.target.value)}
                                style={{ border: errors.userName?.message ? '1px solid red' : '' }}
                                placeholder="Enter your username..."
                            />
                        </div>
                        <div className={cx('email-fill')}>
                            <label>Email</label>
                            <span className={cx('form-message')}>{errors.email?.message}</span>
                            <input
                                type="email"
                                className={cx('registerInput')}
                                {...register('email', {
                                    required: 'This input is required.',
                                    pattern: {
                                        // eslint-disable-next-line no-useless-escape
                                        value: /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/g,
                                        message: 'Invalid email!',
                                    },
                                })}
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                style={{ border: errors.email?.message ? '1px solid red' : '' }}
                                placeholder="Enter your email..."
                            />
                        </div>
                        {newPassword && <p className={cx('passAnnoucement')}> {newPassword}</p>}
                        {error && <p className={cx('loginError')}>{error}</p>}
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
                    <div className={cx('sponsor-info-container')}>
                        <div></div>
                        <p className={cx('sponsor-info')}>*Sponsor by ABC</p>
                    </div>
                    <div className={cx('containerStyles')}>
                        <ImageSliders images={adImages} />
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Forgot;
