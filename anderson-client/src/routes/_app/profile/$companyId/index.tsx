import { useForm } from "@tanstack/react-form";
import { useMutation } from "@tanstack/react-query";
import { createFileRoute, useRouter } from "@tanstack/react-router";
import {
  Briefcase,
  Building,
  FileText,
  Globe,
  Loader2,
  MapPin,
  MonitorCloud,
  Pencil,
  Plus,
  Save,
  Trash2,
  User,
} from "lucide-react";
import { useState } from "react";
import z from "zod";
import type {
  CapabilityDto,
  CompanyProfileDto,
  UpdateCompanyCommand,
} from "@/api/types.gen";
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
import { Textarea } from "@/components/ui/textarea";
import { usePrefetchReferenceData } from "@/hooks/useReferenceData";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/_app/profile/$companyId/")({
  component: ProfileEdit,
  loader: async ({ params }) => {
    // Only fetch company profile, reference data will be cached via hooks
    const company = await callApi({
      data: {
        fn: "getApiCompaniesByIdProfile",
        args: { path: { id: params.companyId } },
      },
    });

    return {
      company: company || {},
    };
  },
});

function ProfileEdit() {
  const { companyId } = Route.useParams();
  const { company } = Route.useLoaderData();

  // Use cached reference data hooks
  const {
    capabilities: capabilitiesQuery,
    industries: industriesQuery,
    countries: countriesQuery,
    regions: regionsQuery,
    serviceTypes: serviceTypesQuery,
    isLoading,
    isError,
  } = usePrefetchReferenceData();

  const initialCompany = company as CompanyProfileDto;
  const router = useRouter();

  // Type assertions after null checks - TypeScript now knows these are defined
  const capabilities = capabilitiesQuery.data;
  const industries = industriesQuery.data;
  const countries = countriesQuery.data;
  const regions = regionsQuery.data;
  const serviceTypes = serviceTypesQuery.data;

  // Refetch handler to update view after mutations
  const refreshData = () => {
    router.invalidate();
  };

  // --- AI Scrape State ---
  const [scrapeUrl, setScrapeUrl] = useState(initialCompany?.websiteUrl || "");
  const scrapeMutation = useMutation({
    mutationFn: async (url: string) => {
      // Note: putApiCompaniesScrapeWebsite needs a body, checking signature...
      // Signature: (options) => ... options.body is ScrapeWebsiteCommand { url: string }
      return callApi({
        data: { fn: "putApiCompaniesScrapeWebsite", args: { body: { url } } },
      });
    },
    onSuccess: () => {
      refreshData();
      alert("AI Sync started! Your data will be updated shortly.");
    },
    onError: (err) => {
      console.error(err);
      alert("Failed to start AI Sync.");
    },
  });

  // --- Main Form (General Info) ---
  const form = useForm({
    defaultValues: {
      name: initialCompany?.name || "",
      shortDescription: initialCompany?.shortDescription || "",
      fullDescription: initialCompany?.fullDescription || "",
      websiteUrl: initialCompany?.websiteUrl || "",
      employeeCount: initialCompany?.employeeCount || 0,
      serviceTypeId: initialCompany?.serviceTypeId || "",
    },
    validators: {
      onSubmit: ({ value }: { value: UpdateCompanyCommand }) => {
        const schema = z.object({
          name: z.string().min(1, "Name is required"),
          shortDescription: z.string().optional(),
          fullDescription: z.string().optional(),
          websiteUrl: z
            .string()
            .url("Invalid URL")
            .optional()
            .or(z.literal("")),
          employeeCount: z.number().min(0).optional(),
          serviceTypeId: z.string().optional(),
        });

        const result = schema.safeParse(value);
        if (!result.success) {
          const errors: Record<string, string> = {};
          result.error.issues.forEach((issue) => {
            if (issue.path[0]) {
              errors[issue.path[0] as string] = issue.message;
            }
          });
          return errors;
        }
        return undefined;
      },
    },
    onSubmit: async ({ value }: { value: UpdateCompanyCommand }) => {
      try {
        // Save Capabilities and Industries first, to ensure embedding are created with the latest data
        await callApi({
          data: {
            fn: "putApiCompaniesByIdCapabilities",
            args: {
              path: { id: companyId },
              body: { id: companyId, capabilityIds: selectedCapabilityIds },
            },
          },
        });

        await callApi({
          data: {
            fn: "putApiCompaniesByIdIndustries",
            args: {
              path: { id: companyId },
              body: { id: companyId, industryIds: selectedIndustryIds },
            },
          },
        });

        // Save Company
        await callApi({
          data: {
            fn: "putApiCompaniesById",
            args: {
              path: { id: companyId },
              body: {
                id: companyId,
                name: value.name,
                shortDescription: value.shortDescription,
                fullDescription: value.fullDescription,
                websiteUrl: value.websiteUrl,
                employeeCount: value.employeeCount,
                serviceTypeId: value.serviceTypeId,
              },
            },
          },
        });

        alert("Profile saved successfully!");
        refreshData();
      } catch (error) {
        console.error(error);
        alert("Failed to save changes.");
      }
    },
    // validatorAdapter: zodValidator(),
  } as any); // Cast to any to avoid complex form typing issues for now

  // --- Capabilities & Industries State ---
  const [selectedCapabilityIds, setSelectedCapabilityIds] = useState<string[]>(
    initialCompany?.capabilities?.map((c) => c.id!).filter(Boolean) || []
  );
  const [selectedIndustryIds, setSelectedIndustryIds] = useState<string[]>(
    initialCompany?.industries?.map((c) => c.id!).filter(Boolean) || []
  );

  // --- Locations State ---
  const [isAddingLocation, setIsAddingLocation] = useState(false);
  const [newLocation, setNewLocation] = useState({
    name: "",
    regionId: "",
    countryId: "",
    isHeadOffice: false,
  });
  const [editingLocationId, setEditingLocationId] = useState<string | null>(
    null
  );
  const [editLocation, setEditLocation] = useState({
    name: "",
    regionId: "",
    countryId: "",
    isHeadOffice: false,
  });

  const addLocationMutation = useMutation({
    mutationFn: async () => {
      return callApi({
        data: {
          fn: "postApiCompaniesByIdLocation",
          args: {
            path: { id: companyId },
            body: {
              id: companyId,
              name: newLocation.name,
              regionId: newLocation.regionId,
              countryId: newLocation.countryId,
              isHeadOffice: newLocation.isHeadOffice,
            },
          },
        },
      });
    },
    onSuccess: () => {
      setIsAddingLocation(false);
      setNewLocation({
        name: "",
        regionId: "",
        countryId: "",
        isHeadOffice: false,
      });
      refreshData();
    },
  });

  const deleteLocationMutation = useMutation({
    mutationFn: async (locationId: string) => {
      return callApi({
        data: {
          fn: "deleteApiCompaniesByIdLocation",
          args: { path: { id: companyId }, query: { locationId } },
        },
      });
    },
    onSuccess: () => {
      refreshData();
    },
  });

  const updateLocationMutation = useMutation({
    mutationFn: async (locationId: string) => {
      return callApi({
        data: {
          fn: "putApiCompaniesByIdLocation",
          args: {
            path: { id: companyId },
            body: {
              id: companyId,
              locationId,
              name: editLocation.name,
              regionId: editLocation.regionId,
              countryId: editLocation.countryId,
              isHeadOffice: editLocation.isHeadOffice,
            },
          },
        },
      });
    },
    onSuccess: () => {
      setEditingLocationId(null);
      setEditLocation({
        name: "",
        regionId: "",
        countryId: "",
        isHeadOffice: false,
      });
      refreshData();
    },
  });

  const startEditLocation = (loc: any) => {
    setEditingLocationId(loc.id!);
    setEditLocation({
      name: loc.name || "",
      regionId: loc.regionId || "",
      countryId: loc.countryId || "",
      isHeadOffice: loc.isHeadOffice,
    });
  };

  // --- Contacts State ---
  const [isAddingContact, setIsAddingContact] = useState(false);
  const [newContact, setNewContact] = useState({
    firstName: "",
    lastName: "",
    email: "",
    position: "",
  });

  // --- Invites State ---
  const [isAddingInvite, setIsAddingInvite] = useState(false);
  const [newInvite, setNewInvite] = useState({ name: "", email: "" });

  const addContactMutation = useMutation({
    mutationFn: async () => {
      // PostApiCompaniesByIdContactData
      return callApi({
        data: {
          fn: "postApiCompaniesByIdContact",
          args: {
            path: { id: companyId },
            body: {
              id: companyId,
              firstName: newContact.firstName,
              lastName: newContact.lastName,
              emailAddress: newContact.email,
              companyPosition: newContact.position,
            },
          },
        },
      });
    },
    onSuccess: () => {
      setIsAddingContact(false);
      setNewContact({ firstName: "", lastName: "", email: "", position: "" });
      refreshData();
    },
  });

  const deleteContactMutation = useMutation({
    mutationFn: async (contactId: string) => {
      return callApi({
        data: {
          fn: "deleteApiCompaniesByIdContact",
          args: { path: { id: companyId }, query: { contactId } },
        },
      });
    },
    onSuccess: () => {
      refreshData();
    },
  });

  // --- ApplicationUser Mutations ---
  const deleteUserMutation = useMutation({
    mutationFn: async (userId: string) => {
      return callApi({
        data: {
          fn: "deleteApiCompaniesByIdUser",
          args: { path: { id: companyId }, query: { userId } },
        },
      });
    },
    onSuccess: () => {
      refreshData();
    },
  });

  // --- Invite Mutations ---
  const addInviteMutation = useMutation({
    mutationFn: async () => {
      return callApi({
        data: {
          fn: "postApiInvites",
          args: {
            body: {
              email: newInvite.email,
              companyId,
            },
          },
        },
      });
    },
    onSuccess: () => {
      setIsAddingInvite(false);
      setNewInvite({ name: "", email: "" });
      refreshData();
    },
  });

  const deleteInviteMutation = useMutation({
    mutationFn: async (inviteId: string) => {
      return callApi({
        data: {
          fn: "deleteApiInvitesById",
          args: {
            path: { id: inviteId },
          },
        },
      });
    },
    onSuccess: () => {
      refreshData();
    },
  });

  const [editingContactId, setEditingContactId] = useState<string | null>(null);
  const [editContact, setEditContact] = useState({
    firstName: "",
    lastName: "",
    email: "",
    position: "",
  });

  const updateContactMutation = useMutation({
    mutationFn: async (contactId: string) => {
      return callApi({
        data: {
          fn: "putApiCompaniesByIdContact",
          args: {
            path: { id: companyId },
            body: {
              id: companyId,
              contactId,
              firstName: editContact.firstName,
              lastName: editContact.lastName,
              emailAddress: editContact.email,
              companyPosition: editContact.position,
            },
          },
        },
      });
    },
    onSuccess: () => {
      setEditingContactId(null);
      setEditContact({ firstName: "", lastName: "", email: "", position: "" });
      refreshData();
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

  const startEditContact = (contact: any) => {
    setEditingContactId(contact.id!);
    setEditContact({
      firstName: contact.firstName || "",
      lastName: contact.lastName || "",
      email: contact.emailAddress || "",
      position: contact.companyPosition || "",
    });
  };

  return (
    <div className="animate-fade-in space-y-8 pb-20">
      {/* Header */}
      <header className="border-gray-200 border-b pb-6">
        <h2 className="mb-3 font-serif text-4xl text-black">
          Edit Firm Profile
        </h2>
        <p className="font-light text-gray-500 text-lg">
          Update your firm's details, capabilities, and global presence.
        </p>
      </header>

      {/* AI Auto-Sync */}
      <div className="relative overflow-hidden rounded-lg bg-black p-8 text-white">
        <div className="absolute top-0 right-0 translate-x-1/2 -translate-y-1/2 transform rounded-full bg-red-600/10 p-32 blur-3xl" />
        <div className="relative z-10">
          <h3 className="mb-4 flex items-center gap-3 font-bold text-xl uppercase tracking-widest">
            <Globe className="h-5 w-5 text-red-500" /> AI Auto-Sync
          </h3>
          <p className="mb-6 max-w-xl text-gray-400 text-sm leading-relaxed">
            Enter your website URL to automatically update your service
            description and capabilities using AI. We'll analyze your public
            presence to suggest relevant tags and descriptions.
          </p>
          <div className="flex max-w-2xl flex-col gap-4 md:flex-row">
            <div className="flex-1">
              <input
                className="w-full rounded border border-white/20 bg-white/10 px-4 py-3 text-sm text-white placeholder-gray-500 transition-colors focus:border-red-600 focus:outline-none"
                onChange={(e) => setScrapeUrl(e.target.value)}
                placeholder="https://www.yourcompany.com"
                type="text"
                value={scrapeUrl}
              />
            </div>
            <button
              className="whitespace-nowrap bg-red-600 px-8 py-3 font-bold text-white text-xs uppercase tracking-[0.2em] transition-colors hover:bg-red-700 disabled:cursor-not-allowed disabled:opacity-50"
              disabled={scrapeMutation.isPending || !scrapeUrl}
              onClick={() => scrapeMutation.mutate(scrapeUrl)}
            >
              {scrapeMutation.isPending ? "Syncing..." : "Sync Firm Data"}
            </button>
          </div>
        </div>
      </div>

      {/* Main Form */}
      <div className="grid grid-cols-1 gap-12 lg:grid-cols-3">
        {/* Left Column: General Info */}
        <div className="space-y-12 lg:col-span-2">
          {/* General Information */}
          <section className="space-y-6">
            <h3 className="border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              General Information
            </h3>

            <form.Field
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor={field.name}>Firm Name</Label>
                  <Input
                    id={field.name}
                    name={field.name}
                    onBlur={field.handleBlur}
                    onChange={(e: any) => field.handleChange(e.target.value)}
                    value={field.state.value as string}
                  />
                </div>
              )}
              name="name"
            />

            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              <form.Field
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Website Address</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      onBlur={field.handleBlur}
                      onChange={(e: any) => field.handleChange(e.target.value)}
                      value={field.state.value as string}
                    />
                  </div>
                )}
                name="websiteUrl"
              />
              <form.Field
                children={(field) => (
                  <div className="space-y-2">
                    <Label htmlFor={field.name}>Employee Count</Label>
                    <Input
                      id={field.name}
                      name={field.name}
                      onBlur={field.handleBlur}
                      onChange={(e: any) =>
                        field.handleChange(Number(e.target.value))
                      }
                      type="number"
                      value={field.state.value as string}
                    />
                  </div>
                )}
                name="employeeCount"
              />
            </div>

            <form.Field
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor={field.name}>Short Description</Label>
                  <Textarea
                    id={field.name}
                    name={field.name}
                    onBlur={field.handleBlur}
                    onChange={(e: any) => field.handleChange(e.target.value)}
                    value={field.state.value as string}
                  />
                  <p className="text-[10px] text-gray-500 uppercase tracking-wide">
                    Brief summary for card views
                  </p>
                </div>
              )}
              name="shortDescription"
            />

            <form.Field
              children={(field) => (
                <div className="space-y-2">
                  <Label htmlFor={field.name}>Firm Description</Label>
                  <Textarea
                    className="min-h-[150px]"
                    id={field.name}
                    name={field.name}
                    onBlur={field.handleBlur}
                    onChange={(e: any) => field.handleChange(e.target.value)}
                    value={field.state.value as string}
                  />
                </div>
              )}
              name="fullDescription"
            />
          </section>

          {/* Detailed Lists (Locations & Contacts) */}

          {/* Locations */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-gray-100 border-b pb-2">
              <h3 className="font-bold text-lg uppercase tracking-widest">
                Office Locations
              </h3>
            </div>

            <div className="space-y-4">
              {initialCompany?.locations?.map((loc) => (
                <div key={loc.id}>
                  {editingLocationId === loc.id ? (
                    <div className="space-y-4 border border-blue-200 bg-blue-50 p-6">
                      <div className="space-y-2">
                        <Label>Office Name</Label>
                        <Input
                          onChange={(e: any) =>
                            setEditLocation({
                              ...editLocation,
                              name: e.target.value,
                            })
                          }
                          placeholder="e.g. London HQ"
                          value={editLocation.name}
                        />
                      </div>
                      <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <Label>Region</Label>
                          <Select
                            onValueChange={(v: string) =>
                              setEditLocation({ ...editLocation, regionId: v })
                            }
                            value={editLocation.regionId}
                          >
                            <SelectTrigger>
                              <SelectValue placeholder="Select Region" />
                            </SelectTrigger>
                            <SelectContent>
                              {regions?.map((r: any) => (
                                <SelectItem key={r.id} value={r.id!}>
                                  {r.name}
                                </SelectItem>
                              ))}
                            </SelectContent>
                          </Select>
                        </div>
                        <div className="space-y-2">
                          <Label>Country</Label>
                          <Select
                            onValueChange={(v: string) =>
                              setEditLocation({ ...editLocation, countryId: v })
                            }
                            value={editLocation.countryId}
                          >
                            <SelectTrigger>
                              <SelectValue placeholder="Select Country" />
                            </SelectTrigger>
                            <SelectContent>
                              {countries
                                ?.filter(
                                  (c: any) =>
                                    !editLocation.regionId ||
                                    c.regionId === editLocation.regionId
                                )
                                .map((c: any) => (
                                  <SelectItem key={c.id} value={c.id!}>
                                    {c.name}
                                  </SelectItem>
                                ))}
                            </SelectContent>
                          </Select>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <Switch
                          checked={editLocation.isHeadOffice}
                          id={`edit-head-office-${loc.id}`}
                          onCheckedChange={(c) =>
                            setEditLocation({
                              ...editLocation,
                              isHeadOffice: c,
                            })
                          }
                        />
                        <Label htmlFor={`edit-head-office-${loc.id}`}>
                          This is the Head Office
                        </Label>
                      </div>
                      <div className="flex justify-end gap-2 pt-2">
                        <Button
                          onClick={() => setEditingLocationId(null)}
                          size="sm"
                          variant="outline"
                        >
                          Cancel
                        </Button>
                        <Button
                          className="bg-blue-600 hover:bg-blue-700"
                          disabled={
                            updateLocationMutation.isPending ||
                            !editLocation.name
                          }
                          onClick={() => updateLocationMutation.mutate(loc.id!)}
                          size="sm"
                        >
                          {updateLocationMutation.isPending
                            ? "Saving..."
                            : "Save Changes"}
                        </Button>
                      </div>
                    </div>
                  ) : (
                    <div className="flex items-center justify-between rounded-sm border border-gray-100 bg-gray-50 p-4">
                      <div className="flex items-center gap-4">
                        <div className="flex h-8 w-8 items-center justify-center border border-gray-200 bg-white">
                          <MapPin className="h-4 w-4 text-gray-400" />
                        </div>
                        <div>
                          <div className="font-bold text-gray-900 text-sm">
                            {loc.name}{" "}
                            {loc.isHeadOffice && (
                              <span className="ml-2 bg-black px-2 py-0.5 text-[10px] text-white uppercase tracking-wide">
                                Head Office
                              </span>
                            )}
                          </div>
                          <div className="mt-1 text-gray-500 text-xs uppercase tracking-wide">
                            {
                              countries?.find((c) => c.id === loc.countryId)
                                ?.name
                            }
                            ,{" "}
                            {regions?.find((r) => r.id === loc.regionId)?.name}
                          </div>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <button
                          className="text-gray-400 transition-colors hover:text-blue-600"
                          onClick={() => startEditLocation(loc)}
                        >
                          <Pencil className="h-4 w-4" />
                        </button>
                        <button
                          className="text-gray-400 transition-colors hover:text-red-600"
                          onClick={() => {
                            if (
                              confirm(
                                "Are you sure you want to delete this location?"
                              )
                            ) {
                              deleteLocationMutation.mutate(loc.id!);
                            }
                          }}
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    </div>
                  )}
                </div>
              ))}

              {isAddingLocation ? (
                <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
                  <div className="space-y-2">
                    <Label>Office Name</Label>
                    <Input
                      onChange={(e: any) =>
                        setNewLocation({ ...newLocation, name: e.target.value })
                      }
                      placeholder="e.g. London HQ"
                      value={newLocation.name}
                    />
                  </div>
                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label>Region</Label>
                      <Select
                        onValueChange={(v: string) =>
                          setNewLocation({ ...newLocation, regionId: v })
                        }
                        value={newLocation.regionId}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Select Region" />
                        </SelectTrigger>
                        <SelectContent>
                          {regions?.map((r: any) => (
                            <SelectItem key={r.id} value={r.id!}>
                              {r.name}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                    </div>
                    <div className="space-y-2">
                      <Label>Country</Label>
                      <Select
                        onValueChange={(v: string) =>
                          setNewLocation({ ...newLocation, countryId: v })
                        }
                        value={newLocation.countryId}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Select Country" />
                        </SelectTrigger>
                        <SelectContent>
                          {countries
                            ?.filter(
                              (c: any) =>
                                !newLocation.regionId ||
                                c.regionId === newLocation.regionId
                            )
                            .map((c: any) => (
                              <SelectItem key={c.id} value={c.id!}>
                                {c.name}
                              </SelectItem>
                            ))}
                        </SelectContent>
                      </Select>
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    <Switch
                      checked={newLocation.isHeadOffice}
                      id="header-office"
                      onCheckedChange={(c) =>
                        setNewLocation({ ...newLocation, isHeadOffice: c })
                      }
                    />
                    <Label htmlFor="head-office">This is the Head Office</Label>
                  </div>
                  <div className="flex justify-end gap-2 pt-2">
                    <Button
                      onClick={() => setIsAddingLocation(false)}
                      size="sm"
                      variant="outline"
                    >
                      Cancel
                    </Button>
                    <Button
                      className="bg-black hover:bg-gray-800"
                      disabled={
                        addLocationMutation.isPending || !newLocation.name
                      }
                      onClick={() => addLocationMutation.mutate()}
                      size="sm"
                    >
                      {addLocationMutation.isPending
                        ? "Adding..."
                        : "Add Location"}
                    </Button>
                  </div>
                </div>
              ) : (
                <Button
                  className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
                  onClick={() => setIsAddingLocation(true)}
                  variant="outline"
                >
                  <Plus className="mr-2 h-4 w-4" /> Add Office Location
                </Button>
              )}
            </div>
          </section>

          {/* Key Personnel */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-gray-100 border-b pb-2">
              <h3 className="font-bold text-lg uppercase tracking-widest">
                Key Personnel
              </h3>
            </div>

            <div className="space-y-4">
              {initialCompany?.contacts?.map((contact) => (
                <div key={contact.id}>
                  {editingContactId === contact.id ? (
                    <div className="space-y-4 border border-blue-200 bg-blue-50 p-6">
                      <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <Label>First Name</Label>
                          <Input
                            onChange={(e: any) =>
                              setEditContact({
                                ...editContact,
                                firstName: e.target.value,
                              })
                            }
                            value={editContact.firstName}
                          />
                        </div>
                        <div className="space-y-2">
                          <Label>Last Name</Label>
                          <Input
                            onChange={(e: any) =>
                              setEditContact({
                                ...editContact,
                                lastName: e.target.value,
                              })
                            }
                            value={editContact.lastName}
                          />
                        </div>
                      </div>
                      <div className="space-y-2">
                        <Label>Job Title</Label>
                        <Input
                          onChange={(e: any) =>
                            setEditContact({
                              ...editContact,
                              position: e.target.value,
                            })
                          }
                          value={editContact.position}
                        />
                      </div>
                      <div className="space-y-2">
                        <Label>Email Address</Label>
                        <Input
                          onChange={(e: any) =>
                            setEditContact({
                              ...editContact,
                              email: e.target.value,
                            })
                          }
                          value={editContact.email}
                        />
                      </div>
                      <div className="flex justify-end gap-2 pt-2">
                        <Button
                          onClick={() => setEditingContactId(null)}
                          size="sm"
                          variant="outline"
                        >
                          Cancel
                        </Button>
                        <Button
                          className="bg-blue-600 hover:bg-blue-700"
                          disabled={
                            updateContactMutation.isPending ||
                            !editContact.firstName
                          }
                          onClick={() =>
                            updateContactMutation.mutate(contact.id!)
                          }
                          size="sm"
                        >
                          {updateContactMutation.isPending
                            ? "Saving..."
                            : "Save Changes"}
                        </Button>
                      </div>
                    </div>
                  ) : (
                    <div className="flex items-center justify-between rounded-sm border border-gray-100 bg-gray-50 p-4">
                      <div className="flex items-center gap-4">
                        <div className="flex h-8 w-8 items-center justify-center rounded-full border border-gray-200 bg-white">
                          <User className="h-4 w-4 text-gray-400" />
                        </div>
                        <div>
                          <div className="font-bold text-gray-900 text-sm">
                            {contact.firstName} {contact.lastName}
                          </div>
                          <div className="mt-1 text-gray-500 text-xs uppercase tracking-wide">
                            {contact.companyPosition || "No Title"} •{" "}
                            {contact.emailAddress}
                          </div>
                        </div>
                      </div>
                      <div className="flex items-center gap-2">
                        <button
                          className="text-gray-400 transition-colors hover:text-blue-600"
                          onClick={() => startEditContact(contact)}
                        >
                          <Pencil className="h-4 w-4" />
                        </button>
                        <button
                          className="text-gray-400 transition-colors hover:text-red-600"
                          onClick={() => {
                            if (
                              confirm(
                                "Are you sure you want to delete this contact?"
                              )
                            ) {
                              deleteContactMutation.mutate(contact.id!);
                            }
                          }}
                        >
                          <Trash2 className="h-4 w-4" />
                        </button>
                      </div>
                    </div>
                  )}
                </div>
              ))}

              {isAddingContact ? (
                <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
                  <div className="grid grid-cols-2 gap-4">
                    <div className="space-y-2">
                      <Label>First Name</Label>
                      <Input
                        onChange={(e: any) =>
                          setNewContact({
                            ...newContact,
                            firstName: e.target.value,
                          })
                        }
                        value={newContact.firstName}
                      />
                    </div>
                    <div className="space-y-2">
                      <Label>Last Name</Label>
                      <Input
                        onChange={(e: any) =>
                          setNewContact({
                            ...newContact,
                            lastName: e.target.value,
                          })
                        }
                        value={newContact.lastName}
                      />
                    </div>
                  </div>
                  <div className="space-y-2">
                    <Label>Job Title</Label>
                    <Input
                      onChange={(e: any) =>
                        setNewContact({
                          ...newContact,
                          position: e.target.value,
                        })
                      }
                      value={newContact.position}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Email Address</Label>
                    <Input
                      onChange={(e: any) =>
                        setNewContact({ ...newContact, email: e.target.value })
                      }
                      value={newContact.email}
                    />
                  </div>
                  <div className="flex justify-end gap-2 pt-2">
                    <Button
                      onClick={() => setIsAddingContact(false)}
                      size="sm"
                      variant="outline"
                    >
                      Cancel
                    </Button>
                    <Button
                      className="bg-black hover:bg-gray-800"
                      disabled={
                        addContactMutation.isPending || !newContact.firstName
                      }
                      onClick={() => addContactMutation.mutate()}
                      size="sm"
                    >
                      {addContactMutation.isPending
                        ? "Adding..."
                        : "Add Person"}
                    </Button>
                  </div>
                </div>
              ) : (
                <Button
                  className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
                  onClick={() => setIsAddingContact(true)}
                  variant="outline"
                >
                  <Plus className="mr-2 h-4 w-4" /> Add Key Person
                </Button>
              )}
            </div>
          </section>

          {/* Application Users */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-gray-100 border-b pb-2">
              <h3 className="font-bold text-lg uppercase tracking-widest">
                Application Users
              </h3>
            </div>

            <div className="space-y-4">
              {initialCompany?.applicationIdentityUsers?.map(
                (user, index: number) => (
                  <div
                    className="flex items-center justify-between rounded-sm border border-gray-100 bg-gray-50 p-4"
                    key={user.id || index}
                  >
                    <div className="flex items-center gap-4">
                      <div className="flex h-8 w-8 items-center justify-center rounded-full border border-gray-200 bg-white">
                        <User className="h-4 w-4 text-gray-400" />
                      </div>
                      <div>
                        <div className="font-bold text-gray-900 text-sm">
                          {user.email || "Unknown User"}
                        </div>
                        <div className="mt-1 text-gray-500 text-xs uppercase tracking-wide">
                          {user.userName || "Application User"}
                        </div>
                      </div>
                    </div>
                    <div className="flex items-center gap-2">
                      <button
                        className="text-gray-400 transition-colors hover:text-red-600"
                        disabled={deleteUserMutation.isPending}
                        onClick={() => {
                          if (
                            confirm(
                              "Are you sure you want to remove this user from the company?"
                            )
                          ) {
                            deleteUserMutation.mutate(user.id!);
                          }
                        }}
                      >
                        <Trash2 className="h-4 w-4" />
                      </button>
                    </div>
                  </div>
                )
              )}

              {(!initialCompany?.applicationIdentityUsers ||
                initialCompany.applicationIdentityUsers.length === 0) && (
                  <div className="border border-gray-200 bg-gray-50 p-6 text-center">
                    <p className="text-gray-500 text-sm">
                      No application users found.
                    </p>
                  </div>
                )}
            </div>
          </section>

          {/* Company Invites */}
          <section className="space-y-6">
            <div className="flex items-center justify-between border-gray-100 border-b pb-2">
              <h3 className="font-bold text-lg uppercase tracking-widest">
                Pending Invites
              </h3>
            </div>

            <div className="space-y-4">
              {initialCompany?.invites?.map((invite) => (
                <div
                  className="flex items-center justify-between rounded-sm border border-gray-100 bg-gray-50 p-4"
                  key={invite.id}
                >
                  <div className="flex items-center gap-4">
                    <div className="flex h-8 w-8 items-center justify-center rounded-full border border-gray-200 bg-white">
                      <User className="h-4 w-4 text-gray-400" />
                    </div>
                    <div>
                      <div className="font-bold text-gray-900 text-sm">
                        {invite.name}
                      </div>
                      <div className="mt-1 text-gray-500 text-xs uppercase tracking-wide">
                        {invite.email} • Pending Invite
                      </div>
                    </div>
                  </div>
                  <div className="flex items-center gap-2">
                    <button
                      className="text-gray-400 transition-colors hover:text-red-600"
                      disabled={deleteInviteMutation.isPending}
                      onClick={() => {
                        if (
                          confirm(
                            "Are you sure you want to cancel this invite?"
                          )
                        ) {
                          deleteInviteMutation.mutate(invite.id!);
                        }
                      }}
                    >
                      <Trash2 className="h-4 w-4" />
                    </button>
                  </div>
                </div>
              ))}

              {isAddingInvite ? (
                <div className="space-y-4 border border-gray-200 bg-gray-50 p-6">
                  <div className="space-y-2">
                    <Label>Full Name</Label>
                    <Input
                      onChange={(e: any) =>
                        setNewInvite({ ...newInvite, name: e.target.value })
                      }
                      placeholder="Enter full name"
                      value={newInvite.name}
                    />
                  </div>
                  <div className="space-y-2">
                    <Label>Email Address</Label>
                    <Input
                      onChange={(e: any) =>
                        setNewInvite({ ...newInvite, email: e.target.value })
                      }
                      placeholder="Enter email address"
                      type="email"
                      value={newInvite.email}
                    />
                  </div>
                  <div className="flex justify-end gap-2 pt-2">
                    <Button
                      onClick={() => setIsAddingInvite(false)}
                      size="sm"
                      variant="outline"
                    >
                      Cancel
                    </Button>
                    <Button
                      className="bg-black hover:bg-gray-800"
                      disabled={
                        addInviteMutation.isPending ||
                        !newInvite.name ||
                        !newInvite.email
                      }
                      onClick={() => addInviteMutation.mutate()}
                      size="sm"
                    >
                      {addInviteMutation.isPending
                        ? "Sending..."
                        : "Send Invite"}
                    </Button>
                  </div>
                </div>
              ) : (
                <Button
                  className="w-full border-gray-300 border-dashed text-gray-500 hover:border-gray-900 hover:text-gray-900"
                  onClick={() => setIsAddingInvite(true)}
                  variant="outline"
                >
                  <Plus className="mr-2 h-4 w-4" /> Send Invite
                </Button>
              )}
            </div>
          </section>
        </div>

        {/* Right Column: Tags & Metadata */}
        <div className="space-y-12">
          {/* Service Type */}
          <section className="space-y-6">
            <h3 className="mb-0 flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <MonitorCloud className="h-4 w-4" /> Service Type
            </h3>
            <div className="w-full border border-gray-200 bg-white p-6 pt-4 shadow-sm">
              <p className="mb-2 text-gray-400 text-xs italic">
                Select the primary service your firm provides.
              </p>
              <form.Field
                children={(field) => (
                  <div>
                    <Select
                      onValueChange={field.handleChange}
                      value={field.state.value as string}
                    >
                      <SelectTrigger className="w-full">
                        <SelectValue placeholder="Select a service type" />
                      </SelectTrigger>
                      <SelectContent className="w-full">
                        {serviceTypes?.map((serviceType) => (
                          <SelectItem
                            key={serviceType.id}
                            value={serviceType.id!}
                          >
                            {serviceType.name}
                          </SelectItem>
                        ))}
                      </SelectContent>
                    </Select>
                    {field.state.meta.errors && (
                      <p className="mt-1 text-red-500 text-xs">
                        {field.state.meta.errors}
                      </p>
                    )}
                  </div>
                )}
                name="serviceTypeId"
              />
            </div>
          </section>

          {/* Reports */}
          <section className="space-y-6">
            <h3 className="mb-0 flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <FileText className="h-4 w-4" /> Quarterly Reports
            </h3>
            <div className="w-full border border-gray-200 bg-white p-6 pt-4 shadow-sm">
              <p className="mb-4 text-gray-400 text-xs italic">
                Manage and submit quarterly business reports for your firm.
              </p>
              <Button
                className="w-full bg-blue-600 font-bold text-white text-xs uppercase tracking-widest hover:bg-blue-700"
                onClick={() =>
                  router.navigate({ to: `/profile/${companyId}/reports` })
                }
              >
                <FileText className="mr-2 h-4 w-4" />
                Manage Reports
              </Button>
            </div>
          </section>

          {/* Core Skills */}
          <section className="space-y-6">
            <h3 className="mb-0 flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <Briefcase className="h-4 w-4" /> Core Skills
            </h3>
            <div className="border border-gray-200 bg-white p-6 pt-4 shadow-sm">
              <Combobox
                items={capabilities || []}
                multiple
                onValueChange={(value: unknown) => {
                  const selectedCapabilities = value as CapabilityDto[];
                  setSelectedCapabilityIds(
                    selectedCapabilities.map((cap) => cap.id!)
                  );
                }}
                value={
                  capabilities?.filter((cap) =>
                    selectedCapabilityIds.includes(cap.id!)
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
                            className="rounded-none border border-black bg-black px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider transition-all"
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
                  Select all that apply to your firm.
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
          <section className="space-y-6">
            <h3 className="mb-0 flex items-center gap-2 border-gray-100 border-b pb-2 font-bold text-lg uppercase tracking-widest">
              <Building className="h-4 w-4" /> Primary Industries
            </h3>
            <div className="border border-gray-200 bg-white p-6 shadow-sm">
              <Combobox
                items={industries || []}
                multiple
                onValueChange={(value: unknown) => {
                  const selectedIndustries = value as any[];
                  setSelectedIndustryIds(
                    selectedIndustries.map((ind) => ind.id!)
                  );
                }}
                value={
                  industries?.filter((ind) =>
                    selectedIndustryIds.includes(ind.id!)
                  ) || []
                }
              >
                <ComboboxChips className="mb-4 border-0 p-0 shadow-none">
                  <ComboboxValue>
                    {(value: any[]) => (
                      <>
                        {value.length === 0 && (
                          <p className="my-2 text-gray-400 text-xs italic">
                            No industries selected.
                          </p>
                        )}
                        {value.map((industry) => (
                          <ComboboxChip
                            aria-label={industry.name}
                            className="rounded-none border border-red-600 bg-red-600 px-3 py-1.5 font-bold text-[10px] text-white uppercase tracking-wider transition-all"
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
                  Select the primary industries your firm serves.
                </p>
                <ComboboxControl>
                  <ComboboxValue>
                    <ComboboxInput placeholder="Search and select industries..." />
                  </ComboboxValue>
                </ComboboxControl>

                <ComboboxContent>
                  <ComboboxEmpty>No industries found.</ComboboxEmpty>
                  <ComboboxList>
                    {(industry: any) => (
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
      </div>

      {/* Footer Actions */}
      <div className="fixed right-0 bottom-0 left-0 z-40 flex items-center justify-between border-gray-200 border-t bg-white p-6 shadow-[0_-4px_6px_-1px_rgba(0,0,0,0.05)] md:left-72">
        <span className="hidden font-medium text-gray-400 text-xs uppercase tracking-widest md:inline-block">
          Last saved: {new Date().toLocaleDateString()}
        </span>
        <div className="flex w-full items-center justify-end gap-4 md:w-auto">
          <Button onClick={() => router.history.back()} variant="outline">
            CANCEL
          </Button>
          <form.Subscribe
            children={([canSubmit, isSubmitting]) => (
              <Button
                className="min-w-[160px] bg-red-600 font-bold text-xs uppercase tracking-widest hover:bg-red-700"
                disabled={!canSubmit}
                onClick={form.handleSubmit}
              >
                {isSubmitting ? (
                  <>
                    Saving <Loader2 className="ml-2 h-3 w-3 animate-spin" />
                  </>
                ) : (
                  <>
                    Save Changes <Save className="ml-2 h-3 w-3" />
                  </>
                )}
              </Button>
            )}
            selector={(state) => [state.canSubmit, state.isSubmitting]}
          />
        </div>
      </div>
    </div>
  );
}
