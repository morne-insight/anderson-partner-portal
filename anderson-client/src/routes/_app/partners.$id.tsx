/** biome-ignore-all lint/suspicious/noArrayIndexKey: <explanation> */

import { headingsPlugin, MDXEditor } from "@mdxeditor/editor";
import { ClientOnly, createFileRoute, Link } from "@tanstack/react-router";
import {
  ArrowLeft,
  Briefcase,
  Building,
  Globe,
  MapPin,
  Users,
} from "lucide-react";
import type { PartnerProfileDto } from "@/api";
import { ConnectRequestDialog } from "@/components/ConnectRequestDialog";
import { ReviewComponent } from "@/components/Reviews/ReviewComponent";
import { callApi } from "@/server/proxy";

export const Route = createFileRoute("/_app/partners/$id")({
  component: PartnerProfile,
  validateSearch: (search: Record<string, unknown>) => ({
    from: search.from as string | undefined,
  }),
  loader: async ({ params }) => {
    // const [partner, countries, regions] = await Promise.all([
    //   callApi({
    //     data: {
    //       fn: "getApiCompaniesByIdPartner",
    //       args: { path: { id: params.id } },
    //     },
    //   }),
    //   callApi({ data: { fn: "getApiCountries" } }),
    //   callApi({ data: { fn: "getApiRegions" } }),
    // ]);

    // Transform PartnerProfile to Partner interface
    // const transformedPartner = {
    //   id: partner.id,
    //   name: partner.name,
    //   website: partner.websiteUrl || "",
    //   description: partner.fullDescription || "",
    //   serviceType: partner.serviceTypes?.[0]?.name || "Partner",
    //   skills: partner.capabilities?.map((c) => c.name) || [],
    //   industries: partner.industries?.map((i) => i.name) || [],
    //   certifications: [], // Not in API currently
    //   verified: true,
    //   contacts:
    //     partner.contacts?.map((c) => ({
    //       name: `${c.firstName} ${c.lastName}`,
    //       email: c.emailAddress,
    //       isDefault: true, // Mocking default
    //     })) || [],
    //   locations:
    //     partner.locations?.map((l) => ({
    //       country:
    //         countries.find((c) => c.id === l.countryId)?.name || "Unknown",
    //       region: regions.find((r) => r.id === l.regionId)?.name || "Unknown",
    //       isHeadOffice: l.isHeadOffice,
    //     })) || [],
    //   users:
    //     partner.contacts?.map((c) => ({
    //       email: c.emailAddress,
    //       firstName: c.firstName,
    //       lastName: c.lastName,
    //       title: c.companyPosition || "Representative",
    //     })) || [],
    //   reviews:
    //     partner.reviews?.map((r) => ({
    //       rating: r.rating || 5,
    //       comment: r.comment || "",
    //       date: "Recently", // Mock date format
    //       reviewerName: "Verified Client",
    //     })) || [],
    // };

    const partner: PartnerProfileDto = await callApi({
      data: {
        fn: "getApiCompaniesByIdPartner",
        args: { path: { id: params.id } },
      },
    });

    if (!partner) {
      return { partner: null };
    }

    return {
      partner,
    };
  },
});

