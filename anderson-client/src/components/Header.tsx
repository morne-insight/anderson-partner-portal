import { Link } from "@tanstack/react-router";
import {
  LogIn,
  LogOut,
  User,
} from "lucide-react";
import { useAuth } from "../contexts/auth-context";

export default function Header() {
  const { user, isLoading, logout } = useAuth();

  return (
    <>
      <header className="flex items-center bg-gray-800 p-4 text-white shadow-lg">
       
        <h1 className="ml-4 font-semibold text-xl">
          <Link to="/">
            Anderson Partner Portal
          </Link>
        </h1>

        <div className="ml-auto flex items-center gap-4">
          {isLoading ? (
            <div className="text-gray-300 text-sm">Loading...</div>
          ) : user ? (
            <div className="flex items-center gap-4">
              <div className="flex items-center gap-2 text-sm">
                <User size={16} />
                <span>{user.email}</span>
              </div>
              <button
                className="flex items-center gap-2 rounded-lg bg-red-600 px-3 py-2 font-medium text-sm text-white transition-colors hover:bg-red-700"
                onClick={logout}
              >
                <LogOut size={16} />
                Sign Out
              </button>
            </div>
          ) : (
            <div className="flex items-center gap-2">
              <Link
                className="flex items-center gap-2 rounded-lg bg-blue-600 px-3 py-2 font-medium text-sm text-white transition-colors hover:bg-blue-700"
                to="/login"
              >
                <LogIn size={16} />
                Sign In
              </Link>
            </div>
          )}
        </div>
      </header>
    </>
  );
}
