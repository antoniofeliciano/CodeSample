import { api, handleCatch, handleSuccess } from "./baseApi";

export async function TenantsForGrid(then, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("Tenant/TenantsForGrid", {
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
export async function TenantData(then) {
    return api.get("Tenant/TenantData")
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}

export async function AddTenant(tenant, then) {
    return api.post("Tenant", tenant)
        .then((res) => {
            handleSuccess("Tenant adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditTenant(tenant, then) {
    return api.put("Tenant", tenant)
        .then((res) => {
            handleSuccess("Tenant atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteTenant(tenant, then) {
    return api.delete("Tenant", {
        params: {
            tenantId:tenant.tenantId,
            tenantId: tenant.id,
        },
    }).then((res) => {
        handleSuccess("Tenant removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}