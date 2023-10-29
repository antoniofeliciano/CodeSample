import PropTypes from "prop-types";
import MDBox from "components/MDBox";
import Card from "@mui/material/Card";
import MDTypography from "components/MDTypography";
import MDAlert from "components/MDAlert";

import DefaultItem from "pages/components/Items/DefaultItem";
import toCamelCaseWithSpaces from "pages/components/Utils/stringUtils"

function ServerInfo({ data }) {
    return (
        data && <Card>
            <MDBox pt={2} px={2} lineHeight={1}>
                <MDTypography variant="h6" fontWeight="medium">
                    {toCamelCaseWithSpaces(data?.name)}
                </MDTypography>
            </MDBox>
            <MDBox p={2}>

                {
                    (data?.state === "error") ?


                        <MDBox mb={2.5}>
                            <MDAlert color="error">
                                <MDTypography variant="body2" color="white">
                                    {JSON.parse(data?.result).Message}
                                </MDTypography>
                            </MDAlert>
                        </MDBox>

                        : data?.result.map(c =>
                            <MDBox key={c.Id} mb={2.5}>
                                <DefaultItem
                                    icon={c.Icon}
                                    title={c.Name}
                                    description={c.Value}
                                />
                            </MDBox>
                        )}
            </MDBox>
        </Card>
    )
}
ServerInfo.propTypes = {
    data: PropTypes.object,
};
export default ServerInfo;