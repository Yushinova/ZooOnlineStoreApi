namespace ZooOnlineStoreApi.Services.Exeptions
{
    public class ValidationException : ApplicationException
    {
        private readonly string targetName;     // наименование невалидного компонента модели
        private readonly object? value;         // само невалидное значение
        private readonly string details;        // детали

        public string TargetName { get { return targetName; } }
        public object? Value { get { return value; } }
        public string Details { get { return details; } }

        public ValidationException(string targetName, string details, object? value = null)
            : base($"{targetName} is invalid: {details}")
        {
            this.targetName = targetName;
            this.details = details;
            this.value = value;
        }
    }
}
