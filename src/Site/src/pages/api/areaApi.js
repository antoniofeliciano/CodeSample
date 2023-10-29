import { api, handleCatch, handleSuccess } from "./baseApi";

export async function AreasForGrid(then, tenantId, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("Area/AreasForGrid", {
        params: {
            tenantId,
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

export async function AddArea(area, then) {
    return api.post("Area", area)
        .then((res) => {
            handleSuccess("Area adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditArea(area, then) {
    return api.put("Area", area)
        .then((res) => {
            handleSuccess("Area atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteArea(area, then) {
    return api.delete("Area", {
        params: {
            tenantId:area.tenantId,
            areaId: area.id,
        },
    }).then((res) => {
        handleSuccess("Area removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}