import Modal from 'react-bootstrap/Modal';
import { Button } from 'react-bootstrap';
import StarRatings from 'react-star-ratings';
import styles from "./ReviewModal.module.scss";
import { useState } from 'react';
import axios from 'axios';
import config from '~/config';

export default function ReviewModal(props) {
    const { show, setShow, coursesEnrollment, isReviewed, rate } = props;
    const tokenStr = localStorage.getItem('accessToken');
    const [rating, setRating] = useState(5);
    const [reviewContent, setReviewContent] = useState("");
    const handleClose = () => setShow(false);
    const handleChangeRating = (newRating) => setRating(newRating);
    const onSubmit = async () => {
        const reviewCourse = {
            "courseId": coursesEnrollment.courseId,
            "stars": rating,
            "content": reviewContent
        }
        await axios.post(`${config.baseUrl}/api/ratings`, reviewCourse, {
            headers: { Authorization: `Bearer ${tokenStr}` }
        })
        .catch(err => console.log(err));
        setShow(false);
        window.location.reload();
    }

    return (
        <div>
            <Modal show={show} onHide={handleClose} backdrop="static" keyboard={false} size="md">
                <Modal.Header closeButton>
                    <h3 style={{ paddingLeft: 130 }}>Review Form</h3>
                </Modal.Header>
                <Modal.Body>
                    <h5>{coursesEnrollment.title}</h5>
                    {isReviewed && (
                        <div className={styles.rating}>
                            <span style={{ color: '#FFC069' }}>{rate[0].stars}</span>
                            <span>
                                <StarRatings
                                    starRatedColor="#FFC069"
                                    rating={rate[0].stars}
                                    starDimension="20px"
                                    starSpacing="4px"
                                    numberOfStars={5}
                                />
                            </span>
                        </div>
                    )}
                    {!isReviewed && (
                        <div>
                            <div className={styles.rating}>
                                <span style={{ color: '#FFC069' }}>{rating}</span>
                                <span>
                                    <StarRatings
                                        rating={rating}
                                        starRatedColor="#FFC069"
                                        changeRating={handleChangeRating}
                                        numberOfStars={5}
                                        starDimension="20px"
                                        starSpacing="4px"
                                    />
                                </span>
                            </div>
                        </div>
                    )}
                    <h5>Comment</h5>
                    {isReviewed && (
                        <textarea
                            style={{ borderRadius: 10, marginTop: 10 }}
                            disabled
                            value={rate[0].content}
                            rows="5" cols="60">
                        </textarea>
                    )}
                    {!isReviewed && (
                        <textarea
                            style={{ borderRadius: 10, marginTop: 10 }}
                            placeholder="“Give any additional context on what happened.”"
                            value={reviewContent}
                            onChange={(event) => setReviewContent(event.target.value)}
                            rows="5" cols="60">
                        </textarea>
                    )}
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>Cancel</Button>
                    <Button variant="success" disabled={isReviewed} onClick={onSubmit}>Save</Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}
