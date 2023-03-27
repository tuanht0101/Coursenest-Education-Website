import axios from 'axios';
import AllCoursesByTopic from '~/components/AllCoursesByTopic/AllCoursesByTopic';
import { useState, useEffect } from 'react';
import 'rsuite/dist/rsuite.min.css';
import { Pagination } from 'rsuite/';
import styles from './Topic.module.scss';
import { useParams, useNavigate } from 'react-router';
import images from '~/assets/images';
import config from '~/config';

export default function Topic() {
    const { id } = useParams();
    const [topic, setTopic] = useState({});
    const [allTopicsBySub, setAllTopicsBySub] = useState([]);
    const [topicSiblings, setTopicSiblings] = useState([]);
    const [subcate, setSubcate] = useState();

    const [listCourses, setListCourses] = useState([]);
    const [countCourse, setCountCourse] = useState();
    const [page, setPage] = useState(1);
    const [pageSize, setPageSize] = useState(1);

    const fetchListCourses = () => {
        axios
            .get(
                `${config.baseUrl}/api/courses?TopicId=${id}&IsApproved=true&SortBy=0&PageNumber=${page}&PageSize=${pageSize}`,
            )
            .then((res) => {
                setCountCourse(res.data.total);
                setListCourses(res.data.queried);
            })
            .catch((err) => {
                console.log(err);
            });
    };
    
    useEffect(() => {
        fetchListCourses();
    }, [page, pageSize, id]);

    useEffect(() => {
        axios
            .get(`${config.baseUrl}/api/topics/${id}`)
            .then((res) => {
                setTopic(res.data);
                return res.data;
            })
            .then((res) => {
                setSubcate(res.subcategoryContent);
                return axios.get(`${config.baseUrl}/api/topics?subcategoryId=${res.subcategoryId}`);
            })
            .then((res) => {
                const index = res.data.findIndex((object) => {
                    return object.topicId == id;
                });
                const firstArray = [...res.data.slice(0, index)];
                const secondArray = [...res.data.slice(index)];
                const newArray = [...secondArray, ...firstArray];
                setAllTopicsBySub(newArray);
                setTopicSiblings(res.data.filter((t) => t.topicId !== id));
            })
            .catch((err) => {
                console.log(err);
            });
    }, [id]);

    const handleOnChangePage = (event) => {
        setPage(parseInt(event));
    };

    const navigate = useNavigate();

    const handleClickTopic = (topicSiblingId) => {
        navigate(`/topics/${topicSiblingId}`);
    };

    const handleClickArrowRight = () => {
        const activeTopic = allTopicsBySub.slice(0, 1);
        const siblingTopics = allTopicsBySub.slice(1);
        setAllTopicsBySub([...siblingTopics, ...activeTopic]);
    };

    return (
        <div className={styles.container}>
            <div className={styles.navigation}>
                <h4>{subcate}</h4>
                <span className={styles.splitContent}></span>
                <div className={styles.navigationItems}>
                    {allTopicsBySub &&
                        allTopicsBySub.map((topic) => {
                            return (
                                <div
                                    className={`${styles.navigationItem} ${topic.topicId == id ? styles.active : ''}`}
                                    key={topic.topicId}
                                    onClick={() => {
                                        handleClickTopic(topic.topicId);
                                    }}
                                >
                                    {topic.content}
                                </div>
                            );
                        })}
                </div>
                <div className={styles.arrowRight} onClick={() => handleClickArrowRight()}>
                    <img src={images.arrowRight} alt="" />
                </div>
            </div>

            <div>
                <h2>{topic.content}</h2>
                <p>
                    {topic.content} related to <span style={{ color: '#C677FC' }}>{subcate}</span>
                </p>
            </div>

            <h4>{topic.content} Student also learn</h4>
            <div className={styles.topicSiblings}>
                {topicSiblings &&
                    topicSiblings.map((topicSibling) => {
                        return (
                            <div
                                className={styles.topicSibling}
                                key={topicSibling.topicId}
                                onClick={() => {
                                    handleClickTopic(topicSibling.topicId);
                                }}
                            >
                                {topicSibling.content}
                            </div>
                        );
                    })}
            </div>

            <div>
                <h4>All Courses</h4>
                <span>{countCourse} results</span>
                <AllCoursesByTopic listCourses={listCourses}/>
            </div>

            <div>
                {countCourse > pageSize && (
                    <div className={styles.pagination}>
                        <Pagination
                            prev
                            last
                            next
                            first
                            size="md"
                            total={countCourse}
                            limit={pageSize}
                            maxButtons={5}
                            activePage={page}
                            onChangePage={handleOnChangePage}
                        />
                    </div>
                )}
            </div>
        </div>
    );
}
