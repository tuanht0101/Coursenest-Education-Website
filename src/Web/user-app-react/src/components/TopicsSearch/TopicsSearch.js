import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import classNames from 'classnames/bind';
import { useEffect, useRef, useState } from 'react';
import HeadlessTippy from '@tippyjs/react/headless';

import styles from './TopicsSearch.module.scss';
import images from '~/assets/images';
import Image from '~/components/Image';
import TopicsList from '../TopicsList/TopicsList';
import { Wrapper as PopperWrapper } from '~/components/Popper';
import { useDebounce } from '~/hooks';
import ChosenTopicsList from '../ChosenTopicsList';
import axios from 'axios';
import topicsApi from '~/api/topicsApi';
import config from '~/config';

const cx = classNames.bind(styles);

function TopicsSearch({ handleGetTopics, handleTopicsId, chosenTopicId, maxTopics }) {
    const [chosenTopics, setChosenTopics] = useState([]);
    const [chosenTopicsId, setChosenTopicsId] = useState([]);
    const [searchValue, setSearchValue] = useState('');
    const [searchResultFiltered, setSearchResultFiltered] = useState([]);
    const [dropDown, setDropDown] = useState(false);
    const [chosenArr, setChosenArr] = useState([]);

    const debouncedValue = useDebounce(searchValue, 1000);

    const inputRef = useRef();

    const accessToken = localStorage.getItem('accessToken');

    useEffect(() => {
        if (chosenTopicId) {
            const fetchTopic = async () => {
                await axios
                    .get(`${config.baseUrl}/api/topics/${chosenTopicId}`, {
                        headers: {
                            Authorization: `Bearer ${accessToken}`,
                        },
                    })
                    .then((response) => {
                        setChosenTopics([response.data]);
                    })
                    .catch((error) => {
                        console.log(error);
                    });
            };
            fetchTopic();
        } else {
            setChosenTopics([]);
        }
    }, [chosenTopicId]);

    useEffect(() => {
        if (!debouncedValue.trim()) {
            return;
        }

        const fetchTopics = async () => {
            const response = await axios.get(`${config.baseUrl}/api/topics?Content=${debouncedValue}`);
            if (chosenTopicsId !== undefined) {
                await setSearchResultFiltered(response.data.filter((item) => !chosenTopicsId.includes(item.topicId)));
            } else setSearchResultFiltered(response.data);
        };
        fetchTopics();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [debouncedValue]);

    const handleDropDownClick = (e) => {
        e.preventDefault();
        setDropDown(!dropDown);
    };

    const handleHideResult = () => {
        setDropDown(false);
    };

    const handleChange = (e) => {
        const searchValue = e.target.value;

        if (!searchValue.startsWith(' ')) {
            setSearchValue(e.target.value);
        }
    };

    const handleClick = async (topicId) => {
        // Check if the limit has been reached
        if (chosenTopics.length >= maxTopics) {
            alert(`You can only choose up to ${maxTopics} topics.`);
            return;
        }

        const chosenTopic = await topicsApi.get(topicId);
        const newArr = searchResultFiltered.filter((item) => item.topicId !== topicId);

        if (chosenTopicsId !== undefined) {
            setChosenTopicsId([topicId, ...chosenTopicsId]);
        } else setChosenTopicsId([topicId]);

        setChosenArr([searchResultFiltered.findIndex((e) => e.topicId === topicId), ...chosenArr]);
        setDropDown(false);
        setSearchResultFiltered(newArr);
        setChosenTopics((chosenTopics) => [chosenTopic, ...chosenTopics]);
        if (handleTopicsId) {
            handleTopicsId(chosenTopics);
        }
        if (handleGetTopics) {
            handleGetTopics(topicId);
        }
    };

    const handleRemoveCourse = async (courseId) => {
        const chosenCourse = await topicsApi.get(courseId);

        const newArr = chosenTopics.filter((item) => item.topicId !== courseId);
        const leftSearchArr = searchResultFiltered.slice(0, chosenArr[0]);
        const rightSearchArr = searchResultFiltered.slice(chosenArr[0], searchResultFiltered.length);
        const searchArr = [...leftSearchArr, chosenCourse, ...rightSearchArr];

        await setSearchResultFiltered(searchArr);
        await setChosenTopics(newArr);
        setChosenTopicsId(...newArr.map((topic) => topic.topicId));

        if (handleTopicsId) {
            handleTopicsId(chosenTopics);
        }
        if (handleGetTopics) {
            return;
        }
    };

    return (
        // Fix tippyjs error by adding a wrapper <div> or <span>
        <div>
            <div>
                <HeadlessTippy
                    interactive
                    visible={dropDown}
                    placement={'bottom-start'}
                    render={(attrs) => (
                        <div className={cx('search-result')} tabIndex="-1" {...attrs}>
                            <PopperWrapper className={cx('popper-result')}>
                                <TopicsList
                                    className={cx('TopicsListDropDown')}
                                    courses={searchResultFiltered}
                                    onChose={handleClick}
                                />
                            </PopperWrapper>
                        </div>
                    )}
                    onClickOutside={handleHideResult}
                >
                    <div className={cx('wrapper')}>
                        <label className={cx('searchLabel')}>Chose topic: </label>
                        <div className={cx('searchDiv')}>
                            <FontAwesomeIcon className={cx('searchIcon')} icon={faMagnifyingGlass} />
                            <input
                                ref={inputRef}
                                value={searchValue}
                                className={cx('searchInput')}
                                type="text"
                                placeholder="Search..."
                                onChange={handleChange}
                                onFocus={() => setDropDown(true)}
                            ></input>
                            <button className={cx('dropDownButton')} onClick={handleDropDownClick}>
                                <Image className={cx('dropDownImg')} src={images.dropDownIcon}></Image>
                            </button>
                        </div>
                    </div>
                </HeadlessTippy>
            </div>
            <ChosenTopicsList courses={chosenTopics} onChose={handleRemoveCourse} />
        </div>
    );
}

export default TopicsSearch;
