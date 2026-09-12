# Telefon Rehberi — İki Yaklaşım

C# konsol uygulaması olarak geliştirilmiş telefon rehberi. Aynı problem iki
farklı yaklaşımla çözüldü ve her iki sürüm de bu repoda tutuluyor.

İTÜ Bilgi İşlem Daire Başkanlığı — Yazılım Geliştirme Grubu (YGG) Yazılım
Geliştirme Proje Sınıfı'nın üçüncü aşaması kapsamında geliştirildi.

## Neden iki sürüm var

İlk sürümde amaç, hazır yapıların arkasında ne olduğunu anlamaktı: `List<T>` ve
LINQ bilinçli olarak kullanılmadı, veri yapısı ve algoritmalar sıfırdan yazıldı.
Bu sürüm mimari açıdan temiz ama özellik olarak sınırlı kaldı.

İkinci sürümde odak özellik tamamlamaya kaydı; düzenleme, silme ve telefon
numarasına göre ayrı arama eklendi. Ancak bunu yaparken `ArrayList` kullanıldı
ve sınıf hiyerarşisi basitleştirildi.

## Karşılaştırma

| | v1 — OOP temelli | v2 — Özellik genişletilmiş |
|---|---|---|
| Veri yapısı | Sıfırdan yazılmış dinamik dizi | `ArrayList` |
| Sınıf yapısı | `Person` (abstract) → `Contact`, `PhoneBookManager` | `telephone`, `Program` |
| Kişi ekleme | ✓ | ✓ |
| Alfabetik listeleme | ✓ (Bubble sort) | ✓ |
| Ada/soyada göre arama | ✓ | ✓ |
| Telefona göre ayrı arama | ✗ (aramaya gömülü) | ✓ |
| Kayıt düzenleme | ✗ | ✓ |
| Kayıt silme | ✗ | ✓ |
| Girdi doğrulama | ✓ | ✓ |
| Mükerrer kayıt uyarısı | ✓ | ✓ |
| Arayüz dili | Türkçe | İngilizce |

## v1 — OOP temelli

`List<T>` ve LINQ kullanılmadan, hazır yapıların karşılıkları elle yazıldı:

| Hazır karşılığı | Bu sürümde |
|---|---|
| `List<T>` | Kapasitesi dolunca ikiye katlanan dinamik dizi (`ExpandArray`) |
| `OrderBy` | Bubble sort — isim eşitse soyada bakan ikincil karşılaştırma |
| `Where` | İki geçişli filtreleme: önce eşleşme sayılır, sonra tam boyutlu dizi üretilir |
| `FirstOrDefault` | Doğrusal arama (`FindExactContact`) |
| `.All` | Karakter karakter doğrulama (`IsAllLetters`, `IsAllDigits`) |

Sınıf yapısı: `Person` soyut temel sınıf, `Contact` ondan türüyor ve
`GetFullName()` ile `ToString()` metodlarını override ediyor. `PhoneBookManager`
yalnızca veri ve algoritmalardan sorumlu, içinde tek bir `Console` çağrısı yok;
arayüz tamamen `Program` sınıfında. Sorumluluklar bu şekilde ayrıldığı için veri
katmanına dokunmadan arayüz değiştirilebilir.

**Eksikleri:** düzenleme ve silme yok. Telefona göre arama ayrı bir menü
seçeneği değil, genel aramanın içine gömülü.

## v2 — Özellik genişletilmiş

Yedi menü seçeneği: ekleme, listeleme, ada/soyada göre arama, telefona göre
arama, düzenleme, silme, çıkış. Girdi doğrulama ve mükerrer kayıt kontrolü
korunuyor.

**Eksikleri:** `ArrayList` kullanıldığı için tip güvenliği yok, elemanlar
`object` olarak saklanıp her erişimde dönüştürülüyor. Sınıf hiyerarşisi ve
soyutlama v1'deki kadar belirgin değil. Veri erişimi ile arayüz aynı sınıfta
iç içe.

## Her iki sürüm için geçerli eksikler

- Veriler bellekte tutuluyor, uygulama kapanınca kayboluyor
- Bir kişiye yalnızca tek numara eklenebiliyor

## Çalıştırma

```bash
git clone https://github.com/enesctttin/telefon-rehberi-oop.git

# v1
cd telefon-rehberi-oop/v1-oop-temelli
dotnet run --project TelefonRehberi

# v2
cd ../v2-ozellik-genisletilmis
dotnet run --project TelefonRehberi2
```
