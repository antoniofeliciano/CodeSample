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
import { RolesForGrid, AddRole, EditRole, DeleteRole } from "pages/api/systemRoleApi";

function Role() {
    const [tenants, setTenants] = useState([])
    const [selectedTenant, setSelectedTenant] = useState(null);
    const [roles, setRoles] = useState([])

    const emptyRole = {
        id: null,
        tenantId: "",
        name: "",
        description: "",

    }
    const [role, setRole] = useState(emptyRole)

    const [openRoleModal, setOpenRoleModal] = useState(false);
    const [openRoleModalAsDelete, setOpenRoleModalAsDelete] = useState(false);

    const handleChangeTenantSelected = async (event, selected) => {
        if (selected) {
            setSelectedTenant(selected);
            await handleUpdateRoleGrid(selected?.id);
        }
    }

    const handleNewRole = () => {
        setRole({ ...emptyRole, tenantId: selectedTenant?.id });
        setOpenRoleModal(true);
    }

    const handleUpdateRoleGrid = async (tenantId) => {
        await RolesForGrid((res) => setRoles(res.data.data), tenantId);
    }
    const handleOpenEditRole = async (roleRow) => {
        setRole(roleRow);
        setOpenRoleModal(true);
    }
    const handleSaveRole = async () => {

        const then = async () => {
            setRole(emptyRole);
            setOpenRoleModal(false);
            await handleUpdateRoleGrid(role.tenantId);
        }

        if (role?.id) {
            await EditRole(role, then)
        }
        else {
            await AddRole(role, then)
        };

    }
    const handleOpenDeleteRole = async (roleRow) => {
        setRole(roleRow);
        setOpenRoleModalAsDelete(true);
    }
    const handleDeleteRole = async () => {
        const then = async () => {
            await handleUpdateRoleGrid(role?.id);
            setOpenRoleModalAsDelete(false);
        }
        await DeleteRole(role, then);
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
                                        textTransform="capitalize">Grupos</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewRole()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [
                                                    { Header: "Nome", accessor: "name", width: "40%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Descrição", accessor: "description", width: "40%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    {
                                                        Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditRole(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteRole(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: roles
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
        }} open={openRoleModal || openRoleModalAsDelete}>
            <MDBox sx={{ width: '85%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openRoleModalAsDelete ? "Remover " : role?.id ? "Editar " : "Novo "}
                            Role
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openRoleModalAsDelete ? setOpenRoleModalAsDelete(false) : setOpenRoleModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>
                                <Grid item xs={6} sm={6}>
                                    <FormField type="text" label="Nome" value={role.name} id='name' onChange={(e) => setRole({ ...role, [e.target?.id]: e.target.value })} required />
                                </Grid>

                                <Grid item xs={6} sm={6}>
                                    <FormField type="text" label="Descrição" value={role.description} id='description' onChange={(e) => setRole({ ...role, [e.target?.id]: e.target.value })} required />
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
                                    setRole(emptyRole);
                                    setOpenRoleModal(false)
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openRoleModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteRole()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveRole()}>
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
export default Role;