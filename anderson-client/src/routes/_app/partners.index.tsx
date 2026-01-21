import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { Search, Sparkles, MapPin, CheckCircle, Filter, ChevronDown, Briefcase, X, User, Loader2 } from "lucide-react";
import React from "react";
import { useStore } from "@tanstack/react-store";
import { callApi } from "@/server/proxy";
import {
  partnersSearchStore,
  setQuery,
  setResults,
  setIsSearching,
  setActiveRegionFilter,
  setSelectedServiceType,
  setSelectedCountry,
  setShowServiceDropdown,
  setShowRegionDropdown,
  setShowCountryDropdown,
  clearFilters
} from "@/stores/partnersSearchStore";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { Button } from "@/components/ui/button";

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
      selectedCapabilities
    },
    showServiceDropdown,
    showRegionDropdown,
    showCountryDropdown
  } = searchState;

  
      
  const allServiceTypes = ["Advisory", "Tax Consulting", "IT Consulting", "Financial Law", "Supply Chain Advisory"];
  const allRegions = regions.data?.map((r: any) => r.name).sort();

  const availableCountries = React.useMemo(() => {
    if (activeRegionFilter === "All") return countries.data?.map((c: any) => c.name).sort();
    const regionId = regions.data?.find((r: any) => r.name === activeRegionFilter)?.id;
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
          fn: 'putApiCompaniesPartners', 
          args: { body: { query } } 
        } 
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
          locations: partner.locations?.map((l: any) => ({
             country: countries?.data?.find((c: any) => c.id === l.countryId)?.name || "Unknown",
             region: regions?.data?.find((r: any) => r.id === l.regionId)?.name || "Unknown",
             isHeadOffice: l.isHeadOffice
          })) || [],
          contacts: partner.contacts?.map((c: any) => ({
            name: `${c.firstName} ${c.lastName}`,
            email: c.emailAddress,
            isDefault: true
          })) || []
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
    if (activeRegionFilter !== "All" && !partner.locations.some((l: any) => l.region === activeRegionFilter)) return false;
    if (selectedServiceType !== "All" && partner.serviceType !== selectedServiceType) return false;
    if (selectedCountry !== "All" && !partner.locations.some((l: any) => l.country === selectedCountry)) return false;
    if (selectedCapabilities.length > 0) {
      const hasAll = selectedCapabilities.every((cap: any) => partner.skills.includes(cap));
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
      <div className="flex justify-center items-center min-h-screen">
        <Loader2 className="w-8 h-8 animate-spin text-gray-300" />
      </div>
    );
  }

  // Ensure we have the data before proceeding
  if (isError) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <p className="text-red-600 mb-4">Failed to load reference data</p>
          <Button onClick={() => window.location.reload()}>Retry</Button>
        </div>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-fade-in">
      <header className="border-b border-gray-200 pb-6">
        <h2 className="text-4xl font-serif text-black mb-3">Find Expertise</h2>
        <p className="text-gray-500 font-light text-lg">
          Use natural language to find the perfect partner across the global network.
        </p>
      </header>

      {/* Main Search Input Container */}
      <div className="bg-white border border-gray-200 shadow-sm relative group overflow-hidden">
        <div className="absolute top-0 left-0 w-1 h-full bg-red-600 transition-all group-hover:w-1.5"></div>
        <form onSubmit={handleSearch} className="flex flex-col p-8 pb-3">
          <div className="relative mb-2">
            <Sparkles className="absolute top-2 left-0 text-red-600 w-5 h-5 opacity-40" />
            <textarea
              value={query}
              onChange={(e) => setQuery(e.target.value)}
              onKeyDown={(e) => {
                if (e.key === 'Enter' && e.shiftKey) {
                  console.log("Shift + Enter pressed");
                  e.preventDefault();
                  handleSearch(e as unknown as React.FormEvent);
                }
              }}
              placeholder="I'm looking for a tax consulting partner based in Europe with SAP expertise..."
              className="w-full pl-10 pr-4 py-1 bg-transparent focus:outline-none resize-none min-h-[80px] text-gray-900 placeholder-gray-300 font-serif text-2xl leading-relaxed rounded-none"
            />
          </div>
          <div className="flex justify-between items-center">
            <span className="text-gray-400 text-xs">Shift + Enter to run search</span>
            <button
              type="submit"
              disabled={isSearching}
              className={`flex items-center gap-3 px-8 py-3 bg-black text-white text-[10px] font-bold uppercase tracking-[0.2em] hover:bg-red-600 transition-all shadow-md ${
                isSearching ? "opacity-70 cursor-wait" : ""
              }`}
            >
              {isSearching ? (
                <>
                  Processing <span className="animate-pulse">...</span>
                </>
              ) : (
                <>
                  Run Search <Search className="w-3.5 h-3.5 ml-1" />
                </>
              )}
            </button>
          </div>
        </form>
      </div>

      {/* Filters Row */}
      <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 pt-2 border-t border-gray-100 mt-4">
        <div className="flex items-center gap-4">
          <span className="text-gray-400 text-[10px] font-bold uppercase tracking-[0.2em] flex items-center">
            <Filter className="w-3 h-3 mr-2" /> Filters
          </span>
          <div className="relative">
            <button
              onClick={() => setShowServiceDropdown(!showServiceDropdown)}
              className={`flex items-center gap-2 px-4 py-2 text-[10px] font-bold uppercase tracking-wider transition-colors border ${
                selectedServiceType !== "All"
                  ? "bg-black text-white border-black"
                  : "bg-white text-gray-900 border-gray-300 hover:border-gray-900"
              }`}
            >
              Service: {selectedServiceType} <ChevronDown className="w-3 h-3" />
            </button>
            {showServiceDropdown && (
              <div className="absolute top-full left-0 mt-1 w-56 bg-white border border-gray-200 shadow-xl z-20 overflow-hidden">
                <button
                  onClick={() => {
                    setSelectedServiceType("All");
                    setShowServiceDropdown(false);
                  }}
                  className="w-full text-left px-4 py-3 text-xs hover:bg-gray-50 border-b"
                >
                  All Services
                </button>
                {allServiceTypes.map((type) => (
                  <button
                    key={type}
                    onClick={() => {
                      setSelectedServiceType(type);
                      setShowServiceDropdown(false);
                    }}
                    className="w-full text-left px-4 py-3 text-xs hover:bg-gray-50 border-b last:border-0"
                  >
                    {type}
                  </button>
                ))}
              </div>
            )}
          </div>
          <div className="relative">
            <button
              onClick={() => setShowRegionDropdown(!showRegionDropdown)}
              className={`flex items-center gap-2 px-4 py-2 text-[10px] font-bold uppercase tracking-wider transition-colors border ${
                activeRegionFilter !== "All"
                  ? "bg-black text-white border-black"
                  : "bg-white text-gray-900 border-gray-300 hover:border-gray-900"
              }`}
            >
              Region: {activeRegionFilter} <ChevronDown className="w-3 h-3" />
            </button>
            {showRegionDropdown && (
              <div className="absolute top-full left-0 mt-1 w-56 bg-white border border-gray-200 shadow-xl z-20 overflow-hidden">
                <button
                  onClick={() => {
                    setActiveRegionFilter("All");
                    setShowRegionDropdown(false);
                  }}
                  className="w-full text-left px-4 py-3 text-xs hover:bg-gray-50 border-b"
                >
                  All Regions
                </button>
                {allRegions?.map((region: any) => (
                  <button
                    key={region}
                    onClick={() => {
                      setActiveRegionFilter(region);
                      setShowRegionDropdown(false);
                    }}
                    className="w-full text-left px-4 py-3 text-xs hover:bg-gray-50 border-b last:border-0"
                  >
                    {region}
                  </button>
                ))}
              </div>
            )}
          </div>
          <div className="relative">
            <button
              onClick={() => setShowCountryDropdown(!showCountryDropdown)}
              className={`flex items-center gap-2 px-4 py-2 text-[10px] font-bold uppercase tracking-wider transition-colors border ${
                selectedCountry !== "All"
                  ? "bg-black text-white border-black"
                  : "bg-white text-gray-900 border-gray-300 hover:border-gray-900"
              }`}
            >
              Country: {selectedCountry} <ChevronDown className="w-3 h-3" />
            </button>
            {showCountryDropdown && (
              <div className="absolute top-full left-0 mt-1 w-56 bg-white border border-gray-200 shadow-xl z-20 max-h-60 overflow-y-auto">
                <button
                  onClick={() => handleCountrySelect("All")}
                  className="w-full text-left px-4 py-3 text-xs hover:bg-gray-50 border-b"
                >
                  All Countries
                </button>
                {availableCountries?.map((country: any) => (
                  <button
                    key={country}
                    onClick={() => handleCountrySelect(country)}
                    className="w-full text-left px-4 py-3 text-xs hover:bg-gray-50 border-b last:border-0"
                  >
                    {country}
                  </button>
                ))}
              </div>
            )}
          </div>
        </div>
        <div className="flex items-center gap-6">
          <span className="text-[10px] font-bold uppercase tracking-widest text-gray-400">
            Found {displayedResults.length} partners
          </span>
          {(activeRegionFilter !== "All" ||
            selectedServiceType !== "All" ||
            selectedCapabilities.length > 0 ||
            selectedCountry !== "All") && (
            <button
              onClick={handleClearFilters}
              className="flex items-center text-red-600 hover:text-black text-[10px] font-bold uppercase tracking-widest transition-colors"
            >
              <X className="w-3 h-3 mr-1" /> Clear All
            </button>
          )}
        </div>
      </div>

      {/* Region Tabs */}
      <div className="flex gap-8 overflow-x-auto pb-0 border-b border-gray-200 no-scrollbar">
        {["All", ...allRegions || []].map((region) => (
          <button
            key={region}
            onClick={() => setActiveRegionFilter(region)}
            className={`pb-4 text-xs font-bold uppercase tracking-widest transition-colors whitespace-nowrap border-b-2 ${
              activeRegionFilter === region
                ? "border-red-600 text-red-600"
                : "border-transparent text-gray-400 hover:text-gray-900"
            }`}
          >
            {region}
          </button>
        ))}
      </div>

      {/* Results Grid */}
      <div className="grid grid-cols-1 gap-6 pt-4">
        {displayedResults.map((partner: any) => {
          const headOffice = partner.locations.find((l: any) => l.isHeadOffice) || partner.locations[0];
          const primaryContact = partner.contacts.find((c: any) => c.isDefault) || partner.contacts[0];

          return (
            <div
              key={partner.id}
              className="bg-white border border-gray-200 p-8 flex flex-col md:flex-row gap-8 hover:shadow-lg transition-all duration-300 group"
            >
              <div className="flex flex-col items-center justify-start min-w-[80px] pt-1">
                <div className="w-16 h-16 flex items-center justify-center border-2 border-gray-100 bg-white group-hover:border-red-600 transition-colors">
                  <span className="text-xl font-serif text-red-600">{partner.matchScore || 85}%</span>
                </div>
                <span className="text-[10px] text-gray-400 mt-2 uppercase tracking-widest font-semibold">
                  Match
                </span>
              </div>
              <div className="flex-1">
                <div className="flex justify-between items-start">
                  <div>
                    <div className="flex flex-wrap items-center gap-3">
                      <h3 className="text-2xl font-serif text-black flex items-center gap-3">
                        {partner.name} {partner.verified && <CheckCircle className="w-4 h-4 text-gray-400" />}
                      </h3>
                      <span className="bg-gray-100 text-gray-500 text-[10px] px-2 py-0.5 font-bold uppercase tracking-widest border border-gray-200 flex items-center">
                        <Briefcase className="w-3 h-3 mr-1" />
                        {partner.serviceType}
                      </span>
                    </div>
                    <div className="flex items-center text-gray-500 text-xs uppercase tracking-wider mt-2 font-medium">
                      <MapPin className="w-3 h-3 mr-1" />
                      {headOffice?.country} <span className="mx-2 text-gray-300">|</span> {headOffice?.region}
                      {partner.locations.length > 1 && (
                        <span className="ml-2 text-red-600 font-bold text-[9px]">
                          + {partner.locations.length - 1} more locations
                        </span>
                      )}
                    </div>
                    <div className="flex items-center text-gray-400 text-[10px] uppercase tracking-widest mt-2 font-bold">
                      <User className="w-3 h-3 mr-1" /> Liaison: {primaryContact?.name}
                    </div>
                  </div>
                </div>
                <p className="text-gray-600 mt-4 text-base leading-relaxed max-w-3xl font-light">
                  {partner.description}
                </p>
                <div className="mt-6 flex flex-wrap gap-2">
                  {partner.skills.slice(0, 5).map((skill: string) => (
                    <span
                      key={skill}
                      className="bg-gray-50 text-gray-600 px-3 py-1.5 text-xs font-medium border border-gray-200 uppercase tracking-wide"
                    >
                      {skill}
                    </span>
                  ))}
                </div>
              </div>
              <div className="md:border-l border-gray-100 md:pl-8 flex flex-col justify-center min-w-[200px] space-y-4">
                <button
                  onClick={() => navigate({ to: `/partners/${partner.id}` })}
                  className="w-full bg-white border border-black text-black py-3 text-xs font-bold uppercase tracking-[0.15em] hover:bg-black hover:text-white transition-colors"
                >
                  View Profile
                </button>
                <button
                  onClick={() => {}}
                  className="w-full bg-red-600 text-white border border-red-600 py-3 text-xs font-bold uppercase tracking-[0.15em] hover:bg-white hover:text-red-600 transition-colors"
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
