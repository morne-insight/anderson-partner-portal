import { Link } from "@tanstack/react-router";
import { FileText, Globe, LayoutDashboard, Search, Users } from "lucide-react";
import type React from "react";
import { useState } from "react";
import { Header } from "./Header";
import { Sidebar } from "./Sidebar";

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
    <div className="flex h-screen overflow-hidden bg-white font-sans">
      <Sidebar />
      <Header
        isMobileMenuOpen={isMobileMenuOpen}
        setIsMobileMenuOpen={setIsMobileMenuOpen}
      />

      {/* Mobile Menu Overlay */}
      {isMobileMenuOpen && (
        <div className="fixed inset-0 z-20 bg-black pt-20 md:hidden">
          <nav className="p-4">
            <ul className="space-y-2">
              {navItems.map((item) => (
                <li key={item.to}>
                  <Link
                    className="flex w-full items-center border-transparent border-l-2 p-4 font-medium text-gray-400 text-sm [&.active]:border-red-600 [&.active]:bg-white/5 [&.active]:text-white"
                    onClick={() => setIsMobileMenuOpen(false)}
                    to={item.to}
                  >
                    <item.icon className="mr-4 h-4 w-4" />
                    <span className="text-xs uppercase tracking-widest">
                      {item.label}
                    </span>
                  </Link>
                </li>
              ))}
              {/* Settings ignored */}
            </ul>
          </nav>
        </div>
      )}

      <main className="flex-1 overflow-y-auto scroll-smooth bg-gray-50 pt-16 md:pt-0">
        <div className="mx-auto min-h-full max-w-7xl p-6 md:p-12">
          {children}
        </div>
      </main>
    </div>
  );
};
