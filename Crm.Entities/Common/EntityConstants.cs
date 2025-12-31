
namespace Crm.Entities.Common
{
    public static class EntityConstants
    {
        public const int NameMax = 200;
        public const int TitleMax = 250;
        public const int EmailMax = 254;
        public const int PhoneMax = 30;

        public const int CodeMax = 50;
        public const int AccountCodeMax = 50;

        public const int SmallTextMax = 500;
        public const int MediumTextMax = 2000;

        public const int CurrencyCodeMax = 3;
        public const int IbanMax = 34;

        public const int FileNameMax = 260;
        public const int ContentTypeMax = 200;
        public const int StorageProviderMax = 50;
        public const int StoragePathMax = 1000;

        public const int ConcurrencyTokenMax = 8; // RowVersion (timestamp/rowversion)
    }
}
