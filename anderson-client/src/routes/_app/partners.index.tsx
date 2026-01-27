import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useStore } from "@tanstack/react-store";
import {
  Briefcase,
  CheckCircle,
  ChevronDown,
  Filter,
  Loader2,
  MapPin,
  Search,
  Sparkles,
  User,
  X,
} from "lucide-react";
import React from "react";
import { Button } from "@/components/ui/button";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";
import {
  clearFilters,
  partnersSearchStore,
  setActiveRegionFilter,
  setIsSearching,
  setQuery,
  setResults,
  setSelectedCountry,
  setSelectedServiceType,
  setShowCountryDropdown,
  setShowRegionDropdown,
  setShowServiceDropdown,
} from "@/stores/partnersSearchStore";

export const Route = createFileRoute("/_app/partners/")({
  component: PartnerSearch,
});

function PartnerSearch() {
  const navigate = useNavigate();
  const { countries, regions, isLoading, isError } = usePrefetchReferenceData();

  const searchState = useStore(partnersSearchStore);
  const {
    query,
    results,
    isSearching,
    filters: {
      activeRegionFilter,
      selectedServiceType,
      selectedCountry,
      selectedCapabilities,
    },
    showServiceDropdown,
    showRegionDropdown,
    showCountryDropdown,
  } = searchState;

  const allServiceTypes = [
    "Advisory",
    "Tax Consulting",
    "IT Consulting",
    "Financial Law",
    "Supply Chain Advisory",
  ];
  const allRegions = regions.data?.map((r: any) => r.name).sort();

  const availableCountries = React.useMemo(() => {
    if (activeRegionFilter === "All")
      return countries.data?.map((c: any) => c.name).sort();
    const regionId = regions.data?.find(
      (r: any) => r.name === activeRegionFilter
    )?.id;
    return countries.data
      ?.filter((c: any) => c.regionId === regionId)
      .map((c: any) => c.name)
      .sort();
  }, [activeRegionFilter, countries, regions]);

  const handleSearch = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!query.trim()) return;

    setIsSearching(true);
    try {
      const response = await callApi({
        data: {
          fn: "putApiCompaniesPartners",
          args: { body: { query } },
        },
      });

      const transformed = (response || []).map((partner: any) => {
        return {
          id: partner.id,
          name: partner.name,
          description: partner.shortDescription || "No description provided.",
          matchScore: partner.matchScore ? Math.round(partner.matchScore) : 85,
          skills: partner.capabilities?.map((c: any) => c.name) || [],
          verified: true, // Default to true for AI results in this UI
          serviceType: "Partner", // Generic since list view doesnt have it
          locations:
            partner.locations?.map((l: any) => ({
              country:
                countries?.data?.find((c: any) => c.id === l.countryId)?.name ||
                "Unknown",
              region:
                regions?.data?.find((r: any) => r.id === l.regionId)?.name ||
                "Unknown",
              isHeadOffice: l.isHeadOffice,
            })) || [],
          contacts:
            partner.contacts?.map((c: any) => ({
              name: `${c.firstName} ${c.lastName}`,
              email: c.emailAddress,
              isDefault: true,
            })) || [],
        };
      });

      setResults(transformed);
    } catch (error) {
      console.error("AI Search failed", error);
    } finally {
      setIsSearching(false);
    }
  };

  const handleCountrySelect = (country: string) => {
    setSelectedCountry(country);
    setShowCountryDropdown(false);
  };

  const displayedResults = results.filter((partner: any) => {
    if (
      activeRegionFilter !== "All" &&
      !partner.locations.some((l: any) => l.region === activeRegionFilter)
    )
      return false;
    if (
      selectedServiceType !== "All" &&
      partner.serviceType !== selectedServiceType
    )
      return false;
    if (
      selectedCountry !== "All" &&
      !partner.locations.some((l: any) => l.country === selectedCountry)
    )
      return false;
    if (selectedCapabilities.length > 0) {
      const hasAll = selectedCapabilities.every((cap: any) =>
        partner.skills.includes(cap)
      );
      if (!hasAll) return false;
    }
    return true;
  });

  const handleClearFilters = () => {
    clearFilters();
  };

  // Show loading state while reference data is loading
  if (isLoading) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-gray-300" />
      </div>
    );
  }

  // Ensure we have the data before proceeding
  if (isError) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <div className="text-center">
          <p className="mb-4 text-red-600">Failed to load reference data</p>
          <Button onClick={() => window.location.reload()}>Retry</Button>
        </div>
      </div>
    );
  }

  return (
    <div className="animate-fade-in space-y-8">
      <header className="border-gray-200 border-b pb-6">
        <h2 className="mb-3 font-serif text-4xl text-black">Find Expertise</h2>
        <p className="font-light text-gray-500 text-lg">
          Use natural language to find the perfect partner across the global
          network.
        </p>
      </header>

      {/* Main Search Input Container */}
      <div className="group relative overflow-hidden border border-gray-200 bg-white shadow-sm">
        <div className="absolute top-0 left-0 h-full w-1 bg-red-600 transition-all group-hover:w-1.5" />
        <form className="flex flex-col p-8 pb-3" onSubmit={handleSearch}>
          <div className="relative mb-2">
            <Sparkles className="absolute top-2 left-0 h-5 w-5 text-red-600 opacity-40" />
            <textarea
              className="min-h-[80px] w-full resize-none rounded-none bg-transparent py-1 pr-4 pl-10 font-serif text-2xl text-gray-900 leading-relaxed placeholder-gray-300 focus:outline-none"
              onChange={(e) => setQuery(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === "Enter" && e.shiftKey) {
                  console.log("Shift + Enter pressed");
                  e.preventDefault();
                  handleSearch(e as unknown as React.FormEvent);
                }
              }}
              placeholder="I'm looking for a tax consulting partner based in Europe with SAP expertise..."
              value={query}
            />
          </div>
          <div className="flex items-center justify-between">
            <span className="text-gray-400 text-xs">
              Shift + Enter to run search
            </span>
            <button
              className={`flex items-center gap-3 bg-black px-8 py-3 font-bold text-[10px] text-white uppercase tracking-[0.2em] shadow-md transition-all hover:bg-red-600 ${
                isSearching ? "cursor-wait opacity-70" : ""
              }`}
              disabled={isSearching}
              type="submit"
            >
              {isSearching ? (
                <>
                  Processing <span className="animate-pulse">...</span>
                </>
              ) : (
                <>
                  Run Search <Search className="ml-1 h-3.5 w-3.5" />
                </>
              )}
            </button>
          </div>
        </form>
      </div>

      {/* Filters Row */}
      <div className="mt-4 flex flex-col justify-between gap-4 border-gray-100 border-t pt-2 md:flex-row md:items-center">
        <div className="flex items-center gap-4">
          <span className="flex items-center font-bold text-[10px] text-gray-400 uppercase tracking-[0.2em]">
            <Filter className="mr-2 h-3 w-3" /> Filters
          </span>
          <div className="relative">
            <button
              className={`flex items-center gap-2 border px-4 py-2 font-bold text-[10px] uppercase tracking-wider transition-colors ${
                selectedServiceType !== "All"
                  ? "border-black bg-black text-white"
                  : "border-gray-300 bg-white text-gray-900 hover:border-gray-900"
              }`}
              onClick={() => setShowServiceDropdown(!showServiceDropdown)}
            >
              Service: {selectedServiceType} <ChevronDown className="h-3 w-3" />
            </button>
            {showServiceDropdown && (
              <div className="absolute top-full left-0 z-20 mt-1 w-56 overflow-hidden border border-gray-200 bg-white shadow-xl">
                <button
                  className="w-full border-b px-4 py-3 text-left text-xs hover:bg-gray-50"
                  onClick={() => {
                    setSelectedServiceType("All");
                    setShowServiceDropdown(false);
                  }}
                >
                  All Services
                </button>
                {allServiceTypes.map((type) => (
                  <button
                    className="w-full border-b px-4 py-3 text-left text-xs last:border-0 hover:bg-gray-50"
                    key={type}
                    onClick={() => {
                      setSelectedServiceType(type);
                      setShowServiceDropdown(false);
                    }}
                  >
                    {type}
                  </button>
                ))}
              </div>
            )}
          </div>
          <div className="relative">
            <button
              className={`flex items-center gap-2 border px-4 py-2 font-bold text-[10px] uppercase tracking-wider transition-colors ${
                activeRegionFilter !== "All"
                  ? "border-black bg-black text-white"
                  : "border-gray-300 bg-white text-gray-900 hover:border-gray-900"
              }`}
              onClick={() => setShowRegionDropdown(!showRegionDropdown)}
            >
              Region: {activeRegionFilter} <ChevronDown className="h-3 w-3" />
            </button>
            {showRegionDropdown && (
              <div className="absolute top-full left-0 z-20 mt-1 w-56 overflow-hidden border border-gray-200 bg-white shadow-xl">
                <button
                  className="w-full border-b px-4 py-3 text-left text-xs hover:bg-gray-50"
                  onClick={() => {
                    setActiveRegionFilter("All");
                    setShowRegionDropdown(false);
                  }}
                >
                  All Regions
                </button>
                {allRegions?.map((region: any) => (
                  <button
                    className="w-full border-b px-4 py-3 text-left text-xs last:border-0 hover:bg-gray-50"
                    key={region}
                    onClick={() => {
                      setActiveRegionFilter(region);
                      setShowRegionDropdown(false);
                    }}
                  >
                    {region}
                  </button>
                ))}
              </div>
            )}
          </div>
          <div className="relative">
            <button
              className={`flex items-center gap-2 border px-4 py-2 font-bold text-[10px] uppercase tracking-wider transition-colors ${
                selectedCountry !== "All"
                  ? "border-black bg-black text-white"
                  : "border-gray-300 bg-white text-gray-900 hover:border-gray-900"
              }`}
              onClick={() => setShowCountryDropdown(!showCountryDropdown)}
            >
              Country: {selectedCountry} <ChevronDown className="h-3 w-3" />
            </button>
            {showCountryDropdown && (
              <div className="absolute top-full left-0 z-20 mt-1 max-h-60 w-56 overflow-y-auto border border-gray-200 bg-white shadow-xl">
                <button
                  className="w-full border-b px-4 py-3 text-left text-xs hover:bg-gray-50"
                  onClick={() => handleCountrySelect("All")}
                >
                  All Countries
                </button>
                {availableCountries?.map((country: any) => (
                  <button
                    className="w-full border-b px-4 py-3 text-left text-xs last:border-0 hover:bg-gray-50"
                    key={country}
                    onClick={() => handleCountrySelect(country)}
                  >
                    {country}
                  </button>
                ))}
              </div>
            )}
          </div>
        </div>
        <div className="flex items-center gap-6">
          <span className="font-bold text-[10px] text-gray-400 uppercase tracking-widest">
            Found {displayedResults.length} partners
          </span>
          {(activeRegionFilter !== "All" ||
            selectedServiceType !== "All" ||
            selectedCapabilities.length > 0 ||
            selectedCountry !== "All") && (
            <button
              className="flex items-center font-bold text-[10px] text-red-600 uppercase tracking-widest transition-colors hover:text-black"
              onClick={handleClearFilters}
            >
              <X className="mr-1 h-3 w-3" /> Clear All
            </button>
          )}
        </div>
      </div>

      {/* Region Tabs */}
      <div className="no-scrollbar flex gap-8 overflow-x-auto border-gray-200 border-b pb-0">
        {["All", ...(allRegions || [])].map((region) => (
          <button
            className={`whitespace-nowrap border-b-2 pb-4 font-bold text-xs uppercase tracking-widest transition-colors ${
              activeRegionFilter === region
                ? "border-red-600 text-red-600"
                : "border-transparent text-gray-400 hover:text-gray-900"
            }`}
            key={region}
            onClick={() => setActiveRegionFilter(region)}
          >
            {region}
          </button>
        ))}
      </div>

      {/* Results Grid */}
      <div className="grid grid-cols-1 gap-6 pt-4">
        {displayedResults.map((partner: any) => {
          const headOffice =
            partner.locations.find((l: any) => l.isHeadOffice) ||
            partner.locations[0];
          const primaryContact =
            partner.contacts.find((c: any) => c.isDefault) ||
            partner.contacts[0];

          return (
            <div
              className="group flex flex-col gap-8 border border-gray-200 bg-white p-8 transition-all duration-300 hover:shadow-lg md:flex-row"
              key={partner.id}
            >
              <div className="flex min-w-[80px] flex-col items-center justify-start pt-1">
                <div className="flex h-16 w-16 items-center justify-center border-2 border-gray-100 bg-white transition-colors group-hover:border-red-600">
                  <span className="font-serif text-red-600 text-xl">
                    {partner.matchScore || 85}%
                  </span>
                </div>
                <span className="mt-2 font-semibold text-[10px] text-gray-400 uppercase tracking-widest">
                  Match
                </span>
              </div>
              <div className="flex-1">
                <div className="flex items-start justify-between">
                  <div>
                    <div className="flex flex-wrap items-center gap-3">
                      <h3 className="flex items-center gap-3 font-serif text-2xl text-black">
                        {partner.name}{" "}
                        {partner.verified && (
                          <CheckCircle className="h-4 w-4 text-gray-400" />
                        )}
                      </h3>
                      <span className="flex items-center border border-gray-200 bg-gray-100 px-2 py-0.5 font-bold text-[10px] text-gray-500 uppercase tracking-widest">
                        <Briefcase className="mr-1 h-3 w-3" />
                        {partner.serviceType}
                      </span>
                    </div>
                    <div className="mt-2 flex items-center font-medium text-gray-500 text-xs uppercase tracking-wider">
                      <MapPin className="mr-1 h-3 w-3" />
                      {headOffice?.country}{" "}
                      <span className="mx-2 text-gray-300">|</span>{" "}
                      {headOffice?.region}
                      {partner.locations.length > 1 && (
                        <span className="ml-2 font-bold text-[9px] text-red-600">
                          + {partner.locations.length - 1} more locations
                        </span>
                      )}
                    </div>
                    <div className="mt-2 flex items-center font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                      <User className="mr-1 h-3 w-3" /> Liaison:{" "}
                      {primaryContact?.name}
                    </div>
                  </div>
                </div>
                <p className="mt-4 max-w-3xl font-light text-base text-gray-600 leading-relaxed">
                  {partner.description}
                </p>
                <div className="mt-6 flex flex-wrap gap-2">
                  {partner.skills.slice(0, 5).map((skill: string) => (
                    <span
                      className="border border-gray-200 bg-gray-50 px-3 py-1.5 font-medium text-gray-600 text-xs uppercase tracking-wide"
                      key={skill}
                    >
                      {skill}
                    </span>
                  ))}
                </div>
              </div>
              <div className="flex min-w-[200px] flex-col justify-center space-y-4 border-gray-100 md:border-l md:pl-8">
                <button
                  className="w-full border border-black bg-white py-3 font-bold text-black text-xs uppercase tracking-[0.15em] transition-colors hover:bg-black hover:text-white"
                  onClick={() => navigate({ to: `/partners/${partner.id}` })}
                >
                  View Profile
                </button>
                <button
                  className="w-full border border-red-600 bg-red-600 py-3 font-bold text-white text-xs uppercase tracking-[0.15em] transition-colors hover:bg-white hover:text-red-600"
                  onClick={() => {}}
                >
                  Connect
                </button>
              </div>
            </div>
          );
        })}
      </div>
    </div>
  );
}
