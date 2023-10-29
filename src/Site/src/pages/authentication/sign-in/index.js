

import { useState, useContext } from "react";
import { AuthContext } from "context/auth";

// react-router-dom components
import { Link } from "react-router-dom";

import Switch from "@mui/material/Switch";

import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import MDInput from "components/MDInput";
import MDButton from "components/MDButton";

import FormField from "pages/components/FormField";

// Authentication layout components
import IllustrationLayout from "../components/IllustrationLayout";



function SignInPage() {
  const { signIn } = useContext(AuthContext);
  const emptyLogin = {
    email: '',
    password: '',
    rememberMe: false
  }

  const [login, setLogin] = useState(emptyLogin)

  return (
    <IllustrationLayout
      title="Entrar"
      description="Informe email e senha para entrar"
    >
      <MDBox component="form" role="form">

        <MDBox mb={2}>
          <FormField type="email" value={login.email} label="Email" id='email' onChange={(e) => setLogin({ ...login, [e.target?.id]: e.target.value })} required />
        </MDBox>
        <MDBox mb={2}>
          <FormField type="password" value={login.password} label="Senha" id='password' onChange={(e) => setLogin({ ...login, [e.target?.id]: e.target.value })} required />
        </MDBox>
        {/* <MDBox display="flex" alignItems="center" ml={-1}>

          <Switch id='rememberMe' checked={login.rememberMe} onChange={(e) => setLogin({ ...login, [e.target?.id]: e.target.checked })} />
          <MDTypography
            variant="button"
            fontWeight="regular"
            color="text"
            // onClick={handleSetRememberMe}
            sx={{ cursor: "pointer", userSelect: "none", ml: -1 }}
          >
            &nbsp;&nbsp;Lembre-me
          </MDTypography>
        </MDBox> */}
        <MDBox mt={4} mb={1}>
          <MDButton variant="gradient" color="secondary" size="large" fullWidth onClick={() => signIn(login?.email, login?.password)}>
            Entrar
          </MDButton>
        </MDBox>
        <MDBox mt={3} textAlign="center">
          <MDTypography variant="button" color="text">
            Ainda não tem um usuário?{" "}
            <MDTypography
              component={Link}
              to="/authentication/sign-up/cover"
              variant="button"
              color="info"
              fontWeight="medium"
              textGradient
            >
              Entre em contato!
            </MDTypography>
          </MDTypography>
        </MDBox>
      </MDBox>
    </IllustrationLayout>
  );
}

export default SignInPage;
