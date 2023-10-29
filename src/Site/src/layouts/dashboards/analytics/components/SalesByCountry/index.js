

// @react-jvectormap components
import { VectorMap } from "@react-jvectormap/core";
import { worldMerc } from "@react-jvectormap/world";
import { brMill } from '@react-jvectormap/brazil';



import Card from "@mui/material/Card";
import Grid from "@mui/material/Grid";
import Icon from "@mui/material/Icon";


import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";

import SalesTable from "examples/Tables/SalesTable";

// Data
import salesTableData from "layouts/dashboards/analytics/components/SalesByCountry/data/salesTableData";


function SalesByCountry() {

  const markers = [
    {
      name: "Acre",
      code: "AC",
      coords: [1, -5],
    },
    {
      name: "Rio de Janeiro",
      code: "RJ",
      coords: [-22.9068, -43.1729],
    },
    {
      name: "Rio Grande do Norte",
      coords: [-5.4026, -36.9541],
      style: { fill: "#e91e63" },
    }
  ];

  const stateStyle = {
    initial: {
      fill: "#e91e63",  // Cor inicial do estado
    },
    selected: {
      fill: "#ff0000",  // Cor quando o estado é selecionado
    },
    hover: {
      fill: "#00ff00",  // Cor quando o mouse está sobre o estado
    },
  };

  return (
    <Card sx={{ width: "100%" }}>
      <MDBox display="flex">
        <MDBox
          display="flex"
          justifyContent="center"
          alignItems="center"
          width="4rem"
          height="4rem"
          variant="gradient"
          bgColor="success"
          color="white"
          shadow="md"
          borderRadius="xl"
          ml={3}
          mt={-2}
        >
          <Icon fontSize="medium" color="inherit">
            language
          </Icon>
        </MDBox>
        <MDTypography variant="h6" sx={{ mt: 2, mb: 1, ml: 2 }}>
          Sales by Country
        </MDTypography>
      </MDBox>
      <MDBox p={2}>
        <Grid container>
          <Grid item xs={12} md={7} lg={6}>
            <SalesTable rows={salesTableData} shadow={false} />
          </Grid>
          <Grid item xs={12} md={5} lg={6} sx={{ mt: { xs: 5, lg: 0 } }}>
            {/* <VectorMap
              map={brMill}
              zoomOnScroll={true}
              zoomButtons={true}
              markersSelectable
              backgroundColor="transparent"
              selectedMarkers={[0, 1]}
              markers={markers}
              regionStyle={{
                initial: {
                  fill: "#dee2e7",
                  "fill-opacity": 1,
                  stroke: "none",
                  "stroke-width": 0,
                  "stroke-opacity": 0,
                },
              }}
              markerStyle={{
                initial: {
                  fill: "#e91e63",
                  stroke: "#ffffff",
                  "stroke-width": 5,
                  "stroke-opacity": 0.5,
                  r: 7,
                },
                hover: {
                  fill: "E91E63",
                  stroke: "#ffffff",
                  "stroke-width": 5,
                  "stroke-opacity": 0.5,
                },
                selected: {
                  fill: "E91E63",
                  stroke: "#ffffff",
                  "stroke-width": 5,
                  "stroke-opacity": 0.5,
                },
              }}
              style={{
                marginTop: "-1.5rem",
              }}
              onRegionTipShow={() => false}
              onMarkerTipShow={() => false}
            /> */}
            <VectorMap
              map={brMill}
              series= {{
                regions: [{
                    values: {
                        // Região Norte
                        ac: '#fff9c2',
                        am: '#fff9c2',
                        ap: '#fff9c2',
                        pa: '#fff9c2',
                        ro: '#fff9c2',
                        rr: '#fff9c2',
                        to: '#fff9c2',
                        // Região Nordeste
                        al: '#fcdeeb',
                        ba: '#fcdeeb',
                        ce: '#fcdeeb',
                        ma: '#fcdeeb',
                        pb: '#fcdeeb',
                        pe: '#fcdeeb',
                        pi: '#fcdeeb',
                        rn: '#fcdeeb',
                        se: '#fcdeeb',
                        // Região Centro-Oeste
                        df: '#feb83d',
                        go: '#feb83d',
                        ms: '#feb83d',
                        mt: '#feb83d',
                        // Região Sudeste
                        es: '#e8ec9b',
                        mg: '#e8ec9b',
                        rj: '#e8ec9b',
                        sp: '#e8ec9b',
                        // Região Sul
                        pr: '#fef56c',
                        rs: '#fef56c',
                        sc: '#fef56c'
                    },
                    attribute: 'fill'
                }]}}
              backgroundColor="#ffffff"
              regionStyle={stateStyle}
              containerStyle={{
                width: "100%",
                height: "500px",
              }}
              onRegionClick={(event, code) => {
                // Lógica para manipular o clique em um estado
                console.log(event);
                console.log(`Estado clicado: ${code}`);
              }}
            />
          </Grid>
        </Grid>
      </MDBox>
    </Card>
  );
}

export default SalesByCountry;
