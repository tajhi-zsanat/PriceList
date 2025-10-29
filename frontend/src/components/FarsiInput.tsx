import { toFarsiNumber } from "@/lib/locale"
import { Input } from "./ui/input"

// Format number: add thousand separators + Farsi digits
export const formatFarsiPrice = (value: number | string | null | undefined): string => {
  if (value === null || value === undefined || value === "") return ""
  const num = Number(value)
  if (!Number.isFinite(num)) return ""
  const formatted = new Intl.NumberFormat("en-US").format(num)
  return toFarsiNumber(formatted)
}

// Parse Farsi input back to a standard number
export const parseFarsiNumber = (value: string): number | null => {
  if (!value.trim()) return null
  const standardDigits = value.replace(/[۰-۹]/g, d => "۰۱۲۳۴۵۶۷۸۹".indexOf(d).toString())
  const cleaned = standardDigits.replace(/,/g, "")
  const num = Number(cleaned)
  return isNaN(num) ? null : num
}

interface FarsiInputProps extends Omit<React.InputHTMLAttributes<HTMLInputElement>, "onChange" | "value"> {
  value: number | null
  onChange?: (value: number | null) => void
}

export default function FarsiInput({ value, onChange, className = "", ...props }: FarsiInputProps) {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const val = e.target.value
    const parsed = parseFarsiNumber(val)
    onChange?.(parsed)
  }

  return (
    <Input
      type="text"
      inputMode="text"
      value={value === null ? "" : formatFarsiPrice(value)}
      onChange={handleChange}
      className={className}
      {...props}
    />
  )
}
