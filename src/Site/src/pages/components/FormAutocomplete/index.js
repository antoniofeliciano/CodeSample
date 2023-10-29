// prop-type is a library for typechecking of props
import PropTypes from "prop-types";
import { useState } from "react";


import MDInput from "components/MDInput";
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import Autocomplete from "@mui/material/Autocomplete";

function FormAutocomplete({ label, ...rest }) {

  const [requiredError, setRequiredError] = useState(false);

  return (
    <MDBox mb={2}>
      <Autocomplete {...rest}
        error={rest.validation?.rule || requiredError}
        variant="standard" label={label}
        renderInput={(params) => <MDInput {...params} label={label} variant="standard" />}

        fullWidth onBlur={() => setRequiredError(rest.required && (rest.value?.length == 0) || !rest.value)} />
      {rest.validation?.rule ?
        <MDBox mt={0.75}>
          <MDTypography component="div" variant="caption" color="error" fontWeight="regular">
            {rest.validation.message}
          </MDTypography>
        </MDBox> : undefined
      }

      {requiredError ?
        <MDBox mt={0.75}>
          <MDTypography component="div" variant="caption" color="error" fontWeight="regular">
            {`'${label}' é um campo obrigatório.`}
          </MDTypography>
        </MDBox> : undefined
      }
    </MDBox>
  );
}

// typechecking props for FormAutocomplete
FormAutocomplete.propTypes = {
  label: PropTypes.string.isRequired,
};

export default FormAutocomplete;
