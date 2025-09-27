import { useFarsiNumbers } from "@/hook/useFarsiNumbers"

export default function FarsiText({ children }: { children: React.ReactNode }) {
  return <>{useFarsiNumbers(children)}</>
}