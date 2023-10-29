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
import FormAutocomplete from "pages/components/FormAutocomplete";
import Switch from "@mui/material/Switch";
import DataTable from "pages/components/Tables/DataTable";
import DefaultCell from "pages/components/Tables/DefaultCell";
import Divider from '@mui/material/Divider';

import { TenantsForGrid } from "pages/api/tenantApi";
import { PermissionsForGrid, AddPermission, EditPermission, DeletePermission } from "pages/api/permissionApi";
import { RolesForGrid } from "pages/api/systemRoleApi";
import { InterfacesForGrid } from "pages/api/interfaceApi";

function Permission() {
    const [tenants, setTenants] = useState([])
    const [selectedTenant, setSelectedTenant] = useState(null);

    const [roles, setRoles] = useState([])
    const [selectedRole, setSelectedRole] = useState(null);
    const [interfaces, setInterfaces] = useState([])
    const [selectedInterface, setSelectedInterface] = useState(null);



    const [permissions, setPermissions] = useState([])

    const emptyPermission = {
        id: null,
        tenantId: "",
        interfaceId: "",
        roleId: "",
        name: "",
        canCreate: false,
        canRead: false,
        canUpdate: false,
        canDelete: false
    }
    const [permission, setPermission] = useState(emptyPermission)

    const [openPermissionModal, setOpenPermissionModal] = useState(false);
    const [openPermissionModalAsDelete, setOpenPermissionModalAsDelete] = useState(false);

    const handleChangeTenantSelected = async (event, selected) => {
        if (selected) {
            setSelectedTenant(selected);
            await handleUpdatePermissionGrid(selected?.id);
            await RolesForGrid((res) => setRoles(res.data.data), selected?.id);
            await InterfacesForGrid((res) => setInterfaces(res.data.data), selected?.id);
        }
    }
    const handleChangeRoleSelected = async (event, selected) => {
        if (selected) {
            setSelectedRole(selected);
            setPermission({...permission,roleId:selected.id})

        }
    }
    const handleChangeInterfaceSelected = async (event, selected) => {
        if (selected) {
            setSelectedInterface(selected);
            setPermission({...permission,interfaceId:selected.id})
        }
    }

    const handleNewPermission = () => {
        setPermission({ ...emptyPermission, tenantId: selectedTenant?.id });
        setOpenPermissionModal(true);
    }

    const handleUpdatePermissionGrid = async (tenantId) => {
        await PermissionsForGrid((res) => setPermissions(res.data.data), tenantId);
    }
    const handleOpenEditPermission = async (permissionRow) => {
        setPermission(permissionRow);
        setSelectedRole(roles.filter(a => a?.id == permissionRow.roleId)[0])
        setSelectedInterface(interfaces.filter(a => a?.id == permissionRow.interfaceId)[0])
        setOpenPermissionModal(true);
    }
    const handleSavePermission = async () => {

        const then = async () => {
            setPermission(emptyPermission);
            setOpenPermissionModal(false);
            await handleUpdatePermissionGrid(permission.tenantId);
        }

        if (permission?.id) {
            await EditPermission(permission, then)
        }
        else {
            await AddPermission(permission, then)
        };

    }
    const handleOpenDeletePermission = async (permissionRow) => {
        setPermission(permissionRow);
        setOpenPermissionModalAsDelete(true);
    }
    const handleDeletePermission = async () => {
        const then = async () => {
            await handleUpdatePermissionGrid(permission?.id);
            setOpenPermissionModalAsDelete(false);
        }
        await DeletePermission(permission, then);
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
                                        textTransform="capitalize">Permissions</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewPermission()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [
                                                    
                                                    { Header: "Grupo", accessor: "roleName", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Area", accessor: "areaName", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Interface", accessor: "interfaceName", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Nome", accessor: "name", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    
                                                    { Header: "Criar", accessor: "canCreate", width: "6%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },                                                    
                                                    { Header: "Ler", accessor: "canRead", width: "6%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    { Header: "Atualizar", accessor: "canUpdate", width: "6%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    { Header: "Deletar", accessor: "canDelete", width: "6%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    { Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditPermission(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeletePermission(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: permissions
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
        }} open={openPermissionModal || openPermissionModalAsDelete}>
            <MDBox sx={{ width: '50%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openPermissionModalAsDelete ? "Remover " : permission?.id ? "Editar " : "Nova "}
                            Permissão
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openPermissionModalAsDelete ? setOpenPermissionModalAsDelete(false) : setOpenPermissionModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        <Grid p={2} item xs={12} lg={12}>

                            <Grid container spacing={3}>

                                <Grid item xs={12} sm={6}>
                                    <FormAutocomplete disabled
                                        value={selectedTenant}
                                        options={tenants}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeTenantSelected}
                                        label={"Empresa"}
                                    />

                                </Grid>

                                <Grid item xs={12} sm={6}>
                                    <FormAutocomplete 
                                        value={selectedRole}
                                        options={roles}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeRoleSelected}
                                        label={"Grupos"}
                                    />

                                </Grid>
                                <Grid item xs={12} sm={6}>
                                    <FormAutocomplete 
                                        value={selectedInterface}
                                        options={interfaces}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeInterfaceSelected}
                                        label={"Interface"}
                                    />
                                </Grid>


                                <Grid item xs={12} sm={6}>
                                    <FormField type="text" label="Nome" value={permission.name} id='name' onChange={(e) => setPermission({ ...permission, [e.target?.id]: e.target.value })} required />
                                </Grid>


                                <Grid item xs={12} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='canCreate' checked={permission.canCreate} onChange={(e) => setPermission({ ...permission, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Criar
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={12} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='canRead' checked={permission.canRead} onChange={(e) => setPermission({ ...permission, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Ler
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={12} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='canUpdate' checked={permission.canUpdate} onChange={(e) => setPermission({ ...permission, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Atualizar
                                            </MDTypography>
                                        </MDBox>
                                    </MDBox>
                                </Grid>
                                <Grid item xs={12} sm={3}>
                                    <MDBox display="flex" alignItems="center" >
                                        <MDBox mt={0.5}>
                                            <Switch id='canDelete' checked={permission.canDelete} onChange={(e) => setPermission({ ...permission, [e.target?.id]: e.target.checked })} />
                                        </MDBox>
                                        <MDBox>
                                            <MDTypography variant="button" fontWeight="regular" color="text">
                                                Deletar
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
                                    setPermission(emptyPermission);
                                    setSelectedRole(null);
                                    setSelectedInterface(null);
                                    setOpenPermissionModal(false);
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openPermissionModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeletePermission()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSavePermission()}>
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
export default Permission;