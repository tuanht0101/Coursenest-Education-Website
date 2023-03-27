import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import instance from "../api/request";

function ModalDeleteSubCate(props) {
    const { show, setShow, deletedData, fetchListCate } = props;
    const handleClose = () => setShow(false);
    const handleSubmitDelSubCate = () => {
        instance
            .delete(`subcategories/${deletedData.subcategoryId}`)
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
                    <Modal.Title>Delete this Subcategory ?</Modal.Title>
                </Modal.Header>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancel
                    </Button>
                    <Button
                        variant="danger"
                        onClick={() => {
                            handleSubmitDelSubCate();
                        }}
                    >
                        Delete
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ModalDeleteSubCate;
