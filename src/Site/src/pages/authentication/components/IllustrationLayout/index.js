

// prop-types is a library for typechecking of props
import PropTypes from "prop-types";
import Grid from "@mui/material/Grid";
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import PageLayout from "pages/components/LayoutContainers/PageLayout";
import { useMaterialUIController } from "context/material";
import { useMediaQuery } from 'react-responsive';
// Image

import logo from "assets/images/illustrations/logo.png";

import bgImage from "assets/images/illustrations/net.jpg";
import bgImage1 from "assets/images/illustrations/net1.jpg";
import bgImage2 from "assets/images/illustrations/net2.jpg";
import bgImage3 from "assets/images/illustrations/net3.jpg";


import Slider from 'react-slick';
import 'slick-carousel/slick/slick.css';
import 'slick-carousel/slick/slick-theme.css';


function IllustrationLayout({ header, title, description, children }) {
  const [controller] = useMaterialUIController();
  const isLargeScreen = useMediaQuery({ minWidth: 1000 }); 

  const settings = {
    dots: false,
    infinite: true,
    speed: 500,
    autoplay: true
  };
  return (
    <PageLayout background="white">
      <Grid
        container
        sx={{
          backgroundColor: ({ palette: { white } }) => white.main,
        }}
      >
        {isLargeScreen  && 
        <Grid item xs={12} lg={5}>
          <Slider {...settings}>
            <MDBox
              display={{ xs: "none", lg: "flex" }}
              width="calc(100% - 2rem)"
              height="calc(100vh)"
              sx={{ backgroundImage: `url(${bgImage})`, backgroundSize: 'cover', }}
            />
            <MDBox
              display={{ xs: "none", lg: "flex" }}
              width="calc(100% - 2rem)"
              height="calc(100vh)"
              sx={{ backgroundImage: `url(${bgImage1})`, backgroundSize: 'cover', }}
            />
            <MDBox
              display={{ xs: "none", lg: "flex" }}
              width="calc(100% - 2rem)"
              height="calc(100vh)"
              sx={{ backgroundImage: `url(${bgImage2})`, backgroundSize: 'cover', }}
            />
            <MDBox
              display={{ xs: "none", lg: "flex" }}
              width="calc(100% - 2rem)"
              height="calc(100vh)"
              sx={{ backgroundImage: `url(${bgImage3})`, backgroundSize: 'cover', }}
            />
          </Slider>

        </Grid>}

        <Grid item xs={11} sm={8} md={6} lg={4} xl={3} sx={{ mx: "auto" }}>

          <MDBox display="flex" flexDirection="column" justifyContent="center" height="100vh">
            <MDBox mt={2} p={0} mb={0} textAlign="center" >
              <img src={logo} alt="Logo" style={{ width: '100px' }} />
            </MDBox>
            <MDBox py={3} px={3} textAlign="center">
              {!header ? (
                <>
                  <MDBox mb={1} textAlign="center">
                    <MDTypography variant="h4" fontWeight="bold">
                      {title}
                    </MDTypography>
                  </MDBox>
                  <MDTypography variant="body2" color="text">
                    {description}
                  </MDTypography>
                </>
              ) : (
                header
              )}
            </MDBox>
            <MDBox p={3}>{children}</MDBox>
          </MDBox>
        </Grid>
      </Grid>
    </PageLayout>
  );
}

// Setting default values for the props of IllustrationLayout
IllustrationLayout.defaultProps = {
  header: "",
  title: "",
  description: "",
};

// Typechecking props for the IllustrationLayout
IllustrationLayout.propTypes = {
  header: PropTypes.node,
  title: PropTypes.string,
  description: PropTypes.string,
  children: PropTypes.node.isRequired
};

export default IllustrationLayout;
