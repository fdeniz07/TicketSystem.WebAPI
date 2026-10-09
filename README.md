# TicketSystem.WebAPI

ASP.NET Core Minimal API ile geliştirilmiş bir destek/ticket sistemi.
Müşteriler ticket oluşturabilir, Admin ve müşteri ticket üzerinden
mesajlaşabilir, müşteri hizmeti değerlendirebilir ve Admin ticket'ı
kapatabilir.

## Özellikler

-   Admin ve Customer kullanıcıları oluşturma, listeleme ve güncelleme
-   Ticket oluşturma, listeleme, detay görüntüleme, güncelleme ve
    kapatma
-   Ticket mesajlarını ekleme ve konuşma geçmişini görüntüleme
-   Ticket başına 1--5 puan ve isteğe bağlı yorumla değerlendirme
-   Entity Framework Core ve SQL Server
-   Carter endpoint modülleri
-   Mapster Entity--DTO dönüşümleri
-   TS.Result standart sonuç nesneleri
-   Scalar ile API testleri
-   Ardalis.SmartEnum ile rol ve ticket durumu

## Teknolojiler

-   ASP.NET Core Minimal API
-   Entity Framework Core
-   Microsoft SQL Server
-   Carter
-   Mapster
-   TS.Result
-   Scalar.AspNetCore
-   Ardalis.SmartEnum

## Proje yapısı

``` text
TicketSystem.WebAPI/
├── Abstractions/Entity.cs
├── Context/ApplicationDbContext.cs
├── DTOs/
│   ├── User/
│   ├── Ticket/
│   ├── TicketReply/
│   └── TicketRating/
├── Mappings/MapsterConfig.cs
├── Models/
├── Modules/
│   ├── UserModule.cs
│   ├── TicketModule.cs
│   ├── TicketReplyModule.cs
│   └── TicketRatingModule.cs
├── Program.cs
└── appsettings.json
```

## Kurulum

### Gereksinimler

-   Projenin kullandığı .NET SDK sürümü
-   SQL Server veya SQL Server Express
-   Visual Studio veya .NET CLI

### Connection string

`appsettings.json` içindeki bağlantıyı kendi SQL Server ortamınıza göre
düzenleyin:

