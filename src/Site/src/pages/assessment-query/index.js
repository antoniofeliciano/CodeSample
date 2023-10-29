/* eslint-disable react/prop-types */
import { Card, Grid, Modal } from '@mui/material';
import Icon from "@mui/material/Icon";
import IconButton from '@mui/material/IconButton';
import Switch from "@mui/material/Switch";
import AceEditor from 'react-ace';
import 'ace-builds/src-noconflict/mode-sql';
import 'ace-builds/src-noconflict/theme-sqlserver';
import MDBox from "components/MDBox";
import MDButton from "components/MDButton";
import MDTypography from "components/MDTypography";
import Autocomplete from "@mui/material/Autocomplete";
import { AddAssessmentQuery, AssessmentQueriesForGrid, DeleteAssessmentQuery, EditAssessmentQuery, AssessmentQueryFile } from "pages/api/assessmentQueryApi";
import { DatabaseTypesForGrid } from 'pages/api/databaseTypeApi';
import FormField from "pages/components/FormField";
import DashboardLayout from "pages/components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "pages/components/Navbars/DashboardNavbar";
import DataTable from "pages/components/Tables/DataTable";
import DefaultCell from "pages/components/Tables/DefaultCell";
import { useEffect, useState } from "react";

function AssessmentQuery() {
    const [selectedDatabaseType, setSelectedDatabaseType] = useState(null);
    const [assessmentQuerys, setAssessmentQuerys] = useState([])
    const [databaseTypes, setDatabaseTypes] = useState([])

    const emptyAssessmentQuery = {
        id: null,
        name: "",
        description: "",
        queryString: 0,
        isMultipleDatabase: false,
        renderGenericView:false
    }
    const [assessmentQuery, setAssessmentQuery] = useState(emptyAssessmentQuery)

    const [openAssessmentQueryModal, setOpenAssessmentQueryModal] = useState(false);
    const [openAssessmentQueryModalAsDelete, setOpenAssessmentQueryModalAsDelete] = useState(false);


    const handleNewAssessmentQuery = () => {
        setAssessmentQuery(emptyAssessmentQuery);
        setOpenAssessmentQueryModal(true);
    }

    const handleUpdateAssessmentQueryGrid = async () => {
        await AssessmentQueriesForGrid((res) => setAssessmentQuerys(res.data.data));
    }
    const handleOpenEditAssessmentQuery = async (assessmentQueryRow) => {
        setAssessmentQuery(assessmentQueryRow);
        setSelectedDatabaseType(databaseTypes.filter(a => a?.id == assessmentQueryRow.databaseTypeId)[0])
        setOpenAssessmentQueryModal(true);
    }
    const handleSaveAssessmentQuery = async () => {

        const then = async () => {
            setAssessmentQuery(emptyAssessmentQuery);
            setOpenAssessmentQueryModal(false);
            await handleUpdateAssessmentQueryGrid();
        }

        if (assessmentQuery?.id) {
            await EditAssessmentQuery(assessmentQuery, then)
        }
        else {
            await AddAssessmentQuery(assessmentQuery, then)
        };

    }
    const handleOpenDeleteAssessmentQuery = async (assessmentQueryRow) => {
        setAssessmentQuery(assessmentQueryRow);
        setOpenAssessmentQueryModalAsDelete(true);
    }
    const handleDeleteAssessmentQuery = async () => {
        const then = async () => {
            await handleUpdateAssessmentQueryGrid(assessmentQuery?.id);
            setOpenAssessmentQueryModalAsDelete(false);
        }
        await DeleteAssessmentQuery(assessmentQuery, then);
    }

    const handleSaveN3b = async () => {
        const then = async (res) => {
            const blob = new Blob([res.data.data], { type: 'application/n3b' });
            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = 'AssessmentQueries.n3b';
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        };

        await AssessmentQueryFile(then);

    }

    const handleChangeDatabaseTypeSelected = async (event, selected) => {
        setSelectedDatabaseType(selected);
        // GetSystemInterfaces((res) => setSystemInterfaces(res.data.data));
        // AreasForGrid((res) => setAreas(res.data.data), selected?.id);
        // await handleUpdateInterfaceGrid(selected?.id);
    }

    useEffect(() => {
        handleUpdateAssessmentQueryGrid();
        DatabaseTypesForGrid((res) => setDatabaseTypes(res.data.data));
    }, []);

    return (<DashboardLayout>
        <DashboardNavbar />
        <MDBox my={3}>
            <MDBox >
                <Grid container spacing={1.5} >
                    <Grid item xs={12} sm={12}>
                        <Card >
                            <Grid item xs={12} sm={12}>
                                <MDBox p={2} >

                                    <MDTypography
                                        fontWeight="regular"
                                        color="text"
                                        textTransform="capitalize">Assessment Queries</MDTypography>
                                    <MDBox>
                                        <Grid p={2} container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewAssessmentQuery()} ><Icon fontSize="medium">add</Icon></IconButton>
                                            <IconButton variant="gradient" color="dark" onClick={() => handleSaveN3b()} ><Icon fontSize="medium">file_download</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [

                                                    { Header: "Ativa", accessor: "isActive", width: "5%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    { Header: "Nome", accessor: "name", width: "15%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Descrição", accessor: "description", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Multiplas Databases", accessor: "isMultipleDatabase", width: "15%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    {
                                                        Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditAssessmentQuery(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteAssessmentQuery(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: assessmentQuerys
                                            }} />
                                    </MDBox>

                                </MDBox>

                            </Grid>
                        </Card>
                    </Grid>

                </Grid>
            </MDBox>
        </MDBox>
        <Modal sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            width: '100%'
        }} open={openAssessmentQueryModal || openAssessmentQueryModalAsDelete}>
            <MDBox sx={{ width: '80%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openAssessmentQueryModalAsDelete ? "Remover " : assessmentQuery?.id ? "Editar " : "Nova "}
                            Query
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openAssessmentQueryModalAsDelete ? setOpenAssessmentQueryModalAsDelete(false) : setOpenAssessmentQueryModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>
                                <Grid item xs={12} sm={12}>
                                    <FormField type="text" label="Nome" value={assessmentQuery.name} id='name' onChange={(e) => setAssessmentQuery({ ...assessmentQuery, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={12} sm={12}>
                                    <Autocomplete
                                        value={selectedDatabaseType}
                                        options={databaseTypes}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeDatabaseTypeSelected}
                                        renderInput={(params) => <FormField {...params} variant="standard"  required label="Database" />}
                                    />
                                </Grid>

                                <Grid item xs={12} sm={12}>
                                    <FormField label="Descrição"
                                        multiline
                                        minRows={4}
                                        maxRows={6}
                                        value={assessmentQuery.description}
                                        fullWidth // Expande a largura para preencher o contêiner
                                        id='description' onChange={(e) => setAssessmentQuery({ ...assessmentQuery, [e.target?.id]: e.target.value })} required />
                                </Grid>
                                <Grid item xs={12} sm={12}>
                                    <AceEditor
                                        mode="sql"
                                        theme="sqlserver"
                                        value={assessmentQuery.queryString}
                                        fontSize={14}
                                        height="300px"
                                        width="100%"
                                        showPrintMargin={true}
                                        showGutter={true}
                                        highlightActiveLine={true}
                                        editorProps={{ $blockScrolling: true }}
                                        id='queryString'
                                        onChange={(e) => setAssessmentQuery({ ...assessmentQuery, queryString: e })}
                                    />

                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='isMultipleDatabase' checked={assessmentQuery.isMultipleDatabase} onChange={(e) => setAssessmentQuery({ ...assessmentQuery, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Multiplas databases?
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='isActive' checked={assessmentQuery.isActive} onChange={(e) => setAssessmentQuery({ ...assessmentQuery, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Ativa?
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='renderGenericView' checked={assessmentQuery.renderGenericView} onChange={(e) => setAssessmentQuery({ ...assessmentQuery, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Visualização genérica?
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                            </Grid>
                        </Grid>
                    </MDBox>




                    <MDBox p={2}>
                        <Grid container sx={{
                            display: "flex",
                            justifyContent: "end",
                            alignItems: "center",
                            width: '100%',
                            marginRight: '20px'

                        }} >
                            <MDBox mr={1}>
                                <MDButton variant="gradient" color="info" onClick={() => {
                                    setAssessmentQuery(emptyAssessmentQuery);
                                    setOpenAssessmentQueryModal(false);
                                    setOpenAssessmentQueryModalAsDelete(false);
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openAssessmentQueryModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteAssessmentQuery()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveAssessmentQuery()}>
                                        Salvar
                                    </MDButton>
                                </MDBox>
                            }
                        </Grid>
                    </MDBox>
                </Card>
            </MDBox>


        </Modal >
    </DashboardLayout >
    );
}
export default AssessmentQuery;