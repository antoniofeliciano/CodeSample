import { api, handleCatch, handleSuccess } from "./baseApi";
import axios from "axios";

export async function DatabaseTypesForGrid(then, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("DatabaseType/DatabaseTypesForGrid", {
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
export async function DatabaseTypeFile(then) {
    return api.get("DatabaseType/QueryFile")
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}


export async function AddDatabaseType(databasetype, then) {
    return api.post("DatabaseType", databasetype)
        .then((res) => {
            handleSuccess("Query adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditDatabaseType(databasetype, then) {
    return api.put("DatabaseType", databasetype)
        .then((res) => {
            handleSuccess("Query atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteDatabaseType(databasetype, then) {
    return api.delete("DatabaseType", {
        params: {
            databasetypeId: databasetype.id,
        },
    }).then((res) => {
        handleSuccess("Query removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}