import { createFileRoute, useNavigate } from "@tanstack/react-router";
import { useStore } from "@tanstack/react-store";
import {
  Briefcase,
  Building,
  CheckCircle,
  Filter,
  Loader2,
  MapPin,
  Search,
  Users,
} from "lucide-react";
import React, { useCallback, useEffect, useState } from "react";
import { ConnectRequestDialog } from "@/components/ConnectRequestDialog";
import {
  Pagination,
  PaginationContent,
  PaginationEllipsis,
  PaginationItem,
  PaginationLink,
  PaginationNext,
  PaginationPrevious,
} from "@/components/ui/pagination";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";
import {
  clearAllDirectoryFilters,
  directoryFilterStore,
  setIsLoading,
  setNameFilter,
  setPageNumber,
  setPagination,
  setSelectedCapability,
  setSelectedCoreService,
  setSelectedCountry,
  setSelectedIndustry,
  setSelectedRegion,
  setSelectedService,
} from "@/stores/directoryFilterStore";

export const Route = createFileRoute("/_app/directory")({
  component: NetworkDirectory,
});

function NetworkDirectory() {
  const navigate = useNavigate();
  const { countries, regions, capabilities, industries, serviceTypes, serviceSubTypes } =
    usePrefetchReferenceData();

  // Get filter state from store
  const filterState = useStore(directoryFilterStore);
  const {
    selectedRegion,
    selectedCountry,
    selectedService,
    selectedCoreService,
    selectedIndustry,
    selectedCapability,
    nameFilter,
    pagination: { pageNumber, pageSize, totalCount, pageCount },
    isLoading,
  } = filterState;

  // State for fetched partners
  const [partners, setPartners] = useState<any[]>([]);

  const fetchPartners = useCallback(
    async (page: number = pageNumber) => {
      setIsLoading(true);
      try {
        const regionIds =
          selectedRegion !== "All"
            ? [regions.data?.find((r) => r.name === selectedRegion)?.id].filter(Boolean) as string[]
            : [];
        const countryIds =
          selectedCountry !== "All"
            ? [countries.data?.find((c) => c.name === selectedCountry)?.id].filter(Boolean) as string[]
            : [];
        const serviceTypeId =
          selectedService !== "All"
            ? serviceTypes.data?.find((s) => s.name === selectedService)?.id || null
            : null;
        const capabilityIds =
          selectedCapability !== "All"
            ? [capabilities.data?.find((c) => c.name === selectedCapability)?.id].filter(Boolean) as string[]
            : [];
        const industryIds =
          selectedIndustry !== "All"
            ? [industries.data?.find((i) => i.name === selectedIndustry)?.id].filter(Boolean) as string[]
            : [];
        const coreServiceIds =
          selectedCoreService !== "All"
            ? [serviceSubTypes.data?.find((s) => s.name === selectedCoreService)?.id].filter(Boolean) as string[]
            : [];

        const response = await callApi({
          data: {
            fn: "putApiCompaniesPartnerDirectory",
            args: {
              body: {
                pageNo: page,
                pageSize,
                orderBy: "name",
                searchTerm: nameFilter.trim() || null,
                serviceType: serviceTypeId,
                regions: regionIds,
                countries: countryIds,
                capabilities: capabilityIds,
                industries: industryIds,
                coreServices: coreServiceIds,
              },
            },
          },
        });

        const transformed = (response?.data || []).map((company: any) => {
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
            serviceSubTypes: company.serviceSubTypes?.map((s: any) => s.name) || [],
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

        setPartners(transformed);
        setPagination({
          pageNumber: response?.pageNumber || 1,
          pageCount: response?.pageCount || 0,
          totalCount: response?.totalCount || 0,
        });
      } catch (error) {
        console.error("Directory search failed", error);
      } finally {
        setIsLoading(false);
      }
    },
    [
      pageNumber,
      pageSize,
      nameFilter,
      selectedRegion,
      selectedCountry,
      selectedService,
      selectedCapability,
      selectedIndustry,
      selectedCoreService,
      regions.data,
      countries.data,
      serviceTypes.data,
      serviceSubTypes.data,
      capabilities.data,
      industries.data,
    ]
  );

  // Fetch on mount and when filters change
  useEffect(() => {
    if (regions.data && countries.data) {
      fetchPartners(1);
      setPageNumber(1);
    }
  }, [
    selectedRegion,
    selectedCountry,
    selectedService,
    selectedCapability,
    selectedIndustry,
    selectedCoreService,
    regions.data,
    countries.data,
  ]);

  const handlePageChange = (page: number) => {
    if (page < 1 || page > pageCount) return;
    setPageNumber(page);
    fetchPartners(page);
  };

  const handleSearch = () => {
    setPageNumber(1);
    fetchPartners(1);
  };

  const renderPaginationItems = () => {
    const items: React.ReactNode[] = [];
    const maxVisiblePages = 5;
    let startPage = Math.max(1, pageNumber - Math.floor(maxVisiblePages / 2));
    const endPage = Math.min(pageCount, startPage + maxVisiblePages - 1);

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1);
    }

    if (startPage > 1) {
      items.push(
        <PaginationItem key={1}>
          <PaginationLink
            onClick={() => handlePageChange(1)}
            isActive={pageNumber === 1}
            className="cursor-pointer"
          >
            1
          </PaginationLink>
        </PaginationItem>
      );
      if (startPage > 2) {
        items.push(
          <PaginationItem key="ellipsis-start">
            <PaginationEllipsis />
          </PaginationItem>
        );
      }
    }

    for (let i = startPage; i <= endPage; i++) {
      if (i === 1 && startPage > 1) continue;
      items.push(
        <PaginationItem key={i}>
          <PaginationLink
            onClick={() => handlePageChange(i)}
            isActive={pageNumber === i}
            className="cursor-pointer"
          >
            {i}
          </PaginationLink>
        </PaginationItem>
      );
    }

    if (endPage < pageCount) {
      if (endPage < pageCount - 1) {
        items.push(
          <PaginationItem key="ellipsis-end">
            <PaginationEllipsis />
          </PaginationItem>
        );
      }
      items.push(
        <PaginationItem key={pageCount}>
          <PaginationLink
            onClick={() => handlePageChange(pageCount)}
            isActive={pageNumber === pageCount}
            className="cursor-pointer"
          >
            {pageCount}
          </PaginationLink>
        </PaginationItem>
      );
    }

    return items;
  };

  const clearAllFilters = () => {
    clearAllDirectoryFilters();
  };

  const activeFiltersCount =
    (selectedRegion !== "All" ? 1 : 0) +
    (selectedCountry !== "All" ? 1 : 0) +
    (selectedService !== "All" ? 1 : 0) +
    (selectedCoreService !== "All" ? 1 : 0) +
    (selectedIndustry !== "All" ? 1 : 0) +
    (selectedCapability !== "All" ? 1 : 0) +
    (nameFilter ? 1 : 0);

  const allRegions = regions?.data?.map((r) => r.name).sort();
  const allServiceTypes = serviceTypes?.data?.map((s) => s.name).sort();
  const allCoreServices = serviceSubTypes?.data?.map((s) => s.name).sort();
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
                    className="w-full border border-gray-200 bg-gray-50 px-3 py-2 pr-8 text-xs outline-none focus:border-black"
                    onChange={(e) => setNameFilter(e.target.value)}
                    onKeyDown={(e) => {
                      if (e.key === "Enter") {
                        handleSearch();
                      }
                    }}
                    placeholder="Enter keywords..."
                    type="text"
                    value={nameFilter}
                  />
                  <button
                    className="absolute top-1.5 right-1.5 p-1 hover:bg-gray-200 rounded"
                    onClick={handleSearch}
                    type="button"
                  >
                    <Search className="h-3 w-3 text-gray-400" />
                  </button>
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

              {/* Service Specialization */}
              <div>
                <label className="mb-2 block font-bold text-[9px] text-gray-400 uppercase tracking-widest">
                  Service Specialization
                </label>
                <select
                  className="w-full appearance-none border border-gray-200 bg-gray-50 px-3 py-2 text-xs outline-none focus:border-black"
                  onChange={(e) => setSelectedCoreService(e.target.value)}
                  value={selectedCoreService}
                >
                  <option value="All">All Service Specializations</option>
                  {serviceSubTypes.data
                    ?.filter(
                      (s: any) =>
                        selectedService === "All" ||
                        serviceTypes.data?.find((st: any) => st.id === s.serviceTypeId)
                          ?.name === selectedService
                    )
                    .map((s: any) => (
                      <option key={s.name} value={s.name}>
                        {s.name}
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
                  Core Service Offerings
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
              {isLoading ? (
                <span className="flex items-center gap-2">
                  <Loader2 className="h-3 w-3 animate-spin" /> Loading...
                </span>
              ) : (
                <>
                  Showing {totalCount} {totalCount === 1 ? "Firm" : "Firms"}
                </>
              )}
            </span>
          </div>

          {partners.length > 0 ? (
            <div className="grid grid-cols-1 gap-4">
              {partners.map((partner: any) => {
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
                      {partner.serviceSubTypes?.length > 0 && (
                        <div className="mt-2 flex flex-wrap gap-1.5">
                          {partner.serviceSubTypes.slice(0, 3).map((sst: string) => (
                            <span
                              className="border border-red-600/30 bg-red-50 px-2 py-0.5 font-bold text-[8px] text-red-600 uppercase tracking-tighter"
                              key={sst}
                            >
                              {sst}
                            </span>
                          ))}
                          {partner.serviceSubTypes.length > 3 && (
                            <span className="self-center text-[8px] text-red-300">
                              +{partner.serviceSubTypes.length - 3}
                            </span>
                          )}
                        </div>
                      )}
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
                {isLoading ? (
                  <Loader2 className="h-8 w-8 animate-spin text-gray-300" />
                ) : (
                  <Users className="h-8 w-8 text-gray-300" />
                )}
              </div>
              <h3 className="mb-2 font-serif text-black text-xl">
                {isLoading ? "Loading..." : "No Matching Firms"}
              </h3>
              {!isLoading && (
                <>
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
                </>
              )}
            </div>
          )}

          {/* Pagination */}
          {pageCount > 1 && (
            <div className="mt-8 flex items-center justify-between border-gray-200 border-t pt-6">
              <span className="font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                Page {pageNumber} of {pageCount}
              </span>
              <Pagination>
                <PaginationContent>
                  <PaginationItem>
                    <PaginationPrevious
                      onClick={() => handlePageChange(pageNumber - 1)}
                      className={pageNumber <= 1 ? "pointer-events-none opacity-50" : "cursor-pointer"}
                    />
                  </PaginationItem>
                  {renderPaginationItems()}
                  <PaginationItem>
                    <PaginationNext
                      onClick={() => handlePageChange(pageNumber + 1)}
                      className={pageNumber >= pageCount ? "pointer-events-none opacity-50" : "cursor-pointer"}
                    />
                  </PaginationItem>
                </PaginationContent>
              </Pagination>
            </div>
          )}
        </div>
      </div>
    </div>
  );
}
