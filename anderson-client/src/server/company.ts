import { getApiCompanyProfiles } from "@/api";
import { createServerFn } from "@tanstack/react-start";

export const companyProfileFn = createServerFn({ method: 'GET' }).handler(async () => {
  const companyProfiles = await getApiCompanyProfiles();
  console.log("server:companyProfiles", companyProfiles);
  return companyProfiles.data;
})