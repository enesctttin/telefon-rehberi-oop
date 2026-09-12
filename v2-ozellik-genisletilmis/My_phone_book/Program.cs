using System;
using System.Collections;


namespace My_phone_book
{
    static class Program

    {
        static ArrayList tel_book_arr = new ArrayList();

        static void Main(string[] args)
        {
            Console.WriteLine("------WELLCOME------");



            int command = -1;
            while (command != 7)
            {
                Console.Clear();

                Console.WriteLine("\nSelect one of the following:");
                Console.WriteLine("1 : Add a contact");
                Console.WriteLine("2 : List contacts");
                Console.WriteLine("3 : Search a contact using by name or surname");
                Console.WriteLine("4 : Search a contact using by phone");
                Console.WriteLine("5 : Edit information");
                Console.WriteLine("6 : Delete information");

                Console.WriteLine("7 : exit");

                command = Convert.ToInt32(Console.ReadLine());

                switch (command)
                {
                    case 1:
                        addContact();
                        break;
                    case 2:
                        listContacts();
                        break;
                    case 3:
                        searchName();
                        break;
                    case 4:
                        searchPhone();
                        break;
                    case 5:
                        edit_info();
                        break;
                    case 6:
                        delet_info();
                        break;
                    default:
                        Console.WriteLine("\nInvalid Option! Please type 1-7.");
                        break;
                }

            }
        }
        static void addContact()
        {
            Console.Clear();

            Console.WriteLine("--- ADD CONTACT ---");

            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            if (!IsOnlyLetters(name)) { ShowError("Name must contain only letters!"); return; }

            Console.Write("Enter surname: ");
            string surname = Console.ReadLine();
            if (!IsOnlyLetters(surname)) { ShowError("Surname must contain only letters!"); return; }

            Console.Write("Enter phone: ");
            string tel = Console.ReadLine();
            if (!IsOnlyDigits(tel)) { ShowError("Phone must contain only digits!"); return; }

            // 2. Koddaki "Aynı kayıt var mı?" kontrolü
            foreach (telephone obj in tel_book_arr)
            {
                if (obj.name.ToLower() == name.ToLower() && obj.surname.ToLower() == surname.ToLower())
                {
                    Console.Write("Person exists. Overwrite phone number? (y/n): ");
                    if (Console.ReadLine().ToLower() == "y")
                    {
                        obj.tel = tel;
                        Console.WriteLine("Updated!");
                    }
                    return;
                }
            }

            telephone t = new telephone { name = name, surname = surname, tel = tel };
            tel_book_arr.Add(t);
            Console.WriteLine("\nSaved!");
            Console.ReadKey();

        }


        static void listContacts()
        {
            Console.Clear();

            // 2. Koddaki otomatik sıralama özelliği
            tel_book_arr.Sort();

            if (tel_book_arr.Count == 0)
            {
                Console.WriteLine("Phone book is empty.");
            }
            else
            {
                foreach (object obj in tel_book_arr)
                {
                    telephone temp = (telephone)obj;   // CAST

                    Console.WriteLine("name : " + temp.name);
                    Console.WriteLine("family : " + temp.surname);
                    Console.WriteLine("tel : " + temp.tel);
                    Console.WriteLine("-----------------------");
                }
            }

            Console.ReadKey();
        }

        static void searchName()
        {
            Console.Clear();
            Console.Write("Enter name or surname to search : ");
            string name = Console.ReadLine();

            bool isFound = false; // Kayıt bulunup bulunmadığını takip etmek için eklendi

            foreach (object obj in tel_book_arr)
            {
                telephone temp = (telephone)obj;

                if (temp.name.ToLower() == name.ToLower() || temp.surname.ToLower() == name.ToLower())
                {
                    Console.WriteLine("\nFound!");
                    Console.WriteLine("name : " + temp.name);
                    Console.WriteLine("family : " + temp.surname);
                    Console.WriteLine("tel : " + temp.tel);
                    Console.WriteLine("-----------------------");
                    isFound = true; // Eşleşme bulundu olarak işaretliyoruz
                                    // Burada eskiden olan "return;" komutunu SİLDİK. Döngü devam edecek.
                }
            }

            // Eğer döngü bittiğinde isFound hala false ise, hiç kayıt bulunamamıştır.
            if (!isFound)
            {
                Console.WriteLine("\nNot found!");
            }

            Console.ReadKey();

        }

