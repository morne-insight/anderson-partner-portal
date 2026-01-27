import { Menu, X } from "lucide-react";
import type React from "react";

interface HeaderProps {
  isMobileMenuOpen: boolean;
  setIsMobileMenuOpen: (open: boolean) => void;
}

export const Header: React.FC<HeaderProps> = ({
  isMobileMenuOpen,
  setIsMobileMenuOpen,
}) => {
  return (
    <div className="fixed top-0 z-30 flex w-full items-center justify-between bg-black p-4 text-white shadow-md md:hidden">
      <div>
        <h1 className="font-bold font-serif text-xl tracking-wider">
          ANDERSEN<span className="text-red-600">.</span>
        </h1>
      </div>
      <button onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}>
        {isMobileMenuOpen ? <X /> : <Menu />}
      </button>
    </div>
  );
};
