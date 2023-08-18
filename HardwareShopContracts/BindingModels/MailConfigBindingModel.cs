namespace HardwareShopContracts.BindingModels
{
    public class MailConfigBindingModel
    {
        public string MailLogin { get; set; } = string.Empty;

        public string MailPassword { get; set; } = string.Empty;
        // откуда отправляем письма
        public string SmtpClientHost { get; set; } = string.Empty;

        public int SmtpClientPort { get; set; }
        // откуда получаем
        public string PopHost { get; set; } = string.Empty;

        public int PopPort { get; set; }
    }
}