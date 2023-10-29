import { api, handleCatch, handleSuccess } from "./baseApi";

export async function PermissionsForGrid(then, tenantId, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("Permission/PermissionsForGrid", {
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

export async function AddPermission(permission, then) {
    return api.post("Permission", permission)
        .then((res) => {
            handleSuccess("Permissão adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditPermission(permission, then) {
    return api.put("Permission", permission)
        .then((res) => {
            handleSuccess("Permissão atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeletePermission(permission, then) {
    return api.delete("Permission", {
        params: {
            tenantId:permission.tenantId,
            permissionId: permission.id,
        },
    }).then((res) => {
        handleSuccess("Permissão removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}