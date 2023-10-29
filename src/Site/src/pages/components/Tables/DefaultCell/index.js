

// prop-types is a library for typechecking of props
import PropTypes from "prop-types";


import MDTypography from "components/MDTypography";

function DefaultCell({ value, suffix,...rest }) {
  return (
    <MDTypography {...rest} variant="caption" fontWeight="medium" color="text">
      {value}
      {suffix && (
        <MDTypography variant="caption" fontWeight="medium" color="secondary">
          &nbsp;&nbsp;{suffix}
        </MDTypography>
      )}
    </MDTypography>
  );
}

// Setting default values for the props of DefaultCell
DefaultCell.defaultProps = {
  suffix: "",
};

// Typechecking props for the DefaultCell
DefaultCell.propTypes = {
  value: PropTypes.string,
  suffix: PropTypes.oneOfType([PropTypes.string, PropTypes.bool]),
};

export default DefaultCell;