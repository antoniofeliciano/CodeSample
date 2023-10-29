// prop-types is a library for typechecking of props
import PropTypes from "prop-types";

// react-flatpickr components
import Flatpickr from "react-flatpickr";

// Importe o pacote de idioma 'Portuguese'
import { Portuguese } from "flatpickr/dist/l10n/pt";

// react-flatpickr styles
import "flatpickr/dist/flatpickr.css";

import MDInput from "components/MDInput";

function MDDatePicker({ input, ...rest }) {
  return (
    <Flatpickr

      // Use o idioma 'Portuguese' configurado anteriormente
      options={{
        locale: Portuguese,
        enableTime: true,
        altFormat: "F j, Y",
        dateFormat: "d/m/Y H:i",
        time_24hr: true,
        timeZone: "America/Sao_Paulo",
      }}
      {...rest}
      render={({ defaultValue }, ref) => (
        <MDInput disabled={rest.disabled} label={rest.label} {...input} defaultValue={defaultValue} inputRef={ref} fullWidth />
      )}
    />
  );
}

// Setting default values for the props of MDDatePicker
MDDatePicker.defaultProps = {
  input: {},
};

// Typechecking props for the MDDatePicker
MDDatePicker.propTypes = {
  input: PropTypes.objectOf(PropTypes.any),
};

export default MDDatePicker;
