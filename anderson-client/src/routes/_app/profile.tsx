import { createFileRoute } from "@tanstack/react-router";
import { Building, Globe, Save } from "lucide-react";
import React, { useState } from "react";
import { MOCK_PARTNERS } from "../../services/mock/data";

export const Route = createFileRoute("/_app/profile")({
  component: MyProfile,
});

function MyProfile() {
  // Mocking the user's firm profile - usually this would come from the auth user's companyId
  const [profile, setProfile] = useState(MOCK_PARTNERS[0]);
  const [isEditing, setIsEditing] = useState(false);

  return (
    <div className="space-y-10 animate-fade-in">
      <header className="border-b border-gray-200 pb-6 flex justify-between items-end">
        <div>
          <h2 className="text-4xl font-serif text-black mb-3">Firm Profile</h2>
          <p className="text-gray-500 font-light text-lg">
            Manage your firm's presence and capabilities in the global network.
          </p>
        </div>
        <button
          onClick={() => setIsEditing(!isEditing)}
          className="bg-black text-white px-8 py-3 text-xs font-bold uppercase tracking-[0.15em] hover:bg-gray-800 transition-colors"
        >
          {isEditing ? (
            <span className="flex items-center">
              <Save className="w-4 h-4 mr-2" /> Save Changes
            </span>
          ) : (
            "Edit Profile"
          )}
        </button>
      </header>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-12">
        <div className="lg:col-span-2 space-y-12">
          {/* Basic Info */}
          <section className="bg-white p-10 border border-gray-200 relative overflow-hidden shadow-sm">
            <div className="absolute top-0 left-0 w-2 h-full bg-black"></div>
            <h3 className="text-xl font-serif text-black mb-8 flex items-center">
              <Building className="w-5 h-5 mr-3 text-red-600" /> General Information
            </h3>

            <div className="grid grid-cols-1 gap-8">
              <div>
                <label className="block text-xs font-bold uppercase tracking-widest text-gray-500 mb-2">
                  Firm Name
                </label>
                <input
                  type="text"
                  value={profile.name}
                  disabled={!isEditing}
                  className="w-full text-2xl font-serif border-b border-gray-300 py-2 focus:border-red-600 focus:outline-none bg-transparent disabled:border-transparent disabled:text-gray-700"
                />
              </div>

              <div>
                <label className="block text-xs font-bold uppercase tracking-widest text-gray-500 mb-2">
                  Website
                </label>
                <div className="flex items-center">
                  <Globe className="w-4 h-4 text-gray-400 mr-3" />
                  <input
                    type="text"
                    value={profile.website}
                    disabled={!isEditing}
                    className="w-full text-sm border-b border-gray-300 py-2 focus:border-red-600 focus:outline-none bg-transparent disabled:border-transparent disabled:text-gray-600"
                  />
                </div>
              </div>

              <div>
                <label className="block text-xs font-bold uppercase tracking-widest text-gray-500 mb-2">
                  Description
                </label>
                <textarea
                  value={profile.description}
                  disabled={!isEditing}
                  rows={4}
                  className="w-full text-lg font-light leading-relaxed border border-gray-200 p-4 focus:border-red-600 focus:outline-none bg-gray-50 disabled:bg-transparent disabled:border-transparent disabled:p-0 disabled:text-gray-600"
                />
              </div>
            </div>
          </section>
          
           {/* Mock Section for Capabilities */}
           <section className="bg-white p-10 border border-gray-200 shadow-sm">
             <div className="text-center py-10">
                <p className="text-gray-500 font-serif text-xl">Additional profile management sections would go here.</p>
                <p className="text-sm text-gray-400 mt-2">(Capabilities, Team, Locations - similar to View Profile but editable)</p>
             </div>
           </section>
        </div>

        {/* Sidebar */}
        <div className="space-y-8">
            <div className="bg-black text-white p-8">
                <h4 className="font-serif text-xl mb-4">Profile Strength</h4>
                <div className="w-full bg-gray-800 h-2 mb-4">
                    <div className="bg-red-600 h-full w-[85%]"></div>
                </div>
                <p className="text-sm text-gray-300">Your profile is <strong>85% complete</strong>. Add more client reviews to reach 100%.</p>
            </div>
        </div>
      </div>
    </div>
  );
}
