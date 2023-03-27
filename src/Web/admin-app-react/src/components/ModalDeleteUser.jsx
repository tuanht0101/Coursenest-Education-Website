import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import instance from "../api/request";

function ModalDeleteUser(props) {
    const { show, setShow, deletedData, fetchListUser } = props;
    const handleClose = () => setShow(false);
    const handleSubmitDelUser = () => {
        instance
            .delete(`users/${deletedData.userId}`)
            .then(() => {
                handleClose();
                fetchListUser();
            });
    };
    return (
        <>
            <Modal
                show={show}
                onHide={handleClose}
                backdrop="static"
                keyboard={false}
                size="lg"
            >
                <Modal.Header closeButton>
                    <Modal.Title>Delete this User ?</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    Are you sure delete{" "}
                    <b>
                        {deletedData && deletedData.email
                            ? deletedData.email
                            : ""}
                    </b>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancel
                    </Button>
                    <Button
                        variant="danger"
                        onClick={() => {
                            handleSubmitDelUser();
                        }}
                    >
                        Delete
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ModalDeleteUser;
