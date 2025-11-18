import type { FeatureChipsProps } from "@/types";


export default function FeatureChips({ data }: FeatureChipsProps) {
    if (!data || data.cells.length < 1) return null;
    return (
        <ul className="flex items-center gap-3">
            {data.cells.map(f => (
                <li key={f.featureId} className="bg-white rounded-lg py-2 px-8 cursor-pointer">{f.featureName}</li>
            ))}
        </ul>
    );
}