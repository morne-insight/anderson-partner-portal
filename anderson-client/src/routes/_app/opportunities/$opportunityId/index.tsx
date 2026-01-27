import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { createFileRoute, Link } from "@tanstack/react-router";
import {
  Bookmark,
  BookmarkMinus,
  Briefcase,
  Building,
  Calendar,
  Loader2,
  MapPin,
  MessageCircle,
  Target,
} from "lucide-react";
import { toast } from "sonner";
import type { CompanyProfileDto, OpportunityViewDto } from "@/api";
import { OpportunityMessages } from "@/components/OpportunityMessages";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/_app/opportunities/$opportunityId/")({
  component: ViewOpportunity,
  loader: async () => {
    const companies = await callApi({ data: { fn: "getApiCompaniesMe" } });

    return {
      companies: (companies as CompanyProfileDto[]) || [],
    };
  },
});

function ViewOpportunity() {
  const { companies } = Route.useLoaderData();

  const { opportunityId } = Route.useParams();
  const queryClient = useQueryClient();

  const { data, isLoading, isError } = useQuery({
    queryKey: ["opportunity", opportunityId],
    queryFn: () =>
      callApi({
        data: {
          fn: "getApiOpportunitiesByIdView",
          args: {
            path: { id: opportunityId },
          },
        },
      }),
  });

  const opportunity = (data as OpportunityViewDto) || {};

  const addInteresedPartner = useMutation({
    mutationFn: async (partnerId: string) => {
      const response = await callApi({
        data: {
          fn: "putApiOpportunitiesByIdAddInterestedPartners",
          args: {
            path: { id: opportunity.id },
            body: { partnerId },
          },
        },
      });
      return response;
    },
    onSuccess: () => {
      console.log("Add interested partner successful");
      queryClient.invalidateQueries({
        queryKey: ["opportunity", opportunity.id],
      });
    },
    onError: (err) => {
      console.error("Failed to add interested partner", err);
      toast.error("Failed to add interested partner. Please try again.");
    },
  });

  const removeInteresedPartner = useMutation({
    mutationFn: async (partnerId: string) => {
      const response = await callApi({
        data: {
          fn: "putApiOpportunitiesByIdRemoveInterestedPartners",
          args: {
            path: { id: opportunity.id },
            body: { partnerId },
          },
        },
      });
      return response;
    },
    onSuccess: () => {
      console.log("Add interested partner successful");
      queryClient.invalidateQueries({
        queryKey: ["opportunity", opportunity.id],
      });
    },
    onError: (err) => {
      console.error("Failed to add interested partner", err);
      toast.error("Failed to add interested partner. Please try again.");
    },
  });

  // Show loading state
  if (isLoading) {
    return (
      <div className="rounded-none border border-gray-200 bg-white">
        <div className="p-6">
          <div className="mb-6 flex items-center gap-2">
            <MessageCircle className="h-5 w-5 text-red-500" />
            <h3 className="font-semibold text-gray-900 text-lg">
              Clarification Q&A
            </h3>
          </div>
          <div className="flex items-center justify-center py-8">
            <Loader2 className="h-6 w-6 animate-spin text-gray-400" />
            <span className="ml-2 text-gray-500 text-sm">
              Loading messages...
            </span>
          </div>
        </div>
      </div>
    );
  }

  // Show error state
  if (isError) {
    return (
      <div className="rounded-none border border-gray-200 bg-white">
        <div className="p-6">
          <div className="mb-6 flex items-center gap-2">
            <MessageCircle className="h-5 w-5 text-red-500" />
            <h3 className="font-semibold text-gray-900 text-lg">Opportunity</h3>
          </div>
          <div className="py-8 text-center text-red-500">
            <p className="text-sm">
              Failed to load opportunity. Please try again.
            </p>
            <Button
              className="mt-2"
              onClick={() =>
                queryClient.invalidateQueries({
                  queryKey: ["opportunity-messages", opportunityId],
                })
              }
              size="sm"
              type="button"
              variant="outline"
            >
              Retry
            </Button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Header */}
      <div className="border-gray-200 border-b bg-white">
        <div className="mx-auto max-w-7xl px-6 py-8">
          <div className="mb-6 flex items-start justify-between">
            <div className="flex gap-3">
              <Badge className="rounded-none border-0 bg-gray-100 px-3 py-1 font-medium text-gray-800 text-xs">
                TENDER
              </Badge>
              <Badge className="rounded-none border border-gray-300 bg-transparent px-3 py-1 font-medium text-gray-500 text-xs">
                OPEN
              </Badge>
            </div>
            <div className="flex gap-3">
              <Button className="rounded-none bg-red-600 px-6 py-2 font-medium text-sm text-white hover:bg-red-700">
                CONNECT
              </Button>
              {companies.length > 0 &&
                (opportunity.interestedPartners?.some((ip) =>
                  companies.some((c) => c.id === ip.id)
                ) ? (
                  <Button
                    className="rounded-none border-gray-300 px-6 py-2 font-medium text-gray-700 text-sm hover:bg-gray-50"
                    disabled={removeInteresedPartner.isPending}
                    onClick={async () => {
                      const matchingCompany = companies.find((c) =>
                        opportunity.interestedPartners?.some(
                          (ip) => ip.id === c.id
                        )
                      );
                      if (matchingCompany) {
                        await removeInteresedPartner.mutate(
                          matchingCompany.id as string
                        );
                      }
                    }}
                    variant="outline"
                  >
                    <BookmarkMinus className="mr-2 h-4 w-4" />
                    REMOVE
                  </Button>
                ) : (
                  <Button
                    className="rounded-none border-gray-300 px-6 py-2 font-medium text-gray-700 text-sm hover:bg-gray-50"
                    disabled={addInteresedPartner.isPending}
                    onClick={async () => {
                      await addInteresedPartner.mutate(
                        companies[0].id as string
                      );
                    }}
                    type="button"
                    variant="outline"
                  >
                    <Bookmark className="mr-2 h-4 w-4" />
                    FAVOURITE
                  </Button>
                ))}
            </div>
          </div>

          <h1 className="mb-4 font-serif text-4xl text-black leading-tight">
            {opportunity.title}
          </h1>

          <div className="flex items-center gap-6 text-gray-500 text-sm">
            <div className="flex items-center gap-2">
              <MapPin className="h-4 w-4" />
              <span className="uppercase tracking-wide">
                {opportunity.country}
              </span>
            </div>
            {opportunity.deadline && (
              <div className="flex items-center gap-2">
                <Calendar className="h-4 w-4" />
                <span className="uppercase tracking-wide">
                  DEADLINE:{" "}
                  {new Date(opportunity.deadline).toLocaleDateString("en-GB", {
                    year: "numeric",
                    month: "2-digit",
                    day: "2-digit",
                  })}
                </span>
              </div>
            )}
            <div className="flex items-center gap-2">
              <span className="uppercase tracking-wide">
                POSTED{" "}
                {Math.floor(
                  (new Date().getTime() -
                    new Date(opportunity.createdDate || "").getTime()) /
                    (1000 * 60 * 60 * 24)
                )}{" "}
                DAYS AGO
              </span>
            </div>
          </div>
        </div>
      </div>

      {/* Main Content */}
      <div className="mx-auto max-w-7xl py-8">
        <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
          {/* Left Column: Project Details */}
          <div className="lg:col-span-2">
            <div className="rounded-none border border-gray-200 bg-white">
              <div className="p-6">
                <h2 className="mb-6 font-semibold text-gray-900 text-xl">
                  Project Details
                </h2>

                {opportunity.fullDescription && (
                  <div className="prose prose-gray max-w-none">
                    <div className="space-y-4 whitespace-pre-wrap text-gray-700 leading-relaxed">
                      {opportunity.fullDescription
                        .split("\n\n")
                        .map((paragraph, index) => (
                          <p className="text-sm leading-relaxed" key={index}>
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
                canAddMessage={true}
                opportunityId={opportunity.id as string}
              />
            </div>
          </div>

          {/* Right Column: Company Profile & Requirements */}
          <div className="space-y-6">
            {/* Company Profile */}
            <div className="rounded-none border border-gray-200 bg-white">
              <div className="p-6">
                <div className="mb-2 text-gray-500 text-xs uppercase tracking-wide">
                  POSTED BY
                </div>
                <div className="mb-4 flex items-start gap-3">
                  <div className="flex h-10 w-10 items-center justify-center rounded-none bg-gray-100">
                    <Building className="h-5 w-5 text-gray-600" />
                  </div>
                  <div>
                    <h3 className="font-semibold text-gray-900 text-lg">
                      {opportunity.companyName}
                    </h3>
                    <div className="mt-1 flex items-center gap-2">
                      <Briefcase className="h-2.5 w-2.5 text-gray-600" />
                      <span className="text-gray-600 text-xs uppercase tracking-wide">
                        {opportunity.companyServiceType}
                      </span>
                    </div>
                    <Link
                      className="rounded-none border-red-500 py-2 font-medium text-red-500 text-xs"
                      params={{ id: opportunity.companyId as string }}
                      search={{ from: "/opportunities" }}
                      to="/partners/$id"
                    >
                      VIEW PARTNER PROFILE
                    </Link>
                  </div>
                </div>
              </div>
            </div>

            {/* Requirements */}
            <div className="rounded-none border border-gray-200 bg-white">
              <div className="p-6">
                <div className="mb-4 text-gray-500 text-xs uppercase tracking-wide">
                  REQUIREMENTS
                </div>

                {/* Service Types */}
                {opportunity.serviceTypes &&
                  opportunity.serviceTypes.length > 0 && (
                    <section className="space-y-4">
                      <h3 className="flex items-center gap-2 font-bold text-gray-500 text-xs uppercase tracking-widest">
                        <Briefcase className="h-4 w-4" />
                        Service Types
                      </h3>
                      <div className="flex flex-wrap gap-2">
                        {opportunity.serviceTypes.map((service, index) => (
                          <Badge
                            className="rounded-none border border-black bg-black px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider"
                            // biome-ignore lint/suspicious/noArrayIndexKey: The order here doesnt matter
                            key={index}
                            variant="secondary"
                          >
                            {service.name}
                          </Badge>
                        ))}
                      </div>
                    </section>
                  )}

                {/* Capabilities */}
                {opportunity.capabilities &&
                  opportunity.capabilities.length > 0 && (
                    <section className="space-y-4 py-2">
                      <h3 className="flex items-center gap-2 font-bold text-gray-500 text-xs uppercase tracking-widest">
                        <Target className="h-4 w-4" />
                        Required Capabilities
                      </h3>
                      <div className="flex flex-wrap gap-2">
                        {opportunity.capabilities.map(
                          (capability, index: number) => (
                            <Badge
                              className="rounded-none border border-red-600 bg-red-600 px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider"
                              // biome-ignore lint/suspicious/noArrayIndexKey: The order here doesnt matter
                              key={index}
                              variant="secondary"
                            >
                              {capability.name}
                            </Badge>
                          )
                        )}
                      </div>
                    </section>
                  )}

                {/* Industries */}
                {opportunity.industries &&
                  opportunity.industries.length > 0 && (
                    <section className="space-y-4 py-2">
                      <h3 className="flex items-center gap-2 font-bold text-gray-500 text-xs uppercase tracking-widest">
                        <Building className="h-4 w-4" />
                        Target Industries
                      </h3>
                      <div className="flex flex-wrap gap-2">
                        {opportunity.industries.map(
                          (industry, index: number) => (
                            <Badge
                              className="rounded-none border border-blue-600 bg-blue-600 px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider"
                              // biome-ignore lint/suspicious/noArrayIndexKey: The order here doesnt matter
                              key={index}
                              variant="secondary"
                            >
                              {industry.name}
                            </Badge>
                          )
                        )}
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
