import { createFileRoute, Link } from "@tanstack/react-router";
import { useQuery } from "@tanstack/react-query";
import { Calendar, MapPin, ArrowRight, Loader2, Building } from "lucide-react";
import { useState } from "react";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/_app/opportunities/")({ 
  component: Opportunities,
});

function Opportunities() {
  const [activeTab, setActiveTab] = useState<'all' | 'me' | 'saved'>('all');

  const { data: allOpportunities, isLoading: isLoadingAll } = useQuery({
    queryKey: ['opportunities', 'all'],
    queryFn: async () => {
        const res = await callApi({data:{fn: 'getApiOpportunities'}});
        if (res.error) throw res.error;
        return res || [];
    }
  });

  const { data: myOpportunities, isLoading: isLoadingMe } = useQuery({
    queryKey: ['opportunities', 'me'],
    queryFn: async () => {
        const res = await callApi({data:{fn: 'getApiOpportunitiesMe'}});
        if (res.error) throw res.error;
        return res || [];
    }
  });

  const { data: savedOpportunities, isLoading: isLoadingSaved } = useQuery({
    queryKey: ['opportunities', 'saved'],
    queryFn: async () => {
        const res = await callApi({data:{fn: 'getApiOpportunitiesSaved'}});
        if (res.error) throw res.error;
        return res || [];
    }
  });

  const currentOpportunities = activeTab === 'all' ? allOpportunities 
    : activeTab === 'me' ? myOpportunities 
    : savedOpportunities;

  const isLoading = activeTab === 'all' ? isLoadingAll 
    : activeTab === 'me' ? isLoadingMe 
    : isLoadingSaved;

  return (
    <div className="space-y-8 animate-fade-in text-left">
      <header className="flex justify-between items-end border-b border-gray-200 pb-6">
        <div>
          <h2 className="text-4xl font-serif text-black mb-3 text-left">Collaboration Hub</h2>
          <p className="text-gray-500 font-light text-lg text-left">
            Active tenders, joint ventures, and resource requests across the network.
          </p>
        </div>
        <Link
          to="/opportunities/new"
          className="bg-black text-white px-8 py-3 text-xs font-bold uppercase tracking-[0.15em] hover:bg-gray-800 transition-colors rounded-none"
        >
          + Post Opportunity
        </Link>
      </header>

      {/* Tabs */}
      <div className="flex gap-8 border-b border-gray-200">
        <button 
            onClick={() => setActiveTab('all')}
            className={`pb-4 text-sm font-bold uppercase tracking-widest transition-all ${
                activeTab === 'all' ? 'border-b-2 border-black text-black' : 'text-gray-400 hover:text-gray-600 border-transparent hover:border-gray-200'
            }`}
        >
            Opportunities
        </button>
        <button 
            onClick={() => setActiveTab('me')}
            className={`pb-4 text-sm font-bold uppercase tracking-widest transition-all ${
                activeTab === 'me' ? 'border-b-2 border-black text-black' : 'text-gray-400 hover:text-gray-600 border-transparent hover:border-gray-200'
            }`}
        >
            My Opportunities
        </button>
        <button 
            onClick={() => setActiveTab('saved')}
            className={`pb-4 text-sm font-bold uppercase tracking-widest transition-all ${
                activeTab === 'saved' ? 'border-b-2 border-black text-black' : 'text-gray-400 hover:text-gray-600 border-transparent hover:border-gray-200'
            }`}
        >
            Saved Opportunities
        </button>
      </div>

      <div className="grid grid-cols-1 gap-8">
        {isLoading ? (
            <div className="flex justify-center py-20">
                <Loader2 className="w-8 h-8 animate-spin text-gray-300" />
            </div>
        ) : currentOpportunities?.length === 0 ? (
            <div className="py-20 text-center text-gray-400 font-light italic">
                No opportunities found in this category.
            </div>
        ) : (
            currentOpportunities?.map((opp: any) => (
            <div
                key={opp.id}
                className="bg-white border border-gray-200 hover:border-red-600 transition-all duration-300 group p-0 overflow-hidden shadow-sm text-left"
            >
                <div className="p-8 md:p-10">
                <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-6">
                    <div className="flex items-center gap-4">
                    <span
                        className={`px-3 py-1 text-[10px] font-bold uppercase tracking-widest border ${
                        opp.opportunityTypeString === "Tender"
                            ? "border-gray-900 text-gray-900"
                            : opp.opportunityTypeString === "Joint Venture"
                            ? "border-red-600 text-red-600"
                            : "border-gray-500 text-gray-500"
                        }`}
                    >
                        {opp.opportunityTypeString || "Opportunity"}
                    </span>
                    {opp.country && (
                        <span className="text-xs text-gray-500 flex items-center font-medium uppercase tracking-wide">
                            <MapPin className="w-3 h-3 mr-1" /> {opp.country}
                        </span>
                    )}
                    {opp.deadline && (
                        <span className="text-xs text-gray-500 flex items-center font-medium uppercase tracking-wide">
                            <Calendar className="w-3 h-3 mr-1" /> Deadline: {new Date(opp.deadline).toLocaleDateString()}
                        </span>
                    )}
                    </div>
                    <span className="text-xs font-bold text-gray-400 uppercase tracking-widest flex items-center gap-2">
                         <Building className="w-3 h-3" />
                         {opp.companyName || opp.companyId || "Unknown Company"}
                    </span>
                </div>

                <Link
                    to="/opportunities/$opportunityId"
                    params={{ opportunityId: opp.id! }}
                    className="block group"
                >
                    <h3 className="text-3xl font-serif text-black mb-4 group-hover:text-red-600 transition-colors cursor-pointer text-left">
                        {opp.title}
                    </h3>
                </Link>

                <p className="text-gray-600 leading-relaxed mb-8 max-w-4xl font-light text-lg border-l-2 border-gray-100 pl-4 text-left">
                    {opp.shortDescription}
                </p>

                {opp.capabilities && opp.capabilities.length > 0 && (
                    <div className="flex flex-wrap items-center gap-3 mb-8">
                        <span className="text-xs font-bold text-gray-900 mr-2 uppercase tracking-wider">
                        Required Capabilities:
                        </span>
                        {opp.capabilities.map((skill: any, index: number) => (
                        <span
                            key={index}
                            className="bg-gray-50 text-gray-600 px-3 py-1 text-xs font-medium border border-gray-200 flex items-center uppercase tracking-wide"
                        >
                            {typeof skill === 'string' ? skill : skill.name}
                        </span>
                        ))}
                    </div>
                )}

                <div className="pt-8 border-t border-gray-100 flex items-center justify-between">
                    <div className="flex items-center">
                        <div className="flex -space-x-3">
                            <div className="w-8 h-8 rounded-full bg-black text-white border-2 border-white flex items-center justify-center text-[10px] font-bold">
                                +{opp.interestedPartners?.length || 0}
                            </div>
                        </div>
                        <span className="ml-3 text-[10px] text-gray-400 self-center font-bold uppercase tracking-widest">
                            {opp.interestedPartners?.length || 0} Partners interested
                        </span>
                    </div>

                    <div className="flex gap-6 items-center">
                        {activeTab !== 'me' && (
                            <button
                                onClick={(e) => {
                                e.stopPropagation();
                                }}
                                className="text-black hover:text-red-600 font-bold text-xs uppercase tracking-[0.15em] transition-colors"
                            >
                                Connect
                            </button>
                        )}
                        <Link
                            to="/opportunities/$opportunityId"
                            params={{ opportunityId: opp.id! }}
                            className="flex items-center text-red-600 font-bold text-xs uppercase tracking-[0.15em] hover:text-red-800 transition-colors"
                        >
                            {activeTab === 'me' ? 'Edit Opportunity' : 'View Details'} <ArrowRight className="ml-2 w-4 h-4" />
                        </Link>
                    </div>
                </div>
                </div>
            </div>
            ))
        )}
      </div>
    </div>
  );
}
