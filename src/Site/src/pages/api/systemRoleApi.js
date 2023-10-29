import { api, handleCatch, handleSuccess } from "./baseApi";

export async function RolesForGrid(then, tenantId, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("SystemRole/RolesForGrid", {
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

export async function AddRole(role, then) {
    return api.post("SystemRole", role)
        .then((res) => {
            handleSuccess("Role adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditRole(role, then) {
    return api.put("SystemRole", role)
        .then((res) => {
            handleSuccess("Role atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteRole(role, then) {
    return api.delete("SystemRole", {
        params: {
            tenantId:role.tenantId,
            roleId: role.id,
        },
    }).then((res) => {
        handleSuccess("Role removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}