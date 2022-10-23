using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Programing_Labs
{
    [Serializable]
    public class PhoneBook : IPhoneBook
    {
        private static int Count { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Year { get; set; }

        public PhoneBook()
        {
        }
        public PhoneBook(string Name, string PhoneNumber, string Year)
        {
            this.Id = ++Count;
            this.Name = Name;
            this.PhoneNumber = PhoneNumber;
            this.Year = Year;
        }

        public static List<PhoneBook> PhoneBooks = new List<PhoneBook>();
        public static ObservableCollection<PhoneBook> PhoneBooksCollection = new ObservableCollection<PhoneBook>();
        public async static Task Add(PhoneBook phoneBook)
        {
            PhoneBooks.Add(phoneBook);
            PhoneBooksCollection.Add(phoneBook);
            await Task.Yield();
        }

        public static void Remove(PhoneBook item) => PhoneBooks.Remove(item);
        public static XmlSerializer Formatter = new XmlSerializer(typeof(PhoneBook));
        private static void Serialization(object serializableObject)
        {
            using (FileStream fs = new FileStream("PhoneBooks.xml", FileMode.OpenOrCreate))
            {
                Formatter.Serialize(fs, serializableObject);
                System.Windows.Forms.MessageBox.Show("Данные успешно сохранены!");
            }
        }
        private static List<PhoneBook> Deserialization()
        {
            using (FileStream fs = new FileStream("people.xml", FileMode.OpenOrCreate))
            {
                List<PhoneBook> phoneBook = Formatter.Deserialize(fs) as List<PhoneBook>;
                return phoneBook;
            }
        }

    }
}
