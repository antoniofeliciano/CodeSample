/* eslint-disable react/prop-types */
import { useState, useEffect } from "react";
import { Card, Grid } from '@mui/material';

import MDBox from "components/MDBox";
import MDButton from "components/MDButton";
import MDTypography from "components/MDTypography";
import Autocomplete from "@mui/material/Autocomplete";
import { Modal } from "@mui/material";

import MDInput from "components/MDInput";


import DashboardLayout from "pages/components/LayoutContainers/DashboardLayout";
import DashboardNavbar from "pages/components/Navbars/DashboardNavbar";

import MDDatePicker from "components/MDDatePicker";
import { v4 as uuidv4 } from 'uuid';
import IconButton from '@mui/material/IconButton';
import Icon from "@mui/material/Icon";
import FormField from "pages/components/FormField";
import Switch from "@mui/material/Switch";
import DataTable from "pages/components/Tables/DataTable";
import DefaultCell from "pages/components/Tables/DefaultCell";
import { format } from 'date-fns';
import ptBR from 'date-fns/locale/pt-BR';

import { TenantsForGrid } from "pages/api/tenantApi";
import { SystemApiKeysForGrid, AddSystemApiKey, EditSystemApiKey, DeleteSystemApiKey } from "pages/api/systemApiKeyApi";

function SystemApiKey() {
    const [tenants, setTenants] = useState([])
    const [selectedTenant, setSelectedTenant] = useState(null);
    const [systemApiKeys, setSystemApiKeys] = useState([])

    const emptySystemApiKey = {
        id: null,
        tenantId: "",
        apiKey: "",
        apiSecret: "",
        appName: "",
        expirationDate: new Date(),
        isActive: false,
        infiniteExpirationDate: false
    }
    const [systemApiKey, setSystemApiKey] = useState(emptySystemApiKey)

    const [openSystemApiKeyModal, setOpenSystemApiKeyModal] = useState(false);
    const [openSystemApiKeyModalAsDelete, setOpenSystemApiKeyModalAsDelete] = useState(false);

    const handleChangeTenantSelected = async (event, selected) => {
        if (selected) {
            setSelectedTenant(selected);
            await handleUpdateSystemApiKeyGrid(selected?.id);
        }
    }

    const handleNewSystemApiKey = () => {
        setSystemApiKey({ ...emptySystemApiKey, tenantId: selectedTenant?.id });
        setOpenSystemApiKeyModal(true);
    }

    const handleUpdateSystemApiKeyGrid = async (tenantId) => {
        await SystemApiKeysForGrid((res) => setSystemApiKeys(res.data.data), tenantId);
    }
    const handleOpenEditSystemApiKey = async (systemApiKeyRow) => {
        setSystemApiKey(systemApiKeyRow);
        setOpenSystemApiKeyModal(true);
    }
    const handleSaveSystemApiKey = async () => {

        const then = async () => {
            setSystemApiKey(emptySystemApiKey);
            setOpenSystemApiKeyModal(false);
            await handleUpdateSystemApiKeyGrid(systemApiKey.tenantId);
        }

        if (systemApiKey?.id) {
            await EditSystemApiKey(systemApiKey, then)
        }
        else {
            await AddSystemApiKey(systemApiKey, then)
        };

    }
    const handleOpenDeleteSystemApiKey = async (systemApiKeyRow) => {
        setSystemApiKey(systemApiKeyRow);
        setOpenSystemApiKeyModalAsDelete(true);
    }
    const handleDeleteSystemApiKey = async () => {
        const then = async () => {
            await handleUpdateSystemApiKeyGrid(systemApiKey?.id);
            setOpenSystemApiKeyModalAsDelete(false);
        }
        await DeleteSystemApiKey(systemApiKey, then);
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
                                        textTransform="capitalize">Api Keys</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewSystemApiKey()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [
                                                    { Header: "Aplicativo", accessor: "appName", width: "15%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Data de validade", accessor: "expirationDate", width: "10%", Cell: ({ value }) => <DefaultCell value={format(new Date(value), "dd/MM/yyyy HH:mm", { locale: ptBR })} />, },
                                                    { Header: "Ativo", accessor: "isActive", width: "5%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    { Header: "Nunca Expira", accessor: "infiniteExpirationDate", width: "5%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    {
                                                        Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditSystemApiKey(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteSystemApiKey(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: systemApiKeys
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
        }} open={openSystemApiKeyModal || openSystemApiKeyModalAsDelete}>
            <MDBox sx={{ width: '50%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openSystemApiKeyModalAsDelete ? "Remover " : systemApiKey?.id ? "Editar " : "Nova "}
                            Api Key
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openSystemApiKeyModalAsDelete ? setOpenSystemApiKeyModalAsDelete(false) : setOpenSystemApiKeyModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>
                                <Grid item xs={6} sm={6}>
                                    <FormField type="text" label="Aplicativo" value={systemApiKey.appName} id='appName' onChange={(e) => setSystemApiKey({ ...systemApiKey, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={6} sm={6}>
                                    <MDDatePicker disabled={systemApiKey.infiniteExpirationDate} input={{ placeholder: "Select a date", label: "Data /Hora Coleta" }} value={new Date(systemApiKey.expirationDate)}
                                        onChange={(e) => setSystemApiKey({ ...systemApiKey, expirationDate: e[0] })} />

                                </Grid>
                                <Grid item xs={8} sm={8}>
                                    <FormField type="text" disabled label="ApiKey" value={systemApiKey.apiKey} id='apiKey' onChange={(e) => setSystemApiKey({ ...systemApiKey, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={4} sm={4}>
                                    <MDButton variant="gradient" disabled={systemApiKey?.id} color="info" fullWidth onClick={() => {

                                        setSystemApiKey({ ...systemApiKey, apiKey: uuidv4().toUpperCase() });
                                    }}>
                                        Gerar Api Key
                                    </MDButton>
                                </Grid>
                                <Grid item xs={8} sm={8}>
                                    <FormField type="text" disabled label="ApiSecret" value={systemApiKey.apiSecret} id='apiSecret' onChange={(e) => setSystemApiKey({ ...systemApiKey, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={4} sm={4}>
                                    <MDButton variant="gradient"  color="error" fullWidth onClick={() => {

                                        setSystemApiKey({ ...systemApiKey, apiSecret: uuidv4().toUpperCase() });
                                    }}>
                                        Gerar Api Secret
                                    </MDButton>
                                </Grid>

                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='isActive' checked={systemApiKey.isActive} onChange={(e) => setSystemApiKey({ ...systemApiKey, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Ativo?
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={6} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='infiniteExpirationDate' checked={systemApiKey.infiniteExpirationDate} onChange={(e) => setSystemApiKey({ ...systemApiKey, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Nunca Expira?
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
                                    setSystemApiKey(emptySystemApiKey);
                                    setOpenSystemApiKeyModal(false)
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openSystemApiKeyModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteSystemApiKey()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveSystemApiKey()}>
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
export default SystemApiKey;