import { createFileRoute } from "@tanstack/react-router";
import { Briefcase, Calendar, MapPin, ArrowRight, X } from "lucide-react";
import React, { useState } from "react";
import { MOCK_OPPORTUNITIES } from "../../services/mock/data";

export const Route = createFileRoute("/_app/opportunities")({
  component: Opportunities,
});

function Opportunities() {
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [opportunities, setOpportunities] = useState(MOCK_OPPORTUNITIES);

  // Simple mock add
  const handleAddOpportunity = (data: any) => {
    // In a real app, this would be a mutation
    // For now, just close modal
    setIsModalOpen(false);
  };

  return (
    <div className="space-y-10 animate-fade-in">
      <header className="flex justify-between items-end border-b border-gray-200 pb-6">
        <div>
          <h2 className="text-4xl font-serif text-black mb-3">Collaboration Hub</h2>
          <p className="text-gray-500 font-light text-lg">
            Active tenders, joint ventures, and resource requests across the network.
          </p>
        </div>
        <button
          onClick={() => setIsModalOpen(true)}
          className="bg-black text-white px-8 py-3 text-xs font-bold uppercase tracking-[0.15em] hover:bg-gray-800 transition-colors rounded-none"
        >
          + Post Opportunity
        </button>
      </header>

      {isModalOpen && (
          <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/50 backdrop-blur-sm p-4">
              <div className="bg-white p-8 w-full max-w-2xl relative shadow-2xl">
                  <button onClick={() => setIsModalOpen(false)} className="absolute top-4 right-4 text-gray-400 hover:text-black">
                      <X className="w-5 h-5"/>
                  </button>
                  <h3 className="text-2xl font-serif text-black mb-6">Post New Opportunity</h3>
                  <div className="p-12 text-center bg-gray-50 border border-dashed border-gray-300">
                      <p className="text-gray-500">Form implementation pending structure definition.</p>
                      <p className="text-xs text-gray-400 mt-2">This feature will be connected to the new API endpoint.</p>
                  </div>
              </div>
          </div>
      )}

      <div className="grid grid-cols-1 gap-8">
        {opportunities.map((opp) => (
          <div
            key={opp.id}
            className="bg-white border border-gray-200 hover:border-red-600 transition-all duration-300 group p-0 overflow-hidden shadow-sm"
          >
            <div className="p-8 md:p-10">
              <div className="flex flex-col md:flex-row md:items-center justify-between gap-4 mb-6">
                <div className="flex items-center gap-4">
                  <span
                    className={`px-3 py-1 text-[10px] font-bold uppercase tracking-widest border ${
                      opp.type === "Tender"
                        ? "border-gray-900 text-gray-900"
                        : opp.type === "Joint Venture"
                        ? "border-red-600 text-red-600"
                        : "border-gray-500 text-gray-500"
                    }`}
                  >
                    {opp.type}
                  </span>
                  <span className="text-xs text-gray-500 flex items-center font-medium uppercase tracking-wide">
                    <MapPin className="w-3 h-3 mr-1" /> {opp.region}
                  </span>
                  <span className="text-xs text-gray-500 flex items-center font-medium uppercase tracking-wide">
                    <Calendar className="w-3 h-3 mr-1" /> Deadline: {opp.deadline}
                  </span>
                </div>
                <span className="text-xs font-bold text-gray-400 uppercase tracking-widest">
                  Posted by {opp.postedBy}
                </span>
              </div>

              <h3
                className="text-3xl font-serif text-black mb-4 group-hover:text-red-600 transition-colors cursor-pointer"
                onClick={() => {}}
              >
                {opp.title}
              </h3>

              <p className="text-gray-600 leading-relaxed mb-8 max-w-4xl font-light text-lg border-l-2 border-gray-100 pl-4">
                {opp.description}
              </p>

              <div className="flex flex-wrap items-center gap-3 mb-8">
                <span className="text-xs font-bold text-gray-900 mr-2 uppercase tracking-wider">
                  Required Capabilities:
                </span>
                {opp.requiredSkills.map((skill, index) => (
                  <span
                    key={index}
                    className="bg-gray-50 text-gray-600 px-3 py-1 text-xs font-medium border border-gray-200 flex items-center uppercase tracking-wide"
                  >
                    {skill}
                  </span>
                ))}
              </div>

              <div className="pt-8 border-t border-gray-100 flex items-center justify-between">
                <div className="flex items-center">
                  <div className="flex -space-x-3">
                    {/* Visual representation of interested partners */}
                    <div className="w-10 h-10 rounded-full bg-gray-200 border-2 border-white flex items-center justify-center text-[10px] text-gray-400 font-bold uppercase overflow-hidden">
                      <div className="bg-gray-300 w-full h-full flex items-center justify-center">
                        A
                      </div>
                    </div>
                    <div className="w-10 h-10 rounded-full bg-gray-300 border-2 border-white flex items-center justify-center text-[10px] text-gray-400 font-bold uppercase overflow-hidden">
                      <div className="bg-gray-400 w-full h-full flex items-center justify-center">
                        B
                      </div>
                    </div>
                    <div className="w-10 h-10 rounded-full bg-black text-white border-2 border-white flex items-center justify-center text-xs font-bold">
                      +{opp.interestedPartnerIds?.length || 0}
                    </div>
                  </div>
                  <span className="ml-5 text-[10px] text-gray-400 self-center font-bold uppercase tracking-widest">
                    {opp.interestedPartnerIds?.length || 0} Partners interested
                  </span>
                </div>

                <div className="flex gap-6 items-center">
                  <button
                    onClick={(e) => {
                      e.stopPropagation();
                    }}
                    className="text-black hover:text-red-600 font-bold text-xs uppercase tracking-[0.15em] transition-colors"
                  >
                    Connect
                  </button>
                  <button
                    onClick={() => {}}
                    className="flex items-center text-red-600 font-bold text-xs uppercase tracking-[0.15em] hover:text-red-800 transition-colors"
                  >
                    View Details <ArrowRight className="ml-2 w-4 h-4" />
                  </button>
                </div>
              </div>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
