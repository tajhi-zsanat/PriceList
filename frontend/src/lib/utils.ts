import { clsx, type ClassValue } from "clsx"
import { twMerge } from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export const UNIT_OPTIONS = ["تعداد","عدد","کیلوگرم"] as const;

export async function freezeScrollDuring(action: () => Promise<void>) {
  const x = window.scrollX;
  const y = window.scrollY;

  const html = document.documentElement;
  const body = document.body;

  const original = {
    htmlScrollBehavior: html.style.scrollBehavior,
    position: body.style.position,
    top: body.style.top,
    width: body.style.width,
    overflowY: body.style.overflowY,
  };

  html.style.scrollBehavior = "auto";
  body.style.position = "fixed";
  body.style.top = `-${y}px`;
  body.style.width = "100%";
  body.style.overflowY = "scroll"; 

  try {
    await action();
    await new Promise<void>(r => requestAnimationFrame(() => r()));
  } finally {
    body.style.position = original.position;
    body.style.top = original.top;
    body.style.width = original.width;
    body.style.overflowY = original.overflowY;
    html.style.scrollBehavior = original.htmlScrollBehavior;

    window.scrollTo(x, y);
  }
}