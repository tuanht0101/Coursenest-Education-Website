import { faEye, faEyeSlash, faArrowRight, faArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { React, useState, useRef } from 'react';
import { Link } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import classNames from 'classnames/bind';

import ReviewList from '~/components/ReviewList';
import styles from './SignUp.module.scss';
import TopicsSearch from '~/components/TopicsSearch';
import axios from 'axios';
import config from '~/config';

const cx = classNames.bind(styles);

function SignUp() {
    const [username, setUsername] = useState('');
    const [fullname, setFullname] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [interestedTopicId, setInterestedTopicId] = useState([]);
    const [error, setError] = useState('');

    const [page, setPage] = useState(0);

    const {
        register,
        handleSubmit,
        watch,
        formState: { errors },
    } = useForm({
        defaultValues: {
            userName: '',
            fullName: '',
            email: '',
            password: '',
            retypePassword: '',
        },
        mode: 'all',
    });

    const [isShow, setIsShow] = useState(false);
    const passwordValue = useRef({});
    // const emailValue = useRef({});

    const emailValue = watch('email');
    // emailValue.current = watch('email');
    // const passwordValue = watch('password', '');
    passwordValue.current = watch('password', '');

    const handleGetTopics = (topicData) => {
        setInterestedTopicId(topicData);
    };

    const handleShowPassword = (event) => {
        event.preventDefault();

        setIsShow(!isShow);
    };

    const handleClickPreviousPage = () => {
        setPage(page - 1);
    };

    const onSubmit = async (e) => {
        if (page === 0) {
            setPage(page + 1);
        } else {
            const chosenTopicsId = [...interestedTopicId.map((item) => item.topicId)];
            console.log(chosenTopicsId);

            setError(false);
            try {
                const res = await axios.post(`${config.baseUrl}/api/authenticate/register`, {
                    username: username,
                    password: password,
                    email: email,
                    fullname: fullname,
                    interestedTopicIds: [...chosenTopicsId],
                });
                window.location.href = '/';
            } catch (error) {
                setError(error.response.data.message);
            }
        }
    };

    return (
        <div className={cx('wrapper')}>
            <div className={cx('register-sidebar')}>
                <h1>TEAM</h1>
                <p className={cx('sidebarTitle')}>
                    Start your <br /> journey with us.
                </p>
                <p className={cx('sidebarBody')}>
                    Discover the knowledge <br /> of Computer Science
                    <br /> via courses.
                </p>
                <ReviewList />
            </div>

            <div className={cx('register')}>
                <div className={cx('regisInfoTitle')}>
                    <div className={cx('regisDiv')}>
                        <div>
                            <h1 className={cx('registerTitle')}>Sign up</h1>
                        </div>
                        <div></div>
                    </div>
                    <div className={cx('regisDiv')}>
                        <div className={cx('member-yet')}>
                            <p>
                                <strong>Have an account? </strong>
                            </p>
                            <p className={cx('sign-in')}>
                                <Link className={cx('sign-in-btn')} to="/sign-in">
                                    Login
                                </Link>
                            </p>
                        </div>
                        <div></div>
                    </div>
                    <p className={cx('forgot')}>
                        <Link className={cx('forgot-btn')} to="/forgot-password">
                            Forgot your password?
                        </Link>
                    </p>
                </div>

                {page === 0 && (
                    <form className={cx('registerForm')} onSubmit={handleSubmit(onSubmit)}>
                        <div className={cx('inputContainer')}>
                            <label className={cx('inputTitle')}>Username</label>
                            <span className={cx('form-message')}>{errors.userName?.message}</span>
                            <input
                                className={cx('registerInput')}
                                {...register('userName', {
                                    required: 'This input is required',
                                    pattern: {
                                        //  username is 8-20 characters long, no _ or . at the beginning, no __ or _. or ._ or .. inside, no _ or . at the end
                                        value: /^(?=.{8,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$/,
                                        message: 'Invalid username! Try another one!',
                                    },
                                })}
                                style={{ border: errors.userName?.message ? '1px solid red' : '' }}
                                placeholder="Enter your username..."
                                onChange={(e) => setUsername(e.target.value)}
                            />
                        </div>

                        <div className={cx('inputContainer')}>
                            <label className={cx('inputTitle')}>Full name</label>
                            <span className={cx('form-message')}>{errors.fullName?.message}</span>
                            <input
                                className={cx('registerInput')}
                                {...register('fullName', {
                                    required: 'This input is required',
                                    pattern: {
                                        //  fullName: contains At least two words, No numbers, Any latin o special character used in names.
                                        value: /^[a-zA-Z]+(?:\s[a-zA-Z]+)+$/,
                                        message: 'Invalid name! Try another one!',
                                    },
                                })}
                                style={{ border: errors.fullName?.message ? '1px solid red' : '' }}
                                placeholder="Enter your full name..."
                                onChange={(e) => setFullname(e.target.value)}
                            />
                        </div>

                        <div className={cx('inputContainer')}>
                            <label className={cx('inputTitle')}>Email</label>
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
                                style={{ border: errors.email?.message ? '1px solid red' : '' }}
                                placeholder="Enter your email..."
                                onChange={(e) => setEmail(e.target.value)}
                            />
                        </div>

                        <div className={cx('inputContainer')}>
                            <label className={cx('inputTitle')}>Password</label>
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
                                        validate: (value) =>
                                            value !== emailValue.current || "Password can't be the same as email!",
                                        pattern: {
                                            // value: /^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?@!$%^&*-./\\';:,]).{8,}$/,
                                            value: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/,
                                            message: `Must be more than 8 character with 1 uppercase, 1 lowercase, 1 number, only 1 special character and no space`,
                                        },
                                    })}
                                    placeholder="Enter your password..."
                                    onChange={(e) => setPassword(e.target.value)}
                                />
                                <button className={cx('showButton')} onClick={(event) => handleShowPassword(event)}>
                                    <FontAwesomeIcon className={cx('eye-icon')} icon={isShow ? faEyeSlash : faEye} />
                                </button>
                            </div>
                        </div>

                        <div className={cx('inputContainer')}>
                            <label className={cx('inputTitle')}>Retype password</label>
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
                                        validate: (value) =>
                                            value === passwordValue.current || "Password doesn't match!",
                                    })}
                                    placeholder="Retype your password..."
                                />
                                <button className={cx('showButton')} onClick={(event) => handleShowPassword(event)}>
                                    <FontAwesomeIcon className={cx('eye-icon')} icon={isShow ? faEyeSlash : faEye} />
                                </button>
                            </div>
                        </div>

                        <button
                            type="submit"
                            className={cx('nextPageButton')}
                            // onClick={() => {
                            //     handleClickNextPage();
                            // }}
                        >
                            Next
                            <FontAwesomeIcon className={cx('rightArrow')} icon={faArrowRight} />
                        </button>
                    </form>
                )}

                {page === 1 && (
                    <div className="signUpSecondPage">
                        <TopicsSearch handleTopicsId={handleGetTopics} maxTopics={999} />
                        {error && <p style={{ color: 'red' }}>Some thing wrong</p>}
                        <div className={cx('buttonsDiv')}>
                            <button className={cx('previousPageButton')} onClick={handleClickPreviousPage}>
                                <FontAwesomeIcon className={cx('leftArrow')} icon={faArrowLeft} />
                                Previous
                            </button>
                            <button className={cx('registerLoginButton')} onClick={handleSubmit(onSubmit)}>
                                Register now
                            </button>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
}

export default SignUp;
