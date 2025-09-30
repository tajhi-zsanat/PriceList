import type { FeatureChipsProps } from "@/types";


export default function FeatureChips({ data }: FeatureChipsProps) {
    if (!data || data.items.length <= 1) return null;
    return (
        <ul className="flex items-center gap-3">
            {data.items.map(f => (
                <li key={f.featuresIDs} className="bg-white rounded-lg py-2 px-8 cursor-pointer">{f.title}</li>
            ))}
        </ul>
    );
}