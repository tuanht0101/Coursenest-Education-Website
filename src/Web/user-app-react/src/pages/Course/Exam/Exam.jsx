import { useParams } from "react-router";
import { useState, useEffect } from "react";
import axios from "axios";
import config from "~/config";
import LoadingSpinner from "~/components/LoadingSpinner/LoadingSpinner";
import Button from "react-bootstrap/Button";
import Countdown from "react-countdown";

export default function Exam() {
    const { examId, enrollementId } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [exam, setExam] = useState({});
    const tokenStr = localStorage.getItem('accessToken');
    const [timeOut, setTimeout] = useState(0);

    useEffect(() => {
        setIsLoading(true);
        axios.get(`${config.baseUrl}/api/units/${examId}/exam`, {
            headers: { Authorization: `Bearer ${tokenStr}` }
        })
        .then((res) => {
            setExam(res.data);
            // console.log(res.data.requiredMinutes);
            const time = (res.data.requiredMinutes) * 60000;
            setTimeout(time);
        })
        .finally(() => setIsLoading(false))
    }, [examId]);

    const handleSubmit = () => {
        if(window.confirm("Done yet? If you accept, the exam could not be changed. ")) {
            const data = {
                examUnitId: examId,
                enrollmentId: enrollementId
            }
            axios.post(`${config.baseUrl}/api/submissions`, data, {
                headers: { Authorization: `Bearer ${tokenStr}` }
            })
            .catch(err => console.log(err));
        }
    }

    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <div>
                <div style={{display: "flex", justifyContent: "space-between"}}>
                    <div>
                        <h3>{exam.title}</h3>
                        <h5 style={{color: "red"}}>{exam.requiredMinutes} minutes</h5>
                    </div>
                    <Countdown date={Date.now() + timeOut}/>
                </div>
                {exam.questions && exam.questions.map((question, index) => {
                    console.log(question)
                    return (
                        <div key={question.questionId}>
                            <p style={{marginTop: 20}}><strong>{index+1}. {question.content}</strong> ({question.point} point)</p>
                            {(question.choices).map((choice) => {
                                return (
                                    <div key={choice.choiceId} style={{marginTop: 10}}>
                                        <label><input type="radio" name={question.questionId}/> {choice.content}</label>
                                    </div>
                                )
                            })}
                        </div>
                    )
                })}
            </div>
            <div style={{marginTop: 30}}>
                <Button 
                    variant="danger"
                    onClick={handleSubmit}
                >
                    Submit
                </Button>
            </div>
        </div>
    )
}