import { api, handleCatch, handleSuccess } from "./baseApi";

export async function SystemApiKeysForGrid(then, tenantId, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("SystemApiKey/SystemApiKeysForGrid", {
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

export async function AddSystemApiKey(systemApiKey, then) {
    return api.post("SystemApiKey", systemApiKey)
        .then((res) => {
            handleSuccess("SystemApiKey adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditSystemApiKey(systemApiKey, then) {
    return api.put("SystemApiKey", systemApiKey)
        .then((res) => {
            handleSuccess("SystemApiKey atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteSystemApiKey(systemApiKey, then) {
    return api.delete("SystemApiKey", {
        params: {
            tenantId:systemApiKey.tenantId,
            systemApiKeyId: systemApiKey.id,
        },
    }).then((res) => {
        handleSuccess("SystemApiKey removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}