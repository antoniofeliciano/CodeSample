import { useState } from "react";
import 'react-draft-wysiwyg/dist/react-draft-wysiwyg.css';
import MDBox from "components/MDBox";
import MDTypography from "components/MDTypography";
import { EditorState, convertToRaw, ContentState } from 'draft-js';
import { Editor } from 'react-draft-wysiwyg';
import draftToHtml from 'draftjs-to-html';
import htmlToDraft from 'html-to-draftjs';

import PropTypes from "prop-types";
import { makeStyles } from '@material-ui/core/styles';
import colors from "assets/theme/base/colors";



const useStyles = makeStyles((theme) => ({
    wrapper: {
        color: '#333',
        borderRadius:10,
        border: 'none',
        borderBottom: `1px solid #ccc`,
        borderLeft: '1px solid #ccc',
        borderRight: '1px solid #ccc',
    },
    editor: {
        backgroundColor: '#fff',
        borderRadius:10,
    },
}));





export function TextEditor({ value, onChangeContent }) {
    const classes = useStyles();
    const [editorState, setEditorState] = useState(() => {
        const contentBlock = htmlToDraft(value ?? "");
        const contentState = ContentState.createFromBlockArray(contentBlock.contentBlocks);
        return EditorState.createWithContent(contentState);
    });

    const handleEditorChange = (newEditorState) => {
        setEditorState(newEditorState);
        const html = draftToHtml(convertToRaw(newEditorState.getCurrentContent()))
        console.log(html);
        onChangeContent(html);
    };


    return (
        <MDBox>
            
                <Editor
                    editorClassName={classes.editor}
                    wrapperClassName={classes.wrapper}            
                    editorState={editorState} onEditorStateChange={handleEditorChange} />
            
        </MDBox>
    )
}





TextEditor.propTypes = {
    onChangeContent: PropTypes.func,
    value: PropTypes.string
};