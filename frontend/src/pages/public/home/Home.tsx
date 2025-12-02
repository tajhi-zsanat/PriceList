import HeroSlider from "@/components/home/HeroSlider";

import landing1 from "@/assets/img/home/landing1.svg";
import landing2 from "@/assets/img/home/landing2_new.svg";
import PoPularForm from "@/components/home/PoPularForm";
import Hero from "@/components/home/Hero";
import IntroduceCompany from "@/components/home/IntroduceCompany";
import Blog from "@/components/home/Blog";
import FrequentlyQuestions from "@/components/home/FrequentlyQuestions";
import { useEffect, useState } from "react";
import api from "@/lib/api";
import type { PopularFormDto } from "@/types";

export default function Home() {
  const [data, setData] = useState<PopularFormDto[]>([]);
  const [loading, setLoading] = useState(true);

  const images = [landing1, landing2];

  useEffect(() => {
    api.get<PopularFormDto[]>("/api/Product/popular")
      .then(r => setData(r.data))
      .catch(e => console.log(e?.response?.data ?? e.message))
      .finally(() => setLoading(false));
  }, []);

  return (
    <div>
      <HeroSlider images={images} />
      <PoPularForm data={data} loading={loading} />
      <Hero />
      <IntroduceCompany />
      <Blog />
      <FrequentlyQuestions />
    </div>
  );
}
