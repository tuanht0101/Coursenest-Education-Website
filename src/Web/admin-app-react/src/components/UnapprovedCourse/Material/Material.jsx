import { useParams } from "react-router";
import instance from "../../../api/request";
import LoadingSpinner from "../../LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";

export default function Material() {
    const { materialId } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [material, setMaterial] = useState({});

    useEffect(() => {
        setIsLoading(true);
        instance.get(`units/${materialId}/material`)
        .then((res) => {
            setMaterial(res.data)
        })
        .finally(() => setIsLoading(false))
    }, [materialId]);

    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <h3>{material.title}</h3>
            <h5 style={{color: "red", marginBottom: 20}}>{material.requiredMinutes} minutes</h5>
            <p>{material.content}</p>
        </div>
    )
}