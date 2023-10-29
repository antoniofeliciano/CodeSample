import { api, handleCatch, handleSuccess } from "./baseApi";
import axios from "axios";

export async function AssessmentCollectsForGrid(then, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("AssessmentCollect/AssessmentCollectsForGrid", {
        params: {
            searchTerm,
            pageNumber,
            pageSize,
            orderBy,
            direction
        },
    })
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}


export async function AddAssessmentCollect(assessmentcollect, then) {
    return api.post("AssessmentCollect", assessmentcollect)
        .then((res) => {
            handleSuccess("Coleta adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditAssessmentCollect(assessmentcollect, then) {
    return api.put("AssessmentCollect", assessmentcollect)
        .then((res) => {
            handleSuccess("Coleta atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteAssessmentCollect(assessmentcollect, then) {
    console.log(assessmentcollect.id);
    return api.delete("AssessmentCollect", {
        params: {
            assessmentcollectId: assessmentcollect.id,
        },
    }).then((res) => {
        handleSuccess("Coleta removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}