``` json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TicketSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

SQL Server Express veya LocalDB kullanıyorsanız `Server` değerini kendi
instance adınıza göre değiştirin. Gerçek parola içeren connection
string'leri kaynak kontrolüne eklemeyin.

### Veritabanını oluşturma

Visual Studio Package Manager Console:

``` powershell
Add-Migration InitialCreate
Update-Database
```

Daha önce migration oluşturduysanız yeni bir `InitialCreate` oluşturmak
yerine mevcut migration'ları kullanın.

### Uygulamayı çalıştırma

Visual Studio'dan çalıştırın veya proje klasöründe:

``` bash
dotnet run
```

Geliştirme ortamında Scalar genellikle `/scalar/v1` adresinde açılır.
Kesin adres için uygulama konsolundaki URL'yi kontrol edin.

## API kullanımı

Aşağıdaki `{id}` ve `{ticketId}` değerlerini gerçek GUID'lerle
değiştirin.

### Users

  Method   Endpoint        Açıklama
  -------- --------------- -----------------------------------------------
  POST     `/users`        Admin veya Customer oluşturur
  GET      `/users`        Kullanıcıları listeler
  GET      `/users/{id}`   Kullanıcı detayını getirir
  PUT      `/users/{id}`   Kullanıcı adını ve e-posta adresini günceller

#### Customer oluşturma --- `POST /users`

``` json
{
  "name": "Emre",
  "email": "emre@example.com",
  "role": "customer"
}
```

#### Admin oluşturma --- `POST /users`

``` json
{
  "name": "Fatih",
  "email": "admin@example.com",
  "role": "admin"
}
```

Rol değeri `admin` veya `customer` olmalıdır. E-posta adresinin
benzersiz olması beklenir.

### Tickets

  Method   Endpoint                Açıklama
  -------- ----------------------- ----------------------------------------------
  POST     `/tickets`              Yeni ticket oluşturur
  GET      `/tickets`              Ticket'ları listeler
  GET      `/tickets/{id}`         Ticket detayını ve konuşma geçmişini getirir
  PUT      `/tickets/{id}`         Ticket başlığını ve açıklamasını günceller
  PUT      `/tickets/{id}/close`   Ticket'ı kapatır

#### Ticket oluşturma --- `POST /tickets`

``` json
{
  "title": "Şifre Hatası",
  "description": "Şifremi değiştiremiyorum.",
  "createdByUserId": "CUSTOMER_GUID"
}
```

`CUSTOMER_GUID`, önceden oluşturulmuş müşteri kullanıcısının ID'sidir.
Yeni ticket'ın başlangıç durumu `open` olur.

#### Ticket detayı ve konuşma geçmişi --- `GET /tickets/{id}`

Yanıtta ticket bilgileri ve `messages` listesi bulunur. İlk müşteri
mesajı `Ticket.Description` alanından, sonraki mesajlar ticket'a bağlı
reply kayıtlarından alınır. Mesajlar oluşturulma tarihine göre
sıralanır.

### Ticket replies

  Method   Endpoint                        Açıklama
  -------- ------------------------------- --------------------------------
  POST     `/tickets/{ticketId}/replies`   Ticket'a mesaj ekler
  GET      `/tickets/{ticketId}/replies`   Ticket'ın mesajlarını listeler

#### Mesaj ekleme --- `POST /tickets/{ticketId}/replies`

``` json
{
  "userId": "ADMIN_GUID",
  "message": "Sorunu kontrol ettim. Tekrar deneyebilir misiniz?"
}
```

`ticketId` URL'den, mesajı yazan kullanıcının ID'si `userId` alanından
alınır. Admin veya müşteri mesaj yazabilir. Kapalı ticket'a mesaj
eklenemez.

### Ticket rating

  ------------------------------------------------------------------------------
  Method                  Endpoint                       Açıklama
  ----------------------- ------------------------------ -----------------------
  POST                    `/tickets/{ticketId}/rating`   Ticket için
                                                         değerlendirme oluşturur

  GET                     `/tickets/{ticketId}/rating`   Ticket
                                                         değerlendirmesini
                                                         getirir
  ------------------------------------------------------------------------------

#### Değerlendirme oluşturma --- `POST /tickets/{ticketId}/rating`

``` json
{
  "userId": "CUSTOMER_GUID",
  "score": 5,
  "comment": "Sorunum hızlı şekilde çözüldü."
}
```

-   `userId`, ticket'ı oluşturan müşterinin ID'si olmalıdır.
-   `score` 1 ile 5 arasında olmalıdır.
-   `comment` isteğe bağlıdır.
-   Her ticket için en fazla bir değerlendirme yapılması hedeflenir.

## Önerilen iş akışı

1.  `POST /users` ile bir Customer oluşturun.
2.  `POST /users` ile bir Admin oluşturun.
3.  `POST /tickets` ile Customer adına ticket oluşturun.
4.  `GET /tickets/{id}` ile ticket'ı ve ilk mesajı kontrol edin.
5.  `POST /tickets/{ticketId}/replies` ile Admin olarak cevap verin.
6.  Gerekirse Customer ve Admin arasında yeni reply kayıtları oluşturun.
7.  `GET /tickets/{id}` veya `GET /tickets/{ticketId}/replies` ile
    konuşma geçmişini görüntüleyin.
8.  `PUT /tickets/{id}/close` ile Admin olarak ticket'ı kapatın.
9.  Müşterinin `POST /tickets/{ticketId}/rating` endpoint'i üzerinden
    değerlendirme yapmasını sağlayın.
10. `GET /tickets/{ticketId}/rating` ile değerlendirmeyi kontrol edin.

## İş kuralları

-   `User` temel sınıftır; `Admin` ve `Customer` ondan türetilir.
-   Ticket durumları `Open`, `InProgress` ve `Closed` olarak
    tanımlanmıştır.
-   Yeni ticket `Open` durumunda başlar.
-   Ticket'a mesaj eklendiğinde mevcut akışta durum `InProgress`
    yapılır.
-   Kapalı ticket güncellenemez ve kapalı ticket'a yeni cevap eklenemez.
-   Ticket'ın `Description` alanı ilk müşteri mesajı olarak korunur.
    `messages` listesi bu mesajı sonraki reply kayıtlarıyla birleştirir.
-   Önerilen iş kuralı, rating'in yalnızca ticket kapatıldıktan sonra
    verilebilmesidir. `TicketRatingModule` içinde kapalı olma kontrolü
    henüz eklenmediyse bu kuralı uygulamadan önce ekleyin.
-   TS.Result yanıtları `data`, `errorMessages`, `isSuccessful` ve
    `statusCode` alanları içerebilir. Bazı hata durumlarında HTTP yanıtı
    `200 OK` olurken gerçek hata kodu Result gövdesindeki `statusCode`
    alanında yer alabilir.

## Notlar ve sonraki geliştirmeler

-   Scalar API test aracıdır; henüz müşteri/admin arayüzü değildir.
-   Şu an örnek isteklerde `userId` istemciden alınmaktadır. Kimlik
    doğrulama eklendiğinde kullanıcı kimliği güvenilir biçimde
    oturum/claim üzerinden alınmalı ve başka kullanıcı adına işlem
    yapılması engellenmelidir.
-   Üretim ortamı için doğrulama, kimlik doğrulama/yetkilendirme,
    loglama, hata yönetimi ve veritabanı kısıtları ayrıca gözden
    geçirilmelidir.


1. `POST /users` endpoint'i ile bir yönetici (Admin) kullanıcısı oluştur.
2. `POST /users` endpoint'i ile bir müşteri (Customer) kullanıcısı oluştur.
3. `POST /tickets` endpoint'i ile müşteri adına yeni bir ticket oluştur.
4. `GET /tickets` endpoint'i ile oluşturulan ticket'ı ve ID'sini kontrol et.
5. `POST /tickets/{ticketId}/replies` endpoint'i ile yönetici olarak ticket'a cevap yaz.
6. `GET /tickets/{id}` endpoint'i ile ticket detayını ve konuşma geçmişini görüntüle.
7. Gerekirse `POST /tickets/{ticketId}/replies` endpoint'i ile müşteri olarak cevap ver ve konuşmayı sürdür.
8. `PUT /tickets/{id}/close` endpoint'i ile yönetici olarak ticket'ı kapat.
9. `POST /tickets/{ticketId}/rating` endpoint'i ile müşteri olarak hizmeti 1–5 arasında puanla ve isteğe bağlı yorum ekle.
10. `GET /tickets/{ticketId}/rating` endpoint'i ile değerlendirmeyi görüntüle.
