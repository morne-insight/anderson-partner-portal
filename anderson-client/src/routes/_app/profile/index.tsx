import { useMutation } from "@tanstack/react-query";
import { createFileRoute, Link, useRouter } from "@tanstack/react-router";
import {
  Building2,
  ChevronRight,
  Globe,
  Loader2,
  Plus,
  Users,
} from "lucide-react";
import { useState } from "react";
import { toast } from "sonner";
import type { CompanyDto } from "@/api";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { callApi } from "@/server/proxy";

// const urlRegex = /^https?:\/\/(?:www\.)?[-a-zA-Z0-9@:%._+~#=]{1,256}\.[a-zA-Z]{2,}(?:[-a-zA-Z0-9()@:%_+.~#?&/=]*)$/;
const urlRegex =
  /^(https?:\/\/)?([\w\d-_]+)\.([\w\d-_.]+)\/?\??([^#\n\r]*)?#?([^\n\r]*)/;

export const Route = createFileRoute("/_app/profile/")({
  component: ProfileIndex,
  loader: async () => {
    const response = await callApi({ data: { fn: "getApiCompaniesMe" } });
    return { companies: response || [] };
  },
});

function ProfileIndex() {
  const { companies } = Route.useLoaderData();
  const router = useRouter();
  const [scrapeUrl, setScrapeUrl] = useState("");
  const [newFirmName, setNewFirmName] = useState("");

  const isValidUrl = (url: string): boolean => {
    const isValid = urlRegex.test(url);
    console.log(`is valid url (${url}):`, isValid);
    return isValid;
  };

  const refreshData = () => {
    router.invalidate();
  };

  const scrapeMutation = useMutation({
    mutationFn: async (url: string) => {
      return await callApi({
        data: { fn: "putApiCompaniesScrapeWebsite", args: { body: { url } } },
      });
    },
    onSuccess: (data) => {
      refreshData();
      if (data?.value) {
        router.navigate({
          to: "/profile/$companyId",
          params: { companyId: data.value },
        });
      }
    },
    onError: (err) => {
      console.error("Scrape failed", err);
      toast.error("Failed to sync website. Please try creating manually.");
    },
  });

  const createMutation = useMutation({
    mutationFn: async () => {
      return await callApi({
        data: {
          fn: "postApiCompanies",
          args: {
            body: {
              name: newFirmName || "New Firm",
              shortDescription: "",
              fullDescription: "",
              websiteUrl: "",
              employeeCount: 0,
              serviceTypes: [],
              capabilities: [],
              industries: [],
            },
          },
        },
      });
    },
    onSuccess: (data) => {
      refreshData();
      if (data?.value) {
        router.navigate({
          to: "/profile/$companyId",
          params: { companyId: data.value },
        });
      }
    },
    onError: (err) => {
      console.error("Create failed", err);
      toast.error("Failed to create profile.");
    },
  });

  const getHostname = (url: string) => {
    try {
      const normalizedUrl = url.startsWith("http") ? url : `https://${url}`;
      return new URL(normalizedUrl).hostname;
    } catch {
      return url;
    }
  };

  return (
    <div className="animate-fade-in space-y-8">
      <header className="border-gray-200 border-b pb-6">
        <h2 className="mb-3 font-serif text-4xl text-black">My Profile</h2>
        <p className="font-light text-gray-500 text-lg">
          Manage your firm's profile, capabilities, and presence in the Anderson
          network.
        </p>
      </header>

      <section>
        <div className="mb-6 flex items-center justify-between">
          <h3 className="flex items-center gap-2 font-bold text-black text-xl uppercase tracking-widest">
            <Building2 className="h-5 w-5" /> Linked Companies
          </h3>
        </div>

        {companies.length === 0 ? (
          <div className="grid grid-cols-1 gap-8 md:grid-cols-2">
            {/* AI Auto-Create */}
            <div className="relative flex flex-col justify-center overflow-hidden rounded-lg bg-black p-8 text-white">
              <div className="absolute top-0 right-0 translate-x-1/2 -translate-y-1/2 transform rounded-full bg-red-600/10 p-32 blur-3xl" />
              <div className="relative z-10 space-y-4">
                <div className="mb-2 flex h-12 w-12 items-center justify-center rounded-full bg-red-600">
                  <Globe className="h-6 w-6 text-white" />
                </div>
                <h3 className="font-serif text-2xl">Auto-Sync from Website</h3>
                <p className="text-gray-400 text-sm leading-relaxed">
                  Enter your website URL. Our AI will analyze your public
                  presence to create your profile and suggest relevant tags.
                </p>

                <div className="space-y-3 pt-4">
                  <Input
                    className="border-white/20 bg-white/10 text-white placeholder-gray-500 focus:border-red-600"
                    onChange={(e) => setScrapeUrl(e.target.value)}
                    placeholder="https://www.yourcompany.com"
                    value={scrapeUrl}
                  />
                  <Button
                    className="w-full bg-red-600 py-6 font-bold uppercase tracking-wider hover:bg-red-700"
                    disabled={
                      scrapeMutation.isPending || !isValidUrl(scrapeUrl)
                    }
                    onClick={() => scrapeMutation.mutate(scrapeUrl)}
                  >
                    {scrapeMutation.isPending ? (
                      <>
                        <Loader2 className="mr-2 h-4 w-4 animate-spin" />{" "}
                        Syncing...
                      </>
                    ) : (
                      "Sync & Create Profile"
                    )}
                  </Button>
                </div>
              </div>
            </div>

            {/* Manual Create */}
            <div className="flex flex-col items-center justify-center space-y-6 rounded-lg border border-gray-200 bg-gray-50 p-8 text-center">
              <div className="flex h-16 w-16 items-center justify-center rounded-full border border-gray-200 bg-white shadow-sm">
                <Plus className="h-8 w-8 text-gray-400" />
              </div>
              <div>
                <h3 className="mb-2 font-bold text-gray-900 text-xl">
                  Create Manually
                </h3>
                <p className="mx-auto mb-4 max-w-sm text-gray-500 text-sm">
                  Start from scratch. Manually enter your firm's details,
                  locations, and personnel to get listed in the directory.
                </p>
                <Input
                  className="mx-auto mt-4 mb-4 max-w-xs"
                  onChange={(e) => setNewFirmName(e.target.value)}
                  placeholder="Enter Firm Name"
                  value={newFirmName}
                />
              </div>
              <Button
                className="border-black px-8 py-6 text-black uppercase tracking-wider hover:bg-black hover:text-white"
                disabled={createMutation.isPending || !newFirmName}
                onClick={() => createMutation.mutate()}
                variant="outline"
              >
                {createMutation.isPending
                  ? "Creating..."
                  : "Create New Profile"}
              </Button>
            </div>
          </div>
        ) : (
          <div className="grid grid-cols-1 gap-6">
            {companies.map((company: CompanyDto) => (
              <div
                className="group flex flex-col items-center gap-8 border border-gray-200 bg-white p-8 transition-all duration-300 hover:shadow-lg md:flex-row"
                key={company.id}
              >
                <div className="flex-1">
                  <h3 className="mb-2 flex items-center gap-3 font-serif text-2xl text-black">
                    {company.name}
                  </h3>
                  <p className="mb-4 line-clamp-2 text-gray-500 text-sm">
                    {company.shortDescription || "No description provided."}
                  </p>

                  <div className="flex flex-wrap gap-6 font-medium text-gray-400 text-xs uppercase tracking-wider">
                    {company.websiteUrl && (
                      <div className="flex items-center gap-2">
                        <Globe className="h-4 w-4" />
                        <a
                          className="transition-colors hover:text-red-600"
                          href={
                            company.websiteUrl.startsWith("http")
                              ? company.websiteUrl
                              : `https://${company.websiteUrl}`
                          }
                          rel="noopener noreferrer"
                          target="_blank"
                        >
                          {getHostname(company.websiteUrl)}
                        </a>
                      </div>
                    )}
                    {company.employeeCount !== undefined && (
                      <div className="flex items-center gap-2">
                        <Users className="h-4 w-4" />
                        <span>{company.employeeCount} Employees</span>
                      </div>
                    )}
                  </div>
                </div>

                <div className="flex flex-col gap-2">
                  <Link
                    className="flex items-center gap-2 border border-black bg-white px-6 py-3 font-bold text-[10px] text-black uppercase tracking-[0.2em] transition-all hover:bg-black hover:text-white group-hover:border-red-600 group-hover:bg-red-600 group-hover:text-white"
                    params={{ companyId: company.id as string }}
                    to="/profile/$companyId"
                  >
                    Manage Firm <ChevronRight className="h-3 w-3" />
                  </Link>
                  <Link
                    className="flex items-center gap-2 border border-black bg-white px-6 py-3 font-bold text-[10px] text-black uppercase tracking-[0.2em] transition-all hover:bg-black hover:text-white group-hover:border-blue-600 group-hover:bg-blue-600 group-hover:text-white"
                    params={{ companyId: company.id as string }}
                    to="/profile/$companyId/reports"
                  >
                    Manage Reports <ChevronRight className="h-3 w-3" />
                  </Link>
                </div>
              </div>
            ))}

            {/* Add another company button - small version for when list exists */}
            {/* <div className="mt-8 flex justify-center">
              <Button
                className="gap-2 text-gray-400 text-xs uppercase tracking-widest hover:text-black"
                onClick={() => createMutation.mutate()}
                variant="ghost"
              >
                <Plus className="h-4 w-4" /> Add Another Firm
              </Button>
            </div> */}
          </div>
        )}
      </section>
    </div>
  );
}
