import { createFileRoute, useRouter } from "@tanstack/react-router";
import {
  ArrowLeft,
  Building2,
  Loader2,
  Plus,
  Save,
  Users,
  X,
} from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import type {
  CountryDto,
  CreateQuarterlyCommand,
  CreateQuarterlyPartnersDto,
  CreateQuarterlyReportsDto,
  ReportQuarter,
} from "@/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Switch } from "@/components/ui/switch";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";
import { PartnerStatus } from "@/types/reports";

interface CreateReportSearch {
  year: string;
  quarter: string;
}

export const Route = createFileRoute("/_app/profile/$companyId/reports/create")(
  {
    component: CreateReportPage,
    validateSearch: (search: Record<string, unknown>): CreateReportSearch => ({
      year: (search.year as string) || "",
      quarter: (search.quarter as string) || "",
    }),
  }
);

function CreateReportPage() {
  const { companyId } = Route.useParams();
  const { year, quarter } = Route.useSearch();
  const router = useRouter();

  // State for report submitted State
  const [isSubmitted, setIsSubmitted] = useState(false);

  // State for report lines management
  const [reportLines, setReportLines] = useState<CreateQuarterlyReportsDto[]>(
    []
  );
  const [isAddingReportLine, setIsAddingReportLine] = useState(false);
  const [newReportLine, setNewReportLine] = useState<CreateQuarterlyReportsDto>(
    {
      partnerCount: 0,
      headcount: 0,
      clientCount: 0,
      officeCount: 0,
      lawyerCount: 0,
      estimatedRevenue: 0,
      countryId: "",
    }
  );

  // State for partners management
  const [partners, setPartners] = useState<CreateQuarterlyPartnersDto[]>([]);
  const [isAddingPartner, setIsAddingPartner] = useState(false);
  const [newPartner, setNewPartner] = useState<CreateQuarterlyPartnersDto>({
    name: "",
    status: 1, // Default status
  });

  const [isSubmitting, setIsSubmitting] = useState(false);
  const { countries, isLoading, isError } = usePrefetchReferenceData();

  // Helper function to get status label
  const getPartnerStatusLabel = (status: number): string => {
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

  const handleAddReportLine = () => {
    if (
      newReportLine.partnerCount &&
      newReportLine.headcount &&
      newReportLine.estimatedRevenue
    ) {
      setReportLines([...reportLines, { ...newReportLine }]);
      setNewReportLine({
        partnerCount: 0,
        headcount: 0,
        clientCount: 0,
        officeCount: 0,
        lawyerCount: 0,
        estimatedRevenue: 0,
        countryId: "",
      });
      setIsAddingReportLine(false);
    }
  };

  const handleRemoveReportLine = (index: number) => {
    setReportLines(reportLines.filter((_, i) => i !== index));
  };

  const handleAddPartner = () => {
    if (newPartner.name?.trim()) {
      setPartners([...partners, { ...newPartner }]);
      setNewPartner({
        name: "",
        status: 1,
      });
      setIsAddingPartner(false);
    }
  };

  const handleRemovePartner = (index: number) => {
    setPartners(partners.filter((_, i) => i !== index));
  };

  const handleSubmit = async () => {
    if (reportLines.length === 0) {
      toast.error("Please add at least one report line.");
      return;
    }

    setIsSubmitting(true);
    try {
      const createCommand: CreateQuarterlyCommand = {
        year: Number.parseInt(year),
        quarter: Number.parseInt(quarter) as ReportQuarter,
        companyId,
        partners,
        reports: reportLines,
        isSubmitted,
      };

      await callApi({
        data: {
          fn: "postApiQuarterlies",
          args: {
            body: createCommand,
          },
        },
      });

      toast.success("Report created successfully!");
      router.navigate({ to: `/profile/${companyId}/reports` });
    } catch (error) {
      console.error(error);
      toast.error("Failed to create report.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="animate-fade-in space-y-8 pb-20">
      {/* Header */}
      <header className="border-gray-200 border-b pb-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="mb-3 font-serif text-4xl text-black">
              Create Q{quarter} {year} Report
            </h2>
            <p className="font-light text-gray-500 text-lg">
              Add report lines and partners to create your quarterly business
              report.
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
      <div className="border border-gray-200 bg-white shadow-sm">
        <div className="border-gray-200 border-b p-6">
          <h3 className="flex items-center gap-2 font-bold text-lg uppercase tracking-widest">
            <Building2 className="h-5 w-5" />
            Report Lines ({reportLines.length})
          </h3>
        </div>

        <div className="space-y-4 p-6">
          {reportLines.length > 0 && (
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
                    <th className="px-4 py-3 text-right font-medium text-gray-500 text-xs uppercase tracking-wider">
                      Actions
                    </th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-200">
                  {reportLines.map((reportLine, index) => {
                    const country = countries?.data?.find(
                      (c) => c.id === reportLine.countryId
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
                        <td className="px-4 py-3 text-right">
                          <Button
                            className="text-red-600 hover:bg-red-50 hover:text-red-700"
                            onClick={() => handleRemoveReportLine(index)}
                            size="sm"
                            variant="outline"
                          >
                            <X className="h-4 w-4" />
                          </Button>
                        </td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          )}

          {isAddingReportLine ? (
            <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Country</Label>
                  <Select
                    onValueChange={(value) =>
                      setNewReportLine({ ...newReportLine, countryId: value })
                    }
                    value={newReportLine.countryId || ""}
                  >
                    <SelectTrigger className="w-full">
                      <SelectValue placeholder="Select Country" />
                    </SelectTrigger>
                    <SelectContent>
                      {countries?.data?.map((country: CountryDto) => (
                        <SelectItem key={country.id} value={country.id || ""}>
                          {country.name}
                        </SelectItem>
                      ))}
                    </SelectContent>
                  </Select>
                </div>
                <div className="space-y-2">
                  <Label>Partner Count</Label>
                  <Input
                    onChange={(e) =>
                      setNewReportLine({
                        ...newReportLine,
                        partnerCount: Number.parseInt(e.target.value) || 0,
                      })
                    }
                    placeholder="0"
                    type="number"
                    value={newReportLine.partnerCount || ""}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Client Count</Label>
                  <Input
                    onChange={(e) =>
                      setNewReportLine({
                        ...newReportLine,
                        clientCount: Number.parseInt(e.target.value) || 0,
                      })
                    }
                    placeholder="0"
                    type="number"
                    value={newReportLine.clientCount || ""}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Office Count</Label>
                  <Input
                    onChange={(e) =>
                      setNewReportLine({
                        ...newReportLine,
                        officeCount: Number.parseInt(e.target.value) || 0,
                      })
                    }
                    placeholder="0"
                    type="number"
                    value={newReportLine.officeCount || ""}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Headcount</Label>
                  <Input
                    onChange={(e) =>
                      setNewReportLine({
                        ...newReportLine,
                        headcount: Number.parseInt(e.target.value) || 0,
                      })
                    }
                    placeholder="0"
                    type="number"
                    value={newReportLine.headcount || ""}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Lawyer Count</Label>
                  <Input
                    onChange={(e) =>
                      setNewReportLine({
                        ...newReportLine,
                        lawyerCount: Number.parseInt(e.target.value) || 0,
                      })
                    }
                    placeholder="0"
                    type="number"
                    value={newReportLine.lawyerCount || ""}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Estimated Revenue ($)</Label>
                  <Input
                    onChange={(e) =>
                      setNewReportLine({
                        ...newReportLine,
                        estimatedRevenue: Number.parseInt(e.target.value) || 0,
                      })
                    }
                    placeholder="0"
                    type="number"
                    value={newReportLine.estimatedRevenue || ""}
                  />
                </div>
              </div>
              <div className="flex justify-end gap-2 pt-2">
                <Button
                  onClick={() => setIsAddingReportLine(false)}
                  size="sm"
                  variant="outline"
                >
                  Cancel
                </Button>
                <Button
                  className="bg-black hover:bg-gray-800"
                  disabled={
                    !(
                      newReportLine.partnerCount &&
                      newReportLine.headcount &&
                      newReportLine.estimatedRevenue
                    )
                  }
                  onClick={handleAddReportLine}
                  size="sm"
                >
                  Add Report Line
                </Button>
              </div>
            </div>
          ) : (
            <Button
              className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
              onClick={() => setIsAddingReportLine(true)}
              variant="outline"
            >
              <Plus className="mr-2 h-4 w-4" />
              Add Report Line
            </Button>
          )}
        </div>
      </div>

      {/* Partners Section */}
      <div className="border border-gray-200 bg-white shadow-sm">
        <div className="border-gray-200 border-b p-6">
          <h3 className="flex items-center gap-2 font-bold text-lg uppercase tracking-widest">
            <Users className="h-5 w-5" />
            Partners ({partners.length})
          </h3>
        </div>

        <div className="space-y-4 p-6">
          {partners.length > 0 && (
            <table className="w-full border border-gray-200">
              <thead className="bg-gray-100">
                <tr>
                  <th className="p-4 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Partner Name
                  </th>
                  <th className="p-4 text-left font-medium text-gray-500 text-xs uppercase tracking-wider">
                    Status
                  </th>
                  <th className="w-16" />
                </tr>
              </thead>
              <tbody>
                {partners.map((partner, index) => (
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
                    <td className="p-4">
                      <Button
                        className="text-red-600 hover:bg-red-50 hover:text-red-700"
                        onClick={() => handleRemovePartner(index)}
                        size="sm"
                        variant="outline"
                      >
                        <X className="h-4 w-4" />
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}

          {isAddingPartner ? (
            <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
              <div className="grid grid-cols-2 gap-4">
                <div className="space-y-2">
                  <Label>Partner Name</Label>
                  <Input
                    onChange={(e) =>
                      setNewPartner({ ...newPartner, name: e.target.value })
                    }
                    placeholder="e.g. John Smith"
                    value={newPartner.name || ""}
                  />
                </div>
                <div className="space-y-2">
                  <Label>Status</Label>
                  <Select
                    onValueChange={(value) =>
                      setNewPartner({
                        ...newPartner,
                        status: Number.parseInt(value),
                      })
                    }
                    value={newPartner.status?.toString() || ""}
                  >
                    <SelectTrigger>
                      <SelectValue placeholder="Select Status" />
                    </SelectTrigger>
                    <SelectContent>
                      <SelectItem value={PartnerStatus.Hired.toString()}>
                        Hired
                      </SelectItem>
                      <SelectItem value={PartnerStatus.Promoted.toString()}>
                        Promoted
                      </SelectItem>
                      <SelectItem value={PartnerStatus.Terminated.toString()}>
                        Terminated
                      </SelectItem>
                    </SelectContent>
                  </Select>
                </div>
              </div>
              <div className="flex justify-end gap-2 pt-2">
                <Button
                  onClick={() => setIsAddingPartner(false)}
                  size="sm"
                  variant="outline"
                >
                  Cancel
                </Button>
                <Button
                  className="bg-black hover:bg-gray-800"
                  disabled={!newPartner.name?.trim()}
                  onClick={handleAddPartner}
                  size="sm"
                >
                  Add Partner
                </Button>
              </div>
            </div>
          ) : (
            <Button
              className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
              onClick={() => setIsAddingPartner(true)}
              variant="outline"
            >
              <Plus className="mr-2 h-4 w-4" />
              Add Partner
            </Button>
          )}
        </div>
      </div>

      {/* Footer Actions */}
      <div className="fixed right-0 bottom-0 left-0 z-40 flex items-center justify-between border-gray-200 border-t bg-white p-6 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)] md:left-72">
        <span className="hidden font-medium text-gray-400 text-xs uppercase tracking-widest md:inline-block">
          Q{quarter} {year} Report
        </span>
        <div className="flex w-full items-center justify-end gap-4 md:w-auto">
          <Button
            onClick={() =>
              router.navigate({ to: `/profile/${companyId}/reports` })
            }
            variant="outline"
          >
            Cancel
          </Button>
          <Button
            className="min-w-[160px] bg-red-600 font-bold text-xs uppercase tracking-widest hover:bg-red-700"
            disabled={isSubmitting || reportLines.length === 0}
            onClick={handleSubmit}
          >
            {isSubmitting ? (
              <>Creating...</>
            ) : (
              <>
                <Save className="mr-2 h-3 w-3" />
                Create Report
              </>
            )}
          </Button>
          <div className="flex items-center space-x-2">
            <Switch
              checked={isSubmitted}
              id="submit-report"
              onCheckedChange={(checked) => setIsSubmitted(checked)}
            />
            <Label htmlFor="submit-report">Submit Report</Label>
          </div>
        </div>
      </div>
    </div>
  );
}
