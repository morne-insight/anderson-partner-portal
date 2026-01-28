import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useStore } from "@tanstack/react-store";
import {
  Briefcase,
  Building,
  CheckCircle,
  Filter,
  MapPin,
  Search,
  Users,
} from "lucide-react";
import { useMemo } from "react";
import type { DirectoryProfileListItem } from "@/api";
import { ConnectRequestDialog } from "@/components/ConnectRequestDialog";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";
import {
  clearAllDirectoryFilters,
  directoryFilterStore,
  setNameFilter,
  setSelectedCapability,
  setSelectedCountry,
  setSelectedIndustry,
  setSelectedRegion,
  setSelectedService,
} from "@/stores/directoryFilterStore";

interface DirectoryLoaderData {
  companies: DirectoryProfileListItem[];
}

export const Route = createFileRoute("/_app/directory")({
  component: NetworkDirectory,
  loader: async () => {
    const [companies] = await Promise.all([
      callApi({ data: { fn: "getApiCompanies" } }),
    ]);

    return {
      companies: companies || [],
    } as DirectoryLoaderData;
  },
});

function NetworkDirectory() {
  const navigate = useNavigate();
  const { companies } = Route.useLoaderData();
  const { countries, regions, capabilities, industries, serviceTypes } =
    usePrefetchReferenceData();

  console.log(companies);

  // Get filter state from store
  const filterState = useStore(directoryFilterStore);
  const {
    selectedRegion,
    selectedCountry,
    selectedService,
    selectedIndustry,
    selectedCapability,
    nameFilter,
  } = filterState;

  // Transform companies data to match expected format
  const transformedCompanies = useMemo(() => {
    return (companies || []).map((company: any) => {
      return {
        id: company.id,
        name: company.name || "Unknown Company",
        description:
          company.shortDescription ||
          company.fullDescription ||
          "No description available.",
        serviceType: company.serviceTypeName || "Professional Services",
        skills: company.capabilities?.map((c: any) => c.name) || [],
        industries: company.industries?.map((i: any) => i.name) || [],
        verified: true,
        locations:
          company.locations?.map((l: any) => ({
            country:
              countries.data?.find((c: any) => c.id === l.countryId)?.name ||
              "Unknown",
            region:
              regions.data?.find((r: any) => r.id === l.regionId)?.name ||
              "Unknown",
            isHeadOffice: l.isHeadOffice,
          })) || [],
        contacts:
          company.contacts?.map((c: any) => ({
            name:
              `${c.firstName || ""} ${c.lastName || ""}`.trim() || "Contact",
            email: c.emailAddress,
            isDefault: true,
          })) || [],
      };
    });
  }, [companies, countries, regions]);

  // Derived filtered results
  const filteredPartners = useMemo(() => {
    return transformedCompanies
      .filter((partner: any) => {
        // Name Search
        if (
          nameFilter &&
          !partner.name.toLowerCase().includes(nameFilter.toLowerCase())
        )
          return false;

        // Region/Country Filter
        const hasRegion =
          selectedRegion === "All" ||
          partner.locations.some((l: any) => l.region === selectedRegion);
        const hasCountry =
          selectedCountry === "All" ||
          partner.locations.some((l: any) => l.country === selectedCountry);
        if (!(hasRegion && hasCountry)) return false;

        // Service Filter
        if (
          selectedService !== "All" &&
          partner.serviceType !== selectedService
        )
          return false;

        // Industry Filter
        if (
          selectedIndustry !== "All" &&
          !partner.industries.includes(selectedIndustry)
        )
          return false;

        // Capability Filter
        if (
          selectedCapability !== "All" &&
          !partner.skills.includes(selectedCapability)
        )
          return false;

        return true;
      })
      .sort((a: any, b: any) => a.name.localeCompare(b.name));
  }, [
    transformedCompanies,
    nameFilter,
    selectedRegion,
    selectedCountry,
    selectedService,
    selectedIndustry,
    selectedCapability,
  ]);

  const clearAllFilters = () => {
    clearAllDirectoryFilters();
  };

  const activeFiltersCount =
    (selectedRegion !== "All" ? 1 : 0) +
    (selectedCountry !== "All" ? 1 : 0) +
    (selectedService !== "All" ? 1 : 0) +
    (selectedIndustry !== "All" ? 1 : 0) +
    (selectedCapability !== "All" ? 1 : 0) +
    (nameFilter ? 1 : 0);

  const allRegions = regions?.data?.map((r) => r.name).sort();
  const allServiceTypes = serviceTypes?.data?.map((s) => s.name).sort();
  const allIndustries = industries?.data?.map((i) => i.name).sort();
  const allCapabilities = capabilities?.data?.map((c) => c.name).sort();

  return (
    <div className="animate-fade-in space-y-10">
      <header className="border-gray-200 border-b pb-8">
        <h2 className="mb-3 font-serif text-4xl text-black">
          Network Directory
        </h2>
        <p className="font-light text-gray-500 text-lg">
          Browse the complete global index of Andersen member firms and
          partners.
        </p>
      </header>

      <div className="flex flex-col gap-10 lg:flex-row">
        {/* Sidebar Filters */}
        <aside className="w-full space-y-8 lg:w-80">
          <div className="border border-gray-200 bg-white p-6 shadow-sm">
            <div className="mb-6 flex items-center justify-between border-gray-100 border-b pb-4">
              <h3 className="flex items-center font-bold text-black text-xs uppercase tracking-[0.2em]">
                <Filter className="mr-2 h-3.5 w-3.5" /> Filters
              </h3>
              {activeFiltersCount > 0 && (
                <button
                  className="font-bold text-[10px] text-red-600 uppercase tracking-widest hover:underline"
                  onClick={clearAllFilters}
                  type="button"
                >
                  Clear All
                </button>
              )}
            </div>

            <div className="space-y-6">
              {/* Name Search */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Search Firm Name
                </label>
                <div className="relative">
                  <input
                    className="w-full border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                    onChange={(e) => setNameFilter(e.target.value)}
                    placeholder="Enter keywords..."
                    type="text"
                    value={nameFilter}
                  />
                  <Search className="absolute top-2.5 right-3 h-3 w-3 text-gray-300" />
                </div>
              </div>

              {/* Region */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Region
                </label>
                <select
                  className="w-full appearance-none border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                  onChange={(e) => setSelectedRegion(e.target.value)}
                  value={selectedRegion}
                >
                  <option value="All">All Regions</option>
                  {allRegions?.map((r) => (
                    <option key={r} value={r}>
                      {r}
                    </option>
                  ))}
                </select>
              </div>

              {/* Country */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Country
                </label>
                <select
                  className="w-full appearance-none border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                  onChange={(e) => setSelectedCountry(e.target.value)}
                  value={selectedCountry}
                >
                  <option value="All">All Countries</option>
                  {countries.data
                    ?.filter(
                      (c: any) =>
                        selectedRegion === "All" ||
                        regions.data?.find((r: any) => r.id === c.regionId)
                          ?.name === selectedRegion
                    )
                    .map((c: any) => (
                      <option key={c.name} value={c.name}>
                        {c.name}
                      </option>
                    ))}
                </select>
              </div>

              {/* Service Line */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Service Line
                </label>
                <select
                  className="w-full appearance-none border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                  onChange={(e) => setSelectedService(e.target.value)}
                  value={selectedService}
                >
                  <option value="All">All Services</option>
                  {allServiceTypes?.map((s) => (
                    <option key={s} value={s}>
                      {s}
                    </option>
                  ))}
                </select>
              </div>

              {/* Industry */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Industry Focus
                </label>
                <select
                  className="w-full appearance-none border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                  onChange={(e) => setSelectedIndustry(e.target.value)}
                  value={selectedIndustry}
                >
                  <option value="All">All Industries</option>
                  {allIndustries?.map((i) => (
                    <option key={i} value={i}>
                      {i}
                    </option>
                  ))}
                </select>
              </div>

              {/* Capabilities */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Key Capabilities
                </label>
                <select
                  className="w-full appearance-none border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                  onChange={(e) => setSelectedCapability(e.target.value)}
                  value={selectedCapability}
                >
                  <option value="All">All Capabilities</option>
                  {allCapabilities?.map((capability) => (
                    <option key={capability} value={capability}>
                      {capability}
                    </option>
                  ))}
                </select>
              </div>
            </div>
          </div>
        </aside>

        {/* Directory List */}
        <div className="flex-1 space-y-6">
          <div className="mb-4 flex items-center justify-between">
            <span className="font-bold text-[10px] text-gray-400 uppercase tracking-widest">
              Showing {filteredPartners.length}{" "}
              {filteredPartners.length === 1 ? "Firm" : "Firms"}
            </span>
          </div>

          {filteredPartners.length > 0 ? (
            <div className="grid grid-cols-1 gap-4">
              {filteredPartners.map((partner: any) => {
                const headOffice =
                  partner.locations.find((l: any) => l.isHeadOffice) ||
                  partner.locations[0];
                return (
                  <div
                    className="group flex flex-col items-start gap-6 border border-gray-200 bg-white p-6 shadow-sm transition-all hover:border-red-600 md:flex-row md:items-center"
                    key={partner.id}
                  >
                    <div className="flex-1">
                      <div className="mb-2 flex items-center gap-3">
                        <h4 className="font-bold font-serif text-black text-xl transition-colors group-hover:text-red-600">
                          {partner.name}
                        </h4>
                        {partner.verified && (
                          <CheckCircle className="h-4 w-4 text-gray-400" />
                        )}
                      </div>

                      <div className="flex flex-wrap items-center gap-4 font-bold text-[10px] text-gray-500 uppercase tracking-widest">
                        <span className="flex items-center gap-1.5">
                          <MapPin className="h-3 w-3 text-red-600" />{" "}
                          {headOffice?.country}, {headOffice?.region}
                        </span>
                        <span className="flex items-center gap-1.5">
                          <Briefcase className="h-3 w-3 text-red-600" />{" "}
                          {partner.serviceType}
                        </span>
                        <span className="flex items-center gap-1.5 border border-gray-100 bg-gray-50 px-2 py-0.5 text-black">
                          <Building className="h-3 w-3" />{" "}
                          {partner.locations.length} Locations
                        </span>
                      </div>

                      <div className="mt-4 flex flex-wrap gap-1.5">
                        {partner.industries.slice(0, 3).map((ind: string) => (
                          <span
                            className="border border-gray-100 px-2 py-0.5 font-bold text-[8px] text-gray-400 uppercase tracking-tighter"
                            key={ind}
                          >
                            {ind}
                          </span>
                        ))}
                        {partner.industries.length > 3 && (
                          <span className="self-center text-[8px] text-gray-300">
                            +{partner.industries.length - 3}
                          </span>
                        )}
                      </div>
                    </div>

                    <div className="flex w-full gap-4 md:w-auto">
                      <button
                        className="flex flex-1 items-center justify-center border border-black px-6 py-2 font-bold text-[10px] text-black uppercase tracking-widest transition-all hover:bg-black hover:text-white md:flex-none"
                        onClick={() =>
                          navigate({
                            to: `/partners/${partner.id}`,
                            search: { from: "directory" },
                          })
                        }
                        type="button"
                      >
                        Profile
                      </button>
                      <ConnectRequestDialog
                        partnerId={partner.id}
                        partnerName={partner.name}
                      >
                        <button
                          className="flex flex-1 items-center justify-center border border-red-600 bg-red-600 px-6 py-2 font-bold text-[10px] text-white uppercase tracking-widest transition-all hover:bg-white hover:text-red-600 md:flex-none"
                          type="button"
                        >
                          Connect
                        </button>
                      </ConnectRequestDialog>
                    </div>
                  </div>
                );
              })}
            </div>
          ) : (
            <div className="border border-gray-200 border-dashed bg-white p-20 text-center">
              <div className="mx-auto mb-6 flex h-16 w-16 items-center justify-center rounded-full bg-gray-50">
                <Users className="h-8 w-8 text-gray-300" />
              </div>
              <h3 className="mb-2 font-serif text-black text-xl">
                No Matching Firms
              </h3>
              <p className="mx-auto mb-8 max-w-sm font-light text-gray-500">
                Try adjusting your filters to broaden your search within the
                network directory.
              </p>
              <button
                className="font-bold text-red-600 text-xs uppercase tracking-widest hover:underline"
                onClick={clearAllFilters}
                type="button"
              >
                Reset All Filters
              </button>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
