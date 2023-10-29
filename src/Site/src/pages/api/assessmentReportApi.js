import { api, handleCatch } from "./baseApi";

export async function CollectData(then, collectId) {
    return api.get("AssessmentReport/CollectData", {
        params: {
            collectId
        },
    }).then((res) => then(res))
      .catch((error) => handleCatch(error));
}