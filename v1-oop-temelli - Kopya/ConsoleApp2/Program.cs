using System;

namespace TelefonRehberiApp
{
    // 1. SOYUTLAMA (ABSTRACTION) VE KALITIM (INHERITANCE)
    public abstract class Person
    {
        protected string _firstName;
        protected string _lastName;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value.Trim(); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value.Trim(); }
        }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public abstract string GetFullName();
    }

    public class Contact : Person
    {
        public string PhoneNumber { get; set; }

        public Contact(string firstName, string lastName, string phoneNumber)
            : base(firstName, lastName)
        {
            PhoneNumber = phoneNumber;
        }

        public override string GetFullName()
        {
            string fName = char.ToUpper(FirstName[0]) + FirstName.Substring(1).ToLower();
            string lName = char.ToUpper(LastName[0]) + LastName.Substring(1).ToLower();
            return $"{fName} {lName}";
        }

        public override string ToString()
        {
            return $"Ad Soyad: {GetFullName()} \tTelefon: {PhoneNumber}";
        }
    }

    // 2. KENDİ LİSTEMİZİ VE ALGORİTMALARIMIZI YAZDIĞIMIZ YÖNETİCİ SINIF
    public class PhoneBookManager
    {
        // "List" kütüphanesi olmadığı için ham dizi (array) kullanıyoruz.
        private Contact[] _contacts;
        private int _count; // Dizideki anlık dolu kayıt sayısı

        public PhoneBookManager()
        {
            _contacts = new Contact[2]; // Başlangıçta 2 kişilik küçük bir kutu açıyoruz
            _count = 0;
        }

        // DİZİYİ GENİŞLETME ALGORİTMASI (List'in arka planda yaptığı iş)
        private void ExpandArray()
        {
            // Mevcut kapasitenin 2 katı büyüklüğünde yeni bir dizi oluşturuyoruz
            Contact[] newArray = new Contact[_contacts.Length * 2];

            // Eski dizidekileri yeni ve büyük diziye kopyalıyoruz
            for (int i = 0; i < _contacts.Length; i++)
            {
                newArray[i] = _contacts[i];
            }
            _contacts = newArray; // Artık ana dizimiz bu geniş dizi oldu
        }

        public void AddContact(Contact contact)
        {
            // Eğer dizi tam doluysa, önce genişlet
            if (_count == _contacts.Length)
            {
                ExpandArray();
            }
            _contacts[_count] = contact; // Kaydı boş olan ilk sıraya ekle
            _count++; // Kişi sayısını 1 artır
        }

        // BİREBİR ARAMA ALGORİTMASI (FirstOrDefault Yerine)
        public Contact FindExactContact(string firstName, string lastName)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_contacts[i].FirstName.ToLower() == firstName.ToLower() &&
                    _contacts[i].LastName.ToLower() == lastName.ToLower())
                {
                    return _contacts[i];
                }
            }
            return null; // Bulamazsa boşa dön
        }

        // FİLTRELEME ALGORİTMASI (Where Yerine)
        public Contact[] SearchContacts(string keyword)
        {
            keyword = keyword.ToLower();
            Contact[] tempResults = new Contact[_count]; // Geçici sonuç dizisi
            int matchCount = 0;

            for (int i = 0; i < _count; i++)
            {
                if (_contacts[i].FirstName.ToLower().Contains(keyword) ||
                    _contacts[i].LastName.ToLower().Contains(keyword) ||
                    _contacts[i].PhoneNumber.Contains(keyword))
                {
                    tempResults[matchCount] = _contacts[i];
                    matchCount++;
                }
            }

            // İçinde boşluklar olmayan, sadece bulduklarımız kadar boyutu olan nihai dizi
            Contact[] finalResults = new Contact[matchCount];
            for (int i = 0; i < matchCount; i++)
            {
                finalResults[i] = tempResults[i];
            }
            return finalResults;
        }

        // ALFABETİK SIRALAMA ALGORİTMASI - BUBBLE SORT (OrderBy Yerine)
        public Contact[] GetAllContactsAlphabetically()
        {
            // Orijinal veriyi bozmamak için dizinin kopyasını alıyoruz
            Contact[] sortedArray = new Contact[_count];
            for (int i = 0; i < _count; i++)
            {
                sortedArray[i] = _contacts[i];
            }

            // Klasik Bubble Sort (Kabarcık Sıralaması)
            for (int i = 0; i < _count - 1; i++)
            {
                for (int j = 0; j < _count - i - 1; j++)
                {
                    // string.Compare: İlk kelime alfabetik olarak gerideysek 1, aynıysa 0, öndeyse -1 döner
                    int compareResult = string.Compare(sortedArray[j].FirstName.ToLower(), sortedArray[j + 1].FirstName.ToLower());

                    // Eğer isimler aynıysa soyisme bakarak karar ver
                    if (compareResult == 0)
                    {
                        compareResult = string.Compare(sortedArray[j].LastName.ToLower(), sortedArray[j + 1].LastName.ToLower());
                    }

                    // Eğer soldaki kişi alfabetik olarak sağdakinden sonraysa, yer değiştir!
                    if (compareResult > 0)
                    {
                        Contact temp = sortedArray[j];
                        sortedArray[j] = sortedArray[j + 1];
                        sortedArray[j + 1] = temp;
                    }
                }
            }

            return sortedArray;
        }
    }

    // 3. ARAYÜZ (UI) VE KONTROL (VALIDATION) METOTLARI
    class Program
    {
        static PhoneBookManager manager = new PhoneBookManager();

        static void Main(string[] args)
        {
            Console.WriteLine("=== TELEFON REHBERİ UYGULAMASINA HOŞ GELDİNİZ ===");

            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\nLütfen yapmak istediğiniz işlemi seçiniz:");
                Console.WriteLine("1. Yeni Numara Kaydet");
                Console.WriteLine("2. Rehberi Listele");
                Console.WriteLine("3. Rehberde Arama Yap");
                Console.WriteLine("4. Çıkış");
                Console.Write("Seçiminiz: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddNewContactUI(); break;
                    case "2": ListContactsUI(); break;
                    case "3": SearchContactUI(); break;
                    case "4":
                        Console.WriteLine("Çıkış yapılıyor. İyi günler!");
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Hatalı seçim! Lütfen 1-4 arasında bir değer giriniz.");
                        break;
                }
            }
        }

        static void AddNewContactUI()
        {
            Console.WriteLine("\n[YENİ KAYIT EKRANI]");

            string name = GetValidStringInput("Ad: ");
            string surname = GetValidStringInput("Soyad: ");
            string phone = GetValidPhoneInput("Telefon Numarası: ");

            Contact existingContact = manager.FindExactContact(name, surname);

            if (existingContact != null)
            {
                Console.WriteLine($"\nUYARI: '{name} {surname}' isimli kişi rehberde zaten kayıtlı!");
                Console.Write("Var olan numaranın üzerine yazılmasını onaylıyor musunuz? (E/H): ");
                if (Console.ReadLine().ToUpper() == "E")
                {
                    existingContact.PhoneNumber = phone;
                    Console.WriteLine("Başarılı: Numara güncellendi!");
                }
                else Console.WriteLine("İşlem iptal edildi.");
            }
            else
            {
                manager.AddContact(new Contact(name, surname, phone));
                Console.WriteLine("Başarılı: Yeni kişi rehbere eklendi!");
            }
        }

        static void ListContactsUI()
        {
            Console.WriteLine("\n[REHBER LİSTESİ]");
            Contact[] contacts = manager.GetAllContactsAlphabetically();

            if (contacts.Length == 0)
            {
                Console.WriteLine("Rehberinizde henüz kayıtlı kimse yok.");
                return;
            }

            for (int i = 0; i < contacts.Length; i++)
            {
                Console.WriteLine(contacts[i].ToString());
            }
        }

        static void SearchContactUI()
        {
            Console.WriteLine("\n[REHBERDE ARAMA]");
            Console.Write("Aramak istediğiniz isim, soyisim veya numarayı giriniz: ");
            string keyword = Console.ReadLine();

            Contact[] results = manager.SearchContacts(keyword);

            if (results.Length == 0) Console.WriteLine("Arama sonucuna uygun rehber kaydı bulunamadı.");
            else
            {
                Console.WriteLine($"\nBulunan Kayıtlar ({results.Length} adet):");
                for (int i = 0; i < results.Length; i++)
                {
                    Console.WriteLine(results[i].ToString());
                }
            }
        }

        // MANUEL KARAKTER KONTROLLERİ

        static string GetValidStringInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) && IsAllLetters(input))
                {
                    return input;
                }
                Console.WriteLine("HATA: Bu alan sadece harflerden oluşmalıdır! Lütfen tekrar giriniz.");
            }
        }

        static string GetValidPhoneInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input) && IsAllDigits(input))
                {
                    return input;
                }
                Console.WriteLine("HATA: Telefon numarası sadece rakamlardan oluşmalıdır! Lütfen tekrar giriniz.");
            }
        }

        // Metnin içindeki harfleri tek tek dönüp bakan kendi fonksiyonumuz
        static bool IsAllLetters(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsLetter(text[i]) && !char.IsWhiteSpace(text[i]))
                    return false;
            }
            return true;
        }

        // Metnin içindeki sayıları tek tek dönüp bakan kendi fonksiyonumuz
        static bool IsAllDigits(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (!char.IsDigit(text[i]))
                    return false;
            }
            return true;
        }
    }
}