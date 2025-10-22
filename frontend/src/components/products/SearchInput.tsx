import type { SearchInputProps } from "@/types";
import search from "../../assets/img/search.png";


export default function SearchInput({ placeholder = "جستجو", value, onChange, className = "w-64", }: SearchInputProps) {
    return (
        <div className={`relative ${className}`}>
            <input
                type="text"
                placeholder={placeholder}
                value={value}
                onChange={e => onChange(e.target.value)}
                className="w-full bg-white border border-gray-300 rounded-lg py-2 pr-10 pl-3 focus:outline-none focus:border-blue-500"
            />
            <img src={search} alt="search" className="absolute right-3 top-1/2 -translate-y-1/2 pointer-events-none" />
        </div>
    );
}