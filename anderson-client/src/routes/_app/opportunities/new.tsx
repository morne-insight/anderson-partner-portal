import { useForm } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createFileRoute, useRouter } from "@tanstack/react-router";
import { ArrowLeft, Briefcase, Building, ChevronDown, Loader2 } from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import type { CapabilityDto, IndustryDto } from "@/api/types.gen";
import {
  Combobox,
  ComboboxChip,
  ComboboxChipRemove,
  ComboboxChips,
  ComboboxContent,
  ComboboxControl,
  ComboboxEmpty,
  ComboboxInput,
  ComboboxItem,
  ComboboxItemIndicator,
  ComboboxList,
  ComboboxValue,
} from "@/components/ui/base-combobox";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Textarea } from "@/components/ui/textarea";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/_app/opportunities/new")({
  component: CreateOpportunity,
  loader: async () => {
    const [companies] = await Promise.all([
      callApi({ data: { fn: "getApiCompaniesMe" } }),
    ]);

    return {
      companies: companies || [],
    };
  },
});

function CreateOpportunity() {
  const router = useRouter();
  const { companies } = Route.useLoaderData();

  const {
    opportunityTypes,
    countries,
    serviceTypes,
    capabilities,
    industries,
    isLoading: isLoadingReferenceData,
    isError: isErrorReferenceData,
  } = usePrefetchReferenceData();

  const [datePickerOpen, setDatePickerOpen] = useState(false);

  // State for capabilities and industries (similar to profile edit)
  const [selectedCapabilityIds, setSelectedCapabilityIds] = useState<string[]>([]);
  const [selectedIndustryIds, setSelectedIndustryIds] = useState<string[]>([]);

  const createMutation = useMutation({
    mutationFn: async (values: any) => {
      const response = await callApi({
        data: { fn: "postApiOpportunities", args: { body: values } },
      });
      if (response.error) {
        throw response.error;
      }
      return response;
    },
    onSuccess: () => {
      router.navigate({ to: "/opportunities" });
    },
    onError: (err) => {
      console.error("Failed to create opportunity", err);
      toast.error("Failed to create opportunity. Please try again.");
    },
  });


  const opportunityTypesData = opportunityTypes.data || [];
  const countriesData = countries.data || [];
  const serviceTypesData = serviceTypes.data || [];
  const capabilitiesData = capabilities.data || [];
  const industriesData = industries.data || [];

  const form = useForm({
    defaultValues: {
      title: "",
      shortDescription: "",
      fullDescription: "",
      deadline: null as Date | null,
      opportunityTypeId: "",
      countryId: "",
      companyId: companies.length === 1 ? companies[0]?.id || "" : "",
      serviceTypes: [] as string[],
      capabilities: [] as string[],
      industries: [] as string[],
    },
    validators: {
      onSubmit: ({ value }) => {
        console.log("Form validation values:", {
          title: value.title,
          shortDescription: value.shortDescription,
          opportunityTypeId: value.opportunityTypeId,
          countryId: value.countryId,
          companyId: value.companyId,
          deadline: value.deadline,
        });
        const errors: any = {};
        if (!value.title?.trim()) errors.title = "Title is required";
        if (!value.shortDescription?.trim())
          errors.shortDescription = "Short description is required";
        if (!value.opportunityTypeId)
          errors.opportunityTypeId = "Type is required";
        if (!value.countryId) errors.countryId = "Country is required";
        if (!value.companyId) errors.companyId = "Company is required";

        console.log("Validation errors:", errors);
        return Object.keys(errors).length > 0 ? errors : undefined;
      },
    },
    onSubmit: async ({ value }) => {
      const payload = {
        ...value,
        deadline: value.deadline
          ? new Date(
            value.deadline.getTime() -
            value.deadline.getTimezoneOffset() * 60_000
          )
            .toISOString()
            .split("T")[0]
          : null,
        capabilities: selectedCapabilityIds,
        industries: selectedIndustryIds,
      };
      console.log("Payload:", payload);
      await createMutation.mutateAsync(payload);
    },
  });

  // Show loading state while reference data is loading
  if (isLoadingReferenceData) {
    return (
      <div className="flex min-h-screen items-center justify-center">
        <Loader2 className="h-8 w-8 animate-spin text-gray-300" />
      </div>
    );
  }

  // Show error state if reference data failed to load
  if (isErrorReferenceData) {
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
        <button
          className="mb-4 flex items-center gap-2 font-bold text-gray-500 text-xs uppercase tracking-widest transition-colors hover:text-black"
          onClick={() => router.history.back()}
        >
          <ArrowLeft className="h-4 w-4" /> Back to Opportunities
        </button>
        <h2 className="mb-3 font-serif text-4xl text-black">
          Post New Opportunity
        </h2>
        <p className="font-light text-gray-500 text-lg">
          Share a tender, joint venture, or resource request with the Anderson
          network.
        </p>
      </header>

      <form
        className="grid grid-cols-1 gap-12 lg:grid-cols-3"
        onSubmit={(e) => {
          e.preventDefault();
          e.stopPropagation();
          form.handleSubmit();
        }}
      >
        {/* Left Column: Main Form */}
        <div className="space-y-8 lg:col-span-2">
          {/* Company Selection */}
          <section className="space-y-4">
            <h3 className="flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <Building className="h-4 w-4" /> Posting Company
            </h3>

            {companies.length === 0 ? (
              <div className="border border-red-200 bg-red-50 p-6 text-red-700 text-sm">
                You are not linked to any companies. Please create a company
                profile first.
              </div>
            ) : companies.length === 1 ? (
              <div className="border border-gray-200 bg-gray-50 p-4">
                <p className="font-bold text-gray-900">{companies[0]?.name}</p>
                <p className="mt-1 text-gray-500 text-xs uppercase tracking-wide">
                  Posting as this company
                </p>
              </div>
            ) : (
              <form.Field
                children={(field) => (
                  <div className="space-y-2">
                    <Label>Select Company</Label>
                    <Select
                      onValueChange={field.handleChange}
                      value={field.state.value}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Select a company" />
                      </SelectTrigger>
                      <SelectContent>
                        {companies.map((company: any) => (
                          <SelectItem key={company.id} value={company.id!}>
                            {company.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
                name="companyId"
              />
            )}
          </section>

          {/* Basic Details */}
          <section className="space-y-4">
            <h3 className="border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              Opportunity Details
            </h3>

            <form.Field
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor="title">Title *</Label>
                  <Input
                    id="title"
                    onBlur={field.handleBlur}
                    onChange={(e) => field.handleChange(e.target.value)}
                    placeholder="e.g. Strategic Partnership for EMEA Market Entry"
                    value={field.state.value}
                  />
                </div>
              )}
              name="title"
            />

            <div className="grid grid-cols-1 gap-4 md:grid-cols-2">
              <form.Field
                children={(field) => (
                  <div className="space-y-2">
                    <Label>Opportunity Type *</Label>
                    <Select
                      onValueChange={field.handleChange}
                      value={field.state.value}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Select type" />
                      </SelectTrigger>
                      <SelectContent>
                        {opportunityTypesData.map((type: any) => (
                          <SelectItem key={type.id} value={type.id!}>
                            {type.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
                name="opportunityTypeId"
              />

              <form.Field
                children={(field) => (
                  <div className="space-y-2">
                    <Label>Country *</Label>
                    <Select
                      onValueChange={field.handleChange}
                      value={field.state.value}
                    >
                      <SelectTrigger>
                        <SelectValue placeholder="Select country" />
                      </SelectTrigger>
                      <SelectContent>
                        {countriesData.map((country: any) => (
                          <SelectItem key={country.id} value={country.id!}>
                            {country.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                  </div>
                )}
                name="countryId"
              />
            </div>

            <form.Field
              children={(field) => {
                console.log("Deadline field state:", field.state.value);
                return (
                  <div className="space-y-2">
                    <Label>Deadline</Label>
                    <Popover
                      onOpenChange={setDatePickerOpen}
                      open={datePickerOpen}
                    >
                      <PopoverTrigger asChild>
                        <Button
                          className="w-full justify-between font-normal md:w-64"
                          variant="outline"
                        >
                          {field.state.value
                            ? field.state.value.toLocaleDateString()
                            : "Select deadline"}
                          <ChevronDown className="h-4 w-4" />
                        </Button>
                      </PopoverTrigger>
                      <PopoverContent
                        align="start"
                        className="w-auto overflow-hidden p-0"
                      >
                        <Calendar
                          captionLayout="dropdown"
                          mode="single"
                          onSelect={(date) => {
                            console.log(
                              "Deadline selected:",
                              date,
                              "Field before change:",
                              field.state.value
                            );
                            field.setValue(date || null);
                            console.log("Using field.setValue method");
                            setDatePickerOpen(false);
                          }}
                          selected={field.state.value ?? undefined}
                        />
                      </PopoverContent>
                    </Popover>
                  </div>
                );
              }}
              mode="value"
              name="deadline"
            />

            <form.Field
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor="shortDescription">Short Description *</Label>
                  <Textarea
                    id="shortDescription"
                    onBlur={field.handleBlur}
                    onChange={(e) => field.handleChange(e.target.value)}
                    placeholder="Brief summary of the opportunity (displayed in listings)"
                    value={field.state.value}
                  />
                  <p className="text-[10px] text-gray-500 uppercase tracking-wide">
                    Max 500 characters
                  </p>
                </div>
              )}
              name="shortDescription"
            />

            <form.Field
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor="fullDescription">Full Description</Label>
                  <Textarea
                    className="min-h-[150px]"
                    id="fullDescription"
                    onBlur={field.handleBlur}
                    onChange={(e) => field.handleChange(e.target.value)}
                    placeholder="Detailed description of the opportunity, requirements, and expectations"
                    value={field.state.value}
                  />
                </div>
              )}
              name="fullDescription"
            />
          </section>
        </div>

        {/* Right Column: Tags & Metadata */}
        <div className="space-y-8">
          {/* Service Types */}
          <section className="space-y-4">
            <h3 className="border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              Service Types
            </h3>
            <div className="border border-gray-200 bg-white p-4 shadow-sm">
              <form.Field
                children={(field) => (
                  <div className="flex flex-wrap gap-2">
                    {serviceTypesData.map((st: any) => {
                      const isSelected = field.state.value.includes(st.id!);
                      return (
                        <button
                          className={`border px-3 py-1.5 font-bold text-[10px] uppercase tracking-wider transition-all ${isSelected
                            ? "border-black bg-black text-white"
                            : "border-gray-200 bg-gray-50 text-gray-400 hover:border-gray-400 hover:text-gray-600"
                            }`}
                          key={st.id}
                          onClick={() => {
                            if (isSelected)
                              field.handleChange(
                                field.state.value.filter(
                                  (id: string) => id !== st.id
                                )
                              );
                            else
                              field.handleChange([
                                ...field.state.value,
                                st.id!,
                              ]);
                          }}
                          type="button"
                        >
                          {st.name}
                        </button>
                      );
                    })}
                  </div>
                )}
                name="serviceTypes"
              />
            </div>
          </section>

          {/* Capabilities */}
          <section className="space-y-4">
            <h3 className="flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <Briefcase className="h-4 w-4" /> Required Capabilities
            </h3>
            <div className="border border-gray-200 bg-white p-4 shadow-sm">
              <Combobox
                items={capabilitiesData || []}
                multiple
                onValueChange={(value: unknown) => {
                  const selectedCapabilities = value as CapabilityDto[];
                  setSelectedCapabilityIds(
                    selectedCapabilities.map((cap) => cap.id as string)
                  );
                }}
                value={
                  capabilitiesData?.filter((cap: CapabilityDto) =>
                    selectedCapabilityIds.includes(cap.id as string)
                  ) || []
                }
              >
                <ComboboxChips className="mb-4 border-0 p-0 shadow-none">
                  <ComboboxValue>
                    {(value: CapabilityDto[]) => (
                      <>
                        {value.length === 0 && (
                          <p className="my-2 text-gray-400 text-xs italic">
                            No capabilities selected.
                          </p>
                        )}
                        {value.map((capability) => (
                          <ComboboxChip
                            aria-label={capability.name}
                            className="rounded-none border border-red-600 bg-red-600 px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider transition-all"
                            key={capability.id}
                          >
                            {capability.name}
                            <ComboboxChipRemove />
                          </ComboboxChip>
                        ))}
                      </>
                    )}
                  </ComboboxValue>
                </ComboboxChips>

                <p className="my-2 text-gray-400 text-xs italic">
                  Select all capabilities required for this opportunity.
                </p>
                <ComboboxControl>
                  <ComboboxValue>
                    <ComboboxInput placeholder="Search and select capabilities..." />
                  </ComboboxValue>
                </ComboboxControl>

                <ComboboxContent>
                  <ComboboxEmpty>No capabilities found.</ComboboxEmpty>
                  <ComboboxList>
                    {(capability: CapabilityDto) => (
                      <ComboboxItem key={capability.id} value={capability}>
                        <ComboboxItemIndicator />
                        <div className="col-start-2">{capability.name}</div>
                      </ComboboxItem>
                    )}
                  </ComboboxList>
                </ComboboxContent>
              </Combobox>
            </div>
          </section>

          {/* Industries */}
          <section className="space-y-4">
            <h3 className="flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <Building className="h-4 w-4" /> Target Industries
            </h3>
            <div className="border border-gray-200 bg-white p-4 shadow-sm">
              <Combobox
                items={industriesData || []}
                multiple
                onValueChange={(value: unknown) => {
                  const selectedIndustries = value as IndustryDto[];
                  setSelectedIndustryIds(
                    selectedIndustries.map((ind) => ind.id as string)
                  );
                }}
                value={
                  industriesData?.filter((ind: IndustryDto) =>
                    selectedIndustryIds.includes(ind.id as string)
                  ) || []
                }
              >
                <ComboboxChips className="mb-4 border-0 p-0 shadow-none">
                  <ComboboxValue>
                    {(value: IndustryDto[]) => (
                      <>
                        {value.length === 0 && (
                          <p className="my-2 text-gray-400 text-xs italic">
                            No industries selected.
                          </p>
                        )}
                        {value.map((industry) => (
                          <ComboboxChip
                            aria-label={industry.name}
                            className="rounded-none border border-blue-600 bg-blue-600 px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider transition-all"
                            key={industry.id}
                          >
                            {industry.name}
                            <ComboboxChipRemove />
                          </ComboboxChip>
                        ))}
                      </>
                    )}
                  </ComboboxValue>
                </ComboboxChips>

                <p className="my-2 text-gray-400 text-xs italic">
                  Select the target industries for this opportunity.
                </p>
                <ComboboxControl>
                  <ComboboxValue>
                    <ComboboxInput placeholder="Search and select industries..." />
                  </ComboboxValue>
                </ComboboxControl>

                <ComboboxContent>
                  <ComboboxEmpty>No industries found.</ComboboxEmpty>
                  <ComboboxList>
                    {(industry: IndustryDto) => (
                      <ComboboxItem key={industry.id} value={industry}>
                        <ComboboxItemIndicator />
                        <div className="col-start-2">{industry.name}</div>
                      </ComboboxItem>
                    )}
                  </ComboboxList>
                </ComboboxContent>
              </Combobox>
            </div>
          </section>
        </div>
      </form>

      {/* Footer Actions */}
      <div className="fixed right-0 bottom-0 left-0 z-40 flex items-center justify-between border-gray-200 border-t bg-white p-6 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)] md:left-72">
        <Button onClick={() => router.history.back()} variant="outline">
          CANCEL
        </Button>
        <form.Subscribe
          children={([canSubmit, isSubmitting]) => (
            <Button
              className="min-w-[160px] bg-red-600 font-bold text-xs uppercase tracking-widest hover:bg-red-700"
              disabled={!canSubmit || isSubmitting || createMutation.isPending}
              onClick={form.handleSubmit}
            >
              {createMutation.isPending || isSubmitting ? (
                <>
                  <Loader2 className="mr-2 h-3 w-3 animate-spin" /> Posting...
                </>
              ) : (
                "Post Opportunity"
              )}
            </Button>
          )}
          selector={(state) => [state.canSubmit, state.isSubmitting]}
        />
      </div>
    </div>
  );
}
