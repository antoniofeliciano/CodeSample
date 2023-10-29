/* eslint-disable react/prop-types */
import { useEffect, useRef, useState } from "react";
import PropTypes from "prop-types";
import MDBox from "components/MDBox";
import Card from "@mui/material/Card";
import Grid from "@mui/material/Grid";

import DataTable from "pages/components/Tables/DataTable";
import DefaultCell from "pages/components/Tables/DefaultCell";

import MDTypography from "components/MDTypography";
import PieChart from "pages/components/Charts/PieChart";

import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";



function DbInfo({ data }) {

    const [chartData, setChartData]= useState([]);
    const [atualDb, setAtualDb] = useState(chartData[0]);

    useEffect(() => {
        if(data?.result){
            setChartData([].concat(...data?.result));
        }
    }, []);
    
    return (
       data && <Card>
            <MDBox pt={2} px={2} lineHeight={1}>
                <MDTypography variant="h6" fontWeight="medium">
                    {data?.Name}
                </MDTypography>
            </MDBox>
            <MDBox p={2}>
                <Grid container spacing={2} >
                    <Grid item xs={6} sm={6} lg={6}>
                        <DataTable
                            entriesPerPage={false}
                            showTotalEntries={false}
                            isSorted={false}
                            table={{
                                columns: [
                                    
                                    { Header: "DatabaseName", accessor: "DatabaseName", width: "30%", Cell: ({ row }) => <DefaultCell value={row.original.DatabaseName} onClick={() => setAtualDb(row.original)} />, },
                                    { Header: "Status", accessor: "Status", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "DbOwner", accessor: "dbOwner", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "AutoShrink", accessor: "AutoShrink", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "AutoStatsCreate", accessor: "AutoStatsCreate", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "AutoStatsUpdate", accessor: "AutoStatsUpdate", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "Collation", accessor: "Collation", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "DataFileSizeMB", accessor: "DataFileSizeMB", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "DataFreeSpaceMB", accessor: "DataFreeSpaceMB", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "DataFreeSpacePct", accessor: "DataFreeSpacePct", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "DataSpaceUsedMB", accessor: "DataSpaceUsedMB", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "LogFileSizeMB", accessor: "LogFileSizeMB", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "LogFreeSpaceMB", accessor: "LogFreeSpaceMB", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "LogFreeSpacePct", accessor: "LogFreeSpacePct", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "LogSpaceUsedMB", accessor: "LogSpaceUsedMB", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                    { Header: "ReadCommitedSnapshot", accessor: "ReadCommitedSnapshot", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                ],
                                rows: chartData
                            }} />
                    </Grid>

                    <Grid item xs={3} sm={3} lg={3}>
                        <MDBox height={"100%"} >
                            <Pie data={{
                                labels: ['DataFreeSpaceMB', 'DataSpaceUsedMB'],
                                datasets: [
                                    {
                                        data: [atualDb.DataFreeSpaceMB, atualDb.DataSpaceUsedMB],
                                        backgroundColor: ['#77C368', '#160C4B',],
                                    },
                                ],
                            }} options={{
                                responsive: true,
                                maintainAspectRatio: false,
                                plugins: {
                                    legend: {
                                        display: true,
                                        position: "bottom",

                                    },  title: {
                                        display: true,            // Exibe o título da legenda
                                        text: "Categorias",       // Texto do título da legenda
                                        color: "blue",            // Cor do título da legenda
                                        font: {
                                            size: 16                // Tamanho da fonte do título da legenda
                                        }
                                    }
                                },
                                interaction: {
                                    intersect: false,
                                    mode: "index",
                                },
                            }} redraw />
                        </MDBox>
                    </Grid>
                    <Grid item xs={3} sm={3} lg={3}>
                        <MDBox height={"100%"}>

                            <Pie data={{
                                labels: ['LogFreeSpaceMB', 'LogSpaceUsedMB'],
                                datasets: [
                                    {
                                        data: [atualDb.LogFreeSpaceMB, atualDb.LogSpaceUsedMB],
                                        backgroundColor: ['#77C368', '#160C4B',],
                                    },
                                ],
                            }} options={{
                                responsive: true,
                                maintainAspectRatio: false,
                                plugins: {
                                    legend: {
                                        display: true,
                                        position: "bottom",

                                    },
                                },
                                interaction: {
                                    intersect: false,
                                    mode: "index",
                                },
                            }} redraw />


                        </MDBox>
                    </Grid>

                </Grid>

            </MDBox>
        </Card >
    )
}
DbInfo.propTypes = {
    data: PropTypes.object,
};
export default DbInfo;