import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { Users, Filter, MapPin, Briefcase, Building, CheckCircle, Search } from "lucide-react";
import { useMemo } from "react";
import { useStore } from "@tanstack/react-store";
import { callApi } from "@/server/proxy";
import {
  directoryFilterStore,
  setSelectedRegion,
  setSelectedCountry,
  setSelectedService,
  setSelectedIndustry,
  setSelectedCapability,
  setNameFilter,
  clearAllDirectoryFilters
} from "@/stores/directoryFilterStore";
import { CountryDto, DirectoryProfileListItem, RegionDto, CapabilityDto, IndustryDto, ServiceTypeDto, CompanyProfileDto } from "@/api";

interface DirectoryLoaderData {
  companies: DirectoryProfileListItem[];
  countries: CountryDto[];
  regions: RegionDto[];
  capabilities: CapabilityDto[];
  industries: IndustryDto[];
  serviceTypes: ServiceTypeDto[];
}


export const Route = createFileRoute("/_app/directory")({
  component: NetworkDirectory,
  loader: async () => {
    const [companies, countries, regions, capabilities, industries, serviceTypes] = await Promise.all([
      callApi({ data: { fn: 'getApiCompanies' } }),
      callApi({ data: { fn: 'getApiCountries' } }),
      callApi({ data: { fn: 'getApiRegions' } }),
      callApi({ data: { fn: 'getApiCapabilities' } }),
      callApi({ data: { fn: 'getApiIndustries' } }),
      callApi({ data: { fn: 'getApiServiceTypes' } })
    ]);

    return {
      companies: companies || [],
      countries: countries || [],
      regions: regions || [],
      capabilities: capabilities || [],
      industries: industries || [],
      serviceTypes: serviceTypes || []
    } as DirectoryLoaderData;
    // return { companies: [] } as ProfileLoaderData;
  }
});

