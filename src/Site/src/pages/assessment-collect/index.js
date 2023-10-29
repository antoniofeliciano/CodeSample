/* eslint-disable react/prop-types */
import { Card, Grid } from '@mui/material';
import { useEffect, useRef, useState } from "react";


import { Modal } from "@mui/material";
import Icon from "@mui/material/Icon";
import IconButton from '@mui/material/IconButton';
import MDBox from "components/MDBox";
import MDButton from "components/MDButton";
import MDDatePicker from "components/MDDatePicker";
import MDTypography from "components/MDTypography";
import { format } from 'date-fns';
import ptBR from 'date-fns/locale/pt-BR';
import { AddAssessmentCollect, AssessmentCollectsForGrid, DeleteAssessmentCollect, EditAssessmentCollect } from "pages/api/assessmentCollectApi";
import FormField from "pages/components/FormField";
import DashboardLayout from "pages/components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "pages/components/Navbars/DashboardNavbar";
import Spinner from 'pages/components/Spinner';
import DataTable from "pages/components/Tables/DataTable";
import DefaultCell from "pages/components/Tables/DefaultCell";

function AssessmentCollect() {
    const fileInputRef = useRef(null);
    const [isLoading, setIsLoading] = useState(false);
    const [assessmentCollects, setAssessmentCollects] = useState([])

    const emptyassessmentCollect = {
        id: null,
        name: "",
        clientName: "",
        details: "",
        collectDate: new Date(),
        collectResult: null,
        fileName: "",
        technicalResponsible:"",
    }
    const [assessmentCollect, setAssessmentCollect] = useState(emptyassessmentCollect)

    const [openassessmentCollectModal, setOpenassessmentCollectModal] = useState(false);
    const [openassessmentCollectModalAsDelete, setOpenassessmentCollectModalAsDelete] = useState(false);


    const handleNewassessmentCollect = () => {
        setAssessmentCollect(emptyassessmentCollect);
        setOpenassessmentCollectModal(true);
    }

    const handleUpdateassessmentCollectGrid = async () => {
        await AssessmentCollectsForGrid((res) => setAssessmentCollects(res.data.data));
    }
    const handleOpenEditAssessmentCollect = async (assessmentCollectRow) => {
        setAssessmentCollect(assessmentCollectRow);
        setOpenassessmentCollectModal(true);
    }
    const handleSaveassessmentCollect = async () => {

        setIsLoading(true);
        const then = async () => {
            setAssessmentCollect(emptyassessmentCollect);
            setOpenassessmentCollectModal(false);

            await handleUpdateassessmentCollectGrid();
        }

        if (assessmentCollect?.id) {
            await EditAssessmentCollect(assessmentCollect, then)
        }
        else {
            await AddAssessmentCollect(assessmentCollect, then)
        };
        setIsLoading(false);

    }
    const handleOpenDeleteAssessmentCollect = async (assessmentCollectRow) => {
        setAssessmentCollect(assessmentCollectRow);
        setOpenassessmentCollectModalAsDelete(true);
    }
    const handleDeleteAssessmentCollect = async () => {
        const then = async () => {
            await handleUpdateassessmentCollectGrid(assessmentCollect?.id);
            setOpenassessmentCollectModalAsDelete(false);
        }
        await DeleteAssessmentCollect(assessmentCollect, then);
    }

    const handleFileChange = (event) => {
        const file = event.target.files[0];
        const reader = new FileReader();

        reader.onload = (e) => {
            const content = e.target.result;
            setAssessmentCollect({ ...assessmentCollect, collectResult: content, fileName: file.name })
        };

        reader.readAsText(file);
    };


    const handleCloseModal = () => {
        setAssessmentCollect(emptyassessmentCollect);
        setOpenassessmentCollectModal(false);
        setOpenassessmentCollectModalAsDelete(false);
        setIsLoading(false);
    }

    useEffect(() => {
        handleUpdateassessmentCollectGrid();
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
                                        textTransform="capitalize">Assessment Colects</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewassessmentCollect()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [

                                                    { Header: "Nome", accessor: "name", width: "15%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Cliente", accessor: "clientName", width: "15%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Responsável", accessor: "technicalResponsible", width: "15%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Arquivo", accessor: "fileName", width: "10%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Data de coleta", accessor: "collectDate", width: "10%", Cell: ({ value }) => <DefaultCell value={format(new Date(value), "dd/MM/yyyy HH:mm", { locale: ptBR })} />, },

                                                    {
                                                        Header: "Açoes", useS: false, width: "10%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => window.open(`assessment-report/${row.original.id}`, "_blank")}> <Icon fontSize="medium">assessment</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditAssessmentCollect(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteAssessmentCollect(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: assessmentCollects
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
        }} open={openassessmentCollectModal || openassessmentCollectModalAsDelete}>
            <MDBox sx={{ width: '40%', borderRadius: '10px' }}>

                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openassessmentCollectModalAsDelete ? "Remover " : assessmentCollect?.id ? "Editar " : "Nova "}
                            Coleta
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => handleCloseModal()}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        {isLoading ? <Spinner /> :
                            <Grid p={2} item xs={12} lg={12}>

                                <Grid container spacing={1}>
                                    <Grid item xs={12} sm={6}>
                                        <FormField type="text" label="Nome" value={assessmentCollect.name} id='name' onChange={(e) => setAssessmentCollect({ ...assessmentCollect, [e.target?.id]: e.target.value })} required />
                                    </Grid>
                                    <Grid item xs={12} sm={6}>
                                        <FormField type="text" label="Cliente" value={assessmentCollect.clientName} id='clientName' onChange={(e) => setAssessmentCollect({ ...assessmentCollect, [e.target?.id]: e.target.value })} required />
                                    </Grid>

                                    <Grid item xs={12} sm={6}>
                                        <FormField type="text" label="Responsável" value={assessmentCollect.technicalResponsible} id='technicalResponsible' onChange={(e) => setAssessmentCollect({ ...assessmentCollect, [e.target?.id]: e.target.value })} required />
                                    </Grid>
                                    
                                    <Grid item xs={12} sm={6}>
                                        <MDDatePicker input={{ placeholder: "Select a date", label: "Data /Hora Coleta" }} value={new Date(assessmentCollect.collectDate)} onChange={(e) => setAssessmentCollect({ ...assessmentCollect, collectDate: e[0] })} />
                                    </Grid>

                                    <Grid item xs={12} sm={12}>
                                        <FormField label="Detalhes"
                                            multiline
                                            minRows={4}
                                            maxRows={6}
                                            value={assessmentCollect.details}
                                            fullWidth
                                            id='details' onChange={(e) => setAssessmentCollect({ ...assessmentCollect, [e.target?.id]: e.target.value })} />
                                    </Grid>

                                    <Grid item xs={9} sm={9}>
                                        <FormField type="text" value={assessmentCollect.fileName} id='selectedFile' required disabled />
                                    </Grid>
                                    <Grid item xs={3} sm={3}>
                                        <MDButton variant="gradient" size={"large"} color="secondary" fullWidth onClick={() => fileInputRef.current.click()}>
                                            selecionar
                                        </MDButton>
                                        <input type="file" accept=".n3b" ref={fileInputRef} style={{ display: "none" }} onChange={handleFileChange} />
                                    </Grid>
                                    
                                </Grid>
                            </Grid>

                        }
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
                                <MDButton variant="gradient" color="info" onClick={() => handleCloseModal()}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openassessmentCollectModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteAssessmentCollect()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton disabled={isLoading} variant="gradient" color="success" onClick={() => handleSaveassessmentCollect()}>
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
export default AssessmentCollect;