function PartnerProfile() {
  const { partner } = Route.useLoaderData();
  const search = Route.useSearch();
  const fromDirectory = search?.from === "directory";

  if (!partner) {
    return (
      <div className="p-12 text-center">
        <h2 className="font-serif text-2xl text-black">Partner not found</h2>
        <Link
          className="mt-4 block text-red-600 hover:underline"
          to="/partners"
        >
          Return to Directory
        </Link>
      </div>
    );
  }

  const headOffice =
    partner.locations?.find((l) => l.isHeadOffice) || partner.locations?.[0];

  return (
    <div className="animate-fade-in space-y-8">
      <nav className="mb-8 flex items-center font-medium text-gray-500 text-xs uppercase tracking-wider">
        <Link
          className="flex items-center transition-colors hover:text-red-600"
          to={fromDirectory ? "/directory" : "/partners"}
        >
          <ArrowLeft className="mr-2 h-3 w-3" />
          {fromDirectory ? "Back to Directory" : "Back to Search"}
        </Link>
        <span className="mx-3 text-gray-300">/</span>
        <span className="text-gray-900">{partner.name}</span>
      </nav>

      {/* Hero Section */}
      <div className="border-gray-200 border-x border-t-4 border-t-red-600 border-b bg-white shadow-sm">
        <div className="p-10">
          <div className="flex flex-col items-start justify-between gap-8 md:flex-row">
            <div>
              <div className="mb-4 flex items-center gap-3">
                <h1 className="font-serif text-5xl text-black leading-tight">
                  {partner.name}
                </h1>
                {/* {partner.verified && (
                  <span className="mt-2" title="Verified Partner">
                    <CheckCircle className="h-6 w-6 text-gray-400" />
                  </span>
                )} */}
              </div>
              <div className="flex flex-wrap items-center gap-4">
                <div className="flex items-center font-medium text-gray-500 text-sm uppercase tracking-wider">
                  <MapPin className="mr-2 h-4 w-4 text-red-600" />
                  {headOffice?.country}{" "}
                  <span className="mx-2 text-gray-300">|</span>{" "}
                  {headOffice?.region}
                  <span className="ml-3 border border-gray-200 bg-gray-100 px-2 py-0.5 font-bold text-[10px] text-gray-400 uppercase tracking-widest">
                    Head Office
                  </span>
                </div>
                <div className="flex items-center bg-black px-3 py-1 font-bold text-[10px] text-white uppercase tracking-[0.2em]">
                  <Briefcase className="mr-2 h-3 w-3" />
                  {partner.serviceTypeName}
                </div>
              </div>
            </div>

            <div className="flex w-full flex-col gap-4 sm:flex-row md:w-auto">
              <ConnectRequestDialog
                partnerId={partner.id as string}
                partnerName={partner.name}
              >
                <button
                  className="min-w-[160px] border border-red-600 bg-red-600 px-10 py-3 font-bold text-white text-xs uppercase tracking-[0.15em] transition-colors hover:bg-white hover:text-red-600"
                  type="button"
                >
                  Connect
                </button>
              </ConnectRequestDialog>
            </div>
          </div>

          <div className="mt-8 max-w-5xl border-gray-200 border-l-2 pl-6">
            <ClientOnly>
              <MDXEditor
                markdown={partner.fullDescription || ""}
                plugins={[headingsPlugin()]}
                readOnly
              />
            </ClientOnly>
          </div>

          <div className="mt-10 flex flex-wrap gap-8 border-gray-100 border-t pt-8">
            <a
              className="flex items-center font-bold text-red-600 text-xs uppercase tracking-wider hover:underline"
              href={partner.websiteUrl || ""}
              rel="noopener noreferrer"
              target="_blank"
            >
              <Globe className="mr-2 h-4 w-4" />
              Visit Website
            </a>
            <div className="flex items-center font-bold text-gray-500 text-xs uppercase tracking-wider">
              <Building className="mr-2 h-4 w-4" />
              Global Presence: {partner.locations?.length || 0} Locations
            </div>
          </div>
        </div>
      </div>

      <div className="grid grid-cols-1 gap-8 lg:grid-cols-3">
        {/* Main Info */}
        <div className="space-y-8 lg:col-span-3">
          {/* Team Section */}
          <div className="border border-gray-200 bg-white p-8 shadow-sm">
            <h3 className="mb-6 flex items-center border-gray-100 border-b pb-4 font-serif text-black text-xl">
              <Users className="mr-3 h-5 w-5 text-red-600" />
              Key Personnel
            </h3>
            <div className="grid grid-cols-1 gap-6 md:grid-cols-2">
              {partner.contacts?.map((user) => (
                <div
                  className="flex items-center gap-4 border border-gray-50 bg-gray-50/50 p-4"
                  key={user.emailAddress}
                >
                  <div className="flex h-12 w-12 items-center justify-center bg-black font-serif text-lg text-white">
                    {user.firstName?.charAt(0)}
                    {user.lastName?.charAt(0)}
                  </div>
                  <div>
                    <p className="font-bold text-black text-sm">
                      {user.firstName} {user.lastName}
                    </p>
                    <p className="text-[10px] text-gray-400 uppercase tracking-widest">
                      {user.companyPosition}
                    </p>
                  </div>
                </div>
              ))}
            </div>
          </div>

          {/* Skills & Industries */}
          <div className="grid grid-cols-1 gap-8 md:grid-cols-2">
            <div className="border border-gray-200 bg-white p-8 shadow-sm">
              <h3 className="mb-6 border-gray-100 border-b pb-4 font-serif text-black text-xl">
                Capabilities
              </h3>
              <div className="flex flex-wrap gap-2">
                {partner.capabilities?.map((skill) => (
                  <span
                    className="border border-gray-100 bg-gray-50 px-3 py-1 font-bold text-[10px] text-gray-600 uppercase tracking-widest"
                    key={skill.id}
                  >
                    {skill.name}
                  </span>
                ))}
              </div>
            </div>
            <div className="border border-gray-200 bg-white p-8 shadow-sm">
              <h3 className="mb-6 border-gray-100 border-b pb-4 font-serif text-black text-xl">
                Industries
              </h3>
              <div className="flex flex-wrap gap-2">
                {partner.industries?.map((ind) => (
                  <span
                    className="bg-black px-3 py-1 font-bold text-[10px] text-white uppercase tracking-widest"
                    key={ind.id}
                  >
                    {ind.name}
                  </span>
                ))}
              </div>
            </div>
          </div>

          {/* Locations */}
          <div className="border border-gray-200 bg-white p-8 shadow-sm">
            <h3 className="mb-6 border-gray-100 border-b pb-4 font-serif text-black text-xl">
              Office Locations
            </h3>
            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
              {partner.locations
                ?.sort((a) => (a.isHeadOffice ? -1 : 1))
                .map((loc, i: number) => (
                  <div
                    className={`border p-4 ${loc.isHeadOffice
                        ? "border-red-600 bg-red-50/20"
                        : "border-gray-100"
                      }`}
                    key={i}
                  >
                    <div className="flex items-start justify-between">
                      <p className="font-bold text-black text-sm">
                        {loc.country}
                      </p>
                      {loc.isHeadOffice && (
                        <span className="bg-red-600 px-2 py-0.5 font-bold text-[9px] text-white uppercase tracking-tighter">
                          Primary
                        </span>
                      )}
                    </div>
                    <p className="mt-1 text-[10px] text-gray-400 uppercase tracking-widest">
                      {loc.region}
                    </p>
                  </div>
                ))}
            </div>
          </div>

          {/* Reviews */}
          <ReviewComponent partnerId={partner.id as string} />
        </div>

        {/* Sidebar Info */}
        <div className="space-y-8">
          {/* Contact Directory and Accreditations sections hidden */}
        </div>
      </div>
    </div>
  );
}
