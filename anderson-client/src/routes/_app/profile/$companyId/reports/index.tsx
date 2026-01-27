import { createFileRoute, useRouter } from "@tanstack/react-router";
import {
  Calendar,
  CheckCircle,
  Clock,
  DollarSign,
  Edit,
  Eye,
  FileText,
  Plus,
  TrendingUp,
} from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import type { QuarterlyDto } from "@/api";
import { Button } from "@/components/ui/button";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { callApi } from "@/server/proxy";
import { AVAILABLE_YEARS, getQuarterLabel, QUARTERS } from "@/types/reports";

export const Route = createFileRoute("/_app/profile/$companyId/reports/")({
  component: ReportsPage,
  loader: async ({ params }) => {
    try {
      // Fetch quarterly reports for the specific company
      const response = await callApi({
        data: {
          fn: "getApiQuarterliesByIdMe",
          args: { path: { id: params.companyId } },
        },
      });

      // Transform QuarterlyDto to match UI expectations
      const quarterlies = response || [];

      return { reports: quarterlies as QuarterlyDto[] };
    } catch (error) {
      console.error("Failed to fetch quarterly reports:", error);
      // Return empty array on error
      return { reports: [] as QuarterlyDto[] };
    }
  },
});

function ReportsPage() {
  const { companyId } = Route.useParams();
  const { reports } = Route.useLoaderData();
  const router = useRouter();
  const [selectedYear, setSelectedYear] = useState<string>("all");
  const [selectedQuarter, setSelectedQuarter] = useState<string>("all");

  // Filter reports based on selected year and quarter
  const filteredReports = reports.filter((report) => {
    const yearMatch =
      selectedYear === "all" || report.year!.toString() === selectedYear;
    const quarterMatch =
      selectedQuarter === "all" ||
      report.quarter!.toString() === selectedQuarter;
    return yearMatch && quarterMatch;
  });

  const handleCreateReport = () => {
    if (selectedYear === "all" || selectedQuarter === "all") {
      toast.error(
        "Please select both year and quarter to create a new report."
      );
      return;
    }

    // Check if report already exists for this year/quarter
    const existingReport = reports.find(
      (r) =>
        r.year!.toString() === selectedYear &&
        r.quarter!.toString() === selectedQuarter
    );

    if (existingReport) {
      toast.error(
        "A report already exists for this year and quarter. Please select a different period."
      );
      return;
    }

    router.navigate({
      to: `/profile/${companyId}/reports/create`,
      search: { year: selectedYear, quarter: selectedQuarter },
    });
  };

  const handleViewReport = (reportId: string) => {
    router.navigate({ to: `/profile/${companyId}/reports/${reportId}` });
  };

  const handleEditReport = (reportId: string) => {
    router.navigate({ to: `/profile/${companyId}/reports/${reportId}/edit` });
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat("en-US", {
      style: "currency",
      currency: "USD",
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  const formatDate = (date: Date) => {
    return new Intl.DateTimeFormat("en-US", {
      year: "numeric",
      month: "short",
      day: "numeric",
    }).format(new Date(date));
  };

  return (
    <div className="animate-fade-in space-y-8 pb-20">
      {/* Header */}
      <header className="border-gray-200 border-b pb-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="mb-3 font-serif text-4xl text-black">
              Quarterly Reports
            </h2>
            <p className="font-light text-gray-500 text-lg">
              Manage and submit your firm's quarterly business reports.
            </p>
          </div>
          <Button
            onClick={() => router.navigate({ to: `/profile/${companyId}` })}
            variant="outline"
          >
            Back to Profile
          </Button>
        </div>
      </header>

      {/* Filters and Create Section */}
      <div className="border border-gray-200 bg-white p-6 shadow-sm">
        <div className="flex flex-col items-end gap-4 md:flex-row">
          <div className="grid flex-1 grid-cols-1 gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <label className="font-medium text-gray-700 text-sm">Year</label>
              <Select onValueChange={setSelectedYear} value={selectedYear}>
                <SelectTrigger>
                  <SelectValue placeholder="Select Year" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Years</SelectItem>
                  {AVAILABLE_YEARS.map((year) => (
                    <SelectItem key={year} value={year.toString()}>
                      {year}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <label className="font-medium text-gray-700 text-sm">
                Quarter
              </label>
              <Select
                onValueChange={setSelectedQuarter}
                value={selectedQuarter}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Select Quarter" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Quarters</SelectItem>
                  {QUARTERS.map((quarter) => (
                    <SelectItem key={quarter} value={quarter.toString()}>
                      {getQuarterLabel(quarter)}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <Button
            className="bg-red-600 font-bold text-white text-xs uppercase tracking-widest hover:bg-red-700"
            disabled={!(selectedYear && selectedQuarter)}
            onClick={handleCreateReport}
          >
            <Plus className="mr-2 h-4 w-4" />
            Create Report
          </Button>
        </div>
      </div>

      {/* Reports Table */}
      <div className="border border-gray-200 bg-white shadow-sm">
        <div className="border-gray-200 border-b p-6">
          <h3 className="flex items-center gap-2 font-bold text-lg uppercase tracking-widest">
            <FileText className="h-5 w-5" />
            Reports ({filteredReports.length})
          </h3>
        </div>

        {filteredReports.length === 0 ? (
          <div className="p-12 text-center">
            <FileText className="mx-auto mb-4 h-12 w-12 text-gray-300" />
            <h3 className="mb-2 font-medium text-gray-900 text-lg">
              No reports found
            </h3>
            <p className="mb-6 text-gray-500">
              {selectedYear || selectedQuarter
                ? "No reports match your current filters. Try adjusting the year or quarter selection."
                : "Get started by creating your first quarterly report."}
            </p>
            {selectedYear && selectedQuarter && (
              <Button
                className="bg-red-600 text-white hover:bg-red-700"
                onClick={handleCreateReport}
              >
                <Plus className="mr-2 h-4 w-4" />
                Create{" "}
                {selectedYear &&
                selectedYear !== "all" &&
                selectedQuarter !== "all"
                  ? `${selectedYear} `
                  : ""}
                {selectedQuarter &&
                selectedQuarter !== "all" &&
                selectedYear !== "all"
                  ? `${getQuarterLabel(Number.parseInt(selectedQuarter))} `
                  : ""}
                Report
              </Button>
            )}
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Period
                  </th>
                  <th className="px-6 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-6 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Estimated Revenue
                  </th>
                  <th className="px-6 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Head Count
                  </th>
                  <th className="px-6 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Created
                  </th>
                  <th className="px-6 py-3 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-200 bg-white">
                {filteredReports.map((report) => (
                  <tr className="hover:bg-gray-50" key={report.id}>
                    <td className="whitespace-nowrap px-6 py-4">
                      <div className="flex items-center">
                        <Calendar className="mr-2 h-4 w-4 text-gray-400" />
                        <div>
                          <div className="font-medium text-gray-900 text-sm">
                            {getQuarterLabel(report.quarter!)} {report.year!}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td className="whitespace-nowrap px-6 py-4">
                      <span
                        className={`inline-flex items-center rounded-full px-2.5 py-0.5 font-medium text-xs ${
                          report.isSubmitted
                            ? "bg-green-100 text-green-800"
                            : "bg-yellow-100 text-yellow-800"
                        }`}
                      >
                        {report.isSubmitted ? (
                          <>
                            <CheckCircle className="mr-1 h-3 w-3" />
                            Submitted
                          </>
                        ) : (
                          <>
                            <Clock className="mr-1 h-3 w-3" />
                            Draft
                          </>
                        )}
                      </span>
                    </td>
                    <td className="whitespace-nowrap px-6 py-4">
                      <div className="flex items-center">
                        <DollarSign className="mr-1 h-4 w-4 text-gray-400" />
                        <span className="text-gray-900 text-sm">
                          {formatCurrency(report.revenue!)}
                        </span>
                      </div>
                    </td>
                    <td className="whitespace-nowrap px-6 py-4">
                      <div className="flex items-center">
                        <TrendingUp className="mr-1 h-4 w-4 text-gray-400" />
                        <span className="text-gray-900 text-sm">
                          {report.headcount!}
                        </span>
                      </div>
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 text-gray-500 text-sm">
                      {formatDate(report.createdDate!)}
                      {report.submittedDate && (
                        <div className="text-gray-400 text-xs">
                          Submitted: {formatDate(report.submittedDate)}
                        </div>
                      )}
                    </td>
                    <td className="whitespace-nowrap px-6 py-4 font-medium text-sm">
                      <div className="flex items-center gap-2">
                        {report.isSubmitted ? (
                          <Button
                            className="text-blue-600 hover:text-blue-700"
                            onClick={() => handleViewReport(report.id!)}
                            size="sm"
                            variant="outline"
                          >
                            <Eye className="mr-1 h-4 w-4" />
                            View
                          </Button>
                        ) : (
                          <Button
                            className="text-green-600 hover:text-green-700"
                            onClick={() => handleEditReport(report.id!)}
                            size="sm"
                            variant="outline"
                          >
                            <Edit className="mr-1 h-4 w-4" />
                            Edit
                          </Button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
