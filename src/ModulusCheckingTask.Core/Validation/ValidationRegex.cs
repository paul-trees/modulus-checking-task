namespace ModulusCheckingTask.Core.Validation
{
    public static class ValidationRegex
    {
        public const string SortCode = "^[0-9]{6}$";
        public const string AccountNumber = "^[0-9]{8}$";
        public const string CombinedSortCodeAndAccountNumber = "^[0-9]{14}$";
    }
}
