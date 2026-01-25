import { createFileRoute, Link } from "@tanstack/react-router";
import { callApi } from "@/server/proxy";
import { Button } from "@/components/ui/button";
import { Badge } from "@/components/ui/badge";
import {
  Building,
  Calendar,
  MapPin,
  Target,
  Briefcase,
} from "lucide-react";
import { OpportunityViewDto } from "@/api";
import { OpportunityMessages } from "@/components/OpportunityMessages";

export const Route = createFileRoute('/_app/opportunities/$opportunityId/')({
  component: ViewOpportunity,
  loader: async ({ context, params }) => {
    const [opportunity, companies] = await Promise.all([
      //callApi({ data: { fn: 'getApiOpportunitiesByIdView', args: { path: { id: params.opportunityId } } } }),
      context.queryClient.ensureQueryData({
        queryKey: ['opportunity', params.opportunityId],
        queryFn: () => callApi({
          data: {
            fn: 'getApiOpportunitiesByIdView',
            args: {
              path: { id: params.opportunityId }
            }
          }
        }),
      }),
      callApi({ data: { fn: 'getApiCompaniesMe' } }),
    ]);

    return {
      opportunity: opportunity as OpportunityViewDto || {},
      companies: companies || [],
    };
  },
});

function ViewOpportunity() {
  const {
    opportunity,
    companies
  } = Route.useLoaderData();

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="bg-white border-b border-gray-200">
        <div className="max-w-7xl mx-auto px-6 py-8">
          <div className="flex justify-between items-start mb-6">
            <div className="flex gap-3">
              <Badge className="bg-gray-100 text-gray-800 text-xs font-medium px-3 py-1 rounded-none border-0">
                TENDER
              </Badge>
              <Badge className="bg-transparent text-gray-500 text-xs font-medium px-3 py-1 rounded-none border border-gray-300">
                OPEN
              </Badge>
            </div>
            <div className="flex gap-3">
              <Button className="bg-red-600 hover:bg-red-700 text-white px-6 py-2 text-sm font-medium rounded-none">
                CONNECT
              </Button>
              <Button
                variant="outline"
                className="border-gray-300 text-gray-700 px-6 py-2 text-sm font-medium rounded-none hover:bg-gray-50"
              >
                FAVOURITE
              </Button>
            </div>
          </div>

          <h1 className="text-4xl font-serif text-black mb-4 leading-tight">{opportunity.title}</h1>

          <div className="flex items-center gap-6 text-sm text-gray-500">
            <div className="flex items-center gap-2">
              <MapPin className="w-4 h-4" />
              <span className="uppercase tracking-wide">{opportunity.country}</span>
            </div>
            {opportunity.deadline && (
              <div className="flex items-center gap-2">
                <Calendar className="w-4 h-4" />
                <span className="uppercase tracking-wide">
                  DEADLINE: {new Date(opportunity.deadline).toLocaleDateString('en-GB', {
                    year: 'numeric',
                    month: '2-digit',
                    day: '2-digit'
                  })}
                </span>
              </div>
            )}
            <div className="flex items-center gap-2">
              <span className="uppercase tracking-wide">POSTED {Math.floor((new Date().getTime() - new Date(opportunity.createdDate || '').getTime()) / (1000 * 60 * 60 * 24))} DAYS AGO</span>
            </div>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="max-w-7xl mx-auto py-8">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Left Column: Project Details */}
          <div className="lg:col-span-2">
            <div className="bg-white rounded-none border border-gray-200">
              <div className="p-6">
                <h2 className="text-xl font-semibold text-gray-900 mb-6">Project Details</h2>

                {opportunity.fullDescription && (
                  <div className="prose prose-gray max-w-none">
                    <div className="text-gray-700 leading-relaxed whitespace-pre-wrap space-y-4">
                      {opportunity.fullDescription.split('\n\n').map((paragraph, index) => (
                        <p key={index} className="text-sm leading-relaxed">
                          {paragraph}
                        </p>
                      ))}
                    </div>
                  </div>
                )}
              </div>
            </div>
            {/* Messages Section */}
            <div className="mt-8">
              <OpportunityMessages
                opportunityId={opportunity.id!}
                canAddMessage={true}
              />
            </div>
          </div>

          {/* Right Column: Company Profile & Requirements */}
          <div className="space-y-6">

            {/* Company Profile */}
            <div className="bg-white border border-gray-200 rounded-none">
              <div className="p-6">
                <div className="text-xs text-gray-500 uppercase tracking-wide mb-2">POSTED BY</div>
                <div className="flex items-start gap-3 mb-4">
                  <div className="w-10 h-10 bg-gray-100 rounded-none flex items-center justify-center">
                    <Building className="w-5 h-5 text-gray-600" />
                  </div>
                  <div>
                    <h3 className="font-semibold text-gray-900 text-lg">{opportunity.companyName}</h3>
                    <div className="flex items-center gap-2 mt-1">
                      <Briefcase className="w-2.5 h-2.5 text-gray-600" />
                      <span className="text-xs text-gray-600 uppercase tracking-wide">{opportunity.companyServiceType}</span>
                    </div>
                    <Link
                      to="/partners/$id"
                      params={{ id: opportunity.companyId! }}
                      search={{ from: "/opportunities" }}
                      className="border-red-500 text-red-500 rounded-none text-xs font-medium py-2"
                    >
                      VIEW PARTNER PROFILE
                    </Link>
                  </div>
                </div>
              </div>
            </div>

            {/* Requirements */}
            <div className="bg-white border border-gray-200 rounded-none">
              <div className="p-6">
                <div className="text-xs text-gray-500 uppercase tracking-wide mb-4">REQUIREMENTS</div>

                {/* Service Types */}
                {opportunity.serviceTypes && opportunity.serviceTypes.length > 0 && (
                  <section className="space-y-4">
                    <h3 className="text-xs text-gray-500 uppercase font-bold tracking-widest flex items-center gap-2">
                      <Briefcase className="w-4 h-4" />
                      Service Types
                    </h3>
                    <div className="flex flex-wrap gap-2">
                      {opportunity.serviceTypes.map((service, index) => (
                        <Badge
                          key={index}
                          variant="secondary"
                          className="bg-black text-white border-black px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border rounded-none"
                        >
                          {service.name}
                        </Badge>
                      ))}
                    </div>
                  </section>
                )}

                {/* Capabilities */}
                {opportunity.capabilities && opportunity.capabilities.length > 0 && (
                  <section className="space-y-4 py-2">
                    <h3 className="text-xs text-gray-500 uppercase font-bold tracking-widest flex items-center gap-2">
                      <Target className="w-4 h-4" />
                      Required Capabilities
                    </h3>
                    <div className="flex flex-wrap gap-2">
                      {opportunity.capabilities.map((capability, index: number) => (
                        <Badge
                          key={index}
                          variant="secondary"
                          className="bg-red-600 text-white border-red-600 px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border rounded-none"
                        >
                          {capability.name}
                        </Badge>
                      ))}
                    </div>
                  </section>
                )}

                {/* Industries */}
                {opportunity.industries && opportunity.industries.length > 0 && (
                  <section className="space-y-4 py-2">
                    <h3 className="text-xs text-gray-500 uppercase font-bold tracking-widest flex items-center gap-2">
                      <Building className="w-4 h-4" />
                      Target Industries
                    </h3>
                    <div className="flex flex-wrap gap-2">
                      {opportunity.industries.map((industry, index: number) => (
                        <Badge
                          key={index}
                          variant="secondary"
                          className="bg-blue-600 text-white border-blue-600 px-3 py-1.5 text-[10px] uppercase font-bold tracking-wider border rounded-none"
                        >
                          {industry.name}
                        </Badge>
                      ))}
                    </div>
                  </section>
                )}
              </div>
            </div>
          </div>
        </div>

      </div>
    </div>
  );
}
