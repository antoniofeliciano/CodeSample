import { api, handleCatch, handleSuccess } from "./baseApi";

export async function GetSystemInterfaces(then, tenantId) {
    return api.get("SystemInterface/SystemInterfaces", {
        params: {
            tenantId
        },
    })
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}

export async function InterfacesForGrid(then, tenantId, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("SystemInterface/InterfacesForGrid", {
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

export async function AddInterface(interface_, then) {
    return api.post("SystemInterface", interface_)
        .then((res) => {
            handleSuccess("Interface_ adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditInterface(interface_, then) {
    return api.put("SystemInterface", interface_)
        .then((res) => {
            handleSuccess("Interface atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteInterface(interface_, then) {
    return api.delete("SystemInterface", {
        params: {
            tenantId:interface_.tenantId,
            interfaceId: interface_.id,
        },
    }).then((res) => {
        handleSuccess("Interface removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}