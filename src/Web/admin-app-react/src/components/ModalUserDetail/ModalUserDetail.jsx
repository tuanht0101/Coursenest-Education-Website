import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import styles from './ModalUserDetail.module.css'
import avatarDefault from '../../assets/avatar.png';
import { useState } from 'react';
import instance from "../../api/request";
import getNumberOfDays from '../../helper/getNumberOfDays';

function ModalDetailUser(props) {
    const { show, 
            setShow, 
            fetchListUser, 
            dataNeedUpdate, 
            handleClickDelUser, 
            roleStudent,
            roleInstructor,
            rolePublisher,
            roleAdmin,
            setRoleStudent,
            setRoleInstructor,
            setRolePublisher,
            setRoleAdmin,
    } = props;

    const [updateStudentRole, setUpdateStudentRole] = useState(null);
    const [updateInstructorRole, setUpdateInstructorRole] = useState(null);
    const [updatePublisherRole, setUpdatePublisherRole] = useState(null);
    const [updateAdminRole, setUpdateAdminRole] = useState(null);

    const [newPassword, setNewPassword] = useState('');

    const handleResetPwd = () => {
        instance
            .put(`authenticate/reset-password`, dataNeedUpdate.userId)
            .then((res) => {
                setNewPassword(res.data);
            })
            .catch((err) => {
                console.log(err);
            })
    }

    const handleUpdateUser = async () => {
        const updateRoles = [];
        if(updateStudentRole !== null) {
            updateRoles.push(updateStudentRole);
            setUpdateStudentRole(null);
        }
        if(updateInstructorRole !== null) {
            updateRoles.push(updateInstructorRole);
            setUpdateInstructorRole(null);
        }
        if(updatePublisherRole !== null) {
            updateRoles.push(updatePublisherRole);
            setUpdatePublisherRole(null);
        }
        if(updateAdminRole !== null) {
            updateRoles.push(updateAdminRole);
            setUpdateAdminRole(null);
        }
        const _ = require('lodash'); 
        if (!(_.isEmpty(updateRoles))) {
            await Promise.all(
                updateRoles.map(async (item) => {
                    await instance.put(`roles/${dataNeedUpdate.userId}`, item);
                })
            );
        }
        handleClose();
        fetchListUser();
    };

    const handleClose = () => {
        setRoleStudent(null);
        setRoleInstructor(null);
        setRolePublisher(null);
        setRoleAdmin(null);
        setShow(false);
    }
    
    const handleChangeExpiry = (event) => {
        let value = new Date(event.target.value).toISOString();
        let name = event.target.name;

        if (name == "student") {
            setUpdateStudentRole({
                "type": 0,
                "expiry": value
            })
        }
        else if (name == "instructor") {
            setUpdateInstructorRole({
                "type": 1,
                "expiry": value
            })
        }
        else if (name == "publisher") {
            setUpdatePublisherRole({
                "type": 2,
                "expiry": value
            })
        }
        else if (name == "admin") {
            setUpdateAdminRole({
                "type": 3,
                "expiry": value
            })
        }
    }

    return (
        <>
            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
                size="xm"
            >
                <Modal.Header closeButton>
                    User Detail
                </Modal.Header>

                <Modal.Body>
                    <div className={styles.userInfo}>
                        <div>
                            <img src={dataNeedUpdate.avatar?.uri ?? avatarDefault} className={styles.avatar} alt="" />
                        </div>
                        <div className={styles.userInfoLeft}>
                            <h3 className={styles.fullName}>{dataNeedUpdate.fullName}</h3>
                            <p>Username: {dataNeedUpdate.username}</p>
                            <p>Email: {dataNeedUpdate.email}</p>
                        </div>
                    </div>
                    <div>
                        <Button
                            className={styles.resetPsw}
                            variant="info"
                            onClick={() => {
                                handleResetPwd();
                            }}
                        >
                            Reset Password
                        </Button>
                        <br /><br />
                        <p>New Password: {newPassword}</p>
                        {/* <input type='text' value={newPassword} onFocus={e => e.target.select()} /> */}
                    </div>
                    <div className={styles.roles}>
                        <h5>Roles</h5>
                        <div className={styles.roundedCorner}>
                            <span>Student</span>
                            <span>{(roleStudent == null) ? "0" : getNumberOfDays(roleStudent.expiry)} day</span>
                            <input name="student" type="date" className={styles.noOutline} onChange={handleChangeExpiry}/>
                        </div>
                        <div className={styles.roundedCorner}>
                            <span>Instructor</span>
                            <span>{roleInstructor == null ? "0" : getNumberOfDays(roleInstructor.expiry)} day</span>
                            <input name="instructor" type="date" className={styles.noOutline} onChange={handleChangeExpiry}/>
                        </div>
                        <div className={styles.roundedCorner}>
                            <span>Publisher</span>
                            <span>{rolePublisher == null ? "0" : getNumberOfDays(rolePublisher.expiry)} day</span>
                            <input name="publisher" type="date" className={styles.noOutline} onChange={handleChangeExpiry}/>
                        </div>
                        <div className={styles.roundedCorner}>
                            <span>Admin</span>
                            <span>{roleAdmin == null ? "0" : getNumberOfDays(roleAdmin.expiry)} day</span>
                            <input name="admin" type="date" className={styles.noOutline} onChange={handleChangeExpiry}/>
                        </div>
                    </div>
                    <Button
                        className={styles.delUser}
                        variant="danger"
                        onClick={() => {
                            handleClickDelUser(dataNeedUpdate);
                        }}
                    >
                        Delete User
                    </Button>
                </Modal.Body>

                <Modal.Footer>
                    <Button
                        variant="secondary"
                        onClick={handleClose}
                    >
                        Cancel
                    </Button>
                    <Button
                        variant="success"
                        onClick={() => {
                            handleUpdateUser();
                        }}
                    >
                        Save
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ModalDetailUser;