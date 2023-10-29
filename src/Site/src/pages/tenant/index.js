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


import { TenantsForGrid, AddTenant, EditTenant, DeleteTenant } from "pages/api/tenantApi";

function Tenant() {
    const [tenants, setTenants] = useState([])

    const emptyTenant = {
        id: null,
        tenantId: "",
        icon: "",
        name: "",
        position: 0,
        visible: false
    }
    const [tenant, setTenant] = useState(emptyTenant)

    const [openTenantModal, setOpenTenantModal] = useState(false);
    const [openTenantModalAsDelete, setOpenTenantModalAsDelete] = useState(false);


    const handleNewTenant = () => {
        setTenant(emptyTenant);
        setOpenTenantModal(true);
    }
    
    const handleUpdateTenantGrid = async (tenantId) => {
        await TenantsForGrid((res) => setTenants(res.data.data),tenantId);
    }
    const handleOpenEditTenant = async (tenantRow) => {
        setTenant(tenantRow);
        setOpenTenantModal(true);
    }
    const handleSaveTenant = async () => {

        const then = async () => {
            setTenant(emptyTenant);
            setOpenTenantModal(false);
            await handleUpdateTenantGrid(tenant.tenantId);
        }

        if (tenant?.id) {
            await EditTenant(tenant, then)
        }
        else {
            await AddTenant(tenant, then)
        };

    }
    const handleOpenDeleteTenant = async (tenantRow) => {
        setTenant(tenantRow);
        setOpenTenantModalAsDelete(true);
    }
    const handleDeleteTenant = async () => {
        const then = async () => {
            await handleUpdateTenantGrid(tenant?.id);
            setOpenTenantModalAsDelete(false);
        }
        await DeleteTenant(tenant, then);
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
                        <Card >
                            <Grid item xs={12} sm={12}>
                                <MDBox p={2} >

                                    <MDTypography
                                        fontWeight="regular"
                                        color="text"
                                        textTransform="capitalize">Tenants</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={()=> handleNewTenant()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [
                                                    
                                                    { Header: "Nome", accessor: "name", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    {
                                                        Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditTenant(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteTenant(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: tenants
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
        }} open={openTenantModal || openTenantModalAsDelete}>
            <MDBox sx={{ width: '50%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openTenantModalAsDelete ? "Remover " : tenant?.id ? "Editar " : "Novo "}
                            Tenant
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openTenantModalAsDelete ? setOpenTenantModalAsDelete(false) : setOpenTenantModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>
                                <Grid item xs={12} sm={12}>
                                    <FormField type="text" label="Nome" value={tenant.name} id='name' onChange={(e) => setTenant({ ...tenant, [e.target?.id]: e.target.value })} required />
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
                                    setTenant(emptyTenant);
                                    setOpenTenantModal(false)
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openTenantModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteTenant()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveTenant()}>
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
export default Tenant;