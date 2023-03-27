import Modal from "react-bootstrap/Modal";
import instance from "../api/request";
import { useState } from "react";

function ModalAddCate(props) {
    const { show, setShow, fetchListCate } = props;
    const handleClose = () => setShow(false);
    const [info, setInfo] = useState("");

    const handleChange = (event) => {
        setInfo(event.target.value);
    }

    const handleSubmit = (event) => {
        event.preventDefault();
        instance
            .post(`categories`, info)
            .then(() => {
                handleClose();
                fetchListCate();
            })
            .catch((err) => {
                console.log(err);
            })
    };
    return (
        <>
            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
                size="sm"
            >
                <Modal.Header closeButton>
                    <Modal.Title>Add Category</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                <form onSubmit={handleSubmit}>
                    <input
                        name='category content'
                        value={info}
                        onChange={handleChange}
                        required
                    />
                    <input type='submit' style={{color: "white", padding: 5, marginLeft: 20, backgroundColor: "blue", borderRadius: 5}} />
                </form>
                </Modal.Body>
            </Modal>
        </>
    );
}

export default ModalAddCate;
