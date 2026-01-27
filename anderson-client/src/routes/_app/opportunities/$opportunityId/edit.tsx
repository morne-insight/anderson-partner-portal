import { useForm } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createFileRoute, useRouter } from "@tanstack/react-router";
import {
  ArrowLeft,
  Building,
  ChevronDown,
  Loader2,
  Trash2,
} from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "@/components/ui/alert-dialog";
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

export const Route = createFileRoute("/_app/opportunities/$opportunityId/edit")(
  {
    component: EditOpportunity,
    loader: async ({ params }) => {
      const [opportunity, companies] = await Promise.all([
        callApi({
          data: {
            fn: "getApiOpportunitiesById",
            args: { path: { id: params.opportunityId } },
          },
        }),
        callApi({ data: { fn: "getApiCompaniesMe" } }),
      ]);

      return {
        opportunity: opportunity || {},
        companies: companies || [],
      };
    },
  }
);

function EditOpportunity() {
  const router = useRouter();
  const { opportunity, companies } = Route.useLoaderData();

  const {
    opportunityTypes,
    countries,
    serviceTypes,
    capabilities,
    industries,
    isLoading,
    isError,
  } = usePrefetchReferenceData();

  // Helper to safely extract IDs from array of objects or strings
  const getIds = (items: any[]) =>
    items?.map((item: any) => (typeof item === "string" ? item : item.id)) ||
    [];

  const [datePickerOpen, setDatePickerOpen] = useState(false);

  const updateMutation = useMutation({
    mutationFn: async (values: any) => {
      const response = await callApi({
        data: {
          fn: "putApiOpportunitiesByIdFull",
          args: { path: { id: opportunity.id }, body: values },
        },
      });
      console.log("Update response:", response);
      // 204 No Content is a successful response for updates
      if (response && response.error && response.status !== 204) {
        throw response.error;
      }
      return response;
    },
    onSuccess: () => {
      console.log("Update successful, navigating to opportunities");
      router.navigate({ to: "/opportunities" });
    },
    onError: (err) => {
      console.error("Failed to update opportunity", err);
      toast.error("Failed to update opportunity. Please try again.");
    },
  });

  const deleteMutation = useMutation({
    mutationFn: async () => {
      const response = await callApi({
        data: {
          fn: "deleteApiOpportunitiesById",
          args: { path: { id: opportunity.id } },
        },
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
      console.error("Failed to delete", err);
      toast.error("Could not delete opportunity.");
    },
  });

  const form = useForm({
    defaultValues: {
      title: opportunity.title || "",
      shortDescription: opportunity.shortDescription || "",
      fullDescription: opportunity.fullDescription || "",
      deadline: opportunity.deadline
        ? new Date(opportunity.deadline)
        : (null as Date | null),
      opportunityTypeId:
        opportunity.opportunityTypeId || opportunity.opportunityType?.id || "",
      countryId: opportunity.countryId || opportunity.country?.id || "",
      companyId:
        opportunity.companyId ||
        opportunity.company?.id ||
        (companies.length === 1 ? companies[0]?.id : ""),
      serviceTypes: getIds(opportunity.serviceTypes),
      capabilities: getIds(opportunity.capabilities),
      industries: getIds(opportunity.industries),
    },
    validators: {
      onSubmit: ({ value }) => {
        const errors: any = {};
        if (!value.title?.trim()) errors.title = "Title is required";
        if (!value.shortDescription?.trim())
          errors.shortDescription = "Short description is required";
        if (!value.opportunityTypeId)
          errors.opportunityTypeId = "Type is required";
        if (!value.countryId) errors.countryId = "Country is required";
        if (!value.companyId) errors.companyId = "Company is required";

        return Object.keys(errors).length > 0 ? errors : undefined;
      },
    },
    onSubmit: async ({ value }) => {
      try {
        // Save main opportunity data
        const payload = {
          id: opportunity.id,
          title: value.title,
          shortDescription: value.shortDescription,
          fullDescription: value.fullDescription,
          deadline: value.deadline
            ? new Date(
                value.deadline.getTime() -
                  value.deadline.getTimezoneOffset() * 60_000
              )
                .toISOString()
                .split("T")[0]
            : null,
          opportunityTypeId: value.opportunityTypeId,
          countryId: value.countryId,
          companyId: value.companyId,
        };

        await updateMutation.mutateAsync(payload);
      } catch (error) {
        console.error("Failed to save opportunity:", error);
        toast.error("Failed to save opportunity. Please try again.");
      }
    },
  });

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
      <header className="flex items-start justify-between border-gray-200 border-b pb-6">
        <div>
          <button
            className="mb-4 flex items-center gap-2 font-bold text-gray-500 text-xs uppercase tracking-widest transition-colors hover:text-black"
            onClick={() => router.history.back()}
          >
            <ArrowLeft className="h-4 w-4" /> Back to Opportunities
          </button>
          <h2 className="mb-3 font-serif text-4xl text-black">
            Edit Opportunity
          </h2>
        </div>
        <AlertDialog>
          <AlertDialogTrigger asChild>
            <Button
              className="text-red-600 hover:bg-red-50 hover:text-red-700"
              variant="ghost"
            >
              <Trash2 className="mr-2 h-4 w-4" /> Delete
            </Button>
          </AlertDialogTrigger>
          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>Delete Opportunity</AlertDialogTitle>
              <AlertDialogDescription>
                Are you sure you want to delete this opportunity? This action
                cannot be undone.
              </AlertDialogDescription>
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel>Cancel</AlertDialogCancel>
              <AlertDialogAction
                className="bg-red-600 font-bold hover:bg-red-700"
                onClick={() => deleteMutation.mutate()}
              >
                Delete
              </AlertDialogAction>
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
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
                You are not linked to any companies.
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
                      value={field.state.value || undefined}
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
                        {opportunityTypes.data?.map((type: any) => (
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
                        {countries.data?.map((country: any) => (
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
              children={(field) => (
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
                          field.handleChange(date ?? null);
                          setDatePickerOpen(false);
                        }}
                        selected={field.state.value ?? undefined}
                      />
                    </PopoverContent>
                  </Popover>
                </div>
              )}
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
                    {serviceTypes.data?.map((st: any) => {
                      const isSelected = field.state.value.includes(st.id!);
                      return (
                        <button
                          className={`border px-3 py-1.5 font-bold text-[10px] uppercase tracking-wider transition-all ${
                            isSelected
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
            <h3 className="border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              Required Capabilities
            </h3>
            <div className="border border-gray-200 bg-white p-4 shadow-sm">
              <form.Field
                children={(field) => (
                  <div className="flex flex-wrap gap-2">
                    {capabilities.data?.map((cap: any) => {
                      const isSelected = field.state.value.includes(cap.id!);
                      return (
                        <button
                          className={`border px-3 py-1.5 font-bold text-[10px] uppercase tracking-wider transition-all ${
                            isSelected
                              ? "border-red-600 bg-red-600 text-white"
                              : "border-gray-200 bg-gray-50 text-gray-400 hover:border-gray-400 hover:text-gray-600"
                          }`}
                          key={cap.id}
                          onClick={() => {
                            if (isSelected)
                              field.handleChange(
                                field.state.value.filter(
                                  (id: string) => id !== cap.id
                                )
                              );
                            else
                              field.handleChange([
                                ...field.state.value,
                                cap.id!,
                              ]);
                          }}
                          type="button"
                        >
                          {cap.name}
                        </button>
                      );
                    })}
                  </div>
                )}
                name="capabilities"
              />
            </div>
          </section>

          {/* Industries */}
          <section className="space-y-4">
            <h3 className="border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              Target Industries
            </h3>
            <div className="border border-gray-200 bg-white p-4 shadow-sm">
              <form.Field
                children={(field) => (
                  <div className="flex flex-wrap gap-2">
                    {industries.data?.map((ind: any) => {
                      const isSelected = field.state.value.includes(ind.id!);
                      return (
                        <button
                          className={`border px-3 py-1.5 font-bold text-[10px] uppercase tracking-wider transition-all ${
                            isSelected
                              ? "border-blue-600 bg-blue-600 text-white"
                              : "border-gray-200 bg-gray-50 text-gray-400 hover:border-gray-400 hover:text-gray-600"
                          }`}
                          key={ind.id}
                          onClick={() => {
                            if (isSelected)
                              field.handleChange(
                                field.state.value.filter(
                                  (id: string) => id !== ind.id
                                )
                              );
                            else
                              field.handleChange([
                                ...field.state.value,
                                ind.id!,
                              ]);
                          }}
                          type="button"
                        >
                          {ind.name}
                        </button>
                      );
                    })}
                  </div>
                )}
                name="industries"
              />
            </div>
          </section>
        </div>
      </form>

      {/* Footer Actions - Moved form key aspects here to connect submit */}
      <div className="fixed right-0 bottom-0 left-0 z-40 flex items-center justify-between border-gray-200 border-t bg-white p-6 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)] md:left-72">
        <Button onClick={() => router.history.back()} variant="outline">
          CANCEL
        </Button>
        <form.Subscribe
          children={([canSubmit, isSubmitting]) => (
            <Button
              className="min-w-[160px] bg-black font-bold text-xs uppercase tracking-widest hover:bg-gray-800"
              disabled={!canSubmit || isSubmitting || updateMutation.isPending}
              onClick={form.handleSubmit}
            >
              {isSubmitting || updateMutation.isPending ? (
                <>
                  <Loader2 className="mr-2 h-3 w-3 animate-spin" /> Saving...
                </>
              ) : (
                "Save Changes"
              )}
            </Button>
          )}
          selector={(state) => [state.canSubmit, state.isSubmitting]}
        />
      </div>
    </div>
  );
}
