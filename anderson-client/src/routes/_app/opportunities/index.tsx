import { useQuery } from "@tanstack/react-query";
import { createFileRoute, Link } from "@tanstack/react-router";
import {
  ArrowRight,
  Building,
  Calendar,
  ChevronDown,
  ChevronUp,
  Loader2,
  MapPin,
} from "lucide-react";
import { useState } from "react";
import { ConnectRequestDialog } from "@/components/ConnectRequestDialog";
import { OpportunityMessages } from "@/components/OpportunityMessages";
import { Button } from "@/components/ui/button";
import {
  Collapsible,
  CollapsibleContent,
  CollapsibleTrigger,
} from "@/components/ui/collapsible";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/_app/opportunities/")({
  component: Opportunities,
});

function Opportunities() {
  const [activeTab, setActiveTab] = useState<"all" | "me" | "saved">("all");
  const [expandedOpportunities, setExpandedOpportunities] = useState<
    Set<string>
  >(new Set());

  const { data: allOpportunities, isLoading: isLoadingAll } = useQuery({
    queryKey: ["opportunities", "all"],
    queryFn: async () => {
      const res = await callApi({ data: { fn: "getApiOpportunities" } });
      if (res.error) {
        throw res.error;
      }
      return res || [];
    },
  });

  const { data: myOpportunities, isLoading: isLoadingMe } = useQuery({
    queryKey: ["opportunities", "me"],
    queryFn: async () => {
      const res = await callApi({ data: { fn: "getApiOpportunitiesMe" } });
      if (res.error) {
        throw res.error;
      }
      return res || [];
    },
  });

  const { data: savedOpportunities, isLoading: isLoadingSaved } = useQuery({
    queryKey: ["opportunities", "saved"],
    queryFn: async () => {
      const res = await callApi({ data: { fn: "getApiOpportunitiesSaved" } });
      if (res.error) {
        throw res.error;
      }
      return res || [];
    },
  });

  const currentOpportunities =
    activeTab === "all"
      ? allOpportunities
      : activeTab === "me"
        ? myOpportunities
        : savedOpportunities;

  const isLoading =
    activeTab === "all"
      ? isLoadingAll
      : activeTab === "me"
        ? isLoadingMe
        : isLoadingSaved;

  const toggleExpanded = (opportunityId: string) => {
    setExpandedOpportunities((prev) => {
      const newSet = new Set(prev);
      if (newSet.has(opportunityId)) {
        newSet.delete(opportunityId);
      } else {
        newSet.add(opportunityId);
      }
      return newSet;
    });
  };

  return (
    <div className="animate-fade-in space-y-8 text-left">
      <header className="flex items-end justify-between border-gray-200 border-b pb-6">
        <div>
          <h2 className="mb-3 text-left font-serif text-4xl text-black">
            Collaboration Hub
          </h2>
          <p className="text-left font-light text-gray-500 text-lg">
            Active tenders, joint ventures, and resource requests across the
            network.
          </p>
        </div>
        <Link
          className="rounded-none bg-black px-8 py-3 font-bold text-white text-xs uppercase tracking-[0.15em] transition-colors hover:bg-gray-800"
          to="/opportunities/new"
        >
          + Post Opportunity
        </Link>
      </header>

      {/* Tabs */}
      <div className="flex gap-8 border-gray-200 border-b">
        <button
          className={`pb-4 font-bold text-sm uppercase tracking-widest transition-all ${activeTab === "all"
            ? "border-black border-b-2 text-black"
            : "border-transparent text-gray-400 hover:border-gray-200 hover:text-gray-600"
            }`}
          onClick={() => setActiveTab("all")}
          type="button"
        >
          Opportunities
        </button>
        <button
          className={`pb-4 font-bold text-sm uppercase tracking-widest transition-all ${activeTab === "me"
            ? "border-black border-b-2 text-black"
            : "border-transparent text-gray-400 hover:border-gray-200 hover:text-gray-600"
            }`}
          onClick={() => setActiveTab("me")}
          type="button"
        >
          My Opportunities
        </button>
        <button
          className={`pb-4 font-bold text-sm uppercase tracking-widest transition-all ${activeTab === "saved"
            ? "border-black border-b-2 text-black"
            : "border-transparent text-gray-400 hover:border-gray-200 hover:text-gray-600"
            }`}
          onClick={() => setActiveTab("saved")}
          type="button"
        >
          Saved Opportunities
        </button>
      </div>

      <div className="grid grid-cols-1 gap-8">
        {isLoading ? (
          <div className="flex justify-center py-20">
            <Loader2 className="h-8 w-8 animate-spin text-gray-300" />
          </div>
        ) : currentOpportunities?.length === 0 ? (
          <div className="py-20 text-center font-light text-gray-400 italic">
            No opportunities found in this category.
          </div>
        ) : (
          currentOpportunities?.map((opp: any) => (
            <Collapsible
              key={opp.id}
              onOpenChange={() => toggleExpanded(opp.id)}
              open={expandedOpportunities.has(opp.id)}
            >
              <div className="group overflow-hidden border border-gray-200 bg-white p-0 text-left shadow-sm transition-all duration-300 hover:border-red-600">
                <div className="p-8 md:p-10">
                  <div className="mb-6 flex flex-col justify-between gap-4 md:flex-row md:items-center">
                    <div className="flex items-center gap-4">
                      <span
                        className={`border px-3 py-1 font-bold text-[10px] uppercase tracking-widest ${opp.opportunityTypeString === "Tender"
                          ? "border-gray-900 text-gray-900"
                          : opp.opportunityTypeString === "Joint Venture"
                            ? "border-red-600 text-red-600"
                            : "border-gray-500 text-gray-500"
                          }`}
                      >
                        {opp.opportunityTypeString || "Opportunity"}
                      </span>
                      {opp.country && (
                        <span className="flex items-center font-medium text-gray-500 text-xs uppercase tracking-wide">
                          <MapPin className="mr-1 h-3 w-3" /> {opp.country}
                        </span>
                      )}
                      {opp.deadline && (
                        <span className="flex items-center font-medium text-gray-500 text-xs uppercase tracking-wide">
                          <Calendar className="mr-1 h-3 w-3" /> Deadline:{" "}
                          {new Date(opp.deadline).toLocaleDateString()}
                        </span>
                      )}
                    </div>
                    <span className="flex items-center gap-2 font-bold text-gray-400 text-xs uppercase tracking-widest">
                      <Building className="h-3 w-3" />
                      {opp.companyName || opp.companyId || "Unknown Company"}
                    </span>
                  </div>

                  <Link
                    className="group block"
                    params={{ opportunityId: opp.id! }}
                    to="/opportunities/$opportunityId"
                  >
                    <h3 className="mb-4 cursor-pointer text-left font-serif text-3xl text-black transition-colors group-hover:text-red-600">
                      {opp.title}
                    </h3>
                  </Link>

                  <p className="mb-8 max-w-4xl border-gray-100 border-l-2 pl-4 text-left font-light text-gray-600 text-lg leading-relaxed">
                    {opp.shortDescription}
                  </p>

                  {opp.capabilities && opp.capabilities.length > 0 && (
                    <div className="mb-8 flex flex-wrap items-center gap-3">
                      <span className="mr-2 font-bold text-gray-900 text-xs uppercase tracking-wider">
                        Required Capabilities:
                      </span>
                      {opp.capabilities.map((skill: any, index: number) => (
                        <span
                          className="flex items-center border border-gray-200 bg-gray-50 px-3 py-1 font-medium text-gray-600 text-xs uppercase tracking-wide"
                          key={index}
                        >
                          {typeof skill === "string" ? skill : skill.name}
                        </span>
                      ))}
                    </div>
                  )}

                  <div className="flex items-center justify-between border-gray-100 border-t pt-8">
                    <div className="flex items-center">
                      <div className="flex -space-x-3">
                        <div className="flex h-8 w-8 items-center justify-center rounded-full border-2 border-white bg-black font-bold text-[10px] text-white">
                          +{opp.interestedPartners?.length || 0}
                        </div>
                      </div>
                      <span className="ml-3 self-center font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                        {opp.interestedPartners?.length || 0} Partners
                        interested
                      </span>
                    </div>

                    <div>
                      {activeTab === "me" ? (
                        <div className="flex items-center gap-4">
                          <Link
                            className="flex items-center font-bold text-red-600 text-xs uppercase tracking-[0.15em] transition-colors hover:text-red-800"
                            params={{ opportunityId: opp.id! }}
                            to="/opportunities/$opportunityId/edit"
                          >
                            Edit Opportunity{" "}
                            <ArrowRight className="ml-2 h-4 w-4" />
                          </Link>
                          <CollapsibleTrigger asChild>
                            <Button
                              className="border-gray-300 font-bold text-xs uppercase tracking-[0.15em] hover:border-gray-400"
                              size="sm"
                              variant="outline"
                            >
                              {expandedOpportunities.has(opp.id) ? (
                                <>
                                  Hide Messages{" "}
                                  <ChevronUp className="ml-2 h-4 w-4" />
                                </>
                              ) : (
                                <>
                                  Show Messages{" "}
                                  <ChevronDown className="ml-2 h-4 w-4" />
                                </>
                              )}
                            </Button>
                          </CollapsibleTrigger>
                        </div>
                      ) : (
                        <div className="flex items-center gap-6">
                          {opp.companyId ? (
                            <ConnectRequestDialog
                              partnerId={String(opp.companyId)}
                              partnerName={opp.companyName}
                            >
                              <button
                                className="font-bold text-black text-xs uppercase tracking-[0.15em] transition-colors hover:text-red-600"
                                onClick={(e) => {
                                  e.stopPropagation();
                                }}
                                type="button"
                              >
                                Connect
                              </button>
                            </ConnectRequestDialog>
                          ) : (
                            <button
                              className="font-bold text-black text-xs uppercase tracking-[0.15em] transition-colors hover:text-red-600"
                              onClick={(e) => {
                                e.stopPropagation();
                              }}
                              type="button"
                            >
                              Connect
                            </button>
                          )}
                          <Link
                            className="flex items-center font-bold text-red-600 text-xs uppercase tracking-[0.15em] transition-colors hover:text-red-800"
                            params={{ opportunityId: opp.id! }}
                            to="/opportunities/$opportunityId"
                          >
                            View Details <ArrowRight className="ml-2 h-4 w-4" />
                          </Link>
                        </div>
                      )}
                    </div>
                  </div>
                </div>

                {/* Collapsible Messages Section - Only for 'me' tab */}
                {activeTab === "me" && (
                  <CollapsibleContent>
                    <div className="mt-0 border-gray-200 border-t">
                      <OpportunityMessages
                        canAddMessage={true}
                        isOwnOpportunity={true}
                        opportunityId={opp.id!}
                      />
                    </div>
                  </CollapsibleContent>
                )}
              </div>
            </Collapsible>
          ))
        )}
      </div>
    </div>
  );
}
