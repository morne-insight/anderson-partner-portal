import { createFileRoute, useRouter } from "@tanstack/react-router";
import { useState } from "react";
import { toast } from "sonner";
import { Button } from "@/components/ui/button";
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select";
import {
  FileText,
  Plus,
  Eye,
  Edit,
  Calendar,
  TrendingUp,
  DollarSign,
  CheckCircle,
  Clock
} from "lucide-react";
import { QUARTERS, AVAILABLE_YEARS, getQuarterLabel } from "@/types/reports";
import { type QuarterlyDto } from "@/api";
import { callApi } from "@/server/proxy";

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
      console.error('Failed to fetch quarterly reports:', error);
      // Return empty array on error
      return { reports: [] as QuarterlyDto[] };
    }
  },
});

function ReportsPage() {
  const { companyId } = Route.useParams();
  const { reports } = Route.useLoaderData();
  const router = useRouter();
  const [selectedYear, setSelectedYear] = useState<string>('all');
  const [selectedQuarter, setSelectedQuarter] = useState<string>('all');

  // Filter reports based on selected year and quarter
  const filteredReports = reports.filter(report => {
    const yearMatch = selectedYear === 'all' || report.year!.toString() === selectedYear;
    const quarterMatch = selectedQuarter === 'all' || report.quarter!.toString() === selectedQuarter;
    return yearMatch && quarterMatch;
  });

  const handleCreateReport = () => {
    if (selectedYear === 'all' || selectedQuarter === 'all') {
      toast.error('Please select both year and quarter to create a new report.');
      return;
    }

    // Check if report already exists for this year/quarter
    const existingReport = reports.find(r =>
      r.year!.toString() === selectedYear && r.quarter!.toString() === selectedQuarter
    );

    if (existingReport) {
      toast.error('A report already exists for this year and quarter. Please select a different period.');
      return;
    }

    router.navigate({
      to: `/profile/${companyId}/reports/create`,
      search: { year: selectedYear, quarter: selectedQuarter }
    });
  };

  const handleViewReport = (reportId: string) => {
    router.navigate({ to: `/profile/${companyId}/reports/${reportId}` });
  };

  const handleEditReport = (reportId: string) => {
    router.navigate({ to: `/profile/${companyId}/reports/${reportId}/edit` });
  };

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  const formatDate = (date: Date) => {
    return new Intl.DateTimeFormat('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
    }).format(new Date(date));
  };

  return (
    <div className="space-y-8 animate-fade-in pb-20">
      {/* Header */}
      <header className="border-b border-gray-200 pb-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-4xl font-serif text-black mb-3">Quarterly Reports</h2>
            <p className="text-gray-500 font-light text-lg">
              Manage and submit your firm's quarterly business reports.
            </p>
          </div>
          <Button
            variant="outline"
            onClick={() => router.navigate({ to: `/profile/${companyId}` })}
          >
            Back to Profile
          </Button>
        </div>
      </header>

      {/* Filters and Create Section */}
      <div className="bg-white p-6 border border-gray-200 shadow-sm">
        <div className="flex flex-col md:flex-row gap-4 items-end">
          <div className="flex-1 grid grid-cols-1 md:grid-cols-2 gap-4">
            <div className="space-y-2">
              <label className="text-sm font-medium text-gray-700">Year</label>
              <Select value={selectedYear} onValueChange={setSelectedYear}>
                <SelectTrigger>
                  <SelectValue placeholder="Select Year" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Years</SelectItem>
                  {AVAILABLE_YEARS.map(year => (
                    <SelectItem key={year} value={year.toString()}>
                      {year}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-medium text-gray-700">Quarter</label>
              <Select value={selectedQuarter} onValueChange={setSelectedQuarter}>
                <SelectTrigger>
                  <SelectValue placeholder="Select Quarter" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="all">All Quarters</SelectItem>
                  {QUARTERS.map(quarter => (
                    <SelectItem key={quarter} value={quarter.toString()}>
                      {getQuarterLabel(quarter)}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <Button
            onClick={handleCreateReport}
            className="bg-red-600 hover:bg-red-700 text-white font-bold uppercase tracking-widest text-xs"
            disabled={!selectedYear || !selectedQuarter}
          >
            <Plus className="w-4 h-4 mr-2" />
            Create Report
          </Button>
        </div>
      </div>

      {/* Reports Table */}
      <div className="bg-white border border-gray-200 shadow-sm">
        <div className="p-6 border-b border-gray-200">
          <h3 className="text-lg font-bold uppercase tracking-widest flex items-center gap-2">
            <FileText className="w-5 h-5" />
            Reports ({filteredReports.length})
          </h3>
        </div>

        {filteredReports.length === 0 ? (
          <div className="p-12 text-center">
            <FileText className="w-12 h-12 text-gray-300 mx-auto mb-4" />
            <h3 className="text-lg font-medium text-gray-900 mb-2">No reports found</h3>
            <p className="text-gray-500 mb-6">
              {selectedYear || selectedQuarter
                ? 'No reports match your current filters. Try adjusting the year or quarter selection.'
                : 'Get started by creating your first quarterly report.'
              }
            </p>
            {selectedYear && selectedQuarter && (
              <Button
                onClick={handleCreateReport}
                className="bg-red-600 hover:bg-red-700 text-white"
              >
                <Plus className="w-4 h-4 mr-2" />
                Create {selectedYear && selectedYear !== 'all' && selectedQuarter !== 'all' ? `${selectedYear} ` : ''}{selectedQuarter && selectedQuarter !== 'all' && selectedYear !== 'all' ? `${getQuarterLabel(parseInt(selectedQuarter))} ` : ''}Report
              </Button>
            )}
          </div>
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Period
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Status
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Estimated Revenue
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Head Count
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Created
                  </th>
                  <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="bg-white divide-y divide-gray-200">
                {filteredReports.map((report) => (
                  <tr key={report.id} className="hover:bg-gray-50">
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <Calendar className="w-4 h-4 text-gray-400 mr-2" />
                        <div>
                          <div className="text-sm font-medium text-gray-900">
                            {getQuarterLabel(report.quarter!)} {report.year!}
                          </div>
                        </div>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${report.isSubmitted
                        ? 'bg-green-100 text-green-800'
                        : 'bg-yellow-100 text-yellow-800'
                        }`}>
                        {report.isSubmitted ? (
                          <>
                            <CheckCircle className="w-3 h-3 mr-1" />
                            Submitted
                          </>
                        ) : (
                          <>
                            <Clock className="w-3 h-3 mr-1" />
                            Draft
                          </>
                        )}
                      </span>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <DollarSign className="w-4 h-4 text-gray-400 mr-1" />
                        <span className="text-sm text-gray-900">
                          {formatCurrency(report.revenue!)}
                        </span>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap">
                      <div className="flex items-center">
                        <TrendingUp className="w-4 h-4 text-gray-400 mr-1" />
                        <span className="text-sm text-gray-900">
                          {report.headcount!}
                        </span>
                      </div>
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm text-gray-500">
                      {formatDate(report.createdDate!)}
                      {report.submittedDate && (
                        <div className="text-xs text-gray-400">
                          Submitted: {formatDate(report.submittedDate)}
                        </div>
                      )}
                    </td>
                    <td className="px-6 py-4 whitespace-nowrap text-sm font-medium">
                      <div className="flex items-center gap-2">
                        {report.isSubmitted ? (
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => handleViewReport(report.id!)}
                            className="text-blue-600 hover:text-blue-700"
                          >
                            <Eye className="w-4 h-4 mr-1" />
                            View
                          </Button>
                        ) : (
                          <Button
                            variant="outline"
                            size="sm"
                            onClick={() => handleEditReport(report.id!)}
                            className="text-green-600 hover:text-green-700"
                          >
                            <Edit className="w-4 h-4 mr-1" />
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
