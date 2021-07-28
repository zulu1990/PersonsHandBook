using System.ComponentModel.DataAnnotations;
using System.IO;

namespace PersonsHandBook.Domain.Models
{
    public static class Constants
    {
        public const string PhotosFolderPath  = "Resources//Photos";

        public static string BasePath => Directory.GetCurrentDirectory();

        public static string CreatePeronsError = "Cant Create New Person";

        public static string CantGetReport = "Can't Get Report";

        public static string ErrorUpdateContact = "Error Update Contact";

        public static string ContactsNotFound = "Contacts Not Found";

        public static string ErrorEditPerson = "Error Edit Person";

        public static string NameLength = "Invalid Name Length";

        public static string OneLanguageLetters = "One Language Letters";

        public static string Exception = "Exception";

        public static string ErrorRemovePhoto = "Error Remove Photo";

        public static object ObjectNull = "Object Is Null";

        public static string PhotoNotFound = "Photo Not Found";

        public static string PersonNotFound = "Person Not Found";

        public static string CouldntDeleteContact = "Could Not Delete Contact";

        public static string ContactNotFound = "Contact Not Found";

        public static string ErrorRemovePerson = "Error Remove Person";
    }
}
