import { useEffect, useRef, useState } from "react";
import PropTypes from "prop-types";
import MDBox from "components/MDBox";
import Card from "@mui/material/Card";
import Grid from "@mui/material/Grid";



import MDTypography from "components/MDTypography";

import DefaultItem from "pages/components/Items/DefaultItem";

function SqlConfiguration({ data }) {
    return (
        data && <Card>
            <MDBox pt={2} px={2} lineHeight={1}>
                <MDTypography variant="h6" fontWeight="medium">
                    {data?.Name}
                </MDTypography>
            </MDBox>
            <MDBox p={2} over>
                {data.Result.map(c => {
                    return (
                        <MDBox mb={2.5} key={c.Id} >
                            <DefaultItem
                                icon="check"
                                title={c.Name}
                                description={c.Value}
                            />

                        </MDBox>)
                })}
            </MDBox>
        </Card>
    )
}
SqlConfiguration.propTypes = {
    data: PropTypes.object,
};
export default SqlConfiguration;