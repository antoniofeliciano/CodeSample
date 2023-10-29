import { api, handleCatch, handleSuccess } from "./baseApi";
import axios from "axios";

export async function AssessmentQueriesForGrid(then, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("AssessmentQuery/AssessmentQueriesForGrid", {
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
export async function AssessmentQueryFile(then) {
    return api.get("AssessmentQuery/QueryFile")
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}


export async function AddAssessmentQuery(assessmentquery, then) {
    return api.post("AssessmentQuery", assessmentquery)
        .then((res) => {
            handleSuccess("Query adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditAssessmentQuery(assessmentquery, then) {
    return api.put("AssessmentQuery", assessmentquery)
        .then((res) => {
            handleSuccess("Query atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteAssessmentQuery(assessmentquery, then) {
    return api.delete("AssessmentQuery", {
        params: {
            assessmentqueryId: assessmentquery.id,
        },
    }).then((res) => {
        handleSuccess("Query removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}