using review_microservice.Models;

namespace review_microservice.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Reviews.Any())
                {
                    context.Reviews.AddRange(new List<Review>()
                    {
                        new Review {Value = 9, Title = "Świetne brzmienie", Content = "Bardzo spójny album, świetna produkcja i rewelacyjne wokale.", AlbumId = 6651, AppUserId = "user-123", CreatedAt = DateTime.Now.AddDays(-15) },
                        new Review {Value = 7, Title = "Dobre, ale bez rewelacji", Content = "Kilka świetnych kawałków, ale reszta to zapychacze.", AlbumId = 6607, AppUserId = "user-456", CreatedAt = DateTime.Now.AddDays(-14) },
                        new Review {Value = 10, Title = "Absolutny klasyk", Content = "Płyta, do której wracam regularnie. Ani trochę się nie zestarzała.", AlbumId = 6495, AppUserId = "user-789", CreatedAt = DateTime.Now.AddDays(-12) },
                        new Review {Value = 8, Title = "Solidna robota", Content = "Bardzo przyjemnie się tego słucha w samochodzie w dłuższej trasie.", AlbumId = 6387, AppUserId = "user-123", CreatedAt = DateTime.Now.AddDays(-10) },
                        new Review {Value = 5, Title = "Przeciętniak", Content = "Spodziewałem się czegoś więcej po zapowiedziach. Trochę wieje nudą.", AlbumId = 6440, AppUserId = "user-999", CreatedAt = DateTime.Now.AddDays(-9) },
                        new Review {Value = 9, Title = "Pozytywne zaskoczenie", Content = "Nie byłem fanem tego zespołu, ale ta płyta to absolutne mistrzostwo.", AlbumId = 6571, AppUserId = "user-456", CreatedAt = DateTime.Now.AddDays(-8) },
                        new Review {Value = 10, Title = "Genialny klimat", Content = "Buduje niesamowitą atmosferę od pierwszej do ostatniej minuty.", AlbumId = 3054827, AppUserId = "user-789", CreatedAt = DateTime.Now.AddDays(-7) },
                        new Review {Value = 6, Title = "Może być", Content = "Nic odkrywczego, ale fanom gatunku na pewno się spodoba.", AlbumId = 6651, AppUserId = "user-111", CreatedAt = DateTime.Now.AddDays(-6) },
                        new Review {Value = 8, Title = "Warto przesłuchać", Content = "Świetne solówki gitarowe i mocny, wyraźny bas. Zdecydowanie polecam.", AlbumId = 6607, AppUserId = "user-222", CreatedAt = DateTime.Now.AddDays(-5) },
                        new Review { Value = 9, Title = "Ponadczasowe", Content = "Każdy utwór na tej płycie to hit. Czysta, rzemieślnicza perfekcja.", AlbumId = 6495, AppUserId = "user-333", CreatedAt = DateTime.Now.AddDays(-5) },
                        new Review { Value = 7, Title = "Ciekawy eksperyment", Content = "Artysta poszedł w nowym kierunku. Nie wszystko zagrało, ale szanuję za odwagę.", AlbumId = 6387, AppUserId = "user-123", CreatedAt = DateTime.Now.AddDays(-4) },
                        new Review { Value = 4, Title = "Słabo", Content = "Niestety, ten album to ewidentny krok wstecz w ich karierze.", AlbumId = 6440, AppUserId = "user-456", CreatedAt = DateTime.Now.AddDays(-3) },
                        new Review { Value = 8, Title = "Bardzo równe tempo", Content = "Płyta trzyma równy, wysoki poziom przez cały czas trwania. Super produkcja.", AlbumId = 6571, AppUserId = "user-789", CreatedAt = DateTime.Now.AddDays(-2) },
                        new Review { Value = 10, Title = "Arcydzieło dekady", Content = "Nie mam pytań. Ten album zmienia sposób patrzenia na współczesną muzykę.", AlbumId = 3054827, AppUserId = "user-999", CreatedAt = DateTime.Now.AddDays(-1) },
                        new Review { Value = 9, Title = "Niesamowita energia", Content = "Świetnie sprawdza się na żywo, a wersja studyjna niewiele jej ustępuje.", AlbumId = 6651, AppUserId = "user-111", CreatedAt = DateTime.Now }
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
