import type { InfiniteSentinelProps } from "@/types";
import { useEffect, useRef } from "react";


export default function InfiniteSentinel({ onHit, rootMargin = "100px", threshold = 0.01 }: InfiniteSentinelProps) {
    const ref = useRef<HTMLDivElement | null>(null);
    useEffect(() => {
        const node = ref.current; if (!node) return;
        const observer = new IntersectionObserver((entries) => {
            if (entries[0].isIntersecting) onHit();
        }, { root: null, rootMargin, threshold });
        observer.observe(node);
        return () => observer.disconnect();
    }, [onHit, rootMargin, threshold]);
    return <div ref={ref} aria-hidden="true" />;
}