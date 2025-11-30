import HeroSlider from "@/components/home/HeroSlider";

import landing1 from "@/assets/img/home/landing1.svg";
import landing2 from "@/assets/img/home/landing2_new.svg";
import PoPularForm from "@/components/home/PoPularForm";
import Hero from "@/components/home/Hero";
import IntroduceCompany from "@/components/home/IntroduceCompany";
import Blog from "@/components/home/Blog";

export default function Home() {
  const images = [landing1, landing2];

  return (
    <div>
      <HeroSlider images={images} />
      <PoPularForm />
      <Hero />
      <IntroduceCompany />
      <Blog />
    </div>
  );
}
