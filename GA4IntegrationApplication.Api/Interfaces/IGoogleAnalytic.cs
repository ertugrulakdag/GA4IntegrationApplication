namespace GA4IntegrationApplication.Api.Interfaces
{
    public interface IGoogleAnalytic
    {
        object ScreenPageViews(DateTime StartDate, DateTime EndDate);//İlgili tarih aralığındaki kullanıcıların görüntülediği uygulama ekranı veya web sayfası sayısı. Tek bir sayfanın veya ekranın tekrarlanan görüntülemeleri sayılır. (screen_view + page_view etkinlikleri).
        object TotalUsers(DateTime StartDate, DateTime EndDate);//İlgili tarih aralığındaki en az bir farklı kullanıcıların sayısı.
        object ActiveUsers(DateTime StartDate, DateTime EndDate);//İlgili tarih aralığındaki sitenizi veya uygulamanızı ziyaret eden farklı kullanıcıların sayısı.
        object Realtime();//Web siteniz veya uygulamanızı an itibariyle ziyaret eden tekil kullanıcı sayısını verir.
    }
}
