// import React, { useState } from "react";
// // import { FaSquare, FaCheckSquare, FaMinusSquare } from "react-icons/fa";
// // import { IoMdArrowDropright } from "react-icons/io";
import Icon from "@mui/material/Icon";
// import TreeView, { flattenTree } from "react-accessible-treeview";
// import cx from "classnames";

import PropTypes from "prop-types";
import React, { useState } from "react";
import MDTypography from "components/MDTypography";
import MDBox from "components/MDBox";

const jsonData = {
    id: "1",
    label: "Produtos",
    children: [
        {
            id: "2",
            label: "Skincare",
            children: [
                { id: "4", label: "agua micelar" },
                { id: "5", label: "esponja" },
            ],
        },
        {
            id: "3",
            label: "Category 2",
            children: [
                {
                    id: "6",
                    label: "Subcategory 2.1",
                    children: [
                        { id: "16", label: "Subcategory 2.1" },
                        { id: "17", label: "Subcategory 2.2" },
                    ],
                },
                { id: "7", label: "Subcategory 2.2" },
            ],
        },
    ],
};


const TreeView = ({ node, onCheckboxChange, checkedList }) => {
    const [expanded, setExpanded] = useState(false);

    const handleCheckboxChange = (event) => {
        onCheckboxChange(node.id, event.target.checked);
    };

    const handleToggle = () => {
        setExpanded(!expanded);
    };

    return (
        <div>
            <div>
                {!node.children && (
                    <MDBox ml={1.5} display="flex" alignItems="center">
                        <input style={{ width: '16px', height: '16px' }} type="checkbox" checked={checkedList?.includes("5")} onChange={handleCheckboxChange} />
                        <MDTypography ml={.8} variant="h6" color="secondary">
                            {node.label}
                        </MDTypography>
                    </MDBox>
                    

                )}
                {node.children && (
                    <MDBox display="flex" alignItems="center">
                        <Icon color="secondary" sx={{ fontWeight: "bold" }} onClick={handleToggle} > {expanded ? "keyboard_arrow_down_outlined_icon " : "keyboard_arrow_right_outlined_icon "}  </Icon>
                        <MDTypography variant="h6" color="secondary">
                            {node.label}
                        </MDTypography>
                    </MDBox>
                )}

            </div>
            {node.children && expanded && (
                <MDBox ml={1} display="flex" alignItems="center">
                    <ul style={{ listStyle: 'none' }} >
                        {node.children.map((childNode) => (
                            <li key={childNode.id}>
                                <TreeView node={childNode} onCheckboxChange={onCheckboxChange} />
                            </li>
                        ))}
                    </ul>
                </MDBox>
            )}
        </div>
    );
};

const MultiSelectCheckbox = ({ product, setProduct }) => {

    const handleCheckboxChange = (nodeId, checked) => {
        if (checked) {
            if (product.categories) {
                setProduct({ ...product, categories: [...product.categories, nodeId] })
            } else {
                setProduct({ ...product, categories: [nodeId] })
            }

        } else {
            setProduct({ ...product, categories: product.categories.filter((id) => id !== nodeId) })
        }
    };

    return (
        <div>
            <TreeView node={jsonData} onCheckboxChange={handleCheckboxChange} checkedList={product.categories} />
        </div>
    );
};

export default MultiSelectCheckbox;

TreeView.propTypes = {

    node: PropTypes.string,
    onCheckboxChange: PropTypes.func,
    checkedList: PropTypes.array
};
MultiSelectCheckbox.propTypes = {

    product: PropTypes.object,
    setProduct: PropTypes.func
};

