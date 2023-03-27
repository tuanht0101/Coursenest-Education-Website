import { Outlet } from 'react-router-dom';
import Footer from '~/components/Footer/Footer';
import Header from '~/components/Header/Header';
import styles from './Layout.module.scss';
import { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate, useLocation } from 'react-router-dom';
import config from '~/config';

export default function Layout(props) {
    const {logged, isInstructor, isPublisher} = props;
    const navigate = useNavigate();

    const path = useLocation().pathname;
    const location = path.split('/')[1];

    const [categories, setCategories] = useState([]);
    const [subcategories, setSubcategories] = useState([]);
    const [topics, setTopics] = useState([]);
    const [isShownCategory, setIsShownCategory] = useState(false);
    const [isShownSubCategory, setIsShownSubCategory] = useState(false);
    const [isShownTopic, setIsShownTopic] = useState(false);

    useEffect(() => {
        axios
            .get(`${config.baseUrl}/api/categories/hierarchy`)
            .then((res) => {
                setCategories(res.data);
            })
            .catch((err) => console.log(err));
    }, []);

    useEffect(() => {
        setIsShownTopic(false);
    }, [subcategories]);

    const handleShowCategory = () => {
        setIsShownCategory(true);
    };

    const handleCloseCategory = () => {
        setIsShownCategory(false);
        setIsShownSubCategory(false);
        setIsShownTopic(false);
    };

    const handleMouseOverCate = (data) => {
        setIsShownSubCategory(true);
        setSubcategories(data);
    };

    const handleMouseOverSubCate = (data) => {
        setIsShownTopic(true);
        setTopics(data);
    };

    const handleClickTopic = (dataTopic) => {
        navigate(`/topics/${dataTopic.id}`);
    };

    return (
        <div className={styles.container}>
            <div className={styles[`${location}`]}>
                <Header
                    className={styles.header}
                    logged={logged}
                    isInstructor={isInstructor} 
                    isPublisher={isPublisher}
                    categories={categories}
                    subcategories={subcategories}
                    topics={topics}
                    handleClickTopic={handleClickTopic}
                    handleShowCategory={handleShowCategory}
                    handleCloseCategory={handleCloseCategory}
                    handleMouseOverCate={handleMouseOverCate}
                    handleMouseOverSubCate={handleMouseOverSubCate}
                    isShownCategory={isShownCategory}
                    isShownSubCategory={isShownSubCategory}
                    isShownTopic={isShownTopic}
                />
                <div className={styles.outlet}>
                    <Outlet />
                </div>
                <Footer />
            </div>
        </div>
    );
}