function NetworkDirectory() {
  const navigate = useNavigate();
  const { companies, countries, regions, capabilities, industries, serviceTypes } = Route.useLoaderData();
  
  // Get filter state from store
  const filterState = useStore(directoryFilterStore);
  const {
    selectedRegion,
    selectedCountry,
    selectedService,
    selectedIndustry,
    selectedCapability,
    nameFilter
  } = filterState;

  // Transform companies data to match expected format
  const transformedCompanies = useMemo(() => {
    return (companies || []).map((company: any) => {
      const headOffice = company.locations?.find((l: any) => l.isHeadOffice) || company.locations?.[0];
      return {
        id: company.id,
        name: company.name || 'Unknown Company',
        description: company.shortDescription || company.fullDescription || 'No description available.',
        serviceType: company.serviceTypes?.[0]?.name || 'Professional Services',
        skills: company.capabilities?.map((c: any) => c.name) || [],
        industries: company.industries?.map((i: any) => i.name) || [],
        verified: true,
        locations: company.locations?.map((l: any) => ({
          country: countries.find((c: any) => c.id === l.countryId)?.name || 'Unknown',
          region: regions.find((r: any) => r.id === l.regionId)?.name || 'Unknown',
          isHeadOffice: l.isHeadOffice
        })) || [],
        contacts: company.contacts?.map((c: any) => ({
          name: `${c.firstName || ''} ${c.lastName || ''}`.trim() || 'Contact',
          email: c.emailAddress,
          isDefault: true
        })) || []
      };
    });
  }, [companies, countries, regions]);

  // Derived filtered results
  const filteredPartners = useMemo(() => {
    return transformedCompanies.filter((partner: any) => {
      // Name Search
      if (nameFilter && !partner.name.toLowerCase().includes(nameFilter.toLowerCase())) return false;

      // Region/Country Filter
      const hasRegion = selectedRegion === 'All' || partner.locations.some((l: any) => l.region === selectedRegion);
      const hasCountry = selectedCountry === 'All' || partner.locations.some((l: any) => l.country === selectedCountry);
      if (!hasRegion || !hasCountry) return false;

      // Service Filter
      if (selectedService !== 'All' && partner.serviceType !== selectedService) return false;

      // Industry Filter
      if (selectedIndustry !== 'All' && !partner.industries.includes(selectedIndustry)) return false;

      // Capability Filter
      if (selectedCapability !== 'All' && !partner.skills.includes(selectedCapability)) return false;

      return true;
    }).sort((a: any, b: any) => a.name.localeCompare(b.name));
  }, [transformedCompanies, nameFilter, selectedRegion, selectedCountry, selectedService, selectedIndustry, selectedCapability]);


  const clearAllFilters = () => {
    clearAllDirectoryFilters();
  };

  const activeFiltersCount = 
    (selectedRegion !== 'All' ? 1 : 0) +
    (selectedCountry !== 'All' ? 1 : 0) +
    (selectedService !== 'All' ? 1 : 0) +
    (selectedIndustry !== 'All' ? 1 : 0) +
    (selectedCapability !== 'All' ? 1 : 0) +
    (nameFilter ? 1 : 0);

  const allRegions = regions.map((r: any) => r.name).sort();
  const allServiceTypes = serviceTypes.map((s: any) => s.name).sort();
  const allIndustries = industries.map((i: any) => i.name).sort();
  const allCapabilities = capabilities.map((c: any) => c.name).sort();

  return (
    <div className="space-y-10 animate-fade-in">
      <header className="border-b border-gray-200 pb-8">
        <h2 className="text-4xl font-serif text-black mb-3">Network Directory</h2>
        <p className="text-gray-500 font-light text-lg">Browse the complete global index of Andersen member firms and partners.</p>
      </header>

      <div className="flex flex-col lg:flex-row gap-10">
        {/* Sidebar Filters */}
        <aside className="w-full lg:w-80 space-y-8">
          <div className="bg-white border border-gray-200 p-6 shadow-sm">
            <div className="flex justify-between items-center mb-6 border-b border-gray-100 pb-4">
              <h3 className="text-xs font-bold uppercase tracking-[0.2em] text-black flex items-center">
                <Filter className="w-3.5 h-3.5 mr-2" /> Filters
              </h3>
              {activeFiltersCount > 0 && (
                <button 
                  onClick={clearAllFilters}
                  className="text-[10px] font-bold uppercase tracking-widest text-red-600 hover:underline"
                >
                  Clear All
                </button>
              )}
            </div>

            <div className="space-y-6">
              {/* Name Search */}
              <div>
                <label className="block text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-2">Search Firm Name</label>
                <div className="relative">
                  <input 
                    type="text" 
                    value={nameFilter}
                    onChange={(e) => setNameFilter(e.target.value)}
                    placeholder="Enter keywords..."
                    className="w-full bg-gray-50 border border-gray-200 px-3 py-2 text-xs outline-none focus:border-black"
                  />
                  <Search className="absolute right-3 top-2.5 w-3 h-3 text-gray-300" />
                </div>
              </div>

              {/* Region */}
              <div>
                <label className="block text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-2">Region</label>
                <select 
                  value={selectedRegion} 
                  onChange={(e) => setSelectedRegion(e.target.value)}
                  className="w-full bg-gray-50 border border-gray-200 px-3 py-2 text-xs outline-none focus:border-black appearance-none"
                >
                  <option value="All">All Regions</option>
                  {allRegions.map(r => <option key={r} value={r}>{r}</option>)}
                </select>
              </div>

              {/* Country */}
              <div>
                <label className="block text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-2">Country</label>
                <select 
                  value={selectedCountry} 
                  onChange={(e) => setSelectedCountry(e.target.value)}
                  className="w-full bg-gray-50 border border-gray-200 px-3 py-2 text-xs outline-none focus:border-black appearance-none"
                >
                  <option value="All">All Countries</option>
                  {countries
                    .filter((c: any) => selectedRegion === 'All' || regions.find((r: any) => r.id === c.regionId)?.name === selectedRegion)
                    .map((c: any) => <option key={c.name} value={c.name}>{c.name}</option>)}
                </select>
              </div>

              {/* Service Line */}
              <div>
                <label className="block text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-2">Service Line</label>
                <select 
                  value={selectedService} 
                  onChange={(e) => setSelectedService(e.target.value)}
                  className="w-full bg-gray-50 border border-gray-200 px-3 py-2 text-xs outline-none focus:border-black appearance-none"
                >
                  <option value="All">All Services</option>
                  {allServiceTypes.map(s => <option key={s} value={s}>{s}</option>)}
                </select>
              </div>

              {/* Industry */}
              <div>
                <label className="block text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-2">Industry Focus</label>
                <select 
                  value={selectedIndustry} 
                  onChange={(e) => setSelectedIndustry(e.target.value)}
                  className="w-full bg-gray-50 border border-gray-200 px-3 py-2 text-xs outline-none focus:border-black appearance-none"
                >
                  <option value="All">All Industries</option>
                  {allIndustries.map(i => <option key={i} value={i}>{i}</option>)}
                </select>
              </div>

              {/* Capabilities */}
              <div>
                <label className="block text-[9px] font-bold text-gray-400 uppercase tracking-widest mb-2">Key Capabilities</label>
                <select 
                  value={selectedCapability} 
                  onChange={(e) => setSelectedCapability(e.target.value)}
                  className="w-full bg-gray-50 border border-gray-200 px-3 py-2 text-xs outline-none focus:border-black appearance-none"
                >
                  <option value="All">All Capabilities</option>
                  {allCapabilities.map((capability: string) => <option key={capability} value={capability}>{capability}</option>)}
                </select>
              </div>
            </div>
          </div>
        </aside>

        {/* Directory List */}
        <div className="flex-1 space-y-6">
          <div className="flex justify-between items-center mb-4">
            <span className="text-[10px] font-bold text-gray-400 uppercase tracking-widest">
              Showing {filteredPartners.length} {filteredPartners.length === 1 ? 'Firm' : 'Firms'}
            </span>
          </div>

          {filteredPartners.length > 0 ? (
            <div className="grid grid-cols-1 gap-4">
              {filteredPartners.map((partner: any) => {
                const headOffice = partner.locations.find((l: any) => l.isHeadOffice) || partner.locations[0];
                return (
                  <div key={partner.id} className="bg-white border border-gray-200 p-6 hover:border-red-600 transition-all group shadow-sm flex flex-col md:flex-row gap-6 items-start md:items-center">
                    <div className="flex-1">
                      <div className="flex items-center gap-3 mb-2">
                        <h4 className="text-xl font-serif text-black font-bold group-hover:text-red-600 transition-colors">{partner.name}</h4>
                        {partner.verified && <CheckCircle className="w-4 h-4 text-gray-400" />}
                      </div>
                      
                      <div className="flex flex-wrap gap-4 items-center text-[10px] font-bold uppercase tracking-widest text-gray-500">
                        <span className="flex items-center gap-1.5"><MapPin className="w-3 h-3 text-red-600" /> {headOffice?.country}, {headOffice?.region}</span>
                        <span className="flex items-center gap-1.5"><Briefcase className="w-3 h-3 text-red-600" /> {partner.serviceType}</span>
                        <span className="flex items-center gap-1.5 text-black bg-gray-50 px-2 py-0.5 border border-gray-100"><Building className="w-3 h-3" /> {partner.locations.length} Locations</span>
                      </div>

                      <div className="mt-4 flex flex-wrap gap-1.5">
                        {partner.industries.slice(0, 3).map((ind: string) => (
                          <span key={ind} className="px-2 py-0.5 border border-gray-100 text-[8px] font-bold uppercase tracking-tighter text-gray-400">
                            {ind}
                          </span>
                        ))}
                        {partner.industries.length > 3 && <span className="text-[8px] text-gray-300 self-center">+{partner.industries.length - 3}</span>}
                      </div>
                    </div>

                    <div className="flex gap-4 w-full md:w-auto">
                      <button 
                        onClick={() => navigate({ to: `/partners/${partner.id}`, search: { from: 'directory' } })}
                        className="flex-1 md:flex-none px-6 py-2 border border-black text-black hover:bg-black hover:text-white transition-all text-[10px] font-bold uppercase tracking-widest flex items-center justify-center"
                      >
                        Profile
                      </button>
                      <button 
                        onClick={() => {}}
                        className="flex-1 md:flex-none px-6 py-2 bg-red-600 text-white border border-red-600 hover:bg-white hover:text-red-600 transition-all text-[10px] font-bold uppercase tracking-widest flex items-center justify-center"
                      >
                        Connect
                      </button>
                    </div>
                  </div>
                );
              })}
            </div>
          ) : (
            <div className="bg-white border border-dashed border-gray-200 p-20 text-center">
              <div className="w-16 h-16 bg-gray-50 rounded-full flex items-center justify-center mx-auto mb-6">
                <Users className="text-gray-300 w-8 h-8" />
              </div>
              <h3 className="text-xl font-serif text-black mb-2">No Matching Firms</h3>
              <p className="text-gray-500 font-light max-w-sm mx-auto mb-8">Try adjusting your filters to broaden your search within the network directory.</p>
              <button 
                onClick={clearAllFilters}
                className="text-red-600 font-bold uppercase tracking-widest text-xs hover:underline"
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
