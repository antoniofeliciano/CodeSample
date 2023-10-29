import { api, handleCatch, handleSuccess } from "./baseApi";

export async function UsersForGrid(then, tenantId, searchTerm = null, pageNumber = null, pageSize = null, orderBy = null, direction = null) {
    return api.get("User/UsersForGrid", {
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
export async function ProfilePicture(then) {
    return api.get("User/ProfilePicture")
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}
export async function AddUser(user, then) {
    return api.post("User", user)
        .then((res) => {
            handleSuccess("User adicionada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function EditUser(user, then) {
    return api.put("User", user)
        .then((res) => {
            handleSuccess("User atualizada!");
            then();
        })
        .catch((error) => handleCatch(error));
}
export async function DeleteUser(user, then) {
    return api.delete("User", {
        params: {
            tenantId:user.tenantId,
            userId: user.id,
        },
    }).then((res) => {
        handleSuccess("User removida!");
        then();
    })
        .catch((error) => handleCatch(error));
}