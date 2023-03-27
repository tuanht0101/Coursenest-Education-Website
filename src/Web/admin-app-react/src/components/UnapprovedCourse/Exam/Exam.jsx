import { useParams } from "react-router";
import instance from "../../../api/request";
import LoadingSpinner from "../../LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";

export default function Exam() {
    const { examId } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [exam, setExam] = useState({});

    useEffect(() => {
        setIsLoading(true);
        instance.get(`units/${examId}/exam`)
        .then((res) => {
            setExam(res.data)
        })
        .finally(() => setIsLoading(false))
    }, [examId]);

    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <h3>{exam.title}</h3>
            <h5 style={{color: "red"}}>{exam.requiredMinutes} minutes</h5>
            {exam.questions && exam.questions.map((question, index) => {
                return (
                    <div key={index}>
                        <p style={{marginTop: 20}}><strong>{index+1}. {question.content}</strong> ({question.point} point)</p>
                        {(question.choices).map((choice, index) => {
                            return (
                                <div key={index} style={{marginTop: 10}}>
                                    <label><input type="radio" disabled/> {choice.content}</label>
                                </div>
                            )
                        })}
                    </div>
                )
            })}
        </div>
    )
}