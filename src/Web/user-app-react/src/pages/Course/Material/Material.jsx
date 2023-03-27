import { useParams } from "react-router";
import axios from "axios";
import LoadingSpinner from "~/components/LoadingSpinner/LoadingSpinner";
import { useState, useEffect } from "react";
import config from "~/config";
import Button from "react-bootstrap/Button";

export default function Material() {
    const { materialId, enrollementId } = useParams();
    const [isLoading, setIsLoading] = useState(false);
    const [material, setMaterial] = useState({});
    const tokenStr = localStorage.getItem('accessToken');

    useEffect(() => {
        getMaterial();
    }, [materialId]);

    const getMaterial = () => {
        setIsLoading(true);
        axios.get(`${config.baseUrl}/api/units/${materialId}/material`, {
            headers: { Authorization: `Bearer ${tokenStr}` }
        })
        .then((res) => {
            setMaterial(res.data)
        })
        .finally(() => setIsLoading(false))
    }

    const handlePostMaterial = () => {
        const data = {
            enrollmentId: enrollementId,
            unitId: materialId
        }
        axios
        .post(`${config.baseUrl}/api/enrollments/material`, data, {
            headers: {
                'Authorization': `Bearer ${tokenStr}`,
                'Content-Type': 'application/json'
            }
        })
        .catch((err) => {
            console.log(err);
        })
        .finally(() => window.location.reload())
    }

    if(isLoading) return <LoadingSpinner />

    return (
        <div>
            <h3>{material.title}</h3>
            <h5 style={{color: "red", marginBottom: 20}}>{material.requiredMinutes} minutes</h5>
            <p>{material.content}</p>
            <Button 
                style={{marginTop: 20, marginLeft: 770}} variant="success"
                onClick={handlePostMaterial}>
                Done
            </Button>
        </div>
    )
}