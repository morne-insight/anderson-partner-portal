import { createFileRoute } from "@tanstack/react-router";
import { useServerFn } from "@tanstack/react-start";
import { useState } from "react";
import { companyProfileFn } from "@/server/company";
import type { CompanyProfileDto } from "../../api";
import { getApiCompanyProfiles } from "../../api";

export const Route = createFileRoute("/_app/dashboard")({
  component: RouteComponent,
});

function RouteComponent() {
  const fetchProfiles = useServerFn(companyProfileFn);
  const [profiles, setProfiles] = useState<CompanyProfileDto[]>([]);
  const [profilesFromServer, setProfilesFromServer] = useState<
    CompanyProfileDto[]
  >([]);

  const onFetchProfiles = async () => {
    const companyProfiles = await getApiCompanyProfiles();
    console.log(companyProfiles);
    setProfiles(companyProfiles.data || []);
  };

  const onFetchProfilesFromServer = async () => {
    const companyProfiles = await fetchProfiles();
    console.log("server:fetch companyProfiles", companyProfiles);
    setProfilesFromServer(companyProfiles || []);
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
      <div>-------------</div>
      <button onClick={onFetchProfilesFromServer} type="button">
        Fetch Profiles from server
      </button>
      {profilesFromServer.map((profile) => (
        <div key={profile.id}>{profile.name}</div>
      ))}

      <div>-------------</div>
    </div>
  );
}
