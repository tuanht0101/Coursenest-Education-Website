import classNames from 'classnames/bind';
import HeadlessTippy from '@tippyjs/react/headless';

import FollowingSubject from '~/components/FollowingSubject';
import { Wrapper as PopperWrapper } from '~/components/Popper';
import styles from './Following.module.scss';
import Image from '~/components/Image';
import images from '~/assets/images';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import TopicsList from '~/components/TopicsList';
import { useEffect } from 'react';
import { useRef } from 'react';
import { useState } from 'react';
import { useDebounce } from '~/hooks';
import InstructorSearchResult from '~/components/InstructorSearchResult';

const cx = classNames.bind(styles);

function Following() {
    const courses = [
        { Title: 'Introduction to React', topic: 'React', category: 'Web Development', subCategory: 'Front End' },
        {
            Title: 'Data Structures and Algorithms',
            topic: 'Computer Science',
            category: 'Programming',
            subCategory: 'Algorithms',
        },
        {
            Title: 'Introduction to Machine Learning',
            topic: 'Machine Learning',
            category: 'Data Science',
            subCategory: 'Artificial Intelligence',
        },
    ];

    const topics = [
        {
            TopicId: 1,
            Content: 'Topic number 1',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
        {
            TopicId: 2,
            Content: 'Topic number 2',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
        {
            TopicId: 3,
            Content: 'Topic number 3',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
        {
            TopicId: 4,
            Content: 'Topic number 4',
            CategoryContent: 'CategoryContent number 1',
            SubcategoryContent: 'SubcategoryContent number 1',
        },
    ];

    const categories = [
        { Content: 'Web Development', subCategory: 'Front End' },
        { Content: 'Programming', subCategory: 'Algorithms' },
        { Content: 'Data Science', subCategory: 'Artificial Intelligence' },
        { Content: 'Data Science', subCategory: 'Artificial Intelligence' },
        { Content: 'Data Science', subCategory: 'Artificial Intelligence' },
        { Content: 'Data Science', subCategory: 'Artificial Intelligence' },
        { Content: 'Data Science', subCategory: 'Artificial Intelligence' },
    ];
    const subCategories = [{ Content: 'Front End' }, { Content: 'Algorithms' }, { Content: 'Artificial Intelligence' }];

    const items = [...courses, ...topics, ...categories, ...subCategories];

    const [allCourses, setAllCourses] = useState([]);
    const [chosenCourses, setChosenCourses] = useState([]);
    const [searchValue, setSearchValue] = useState('');
    const [searchResult, setSearchResult] = useState([]);
    const [dropDown, setDropDown] = useState(false);
    const [chosenArr, setChosenArr] = useState([]);

    const debouncedValue = useDebounce(searchValue, 500);

    const inputRef = useRef();

    useEffect(() => {
        // const fetchCourses = async () => {
        //      const TopicsList = await coursesApi.getAll();
        //     setAllCourses(TopicsList);
        //     setSearchResult(TopicsList);
        // };
        // fetchCourses();
    }, []);

    useEffect(() => {
        if (!debouncedValue.trim()) {
            setSearchResult(allCourses);
            return;
        }
        const resultsArray = allCourses.filter(
            (post) =>
                post.title.includes(debouncedValue.toLowerCase()) || post.body.includes(debouncedValue.toLowerCase()),
        );

        setSearchResult(resultsArray);

        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [debouncedValue]);

    const handleClick = async (courseId) => {
        // const chosenCourse = await coursesApi.get(courseId);
        const newArr = searchResult.filter((item) => item.id !== courseId);

        setChosenArr([searchResult.findIndex((e) => e.id === courseId), ...chosenArr]);
        setSearchValue('');
        setDropDown(false);
        setSearchResult(newArr);
        // await setChosenCourses((chosenCourses) => [chosenCourse, ...chosenCourses]);
    };

    const handleDropDownClick = () => {
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

    return (
        <>
            <div className={cx('wrapper')}>
                <div className={cx('followingContainer')}>
                    <p className={cx('mainTab')}>Following</p>
                    <FollowingSubject title={'Topics'} items={topics} type={'Topic'} />
                    <FollowingSubject title={'Sub-Categories'} items={topics} type={'Topic'} />
                    <FollowingSubject title={'Categories'} items={topics} type={'Topic'} />
                    <FollowingSubject title={'Courses'} items={topics} type={'Topic'} />
                </div>
                <div className={cx('searchContainer')}>
                    <div>
                        <HeadlessTippy
                            interactive
                            visible={dropDown}
                            placement={'bottom-start'}
                            render={(attrs) => (
                                <div className={cx('search-result')} tabIndex="-1" {...attrs}>
                                    <PopperWrapper className={cx('popper-result')}>
                                        <InstructorSearchResult
                                            className={cx('TopicsListDropDown')}
                                            courses={courses}
                                            topics={topics}
                                            subCategories={subCategories}
                                            categories={categories}
                                            onChose={handleClick}
                                        />
                                    </PopperWrapper>
                                </div>
                            )}
                            onClickOutside={handleHideResult}
                        >
                            <div className={cx('searchWrapper')}>
                                <label className={cx('searchTitle')}>Search</label>
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
                </div>
            </div>
        </>
    );
}

export default Following;
