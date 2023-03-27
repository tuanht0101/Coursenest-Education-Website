import { useEffect, useState } from 'react';
import styles from './Profile.module.scss';
import Modal from 'react-bootstrap/Modal';
import Button from 'react-bootstrap/Button';
import Achievement from '~/components/Achievement/Achievement';
import avatarImg from '../../assets/images/user-avatar.png';
import experienceImg from '../../assets/images/experience.png';
import config from '~/config';
import axios from 'axios';
import { isEqual, uniq } from "lodash";
import LoadingSpinner from '~/components/LoadingSpinner/LoadingSpinner';

export default function Profile() {

    const userId = localStorage.getItem("userId")
    const token = localStorage.getItem("accessToken");

    const [isLoading, setIsLoading] = useState(false);

    const [userInfo, setUserInfo] = useState({});
    const [userInfoNeedUpdate, setUserInfoNeedUpdate] = useState({});
    const [currentInfo, setCurrentInfo] = useState({});
    const [changedInfo, setChangedInfo] = useState({});
    const [aboutMe, setAboutMe] = useState({ "aboutMe": "" });
    const [gender, setGender] = useState({});
    const [experience, setExperience] = useState({
        "name": "",
        "title": "",
        "started": "",
        "ended": ""
    })
    const [deletedExperience, setDeletedExperience] = useState();

    const [showModalAchievement, setShowModalAchievement] = useState(false);
    const [showModalChangeAvatar, setShowModalChangeAvatar] = useState(false);
    const [showModalEditInfo, setShowModalEditInfo] = useState(false);
    const [showModalEditAboutMe, setShowModalEditAboutMe] = useState(false);
    const [showModalAddExperience, setShowModalAddExperience] = useState(false);
    const [showModalDeleteExperience, setShowModalDeleteExperience] = useState(false);


    const [avatar, setAvatar] = useState(avatarImg);
    const [file, setFile] = useState(null);
    const [preview, setPreview] = useState();

    const [test, setTest] = useState({});


    useEffect(() => {
        fetchInfoUser();
    }, []);

    const fetchInfoUser = () => {
        setIsLoading(true);
        axios
            .get(`${config.baseUrl}/api/users/${userId}`)
            .then((res) => {
                setUserInfo(res.data);
                if (res.data.avatar != null) setAvatar(res.data.avatar.uri);
                setUserInfoNeedUpdate({
                    "email": res.data.email,
                    "phonenumber": res.data.phonenumber,
                    "fullName": res.data.fullName,
                    "title": res.data.title,
                    "location": res.data.location,
                    // "gender": res.data.gender,
                    "dateOfBirth": res.data.dateOfBirth
                })
                setAboutMe({ "aboutMe": res.data.aboutMe });
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false));
    }

    // update basic info
    const handleClickEditInfo = () => {
        setShowModalEditInfo(true);
        setCurrentInfo(userInfoNeedUpdate);
    }

    const handleChangeInfo = (event) => {
        let value = event.target.value;
        let name = event.target.name;

        if (name == "dateOfBirth") {
            value = new Date(event.target.value).toISOString();
        }
        setTest({
            ...test,
            [name]: value
        })
        setUserInfoNeedUpdate({
            ...userInfoNeedUpdate,
            [name]: value,
        });
    }

    const getUpdatedKeys = (newData, oldData) => {
        const data = uniq([...Object.keys(newData), ...Object.keys(oldData)]);
        const keys = [];
        for (const key of data) {
            if (!isEqual(oldData[key], newData[key])) {
                keys.push(key);
                setChangedInfo({
                    ...changedInfo,
                    [key]: newData[key]
                })
            }
        }
        return keys;
    }

    const handleConfirmUpdateInfo = (event) => {
        event.preventDefault();
        const update = getUpdatedKeys(userInfoNeedUpdate, currentInfo);

        if (update.length === 0) {
            setShowModalEditInfo(false);
        }
        else {
            axios.put(`${config.baseUrl}/api/users/me`, test, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
                .then(() => {
                    setShowModalEditInfo(false);
                    fetchInfoUser();
                })
                .catch((err) => {
                    console.log(err);
                })
        }
    }

    // change avatar
    const handleNewImage = (e) => {
        setFile(e.target.files[0]);
        const objectUrl = URL.createObjectURL(e.target.files[0]);
        setPreview(objectUrl);
    }

    const handleClickSaveChangeAvatar = () => {
        const formData = new FormData();
        formData.append("formFile", file);
        axios
            .put(`${config.baseUrl}/api/users/me/cover`, formData, {
                headers: {
                    'Content-Type': "multipart/form-data",
                    'Authorization': `Bearer ${token}`
                }
            }
            )
            .then(() => {
                fetchInfoUser();
                setShowModalChangeAvatar(false);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    // update about me info
    const handleConfirmUpdateAboutMe = (event) => {
        event.preventDefault();
        axios
            .put(`${config.baseUrl}/api/users/me`, aboutMe, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
            .then(() => {
                setShowModalEditAboutMe(false);
                fetchInfoUser();
            })
            .catch((err) => {
                console.log(err);
            })
    }

    const handleChangeAboutMe = (event) => {
        const value = event.target.value;
        setAboutMe({ "aboutMe": value });
    }

    const handleClickEditAboutMe = () => {
        setShowModalEditAboutMe(true);
    }

    // add experience
    const handleClickAddExperience = () => {
        setShowModalAddExperience(true);
    }

    const handleChangeExperience = (event) => {
        let value = event.target.value;
        let name = event.target.name;

        if (name == "started" || name == "ended") {
            value = new Date(event.target.value).toISOString();
        }
        setExperience({
            ...experience,
            [name]: value
        })
    }

    const handleConfirmAddExperience = (event) => {
        event.preventDefault();
        axios
            .post(`${config.baseUrl}/api/users/me/experiences`, experience, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
            .then(() => {
                setShowModalAddExperience(false);
                fetchInfoUser();
            })
            .catch((err) => {
                console.log(err);
            })
    }

    // delete experience
    const handleClickDeleteExperience = (experienceId) => {
        setShowModalDeleteExperience(true);
        setDeletedExperience(experienceId);
    }

    const handleSubmitDeleteExperience = () => {
        axios
            .delete(`${config.baseUrl}/api/users/me/experiences/${deletedExperience}`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                }
            })
            .then(() => {
                setShowModalDeleteExperience(false);
                fetchInfoUser();
            })
            .catch((err) => {
                console.log(err);
            })
    }
    
    if(isLoading) return <LoadingSpinner />

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <div className={styles.avatarContainer} onClick={() => setShowModalChangeAvatar(true)}>
                    <img className={styles.roundAvatar} src={avatar} alt="" />
                    <div>
                        <div className={styles.middle}></div>
                        <p className={styles.text}>Edit</p>
                    </div>
                </div>
                <div className={styles.rightHeader}>
                    <h2>{userInfo.fullName}</h2>
                    <p className={styles.jobTitleLabel}>Job Title</p>
                    <p>{userInfo.title}</p>
                </div>
            </div>

            <div className={styles.body}>
                <div>
                    {/* Achievements la enrollment completed */}
                    <div className={styles.heading}>
                        <h2>Achievements</h2>
                        <p className={styles.seeAllBtn} onClick={() => setShowModalAchievement(true)}>
                            SEE ALL
                        </p>
                    </div>
                    <div className={styles.listAchievement}>
                        {userInfo.achievements &&
                            userInfo.achievements.slice(0, 6).map((achievement) => {
                                return <Achievement key={achievement.achievementId} achievement={achievement} />;
                            })}
                    </div>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>About Me</h2>
                        <p className={styles.editBtn} onClick={() => handleClickEditAboutMe()}>Edit</p>
                    </div>
                    <p>{userInfo.aboutMe}</p>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>Basic Information</h2>
                        <p className={styles.editBtn} onClick={() => handleClickEditInfo()}>Edit</p>
                    </div>
                    <table className={styles.tableInfo}>
                        <tbody>
                            {/* <tr>
                                <td>Gender</td>
                                <td className={styles.tableRightContent}>
                                    {userInfo.gender === 1 ? 'female' : userInfo.gender === 0 ? 'male' : 'N/A'}
                                </td>
                            </tr> */}
                            <tr>
                                <td>Date of Birth</td>
                                <td className={styles.tableRightContent}>
                                    {userInfo.dateOfBirth == null
                                        ? 'N/A'
                                        : new Date(userInfo.dateOfBirth).toDateString()}
                                </td>
                            </tr>
                            <tr>
                                <td>Phone number</td>
                                <td className={styles.tableRightContent}>{userInfo.phonenumber == null ? 'N/A' : userInfo.phonenumber}</td>
                            </tr>
                            <tr>
                                <td>Email</td>
                                <td className={styles.tableRightContent}>{userInfo.email}</td>
                            </tr>
                            <tr>
                                <td>Location</td>
                                <td className={styles.tableRightContent}>{userInfo.location == null ? 'N/A' : userInfo.location}</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div>
                    <div className={styles.heading}>
                        <h2>Work Experiences</h2>
                        <p className={styles.editBtn} onClick={() => handleClickAddExperience()}>Add</p>
                    </div>
                    <div className={styles.listExperience}>
                        {userInfo.experiences &&
                            userInfo.experiences.map((experience) => {
                                return (
                                    <div key={experience.experienceId} className={styles.experience}>
                                        <img src={experienceImg} alt="" className={styles.experienceImage} />
                                        <div className={styles.experienceContent}>
                                            <h5>{experience.name}</h5>
                                            <p>{experience.title}</p>
                                            <div style={{ display: "flex", gap: 10, alignItems: "baseline" }}>
                                                <p>{new Date(experience.started).toDateString()}</p>
                                                <p> - </p>
                                                <p>{new Date(experience.ended).toDateString()}</p>
                                                <i class="fa fa-trash" onClick={() => handleClickDeleteExperience(experience.experienceId)}></i>
                                            </div>
                                        </div>
                                    </div>
                                );
                            })}
                    </div>
                </div>
            </div>

            <Modal
                show={showModalChangeAvatar}
                onHide={() => setShowModalChangeAvatar(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>Change Avatar</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body style={{ display: 'flex', justifyContent: 'center', alignItems: 'center', gap: 30 }} >
                    <img style={{ width: 200, height: 200 }} src={preview} alt="" />
                    <input name="newImage" type="file" onChange={handleNewImage} />
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="primary" onClick={handleClickSaveChangeAvatar}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>

            <Modal
                show={showModalEditAboutMe}
                onHide={() => setShowModalEditAboutMe(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>About Me</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form onSubmit={handleConfirmUpdateAboutMe} style={{ paddingLeft: 50, paddingRight: 50 }}>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>About me:</label>
                            <textarea
                                cols="70"
                                rows="10"
                                type="text"
                                name="aboutMe"
                                value={aboutMe?.aboutMe}
                                onChange={handleChangeAboutMe}>
                            </textarea>
                        </div>
                        <div>
                            <input type='submit' style={{ color: "white", padding: 5, backgroundColor: "blue", borderRadius: 5 }} />
                        </div>
                    </form>
                </Modal.Body>
            </Modal>

            <Modal
                show={showModalEditInfo}
                onHide={() => setShowModalEditInfo(false)}
                backdrop="static"
                keyboard={false}
                size="md"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>Edit Infomations</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form onSubmit={handleConfirmUpdateInfo} style={{ paddingLeft: 50, paddingRight: 50 }}>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Email:</label>
                            <input
                                type="text"
                                name="email"
                                value={userInfoNeedUpdate.email}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Phone Number:</label>
                            <input
                                type="number"
                                name="phonenumber"
                                value={userInfoNeedUpdate.phonenumber}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Full Name:</label>
                            <input
                                type="text"
                                name="fullName"
                                value={userInfoNeedUpdate.fullName}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Title:</label>
                            <input
                                type="text"
                                name="title"
                                value={userInfoNeedUpdate.title}
                                onChange={handleChangeInfo}
                            />
                        </div>

                        {/* <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Gender: </label>
                            //<select name='gender' defaultValue={userInfoNeedUpdate.gender} onChange={handleChangeInfo}>
                            <select name='gender' onChange={(event) => setGender({ "gender": Number(event.target.value) })}>
                                <option value={0}>Male</option>
                                <option value={1}>Female</option>
                            </select>
                        </div> 
                        */}

                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Date of birth:</label>
                            <input type="date" name='dateOfBirth' onChange={handleChangeInfo} />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Location:</label>
                            <input
                                type="text"
                                name="location"
                                value={userInfoNeedUpdate.location}
                                onChange={handleChangeInfo}
                            />
                        </div>
                        <div>
                            <input type='submit' style={{ color: "white", padding: 5, backgroundColor: "blue", borderRadius: 5 }} />
                        </div>
                    </form>
                </Modal.Body>
            </Modal>

            <Modal
                show={showModalAchievement}
                onHide={() => setShowModalAchievement(false)}
                backdrop="static"
                keyboard={false}
                size="lg"
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h2>Achievements</h2>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body style={{ display: 'flex', gap: 20, flexWrap: 'wrap', justifyContent: 'center' }}>
                    {userInfo.achievements &&
                        userInfo.achievements.map((achievement) => {
                            return <Achievement key={achievement.achievementId} achievement={achievement} />;
                        })}
                </Modal.Body>
            </Modal>

            <Modal
                show={showModalAddExperience}
                onHide={() => setShowModalAddExperience(false)}
                backdrop="static"
                keyboard={false}
                size="md"
                centered
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h3>Add Experience</h3>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form onSubmit={handleConfirmAddExperience} style={{ paddingLeft: 50, paddingRight: 50 }}>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Name:</label>
                            <input
                                type="text"
                                name="name"
                                required
                                onChange={handleChangeExperience}
                            />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Title:</label>
                            <input
                                type="text"
                                name="title"
                                required
                                onChange={handleChangeExperience}
                            />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Started:</label>
                            <input
                                type="date"
                                name='started'
                                required
                                onChange={handleChangeExperience}
                            />
                        </div>
                        <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 20 }}>
                            <label>Ended:</label>
                            <input
                                type="date"
                                name='ended'
                                required
                                onChange={handleChangeExperience}
                            />
                        </div>
                        <div>
                            <input type='submit' style={{ color: "white", padding: 5, backgroundColor: "blue", borderRadius: 5 }} />
                        </div>
                    </form>
                </Modal.Body>
            </Modal>

            <Modal
                show={showModalDeleteExperience}
                onHide={() => setShowModalDeleteExperience(false)}
                backdrop="static"
                keyboard={false}
                size="sm"
                scrollable={true}
            >
                <Modal.Header closeButton>
                    <Modal.Title>
                        <h5>Delete This Experience ?</h5>
                    </Modal.Title>
                </Modal.Header>
                <Modal.Footer>
                    <Button variant="secondary" onClick={() => setShowModalDeleteExperience(false)}>
                        Cancel
                    </Button>
                    <Button
                        variant="danger"
                        onClick={() => {
                            handleSubmitDeleteExperience();
                        }}
                    >
                        Delete
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}