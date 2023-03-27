export default function Units(props) {
    const { listLessons, courseInfo, handleOpenDetailUnit} = props;
    return (
        <div>
            <h3>{courseInfo.title}</h3>
            <h5 style={{paddingTop: 20}}>{courseInfo.description}</h5>
            <p style={{paddingTop: 20}}>{courseInfo.about}</p>
            <div style={{marginTop: 20}}>
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
    )
}