import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import { useNavigate } from "react-router-dom";

function ModalConfirm(props) {
    const { show, setShow, unit } = props;
    const handleClose = () => setShow(false);
    console.log(unit)

    const navigate = useNavigate();
    const handleConfirm = () => {
        setShow(false);
        navigate(`exam/${unit.unitId}`)
    }
    
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
                    <Modal.Title>Confirmation</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <p>Are you sure to take the exam?</p>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Cancel
                    </Button>
                    <Button
                        variant="danger"
                        onClick={() => {
                            handleConfirm();
                        }}
                    >
                        Yes
                    </Button>
                </Modal.Footer>
            </Modal>
        </>
    );
}

export default ModalConfirm;
