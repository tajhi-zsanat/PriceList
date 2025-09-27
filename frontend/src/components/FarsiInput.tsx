import { toFarsiNumber } from "@/lib/locale"
// Format number: add thousand separators + Farsi digits
export const formatFarsiPrice = (value: number | string): string => {
  const formatted = new Intl.NumberFormat("en-US").format(Number(value))
  return toFarsiNumber(formatted)
}
// Parse Farsi input back to a standard number
export const parseFarsiNumber = (value: string): number => {
  const standardDigits = value.replace(/[۰-۹]/g, (d) => "۰۱۲۳۴۵۶۷۸۹".indexOf(d).toString())
  const cleaned = standardDigits.replace(/,/g, "")
  const num = Number(cleaned)
  return isNaN(num) ? 0 : num
}
interface FarsiInput extends Omit<React.InputHTMLAttributes<HTMLInputElement>, "onChange" | "value"> {
  value: number
  onChange?: (value: number) => void
}
export default function FarsiInput({ value, onChange, className = "", ...props }: FarsiInput) {
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const numericValue = parseFarsiNumber(e.target.value)
    onChange?.(numericValue)
  }
  return (
    <input
      type="text"
      inputMode="numeric"
      value={formatFarsiPrice(value)}
      onChange={handleChange}
      className={`${className}`}
      {...props}
    />
  )
}