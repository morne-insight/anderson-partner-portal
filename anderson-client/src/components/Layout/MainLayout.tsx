import React, { useState } from "react";
import { Sidebar } from "./Sidebar";
import { Header } from "./Header";
import { Link } from "@tanstack/react-router";
import { LayoutDashboard, Search, Globe, FileText, Users, Settings as SettingsIcon } from "lucide-react";

interface MainLayoutProps {
  children: React.ReactNode;
}

export const MainLayout: React.FC<MainLayoutProps> = ({ children }) => {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);

  // Mobile menu items duplicate sidebar items for now - ideal refactor would be to centralize nav config
  const navItems = [
    { to: "/dashboard", label: "Overview", icon: LayoutDashboard },
    { to: "/partners", label: "Find Partners", icon: Search },
    { to: "/opportunities", label: "Opportunities", icon: Globe },
    { to: "/profile", label: "My Profile", icon: FileText },
    { to: "/directory", label: "Network Directory", icon: Users },
  ];

  return (
    <div className="flex h-screen bg-white overflow-hidden font-sans">
      <Sidebar />
      <Header isMobileMenuOpen={isMobileMenuOpen} setIsMobileMenuOpen={setIsMobileMenuOpen} />

      {/* Mobile Menu Overlay */}
      {isMobileMenuOpen && (
        <div className="md:hidden fixed inset-0 bg-black z-20 pt-20">
          <nav className="p-4">
            <ul className="space-y-2">
              {navItems.map((item) => (
                <li key={item.to}>
                  <Link
                    to={item.to}
                    onClick={() => setIsMobileMenuOpen(false)}
                    className="w-full flex items-center p-4 text-sm font-medium border-l-2 border-transparent text-gray-400 [&.active]:border-red-600 [&.active]:bg-white/5 [&.active]:text-white"
                  >
                    <item.icon className="w-4 h-4 mr-4" />
                    <span className="uppercase tracking-widest text-xs">{item.label}</span>
                  </Link>
                </li>
              ))}
              {/* Settings ignored */}
            </ul>
          </nav>
        </div>
      )}

      <main className="flex-1 overflow-y-auto bg-gray-50 pt-16 md:pt-0 scroll-smooth">
        <div className="max-w-7xl mx-auto p-6 md:p-12 min-h-full">
          {children}
        </div>
      </main>
    </div>
  );
};
