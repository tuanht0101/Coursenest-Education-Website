import { useState, useEffect, useMemo } from "react";
import editIcon from "../../assets/edit-icon.png";
import deleteIcon from "../../assets/delete-icon.png";
import arrowDown from "../../assets/black-arrow-down.png";
import styles from "./ListCategories.module.css";
import instance from "../../api/request";
import LoadingSpinner from "../LoadingSpinner/LoadingSpinner";

import Search from "../Search";
import ModalDeleteCate from "../ModalDeleteCate";
import ModalDeleteSubCate from "../ModalDeleteSubCate";
import ModalAddCate from "../ModalAddCate";
import ModalAddSubCate from "../ModalAddSubCate";
import ModalEditSubCate from "../ModalEditSubCate";
import ModalEditCate from "../ModalEditCate";
import ModalAddTopic from "../ModalAddTopic";
import ModalDeleteTopic from "../ModalDeleteTopic";
import ModalEditTopic from "../ModalEditTopic";

export default function ListCategories() {

    const [keyword, setKeyWord] = useState();
    const [data, setData] = useState([]);

    const [showModalDeleteCate, setShowModalDeleteCate] = useState(false);
    const [showModalDeleteSubCate, setShowModalDeleteSubCate] = useState(false);
    const [showModalDeleteTopic, setShowModalDeleteTopic] = useState(false);

    const [showModalAddCate, setShowModalAddCate] = useState(false);
    const [showModalAddSubCate, setShowModalAddSubCate] = useState(false);
    const [showModalAddTopic, setShowModalAddTopic] = useState(false);

    const [showModalEditSubCate, setShowModalEditSubCate] = useState(false);
    const [showModalEditCate, setShowModalEditCate] = useState(false);
    const [showModalEditTopic, setShowModalEditTopic] = useState(false);

    const [deletedData, setDeletedData] = useState({})
    const [deletedSub, setDeletedSub] = useState({});
    const [deletedTopic, setDeletedTopic] = useState({});

    const [cateId, setCateId] = useState();
    const [subcateId, setSubcateId] = useState();
    const [subcate, setSubcate] = useState({});
    const [cate, setCate] = useState({});
    const [topicNeedUpdate, setTopicNeedUpdate] = useState({});

    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        fetchListCate();
    }, []);

    const fetchListCate = () => {
        setIsLoading(true);
        instance
            .get("categories/hierarchy")
            .then((res) => {
                setData(res.data);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }

    function getFilteredListCate() {
        if (!keyword) {
            return data;
        }
        return data.filter((cate) => {
            return (
                cate.content.toLowerCase().indexOf(keyword.toLowerCase()) !== -1
            );
        });
    }

    var filteredListCate = useMemo(getFilteredListCate, [keyword, data]);

    const handleClickDelCate = (cate) => {
        setShowModalDeleteCate(true);
        setDeletedData(cate);
    };

    const handleClickDelSubCate = (sub) => {
        setShowModalDeleteSubCate(true);
        setDeletedSub(sub);
    };

    const handleClickDelTopic = (topic) => {
        setShowModalDeleteTopic(true);
        setDeletedTopic(topic);
    }
    const handleAddSubCate = (data) => {
        setShowModalAddSubCate(true);
        setCateId(data);
    }
    const handleAddTopic = (data) => {
        console.log(data);
        setShowModalAddTopic(true);
        setSubcateId(data);
    }
    const handleEditTopic = (data) => {
        setShowModalEditTopic(true);
        setTopicNeedUpdate(data);
    }
    const handleEditSubCate = (data) => {
        setShowModalEditSubCate(true);
        setSubcate(data);
    }
    const handleEditCate = (data) => {
        setShowModalEditCate(true);
        setCate(data);
    }

    const [open, setOpen] = useState(0);
    const handleToggleButton = (sub) => {
        setOpen(sub.subcategoryId)
    }

    if(isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <h4 style={{minWidth: 180}}>Categories: {filteredListCate.length}</h4>
                <Search setKeyWord={setKeyWord} />
                <button style={{width: 130, height: 35}} className={styles.buttonAdd} onClick={() => setShowModalAddCate(true)}>Add Category</button>
            </div>
            <div>
                {filteredListCate && filteredListCate.map((cate, index) => {
                    return (
                        <div className={styles.cateBox} key={cate.categoryId}>
                            <div className={styles.cateHeader}>
                                <h4>{index+1}. {cate.content}</h4>
                                <div className={styles.actions}>
                                    <img title="edit cate"  src={editIcon} alt="" onClick={() => handleEditCate(cate)} />
                                    <img title="delete cate" src={deleteIcon} alt="" onClick={() => handleClickDelCate(cate)} />
                                    <button style={{width: 130, height: 35}}  className={styles.buttonAdd} onClick={() => handleAddSubCate(cate.categoryId)}>Add Subcategory</button>
                                </div>
                            </div>
                            <div className={styles.subBox}>
                                <div className={styles.subBoxContent}>
                                    {
                                        cate.subcategories && cate.subcategories.map((sub, index) => {
                                            return (
                                                <div key={sub.subcategoryId}>
                                                    {index != 0 && (
                                                        <hr className={styles.devider} />
                                                    )}
                                                    <div className={styles.subcateContent} >
                                                        <h5>{sub.content}: {sub.topics.length} topics</h5>
                                                        <div className={styles.actions}>
                                                            <button title="Add topic" style={{width: 23, height: 23}}  className={styles.buttonAdd} onClick={() => handleAddTopic(sub.subcategoryId)}>+</button>
                                                            <img title="Edit subcate" src={editIcon} alt="" onClick={() => handleEditSubCate(sub)} />
                                                            <img title="Delete subcate" src={deleteIcon} alt="" onClick={() => handleClickDelSubCate(sub)} />
                                                            <img src={arrowDown} alt="" style={{width: 30}} onClick={() => {handleToggleButton(sub)}}/>
                                                        </div>
                                                    </div>
                                                    {(open == sub.subcategoryId) && (
                                                    <div className={styles.listTopics}>
                                                        <div>
                                                        {
                                                            sub.topics && sub.topics.map((topic, index) => {
                                                                return (
                                                                    <div key={topic.id}>
                                                                        {index != 0 && (
                                                                            <hr className={styles.deviderTopic} />
                                                                        )}
                                                                        <div className={styles.topic}>
                                                                            <p>{topic.content}</p>
                                                                            <div className={styles.topicActions}>
                                                                                <img title="edit topic" src={editIcon} alt="" onClick={() => handleEditTopic(topic)}/>
                                                                                <img title="delete topic" src={deleteIcon} alt="" onClick={() => handleClickDelTopic(topic)} />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                )
                                                            })
                                                        }
                                                        </div>
                                                    </div>
                                                    )}
                                                </div>
                                            )
                                        })
                                    }
                                </div>
                            </div>
                        </div>
                    )
                })}
            </div>
            <ModalDeleteCate
                show={showModalDeleteCate}
                setShow={setShowModalDeleteCate}
                deletedData={deletedData}
                fetchListCate={fetchListCate}
            />
            <ModalDeleteSubCate
                show={showModalDeleteSubCate}
                setShow={setShowModalDeleteSubCate}
                deletedData={deletedSub}
                fetchListCate={fetchListCate}
            />
            <ModalDeleteTopic
                show={showModalDeleteTopic}
                setShow={setShowModalDeleteTopic}
                deletedData={deletedTopic}
                fetchListCate={fetchListCate}
            />
            <ModalAddCate
                show={showModalAddCate}
                setShow={setShowModalAddCate}
                fetchListCate={fetchListCate}
            />
            <ModalAddSubCate
                show={showModalAddSubCate}
                setShow={setShowModalAddSubCate}
                cateId={cateId}
                fetchListCate={fetchListCate}
            />
            <ModalAddTopic
                show={showModalAddTopic}
                setShow={setShowModalAddTopic}
                subcateId={subcateId}
                fetchListCate={fetchListCate}
            />
            <ModalEditSubCate
                show={showModalEditSubCate}
                setShow={setShowModalEditSubCate}
                subcate={subcate}
                fetchListCate={fetchListCate}
            />
            <ModalEditCate
                show={showModalEditCate}
                setShow={setShowModalEditCate}
                cate={cate}
                fetchListCate={fetchListCate} />
            <ModalEditTopic
                show={showModalEditTopic}
                setShow={setShowModalEditTopic}
                topicNeedUpdate={topicNeedUpdate}
                fetchListCate={fetchListCate} />
        </div>
    )
}