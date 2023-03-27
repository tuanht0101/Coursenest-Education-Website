import Badge from 'react-bootstrap/Badge';
import getNumberOfDays from '../helper/getNumberOfDays';

export default function GetRolesByUserId(props) {

    const { roles } = props;
    const checkRole = (role) => {
        if((role.type == 0) && (getNumberOfDays(role.expiry) > 0)) return 0;
        else if ((role.type == 1) && (getNumberOfDays(role.expiry) > 0)) return 1;
        else if ((role.type == 2) && (getNumberOfDays(role.expiry) > 0)) return 2;
        else if ((role.type == 3) && (getNumberOfDays(role.expiry) > 0)) return 3;
    }
    return (
        <div>
            {(roles) && (roles).map((role, i) => {
                return (
                    <Badge
                        style={{ marginRight: 10 }}
                        key={i} pill
                        bg={(checkRole(role) == 0) ? "info" 
                        : (checkRole(role) == 1) ? "secondary" 
                        : (checkRole(role) == 2) ? "success" 
                        : (checkRole(role) == 3) ? "dark" : ""}
                    >
                        {
                            (checkRole(role) == 0) ? "Student"
                                : (checkRole(role) == 1) ? "Instructor"
                                    : (checkRole(role) == 2) ? "Publisher"
                                        : (checkRole(role) == 3) ? "Admin"
                                            : ""
                        }
                    </Badge>
                );
            })}
        </div>
    );
}