        static void searchPhone()
        {
            Console.Clear();
            Console.Write("Enter phone to search : ");
            string phone = Console.ReadLine();

            foreach (object obj in tel_book_arr)
            {
                telephone temp = (telephone)obj;

                if (temp.tel == phone)
                {
                    Console.WriteLine("\nFound!");
                    Console.WriteLine("name : " + temp.name);
                    Console.WriteLine("surname : " + temp.surname);
                    Console.WriteLine("tel : " + temp.tel);
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("\nNot found!");
            Console.ReadKey();


        }

        static void edit_info()
        {

            Console.Clear();
            Console.Write("Enter phone number to edit : ");
            string phoneToEdit = Console.ReadLine();

            foreach (object obj in tel_book_arr)
            {
                telephone temp = (telephone)obj;

                if (temp.tel == phoneToEdit)
                {
                    // Kullanıcının doğru kişiyi düzenlediğini görmesi için adını ekrana yazdıralım
                    Console.WriteLine($"\nEditing Contact: {temp.name} {temp.surname}\n");

                    Console.Write("New name : ");
                    string newName = Console.ReadLine();
                    if (!IsOnlyLetters(newName)) { ShowError("Name must contain only letters!"); return; }

                    Console.Write("New surname : ");
                    string newSurname = Console.ReadLine();
                    if (!IsOnlyLetters(newSurname)) { ShowError("Surname must contain only letters!"); return; }

                    Console.Write("New phone : ");
                    string newTel = Console.ReadLine();
                    if (!IsOnlyDigits(newTel)) { ShowError("Phone must contain only digits!"); return; }

                    temp.name = newName;
                    temp.surname = newSurname;
                    temp.tel = newTel;

                    Console.WriteLine("\nUpdated!");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("\nNot found!");
            Console.ReadKey();

        }


        static void delet_info()
        {
            Console.Clear();
            Console.Write("Enter the phone number of the contact to delete : ");
            string phone = Console.ReadLine();

            for (int i = 0; i < tel_book_arr.Count; i++)
            {
                telephone temp = (telephone)tel_book_arr[i];

                if (temp.tel == phone)
                {
                    tel_book_arr.RemoveAt(i);
                    Console.WriteLine("\nDeleted!");
                    Console.ReadKey();
                    return;
                }
            }

            Console.WriteLine("\nNot found!");
            Console.ReadKey();
        }


        static bool IsOnlyLetters(string input)
        {
            foreach (char c in input)
                if (!char.IsLetter(c) && c != ' ')
                    return false;
            return true;
        }


        static bool IsOnlyDigits(string input)
        {
            foreach (char c in input)
                if (!char.IsDigit(c))
                    return false;
            return true;
        }


        static void ShowError(string msg)
        {
            Console.WriteLine($"ERROR: {msg}");
            Console.ReadKey();
        }

    }


    class telephone : IComparable
    {
        public string name;
        public string surname;
        public string tel;


        public int CompareTo(object obj)
        {
            telephone other = (telephone)obj;
            int res = string.Compare(this.name, other.name, StringComparison.OrdinalIgnoreCase);

            // Eğer isimler aynıysa (res == 0) soyisime bak, değilse direkt res'i (ismin sonucunu) dön.
            return (res == 0) ? string.Compare(this.surname, other.surname, StringComparison.OrdinalIgnoreCase) : res;
        }


    }


}
