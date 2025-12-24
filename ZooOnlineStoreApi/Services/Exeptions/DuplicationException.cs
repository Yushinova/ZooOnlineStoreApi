namespace ZooOnlineStoreApi.Services.Exeptions
{
    public class DuplicationException: ApplicationException
    {
        private readonly string targetName;     // наименование компонента модели, для которого зафиксировано дублирование
        private readonly object? value;         // само дублированное значение

        public string TargetName { get { return targetName; } }
        public object? Value { get { return value; } }

        public DuplicationException(string targetName, object? value = null)
            : base($"{targetName} is duplicated")
        {
            this.targetName = targetName;
            this.value = value;
        }
    }
}
