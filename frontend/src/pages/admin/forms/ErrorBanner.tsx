export default function ErrorBanner({ message }: { message: string }) {
  return (
    <div className="text-red-600 py-4" role="alert">
      خطا: {message}
    </div>
  );
}
