import { api ,handleCatch, handleSuccess } from "./baseApi";

export async function SignInRequest(data) {    
    return api.post("Authentication/SignIn",data)
    .catch((error) => handleCatch(error));
}

export async function ProfilePicture(then) {
    return api.get("User/ProfilePicture")
        .then((res) => then(res))
        .catch((error) => handleCatch(error));
}