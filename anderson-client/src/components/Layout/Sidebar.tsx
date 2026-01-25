import { Link, useRouterState } from "@tanstack/react-router";
import {
  LayoutDashboard,
  Globe,
  Users,
  FileText,
  Search,
  ShieldCheck,
  LogOut
} from "lucide-react";
import React from "react";
import { useAuth } from "../../contexts/auth-context";

interface SidebarProps {
  isAdmin?: boolean;
}

export const Sidebar: React.FC<SidebarProps> = ({ isAdmin }) => {
  const { user, logout } = useAuth();
  const routerState = useRouterState();
  const currentPath = routerState.location.pathname;

  const navItems = [
    { to: "/dashboard", label: "Overview", icon: LayoutDashboard, exact: true },
    { to: "/partners", label: "Find Partners", icon: Search },
    { to: "/opportunities", label: "Opportunities", icon: Globe },
    { to: "/profile", label: "My Profile", icon: FileText },
    { to: "/directory", label: "Network Directory", icon: Users },
    { to: "/dev", label: "Dev", icon: Users },
  ];

  if (isAdmin) {
    navItems.push({ to: "/admin", label: "Back Office", icon: ShieldCheck });
  }

  const isActive = (path: string, exact: boolean = false) => {
    if (exact) return currentPath === path;
    return currentPath.startsWith(path);
  };

  return (
    <aside className="hidden md:flex flex-col w-72 bg-black text-white h-full z-20">
      <div className="p-8 border-b border-gray-800">
        <h1 className="text-2xl font-serif font-bold tracking-wider text-white">
          ANDERSEN<span className="text-red-600">.</span>
        </h1>
        <p className="text-[10px] text-gray-400 mt-2 uppercase tracking-[0.2em]">Partner Portal</p>
      </div>

      <nav className="flex-1 overflow-y-auto py-8">
        <ul className="space-y-1">
          {navItems.map((item) => (
            <li key={item.to}>
              <Link
                to={item.to}
                className={`group w-full flex items-center px-8 py-4 text-sm font-medium transition-all duration-200 border-l-2 ${isActive(item.to, item.exact)
                  ? "border-red-600 bg-white/5 text-white"
                  : "border-transparent text-gray-400 hover:text-white hover:bg-white/5 hover:border-gray-700"
                  }`}
              >
                <item.icon
                  className={`w-4 h-4 mr-4 transition-colors ${isActive(item.to, item.exact)
                    ? item.to === "/admin"
                      ? "text-yellow-400"
                      : "text-red-600"
                    : "group-hover:text-white"
                    }`}
                />
                <span
                  className={`uppercase tracking-widest text-xs ${item.to === "/admin" && isActive(item.to) ? "text-yellow-400" : ""
                    }`}
                >
                  {item.label}
                </span>
              </Link>
            </li>
          ))}
        </ul>
      </nav>

      <div className="p-8 border-t border-gray-800">
        {user && (
          <div className="flex flex-col gap-6">
            <div className="flex items-center gap-3">
              <div className="w-8 h-8 rounded-full bg-red-600 flex items-center justify-center text-white font-bold text-xs ring-2 ring-black">
                {user.email?.[0]?.toUpperCase()}
              </div>
              <div className="flex flex-col overflow-hidden">
                <span className="text-sm font-medium text-white truncate w-40" title={user.email}>{user.email}</span>
                <span className="text-[10px] text-gray-500 uppercase tracking-widest font-semibold">Partner</span>
              </div>
            </div>
            <button
              onClick={() => logout()}
              className="flex items-center gap-3 text-gray-400 hover:text-red-500 transition-colors text-xs font-bold uppercase tracking-widest group"
            >
              <LogOut className="w-4 h-4 transition-transform group-hover:-translate-x-1" />
              Sign Out
            </button>
          </div>
        )}
      </div>
    </aside>
  );
};
