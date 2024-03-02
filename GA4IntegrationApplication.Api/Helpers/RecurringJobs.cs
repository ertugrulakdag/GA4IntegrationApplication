using GA4IntegrationApplication.Api.Interfaces;
using Hangfire;
using Hangfire.States;

namespace GA4IntegrationApplication.Api.Helpers
{
    public static class BackgroundTasks
    {
        public static void GetJobs()
        {
            //Fire-And-Forget Jobs:İş tanımlanır ve hemen ardından bir kereye mahsus tetiklenir.
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire-And-Forget Jobs tetiklendi"));

            //Recurring Jobs:Belirlenen CRON zamanlamasına göre tekrarlanan işler tanımlanır.
            //Hangfire.RecurringJob.AddOrUpdate<IGoogleAnalytic>("RealTimeJob", x => x.Realtime(), Hangfire.Cron.MinuteInterval(1), TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time")); //Her 1 dakkada bir çalışır
            RecurringJob.AddOrUpdate("console-write-line", () => Console.WriteLine($"Hi:{DateTime.UtcNow}"), "*/10 * * * * *");//On Saniyede Bir Calişır
            RecurringJob.AddOrUpdate<IGoogleAnalytic>("real-time-function", g => g.Realtime(), "*/15 * * * * *");


            //Delayed Jobs:Oluşturulduktan belirli bir zaman sonra sadece bir seferliğine tetiklenecek olan görevler tanımlanır.
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed jobs tetiklendi!"), TimeSpan.FromSeconds(10));

            //Continuations:Birbiriyle ilişkili işlerin olduğu durumlarda alınan aksiyondur. Bir jobun tetiklenebilmesi için bir öncekinin tamamlanması gerekmektedir.

            var jobId = BackgroundJob.Schedule<IGoogleAnalytic>((g) => g.ScreenPageViews(DateTime.Now, DateTime.Now), TimeSpan.FromSeconds(10));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"ScreenPageViewsJobs isimli job {jobId} numarası ile {DateTime.Now} tarihinde çalıştı."));

        }
    }
}
//"00 0,1,7,8,13,14,18,20 * * *" //İlgili Saatlerde Çalışır
//"*/30 * * * *" //Her 30 Dakikada Bir Çalışır
//"*/10 * * * * *" //On Saniyede Bir Calişır
//"1 * * * * *" //Bir Dakikada Bir Calişır
//"00 12,18 * * *" //Her Gün Öglen OnIki ve Akşam OnSekiz Calişır
//"00 07-18 * * *" //Sabah Yedi Akşam OnSekiz Arasında Her Saat Çalışır
//"00 09-18 * * 1-5" //Hafta İçi Sabah Dokuz Akşam OnSekiz Arasinda Her Saat Çalışır
//"*/10 * * * * *" //OnSaniyede Bir Çalişır
//"0 9 * * 0" //Her Pazar Çalişır
//"0 5 1 * *" //Her Ayın İlk Günü Saat Beşte Çaliş