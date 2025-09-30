import loadingImage from '@/assets/img/loading.gif';

export default function Loading() {
  return (
    <div className="flex justify-center py-10" aria-live="polite">
      <img src={loadingImage} alt="در حال بارگذاری..." loading="lazy" />
    </div>
  );
}
