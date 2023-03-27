import axios from 'axios';
import { useEffect, useState } from 'react';
import AllCoursesByTopic from '~/components/AllCoursesByTopic/AllCoursesByTopic';
import config from '~/config';
import styles from "./InterestedTopics.module.scss"

export default function InterestedTopics(props) {
    const {logged} = props;

    const userId = localStorage.getItem('userId');
    const [topics, setTopics] = useState([]);
    const [activeTopic, setActiveTopic] = useState();
    const [listCourses, setListCourses] = useState([]);

    const getInterestTopics = () => {
        axios
            .get(`${config.baseUrl}/api/users/${userId}`)
            .then((res) => {
                setTopics(res.data.interestedTopics);
                return res.data.interestedTopics;
            })
            .then(async (res) => {
                const allInterestedTopics = [];
                await Promise.all(
                    res.map(async (id) => {
                        const response = await axios.get(`${config.baseUrl}/api/topics/${id}`);
                        const topic = response.data;
                        allInterestedTopics.push(topic);
                    }),
                );
                setTopics(allInterestedTopics);
                setActiveTopic(allInterestedTopics[0]);
            })
            .catch((err) => {
                console.log(err);
            });
    }

    const getListCourse = (topicId) => {
        axios
            .get(
                `${config.baseUrl}/api/courses?TopicId=${topicId}&IsApproved=true&SortBy=0&PageNumber=${1}&PageSize=${5}`,
            )
            .then((res) => {
                setListCourses(res.data);
            })
            .catch((err) => {
                console.log(err);
            });
    }

    useEffect(() => {
        getInterestTopics();
    }, []);

    useEffect(() => {
        if(activeTopic != undefined) {
            getListCourse(activeTopic?.topicId);
        }
    }, [activeTopic]);

    const handleClickTopic = (data) => {
        setActiveTopic(data);
    };

    return (
        <div className={styles.container}>
            <h3 className={styles.heading}>Most Popular Topics</h3>
                        <div className={styles.listInterestedTopic}>
                            {topics &&
                                topics.map((topic, index) => {
                                    return (
                                        <div
                                            key={index}
                                            className={styles.interestedTopic}
                                            // key={topic.topicId}
                                            onClick={() => {
                                                handleClickTopic(topic);
                                            }}
                                        >
                                            {topic.content}
                                        </div>
                                    );
                                })}
                        </div>
                        <div>
                            <h3 className={styles.heading}>Top Courses</h3>
                            <AllCoursesByTopic listCourses={listCourses.queried} logged={logged}/>
                        </div>
        </div>
    );
}
