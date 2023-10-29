import React from 'react';
import { Oval,TailSpin,RotatingLines ,MutatingDots,LineWave,ColorRing } from 'react-loader-spinner';
import MDBox from 'components/MDBox';

function Spinner() {
    return (
        <MDBox sx={{
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            width: '100%'
        }}>

            <TailSpin 
                ariaLabel="loading-indicator"
                height={100}
                width={100}
                strokeWidth={5}
                strokeWidthSecondary={1}
                color="#74C27E"
                secondaryColor="#5DCAF2"
            />

        </MDBox>
    );
}

export default Spinner;
