import { createFileRoute } from "@tanstack/react-router";
import { useState } from "react";
import { client } from "../api/client.gen";
import { getApiCompanyProfiles } from "../api/sdk.gen";
import type { AndersonApiApplicationCompanyProfilesCompanyProfileDto } from "../api/types.gen";

export const Route = createFileRoute("/dashboard")({
  ssr: false,
  component: RouteComponent,
});

client.setConfig({
  baseUrl: "https://localhost:44395",
});

client.interceptors.response.use((response) => {
  if (response.status === 200) {
    console.log(`request to ${response.url} was successful`);
  }
  return response;
});

function RouteComponent() {
  const [profiles, setProfiles] = useState<
    AndersonApiApplicationCompanyProfilesCompanyProfileDto[]
  >([]);

  const onFetchProfiles = async () => {
    const companyProfiles = await getApiCompanyProfiles();
    console.log(companyProfiles);
    setProfiles(companyProfiles.data || []);
  };

  return (
    <div>
      <div>Hello "/dashboard"!</div>
      <button onClick={onFetchProfiles} type="button">
        Fetch Profiles
      </button>
      {profiles.map((profile) => (
        <div key={profile.id}>{profile.name}</div>
      ))}
    </div>
  );
}
