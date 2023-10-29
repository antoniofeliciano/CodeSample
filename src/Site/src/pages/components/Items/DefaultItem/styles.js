function defaultItemIconBox(theme, ownerState) {
  const { functions, palette, borders } = theme;
  const { color } = ownerState;

  const { pxToRem, linearGradient } = functions;
  

  return {
    display: "grid",
    placeItems: "center",
    width: pxToRem(35),
    height: pxToRem(35),
    borderRadius: "100px",
    color: "#fff",
    background: linearGradient("#76C268", "#16CAF1"),
    
  };
}

export default defaultItemIconBox;
