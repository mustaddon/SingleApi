namespace SingleApi
{

    public interface ISapiFileResponse : ISapiFile
    {
        bool InlineDisposition { get; set; }
    }
}
