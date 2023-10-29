
import colors from "assets/theme/base/colors";


import pxToRem from "assets/theme/functions/pxToRem";
import boxShadow from "assets/theme/functions/boxShadow";

const { secondary,white,success } = colors;

const stepIcon = {
  styleOverrides: {
    root: {
      background:secondary.main,
      fill: secondary.main,
      stroke: secondary.main,
      strokeWidth: pxToRem(10),
      width: pxToRem(13),
      height: pxToRem(13),
      borderRadius: "50%",
      zIndex: 99,
      transition: "all 200ms linear",

      "&.Mui-active": {
        background: white.main,
        fill: white.main,
        stroke: white.main,
        borderColor: white.main,
        boxShadow: boxShadow([0, 0], [0, 2], white.main, 1),
      },

      "&.Mui-completed": {
        background: success.main,
        fill: success.main,
        stroke: success.main,
        borderColor: success.main,
        boxShadow: boxShadow([0, 0], [0, 2], success.main, 1),
      },
    },
  },
};

export default stepIcon;
