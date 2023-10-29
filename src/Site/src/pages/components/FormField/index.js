import React, { useState, useRef } from "react";
import PropTypes from "prop-types";
import MDInput from "components/MDInput";
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";

function FormField({ label, ...rest }) {
  const [requiredError, setRequiredError] = useState(false);

  // Cria uma referência para o input
  const inputRef = useRef();

  return (
    <MDBox mb={2}>
      <MDInput
        {...rest}
        inputRef={inputRef}
        error={rest.validation?.rule || requiredError}
        variant="outlined"
        label={label}
        fullWidth
        onBlur={() => {
          setRequiredError(
            rest.required && (
              ((rest?.InputProps?.className && rest.InputProps.className != 'MuiAutocomplete-inputRoot') && !rest.value || rest.value?.length === 0)
              ||
              ((rest?.InputProps?.className && rest.InputProps.className == 'MuiAutocomplete-inputRoot') && inputRef.current.value?.length === 0)
            )
          )
        }}
      />

      {rest.validation?.rule ? (
        <MDBox mt={0.75}>
          <MDTypography component="div" variant="caption" color="error" fontWeight="regular">
            {rest.validation.message}
          </MDTypography>
        </MDBox>
      ) : undefined}

      {requiredError ? (
        <MDBox mt={0.75}>
          <MDTypography component="div" variant="caption" color="error" fontWeight="regular">
            {`${label} é um campo obrigatório.`}
          </MDTypography>
        </MDBox>
      ) : undefined}
    </MDBox>
  );
}

FormField.propTypes = {
  label: PropTypes.string.isRequired,
};

export default FormField;
