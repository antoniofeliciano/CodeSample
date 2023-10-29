/* eslint-disable react/prop-types */
import { useState, useEffect, useRef } from "react";
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
import CustomerCell from "pages/components/Tables/CustomerCell"
import Divider from '@mui/material/Divider';
import MDAvatar from "components/MDAvatar";

import { TenantsForGrid } from "pages/api/tenantApi";
import { UsersForGrid, AddUser, EditUser, DeleteUser } from "pages/api/userApi";
import { RolesForGrid } from "pages/api/systemRoleApi";

function User() {
    const fileInputRef = useRef(null);
    const [tenants, setTenants] = useState([])
    const [selectedTenant, setSelectedTenant] = useState(null);
    const [roles, setRoles] = useState([])
    const [selectedRole, setSelectedRole] = useState(null);
    const [users, setUsers] = useState([])

    const emptyUser = {
        id: null,
        tenantId: "",
        icon: "",
        name: "",
        position: 0,
        visible: false
    }

    const [user, setUser] = useState(emptyUser)

    const [openUserModal, setOpenUserModal] = useState(false);
    const [openUserModalAsDelete, setOpenUserModalAsDelete] = useState(false);

    const handleChangeTenantSelected = async (event, selected) => {
        if (selected) {
            setSelectedTenant(selected);
            await handleUpdateUserGrid(selected?.id);
            await RolesForGrid((res) => setRoles(res.data.data), selected?.id);
        }
    }
    const handleChangeRoleSelected = async (event, selected) => {
        if (selected) {
            setSelectedRole(selected);
        }
    }

    const handleNewUser = () => {
        setUser({ ...emptyUser, tenantId: selectedTenant?.id });
        setOpenUserModal(true);
    }

    const handleUpdateUserGrid = async (tenantId) => {
        await UsersForGrid((res) => setUsers(res.data.data), tenantId);
    }
    const handleOpenEditUser = async (userRow) => {
        setSelectedRole(roles.filter(r=>r.id == userRow.roleId)[0])
        setUser(userRow);
        setOpenUserModal(true);
    }
    const handleSaveUser = async () => {

        const then = async () => {
            setUser(emptyUser);
            setOpenUserModal(false);
            await handleUpdateUserGrid(user.tenantId);
        }
    var currentUser = {
        id:user?.id,
        email:user.email,
        isActive:user.isActive,
        profilePicture:user.profilePicture,
        username:user.username,
        roleId: selectedRole.id,
        tenantId:selectedTenant.id,
        password:user.password
    }


        if (user?.id) {
            await EditUser(currentUser, then)
        }
        else {
            await AddUser(currentUser, then)
        };

    }
    const handleOpenDeleteUser = async (userRow) => {
        setUser(userRow);
        setOpenUserModalAsDelete(true);
    }
    const handleDeleteUser = async () => {
        const then = async () => {
            await handleUpdateUserGrid(user?.id);
            setOpenUserModalAsDelete(false);
        }
        await DeleteUser(user, then);
    }
    const handleFileChange = (event) => {
        const file = event.target.files[0];
        const reader = new FileReader();

        reader.onload = (e) => {
            const content = reader.result;
            setUser({ ...user, profilePicture: content })
        };
        reader.readAsDataURL(file);
    };

    function generatePassword(size) {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
        var password = "";
        for (var i = 0; i < size; i++) {
            var ix = Math.floor(Math.random() * chars.length);
            password += chars.charAt(ix);
        }
        return password;
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
                                        textTransform="capitalize">Users</MDTypography>
                                    <MDBox>
                                        <Grid container sx={{
                                            display: "flex",
                                            justifyContent: "space-between",
                                            alignItems: "center",
                                            width: '100%'
                                        }} >
                                            <IconButton variant="gradient" color="dark" onClick={() => handleNewUser()} ><Icon fontSize="medium">add</Icon></IconButton>
                                        </Grid>


                                        <DataTable
                                            canSearch

                                            table={{
                                                columns: [
                                                    { Header: "Foto", width: "1%", accessor: "profilePicture", Cell: ({ value }) => <MDAvatar src={value} alt={user?.username} size="sm" />, },
                                                    { Header: "Username", accessor: "username", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "E-mail", accessor: "email", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    { Header: "Ativo", accessor: "isActive", width: "5%", align: 'center', Cell: ({ value }) => <Icon fontSize="medium">{value ? "check" : "close"}</Icon>, },
                                                    { Header: "Grupo", accessor: "role.name", width: "5%", Cell: ({ value }) => <DefaultCell value={value} />, },
                                                    {
                                                        Header: "Açoes", useS: false, width: "5%", canSort: true, align: 'center',
                                                        Cell: ({ row }) => (<>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenEditUser(row.original)}> <Icon fontSize="medium">edit</Icon></IconButton>
                                                            <IconButton variant="gradient" color="dark" onClick={() => handleOpenDeleteUser(row.original)}> <Icon fontSize="medium">delete</Icon></IconButton>
                                                        </>
                                                        ),
                                                    },
                                                ],
                                                rows: users
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
        }} open={openUserModal || openUserModalAsDelete}>
            <MDBox sx={{ width: '50%', borderRadius: '10px' }}>
                <Card>
                    <Grid container sx={{
                        display: "flex",
                        justifyContent: "space-between",
                        alignItems: "center",
                        width: '100%'
                    }} >
                        <MDTypography p={2} variant="h4" fontWeight="bold">
                            {openUserModalAsDelete ? "Remover " : user?.id ? "Editar " : "Novo "}
                            User
                        </MDTypography>
                        <IconButton variant="gradient" color="dark" onClick={() => openUserModalAsDelete ? setOpenUserModalAsDelete(false) : setOpenUserModal(false)}><Icon fontSize="medium">close</Icon></IconButton>
                    </Grid>
                    <MDBox sx={{ maxHeight: '800px', overflowY: 'auto' }}>
                        
                        <Grid p={2} item xs={12} lg={12}>
                            <Grid container spacing={3}>
                                <Grid item xs={12} sm={6}>
                                    <Autocomplete
                                        value={selectedTenant}
                                        options={tenants}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeTenantSelected}
                                        disabled
                                        renderInput={(params) => <FormField {...params} variant="standard" />}
                                    />
                                </Grid>

                                <Grid item xs={12} sm={6}>
                                    <Autocomplete
                                        value={selectedRole}
                                        options={roles}
                                        getOptionLabel={(o) => o.name}
                                        onChange={handleChangeRoleSelected}
                                        renderInput={(params) => <FormField {...params} variant="standard" />}
                                    />
                                </Grid>
                            </Grid>

                            <Grid  container spacing={3} alignItems="center">

                                <Grid item xs={12} sm={3}>
                                    <Grid container direction="column" alignItems="center">
                                        <Grid item xs={12} sm={6} style={{ border: '1px' }}>
                                            <MDAvatar src={user.profilePicture ?? selectedTenant} alt="profile picture" size="xxl" variant="rounded" />
                                        </Grid>
                                        <Grid p={2} item xs={12} sm={6}>
                                            <MDButton variant="gradient" size={"large"} color="secondary" fullWidth onClick={() => fileInputRef.current.click()}>
                                                editar
                                            </MDButton>
                                            <input type="file" accept="image/*" ref={fileInputRef} style={{ display: "none" }} onChange={handleFileChange} />
                                        </Grid>
                                    </Grid>
                                </Grid>



                                <Grid item xs={12} sm={9}>
                                    <Grid container spacing={3}>
                                        <Grid item xs={12} sm={6}>
                                            <FormField type="text" label="Nome" value={user.username} id='username' onChange={(e) => setUser({ ...user, [e.target?.id]: e.target.value })} required />
                                        </Grid>
                                        <Grid item xs={12} sm={6}>
                                            <FormField type="text" label="Email" value={user.email} id='email' onChange={(e) => setUser({ ...user, [e.target?.id]: e.target.value })} required />
                                        </Grid>
                                        <Grid item xs={12} sm={4}>
                                            <FormField type="text" label="Senha" value={user.password} id='password' onChange={(e) => setUser({ ...user, [e.target?.id]: e.target.value })} required />
                                        </Grid>
                                        <Grid item xs={12} sm={4}>
                                            <MDButton variant="gradient" size={"medium"} color="secondary" fullWidth onClick={(e) => setUser({ ...user, password: generatePassword(8) })}>
                                                Gerar senha
                                            </MDButton>
                                        </Grid>
                                        <Grid item xs={12} sm={4}>
                                            <MDBox display="flex" alignItems="center">
                                                <MDBox mt={0.5}>
                                                    <Switch id='isActive' checked={user.isActive} onChange={(e) => setUser({ ...user, [e.target?.id]: e.target.checked })} />
                                                </MDBox>
                                                <MDBox>
                                                    <MDTypography variant="button" fontWeight="regular" color="text">
                                                        Ativo?
                                                    </MDTypography>
                                                </MDBox>
                                            </MDBox>
                                        </Grid>
                                    </Grid>
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
                                    setUser(emptyUser);
                                    setOpenUserModal(false);
                                    setOpenUserModalAsDelete(false);
                                }}>
                                    Cancelar
                                </MDButton>
                            </MDBox>
                            {openUserModalAsDelete ?

                                <MDBox >
                                    <MDButton variant="gradient" color="error" onClick={() => handleDeleteUser()}>
                                        Remover
                                    </MDButton>
                                </MDBox>
                                :
                                <MDBox >
                                    <MDButton variant="gradient" color="success" onClick={() => handleSaveUser()}>
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
export default User;