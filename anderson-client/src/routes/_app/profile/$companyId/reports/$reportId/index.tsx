import { createFileRoute, useRouter } from "@tanstack/react-router";
import {
  ArrowLeft,
  Building2,
  Calendar,
  CheckCircle,
  FileText,
  Loader2,
  Users,
} from "lucide-react";
import type {
  CountryDto,
  QuarterlyReportLineDto,
  QuarterlyReportPartnerDto,
} from "@/api/types.gen";
import { Button } from "@/components/ui/button";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";
import { PartnerStatus } from "@/types/reports";

export const Route = createFileRoute(
  "/_app/profile/$companyId/reports/$reportId/"
)({
  component: ViewReportPage,
  loader: async ({ params }) => {
    try {
      const response = await callApi({
        data: {
          fn: "getApiQuarterliesById",
          args: {
            path: {
              id: params.reportId,
            },
          },
        },
      });

      return { report: response };
    } catch (error) {
      console.error("Failed to fetch report:", error);
      throw new Error("Failed to load report");
    }
  },
});

function ViewReportPage() {
  const { companyId } = Route.useParams();
  const { report } = Route.useLoaderData();
  const router = useRouter();

  // Fetch country reference data
  const { countries, isLoading, isError } = usePrefetchReferenceData();

  const getPartnerStatusLabel = (status: number) => {
    switch (status) {
      case PartnerStatus.Hired:
        return "Hired";
      case PartnerStatus.Promoted:
        return "Promoted";
      case PartnerStatus.Terminated:
        return "Terminated";
      default:
        return "Unknown";
    }
  };

  const formatDate = (date: Date) => {
    return new Intl.DateTimeFormat("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    }).format(new Date(date));
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
    <div className="animate-fade-in space-y-8 pb-20">
      {/* Header */}
      <header className="border-gray-200 border-b pb-6">
        <div className="flex items-center justify-between">
          <div>
            <div className="mb-3 flex items-center gap-3">
              <h2 className="font-serif text-4xl text-black">
                Q{report.quarter} {report.year} Report
              </h2>
              {report.isSubmitted && (
                <span className="inline-flex items-center rounded-full bg-green-100 px-3 py-1 font-medium text-green-800 text-sm">
                  <CheckCircle className="mr-1 h-4 w-4" />
                  Submitted
                </span>
              )}
            </div>
            <p className="font-light text-gray-500 text-lg">
              {report.isSubmitted && report.submittedDate
                ? `Submitted on ${formatDate(report.submittedDate)} • `
                : ""}
              Created {formatDate(report.createdDate!)}
            </p>
          </div>
          <Button
            onClick={() =>
              router.navigate({ to: `/profile/${companyId}/reports` })
            }
            variant="outline"
          >
            <ArrowLeft className="mr-2 h-4 w-4" />
            Back to Reports
          </Button>
        </div>
      </header>

      {/* Report Lines Section */}
      {report.reportLines && report.reportLines.length > 0 && (
        <div className="border border-gray-200 bg-white shadow-sm">
          <div className="border-gray-200 border-b p-6">
            <h3 className="flex items-center gap-2 font-bold text-lg uppercase tracking-widest">
              <Building2 className="h-5 w-5" />
              Report Lines ({report.reportLines.length})
            </h3>
          </div>

          <div className="p-6">
            <div className="overflow-hidden border border-gray-200">
              <table className="w-full">
                <thead className="bg-gray-100">
                  <tr>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Country
                    </th>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Partners
                    </th>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Headcount
                    </th>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Clients
                    </th>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Offices
                    </th>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Lawyers
                    </th>
                    <th className="px-4 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Est. Revenue
                    </th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {report.reportLines.map(
                    (reportLine: QuarterlyReportLineDto, index: number) => {
                      const country = countries?.data?.find(
                        (c: CountryDto) => c.id === reportLine.countryId
                      );
                      return (
                        <tr className="bg-white hover:bg-gray-50" key={index}>
                          <td className="px-4 py-3 font-medium text-sm">
                            {country?.name || "Not specified"}
                          </td>
                          <td className="px-4 py-3 text-sm">
                            {reportLine.partnerCount}
                          </td>
                          <td className="px-4 py-3 text-sm">
                            {reportLine.headcount}
                          </td>
                          <td className="px-4 py-3 text-sm">
                            {reportLine.clientCount}
                          </td>
                          <td className="px-4 py-3 text-sm">
                            {reportLine.officeCount}
                          </td>
                          <td className="px-4 py-3 text-sm">
                            {reportLine.lawyerCount}
                          </td>
                          <td className="px-4 py-3 text-sm">
                            ${reportLine.estimatedRevenue?.toLocaleString()}
                          </td>
                        </tr>
                      );
                    }
                  )}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      )}

      {/* Partners Section */}
      {report.partners && report.partners.length > 0 && (
        <div className="border border-gray-200 bg-white shadow-sm">
          <div className="border-gray-200 border-b p-6">
            <h3 className="flex items-center gap-2 font-bold text-lg uppercase tracking-widest">
              <Users className="h-5 w-5" />
              Partners ({report.partners.length})
            </h3>
          </div>

          <div className="p-6">
            <table className="w-full border border-gray-200">
              <thead className="bg-gray-100">
                <tr>
                  <th className="p-4 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Partner Name
                  </th>
                  <th className="p-4 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Status
                  </th>
                </tr>
              </thead>
              <tbody>
                {report.partners.map(
                  (partner: QuarterlyReportPartnerDto, index: number) => (
                    <tr
                      className="border-gray-200 border-t bg-gray-50"
                      key={index}
                    >
                      <td className="p-4">
                        <p className="font-medium text-sm">{partner.name}</p>
                      </td>
                      <td className="p-4">
                        <p className="font-medium text-sm">
                          {getPartnerStatusLabel(partner.status || 1)}
                        </p>
                      </td>
                    </tr>
                  )
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Report Metadata */}
      <div className="border border-gray-200 bg-gray-50 p-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-6 text-gray-500 text-sm">
            <div className="flex items-center gap-2">
              <Calendar className="h-4 w-4" />
              <span>Created: {formatDate(report.createdDate!)}</span>
            </div>
            {report.isSubmitted && report.submittedDate && (
              <div className="flex items-center gap-2">
                <CheckCircle className="h-4 w-4" />
                <span>Submitted: {formatDate(report.submittedDate)}</span>
              </div>
            )}
            <div className="flex items-center gap-2">
              <FileText className="h-4 w-4" />
              <span>
                Q{report.quarter} {report.year} Report
              </span>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
