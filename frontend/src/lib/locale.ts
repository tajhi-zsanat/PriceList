export const toFarsiNumber = (number: string | number): string => {
  return number
    .toString()
    .replace(/\d/g, (d) => "۰۱۲۳۴۵۶۷۸۹".charAt(Number(d)))
}