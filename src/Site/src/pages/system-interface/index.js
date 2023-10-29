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
import { InterfacesForGrid, GetSystemInterfaces, AddInterface, EditInterface, DeleteInterface } from "pages/api/interfaceApi";
import { AreasForGrid } from "pages/api/areaApi";

function SystemInterface() {
    const [tenants, setTenants] = useState([])
    const [selectedTenant, setSelectedTenant] = useState(null);

    const [interfaces, setInterfaces] = useState([])
    const [systemInterfaces, setSystemInterfaces] = useState([])

    const emptyInterface = {
        id: null,
        tenantId: '',
        areaId: '',
        icon: '',
        name: '',
        systemName: '',
        route: '',
        visible:false,
        renderable:false
    }
    const [interface_, setInterface] = useState(emptyInterface)

    const [openInterfaceModal, setOpenInterfaceModal] = useState(false);
    const [openInterfaceModalAsDelete, setOpenInterfaceModalAsDelete] = useState(false);

    const [areas, setAreas] = useState([])
    const [selectedArea, setSelectedArea] = useState(null);

    const handleChangeAreaSelected = (event, selected) => {
        if (selected) {
            setSelectedArea(selected);
            setInterface({ ...interface_, areaId: selected?.id });
        }
    }

    const handleChangeTenantSelected = async (event, selected) => {
        setSelectedTenant(selected);
        GetSystemInterfaces((res) => setSystemInterfaces(res.data.data));
        AreasForGrid((res) => setAreas(res.data.data), selected?.id);
        await handleUpdateInterfaceGrid(selected?.id);
    }

    const handleNewInterface = (interfacerow) => {
        setInterface({
            ...emptyInterface,
            tenantId: selectedTenant?.id,
            name: interfacerow.name,
            route: interfacerow.route,
            systemName:interfacerow.systemName
        });
        setOpenInterfaceModal(true);
    }

    const handleUpdateInterfaceGrid = async (tenantId) => {
        await InterfacesForGrid((res) => setInterfaces(res.data.data), tenantId);
    }
    const handleOpenEditInterface = async (interfaceRow) => {
        setInterface(interfaceRow);
        setSelectedArea(areas.filter(a => a?.id == interfaceRow.areaId)[0])
        setOpenInterfaceModal(true);
    }
    const handleSaveInterface = async () => {
        const then = async () => {
            setInterface(emptyInterface);
            setSelectedArea(null)
            setOpenInterfaceModal(false);
            await handleUpdateInterfaceGrid(selectedTenant.id);
        }

        if (interface_?.id) {
            await EditInterface(interface_, then)
        }
        else {
            await AddInterface(interface_, then)
        };

    }
    const handleOpenDeleteInterface = async (interfaceRow) => {
        setInterface(interfaceRow);
        setOpenInterfaceModalAsDelete(true);
    }
    
    const handleDeleteInterface = async () => {
        const then = async () => {
            await handleUpdateInterfaceGrid(interface_.tenantId);
            setOpenInterfaceModalAsDelete(false);
        }
        await DeleteInterface(interface_, then);
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
                            <Grid container xs={12} sm={12} spacing={1.5}>

                                <Grid item xs={12} sm={5.9}>

                                    <MDTypography p={2}
                                        fontWeight="regular"
                                        color="text"
                                        textTransform="capitalize">Interfaces do sistema
                                    </MDTypography>

                                    <DataTable
                                        canSearch
                                        table={{
                                            columns: [
                                                { Header: "Rota", accessor: "route", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                { Header: "Nome", accessor: "name", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                { Header: "Nome Sistema", accessor: "systemName", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                {
                                                    Header: "Açoes", useS: false, width: "10%", canSort: true, align: 'center',
                                                    Cell: ({ row }) => (<IconButton variant="gradient" color="dark" onClick={() => interfaces.some(i => i.systemName == row.original.systemName) ? undefined : handleNewInterface(row.original)} > <Icon fontSize="medium">{interfaces.some(i => i.systemName == row.original.systemName) ? "check" : "arrow_forward_icon"}</Icon></IconButton>),

                                                },
                                            ],
                                            rows: systemInterfaces
                                        }} />

                                </Grid>

                                <Grid item xs={0} sm={0.2}>
                                    <Divider orientation="vertical" sx={{ ml: 0, mr: 0 }} />
                                </Grid>

                                <Grid item xs={12} sm={5.9}>

                                    <MDTypography p={2}
                                        fontWeight="regular"
                                        color="text"
                                        textTransform="capitalize">Interfaces de {selectedTenant?.name}
                                    </MDTypography>
                                    <DataTable
                                        canSearch
                                        table={{
                                            columns: [
                                                { Header: "Rota", accessor: "route", width: "10%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                { Header: "Área", accessor: "area", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                { Header: "Nome", accessor: "name", width: "10%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                { Header: "Nome Sistema", accessor: "systemName", width: "30%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                { Header: "Ícone", width: "5%", align: 'center', Cell: ({ row }) => <DefaultCell value={<><Icon fontSize="medium">{row.original.icon}</Icon></>} /> },

                                                {
                                                    Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                    Cell: ({ row }) => (
                                                        <>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditInterface(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteInterface(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                    ),
                                                },
                                            ],
                                            rows: interfaces
                                        }} />

                                </Grid>

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
        }} open={openInterfaceModal || openInterfaceModalAsDelete}>
            <MDBox sx={{ width: '85%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openInterfaceModalAsDelete ? "Remover " : interface_?.id ? "Editar " : "Nova "}
                            Interface
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openInterfaceModalAsDelete ? setOpenInterfaceModalAsDelete(false) : setOpenInterfaceModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>
                                <Grid item xs={12} sm={3}>
                                    <FormField type="text" label="Nome" value={interface_.name} id='name' onChange={(e) => setInterface({ ...interface_, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={12} sm={3}>
                                    <FormField type="text" label="Rota" value={interface_.route} id='route' onChange={(e) => setInterface({ ...interface_, [e.target?.id]: e.target.value })} required />
                                </Grid>
                                <Grid item xs={12} sm={3}>

                                    <Autocomplete
                                        value={selectedArea}
                                        options={areas}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeAreaSelected}
                                        renderInput={(params) => <FormField {...params} required label="Area" />}
                                    />

                                </Grid>
                                <Grid item xs={12} sm={3}>
                                    <FormField type="text" label="Ícone (MUI-Icons (snake_case))"
                                        value={interface_.icon} id='icon'
                                        onChange={(e) => setInterface({ ...interface_, [e.target?.id]: e.target.value })} />
                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='visible' checked={interface_.visible} onChange={(e) => setInterface({ ...interface_, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Visível?
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='renderable' checked={interface_.renderable} onChange={(e) => setInterface({ ...interface_, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Renderizável?
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
                                    setInterface(emptyInterface);
                                    setSelectedArea(null);
                                    setOpenInterfaceModal(false)
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openInterfaceModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteInterface()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveInterface()}>
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
export default SystemInterface;