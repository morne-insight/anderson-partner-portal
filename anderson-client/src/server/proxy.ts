// src/server/proxy.ts
import { createServerFn } from "@tanstack/react-start";
import * as sdk from "../api/sdk.gen";
/**
 * A generic proxy to call any SDK function from the client via the server.
 * This ensures the session cookie is sent and the Bearer token is injected.
 */
export const callApi = createServerFn({ method: "POST" })
  .inputValidator((payload: { fn: keyof typeof sdk; args?: any }) => payload)
  .handler(async ({ data }) => {
    const apiFn = sdk[data.fn as keyof typeof sdk] as any;
    
    console.log("serverApiFn", apiFn)
    
    if (typeof apiFn !== 'function') {
      throw new Error(`API function "${String(data.fn)}" not found`);
    }
    // The Global Interceptor we added in Part 1 will handle the token!
    const result = await apiFn(data.args);
    
    console.log("serverApiResult", result)

    if (result.error) throw result.error;
    return result.data;
  });