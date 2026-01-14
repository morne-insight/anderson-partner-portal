import { createServerFn } from "@tanstack/react-start";
import { getApiCompanyProfiles } from "@/api";

export const companyProfileFn = createServerFn({ method: "GET" }).handler(
  async () => {
    const companyProfiles = await getApiCompanyProfiles();
    console.log("server:companyProfiles", companyProfiles);
    return companyProfiles.data;
  }
);
