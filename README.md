# TicketSystem.WebAPI

## Kullanım Adımları

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
