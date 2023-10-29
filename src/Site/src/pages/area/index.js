/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import { Card, Grid } from '@mui/material';
import { Accordion, AccordionSummary, Typography, AccordionDetails } from '@mui/material';

import ExpandMoreIcon from '@mui/icons-material/ExpandMore';

import MDBox from "components/MDBox";
import MDButton from "components/MDButton";
import MDTypography from "components/MDTypography";
import Autocomplete from "@mui/material/Autocomplete";
import { Modal } from "@mui/material";

import MDInput from "components/MDInput";


import DashboardLayout from "pages/components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "pages/components/Navbars/DashboardNavbar";


import IconButton from '@mui/material/IconButton';
import Icon from "@mui/material/Icon";
import FormField from "pages/components/FormField";
import Switch from "@mui/material/Switch";
import DataTable from "pages/components/Tables/DataTable";
import DefaultCell from "pages/components/Tables/DefaultCell";
import Divider from '@mui/material/Divider';

import { TenantsForGrid } from "pages/api/tenantApi";
import { AreasForGrid, AddArea, EditArea, DeleteArea } from "pages/api/areaApi";

function Area() {
    const [tenants, setTenants] = useState([])
    const [selectedTenant, setSelectedTenant] = useState(null);
    const [areas, setAreas] = useState([])

    const emptyArea = {
        id: null,
        tenantId: "",
        icon: "",
        name: "",
        position: 0,
        visible: false
    }
    const [area, setArea] = useState(emptyArea)

    const [openAreaModal, setOpenAreaModal] = useState(false);
    const [openAreaModalAsDelete, setOpenAreaModalAsDelete] = useState(false);

    const handleChangeTenantSelected = async (event, selected) => {
        if (selected) {
            setSelectedTenant(selected);
            await handleUpdateAreaGrid(selected?.id);
        }
    }

    const handleNewArea = () => {
        setArea({ ...emptyArea, tenantId: selectedTenant?.id });
        setOpenAreaModal(true);
    }

    const handleUpdateAreaGrid = async (tenantId) => {
        await AreasForGrid((res) => setAreas(res.data.data), tenantId);
    }
    const handleOpenEditArea = async (areaRow) => {
        setArea(areaRow);
        setOpenAreaModal(true);
    }
    const handleSaveArea = async () => {

        const then = async () => {
            setArea(emptyArea);
            setOpenAreaModal(false);
            await handleUpdateAreaGrid(selectedTenant.id);
        }

        if (area?.id) {
            await EditArea(area, then)
        }
        else {
            await AddArea(area, then)
        };

    }
    const handleOpenDeleteArea = async (areaRow) => {
        setArea(areaRow);
        setOpenAreaModalAsDelete(true);
    }
    const handleDeleteArea = async () => {
        const then = async () => {
            await handleUpdateAreaGrid(area?.id);
            setOpenAreaModalAsDelete(false);
        }
        await DeleteArea(area, then);
    }




    useEffect(() => {
        TenantsForGrid((res) => setTenants(res.data.data));
    }, []);

    return (<DashboardLayout>
        <DashboardNavbar />
        <MDBox my={3}>
            <MDBox >
                <Grid container spacing={1.5} >
                    <Grid item xs={12} sm={12}>
                        <Card>
                            <MDBox p={2}>
                                <MDBox mb={1.625} display="inline-block">
                                    <MDTypography
                                        fontWeight="regular"
                                        color="text"
                                        textTransform="capitalize"
                                    >
                                        Tenant
                                    </MDTypography>
                                </MDBox>
                                <Autocomplete
                                    value={selectedTenant}
                                    options={tenants}
                                    getOptionLabel={(o) => o.name}
                                    onChange={handleChangeTenantSelected}
                                    renderInput={(params) => <MDInput {...params} variant="standard" />}
                                />
                            </MDBox>
                        </Card>
                    </Grid>


                    <Grid item xs={12} sm={12}>
                        <Card >
                            <Grid item xs={12} sm={12}>
                                <MDBox p={2} >

                                    <MDTypography
                                        fontWeight="regular"
                                        color="text"
                                        textTransform="capitalize">Areas</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewArea()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [
                                                    { Header: "Posição", accessor: "position", width: "3%", align: 'center', Cell: ({ value }) => <DefaultCell value={value.toString()} />, },
                                                    { Header: "Ícone", width: "5%", align: 'center', Cell: ({ row }) => <Icon fontSize="medium">{row.original.icon}</Icon> },
                                                    { Header: "Nome", accessor: "name", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    
                                                    { Header: "Visível", accessor: "visible", width: "3%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "visibility" : "visibility_off"}</Icon>, },
                                                    {
                                                        Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditArea(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteArea(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: areas
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
        }} open={openAreaModal || openAreaModalAsDelete}>
            <MDBox sx={{ width: '85%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openAreaModalAsDelete ? "Remover " : area?.id ? "Editar " : "Novo "}
                            Area
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openAreaModalAsDelete ? setOpenAreaModalAsDelete(false) : setOpenAreaModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>
                                <Grid item xs={6} sm={3}>
                                    <FormField type="text" label="Nome" value={area.name} id='name' onChange={(e) => setArea({ ...area, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={6} sm={3}>
                                    <FormField type="text" label="Ícone" value={area.icon} id='icon' onChange={(e) => setArea({ ...area, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={6} sm={3}>
                                    <FormField type="text" label="Posição" value={area.position} id='position' onChange={(e) => setArea({ ...area, [e.target?.id]: e.target.value })} required />
                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='visible' checked={area.visible} onChange={(e) => setArea({ ...area, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Visível?
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
                                    setArea(emptyArea);
                                    setOpenAreaModal(false)
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openAreaModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteArea()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveArea()}>
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
export default Area;