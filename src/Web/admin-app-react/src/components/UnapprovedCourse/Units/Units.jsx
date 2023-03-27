import { useParams } from "react-router";
import LoadingSpinner from "../../LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";
import instance from "../../../api/request";
import { useNavigate } from "react-router-dom";

export default function Units() {

    const { id } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [courseInfo, setCourseInfo] = useState({});
    const [listLessons, setListLessons] = useState([])
    const navigate = useNavigate()

    useEffect(() => {
       setIsLoading(true);
       instance
            .get(`courses/${id}`)
            .then((res) => {
                setCourseInfo(res.data);
                return instance.get(`lessons?courseId=${id}`)
            })
            .then((res) => {
                return res.data;
            })
            .then(async (listLessons) => {
                const newListLessons = [];
                await Promise.all(
                    (listLessons).map(async (lesson) => {
                        const response = await instance.get(`units?lessonId=${lesson.lessonId}`);
                        const units = response.data;
                        const newLesson = {...lesson, "units": units}
                        newListLessons.push(newLesson);
                    }),
                );
                setListLessons(newListLessons);
            })
            .catch((err) => console.log(err))
            .finally(() => setIsLoading(false))
    }, []);

    const handleOpenDetailUnit = (unit) => {
        if(unit.isExam)
            navigate(`exam/${unit.unitId}`)
        else
            navigate(`material/${unit.unitId}`)
    }
 
    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <h3>{courseInfo.title}</h3>
            <h5>{courseInfo.description}</h5>
            <p>{courseInfo.about}</p>
            <div style={{marginTop: 20}}>
            <div>
                {listLessons && listLessons.map((lesson) => {
                    return (
                        <>
                            <div key={lesson.lessonId} style={{marginBottom: 20}}>
                                <div style={{display: "flex", alignItems: "center", justifyContent: "space-between"}}>
                                    <h6>{lesson.title}</h6>
                                </div>
                                {lesson.lessonId && lesson.units && lesson.units.map((unit) => (
                                    <div key={unit.unitId} style={{paddingLeft: 20, marginTop: 12}}>
                                        <p  onClick={() => handleOpenDetailUnit(unit)}
                                            style={{cursor: "pointer"}}>
                                            <strong>{unit.title}</strong> ( {unit.requiredMinutes} mins )
                                        </p>
                                    </div>
                                ))}
                            </div>
                        </>
                    )
                })}
                </div>
            </div>
        </div>
    )
}