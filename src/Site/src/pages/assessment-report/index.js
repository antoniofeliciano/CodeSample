import { useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import DashboardLayout from "pages/components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "pages/components/Navbars/DashboardNavbar";
import MDBox from "components/MDBox";
import { CollectData } from "pages/api/assessmentReportApi";
import Spinner from 'pages/components/Spinner';
import Grid from "@mui/material/Grid";
import ServerInfo from "./server-Info";
import SqlConfiguration from "./sql-configuration";
import DbInfo from "./db-info";
import ReplicationDatabases from "./replication-databases";

function AssessmentReport() {

    const [collectData, setCollectData] = useState();
    const [isLoading, setIsLoading] = useState(true);

    const { id } = useParams();

    useEffect(() => {
        CollectData((res) => {
            setCollectData(JSON.parse(res.data.data));
            setIsLoading(false);
        }, id)
    }, []);


    return (<DashboardLayout>
        <DashboardNavbar />
        {isLoading ? <Spinner /> :
            <MDBox my={3}>
                <Grid container spacing={2} >
                    <Grid item xs={6} sm={6} lg={6}>
                        <ServerInfo data={collectData.filter(o => o.name === "ServerInfo")[0]} />
                    </Grid>
                    <Grid item xs={6} sm={6} lg={6}>
                        <ReplicationDatabases data={collectData.filter(o => o.name === "CheckReplicationDatabases")[0]} />
                    </Grid>
                    <Grid item xs={6} sm={6} lg={6}>
                        <SqlConfiguration data={null} />
                    </Grid>
                    <Grid item xs={12} sm={12} lg={12}>
                        <DbInfo data={collectData[8]} />
                    </Grid>
                </Grid>
            </MDBox>
        }
    </DashboardLayout>
    )
}
export default AssessmentReport;