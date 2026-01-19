import { Menu, X } from "lucide-react";
import React from "react";
import { Link } from "@tanstack/react-router";

interface HeaderProps {
  isMobileMenuOpen: boolean;
  setIsMobileMenuOpen: (open: boolean) => void;
}

export const Header: React.FC<HeaderProps> = ({ isMobileMenuOpen, setIsMobileMenuOpen }) => {
  return (
    <div className="md:hidden fixed top-0 w-full bg-black text-white z-30 flex items-center justify-between p-4 shadow-md">
      <div>
        <h1 className="text-xl font-serif font-bold tracking-wider">
          ANDERSEN<span className="text-red-600">.</span>
        </h1>
      </div>
      <button onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}>
        {isMobileMenuOpen ? <X /> : <Menu />}
      </button>
    </div>
  );
};
