import { useKeenSlider } from "keen-slider/react";
import "keen-slider/keen-slider.min.css";
import type { JSX } from "react";
import HeroSearchBox from "./HeroSearchBox";

import bottomArrow from "@/assets/img/home/arrow-down.png";
import arrowRightIcon from "@/assets/img/home/arrow-right.png";
import arrowLeftIcon from "@/assets/img/home/arrow-left.png";

interface HeroSliderProps {
  images: string[];
}

export default function HeroSlider({ images }: HeroSliderProps): JSX.Element {
  const [sliderRef, slider] = useKeenSlider<HTMLDivElement>({
    rtl: true,
    loop: true,
    renderMode: "performance",
    slides: {
      perView: 1,
    },
    defaultAnimation: {
      duration: 1200,
    },
  });

  return (
    <div className="relative mb-28 max-w-[1920px] m-auto h-full" dir="rtl">
      <div ref={sliderRef} className="keen-slider">
        {images.map((img, i) => (
          <div className="keen-slider__slide" key={i}>
            <img src={img} className="w-full" alt={`اسلاید ${i + 1}`} />
          </div>
        ))}
      </div>

      {/* Prev (right side in RTL) */}
      <button
        type="button"
        onClick={() => slider.current?.prev()}
        className="
          absolute top-1/2 -translate-y-1/2 right-3 z-10
          w-9 h-9 md:w-10 md:h-10
          bg-[#FFFFFF80] hover:bg-[#aaaaaa80]
          rounded-full flex items-center justify-center
          transition cursor-pointer
        "
      >
        {/* You can pass icons via props if you like */}
        <img src={arrowRightIcon} alt="" />
      </button>

      {/* Next (left side in RTL) */}
      <button
        type="button"
        onClick={() => slider.current?.next()}
        className="
          absolute top-1/2 -translate-y-1/2 left-3 z-10
          w-9 h-9 md:w-10 md:h-10
          bg-[#FFFFFF80] hover:bg-[#aaaaaa80]
          rounded-full flex items-center justify-center
          transition cursor-pointer
        "
      >
        <img src={arrowLeftIcon} alt="" />
      </button>

      <HeroSearchBox arrowDownIcon={bottomArrow} />
    </div>
  );
}
